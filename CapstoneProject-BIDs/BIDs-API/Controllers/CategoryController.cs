﻿using AutoMapper;
using BIDs_API.SignalR;
using Business_Logic.Modules.CategoryModule.Interface;
using Business_Logic.Modules.CategoryModule.Request;
using Business_Logic.Modules.CategoryModule.Response;
using Data_Access.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategorysController : ControllerBase
    {
        private readonly ICategoryService _CategoryService;
        public readonly IMapper _mapper;
        private readonly IHubContext<CategoryHub> _hubContext;

        public CategorysController(ICategoryService CategoryService
            , IMapper mapper
            , IHubContext<CategoryHub> hubContext)
        {
            _CategoryService = CategoryService;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        // GET api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategorysForAdmin()
        {
            try
            {
                var list = await _CategoryService.GetAll();
                if (list == null)
                {
                    return NotFound();
                }
                foreach(var category in list)
                {
                    foreach (var description in category.Descriptions)
                    {
                        if (description.Status == false)
                            category.Descriptions.Remove(description);
                    }
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Category, CategoryResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>
        [HttpGet("valid")]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategorysValid()
        {
            try
            {
                var list = await _CategoryService.GetCategorysIsValid();
                if (list == null)
                {
                    return NotFound();
                }
                foreach (var category in list)
                {
                    foreach (var description in category.Descriptions)
                    {
                        if (description.Status == false)
                            category.Descriptions.Remove(description);
                    }
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Category, CategoryResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("by_id")]
        [Authorize(Roles = "Admin,Dev")]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategoryByID([FromQuery] Guid? id)
        {
            try
            {
                var list = await _CategoryService.GetCategoryByID(id);
                if (list == null)
                {
                    return NotFound();
                }
                foreach (var description in list.ElementAt(0).Descriptions)
                {
                    if (description.Status == false)
                        list.ElementAt(0).Descriptions.Remove(description);
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Category, CategoryResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_name")]
        [Authorize(Roles = "Admin,Dev")]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategoryByName([FromQuery] string name)
        {
            try
            {
                var list = await _CategoryService.GetCategoryByName(name);
                if (list == null)
                {
                    return NotFound();
                }
                foreach (var description in list.ElementAt(0).Descriptions)
                {
                    if (description.Status == false)
                        list.ElementAt(0).Descriptions.Remove(description);
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Category, CategoryResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize(Roles = "Admin,Dev")]
        public async Task<IActionResult> PutCategory([FromBody] UpdateCategoryRequest updateCategoryRequest)
        {
            try
            {
                var Category = await _CategoryService.UpdateCategory(updateCategoryRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveCategoryUpdate", Category);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin,Dev")]
        public async Task<ActionResult<CategoryResponse>> PostCategory([FromBody] CreateCategoryRequest createCategoryRequest)
        {
            try
            {
                var Category = await _CategoryService.AddNewCategory(createCategoryRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveCategoryAdd", Category);
                return Ok(_mapper.Map<CategoryResponse>(Category));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete]
        [Authorize(Roles = "Admin,Dev")]
        public async Task<IActionResult> DeleteCategory([FromQuery] Guid? id)
        {
            try
            {
                var Category = await _CategoryService.DeleteCategory(id);
                await _hubContext.Clients.All.SendAsync("ReceiveCategoryDelete", Category);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
