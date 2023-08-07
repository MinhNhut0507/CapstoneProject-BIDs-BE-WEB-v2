using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data_Access.Entities;
using Business_Logic.Modules.DescriptionModule.Interface;
using Business_Logic.Modules.DescriptionModule.Request;
using Microsoft.AspNetCore.Authorization;
using Business_Logic.Modules.DescriptionModule.Response;
using AutoMapper;
using BIDs_API.SignalR;
using Microsoft.AspNetCore.SignalR;
using Business_Logic.Modules.CategoryModule.Response;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Dev")]
    public class DescriptionController : ControllerBase
    {
        private readonly IDescriptionService _DescriptionService;
        public readonly IMapper _mapper;
        private readonly IHubContext<DescriptionHub> _hubContext;

        public DescriptionController(IDescriptionService DescriptionService
            , IMapper mapper
            , IHubContext<DescriptionHub> hubContext)
        {
            _DescriptionService = DescriptionService;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        // GET api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DescriptionDetailResponse>>> GetDescriptionsForAdmin()
        {
            try
            {
                var list = await _DescriptionService.GetAll();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Description, DescriptionDetailResponse>(emp)
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
        public async Task<ActionResult<IEnumerable<DescriptionDetailResponse>>> GetDescriptionByID([FromQuery] Guid? id)
        {
            try
            {
                var list = await _DescriptionService.GetDescriptionByID(id);
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Description, DescriptionDetailResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_category_name")]
        public async Task<ActionResult<IEnumerable<DescriptionDetailResponse>>> GetDescriptionByCategoryName([FromQuery] string name)
        {
            try
            {
                var list = await _DescriptionService.GetDescriptionByCategoryName(name);
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Description, DescriptionDetailResponse>(emp)
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
        public async Task<IActionResult> PutDescription([FromBody] UpdateDescriptionRequest updateDescriptionRequest)
        {
            try
            {
                var description = await _DescriptionService.UpdateDescription(updateDescriptionRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveDescriptionUpdate", description);
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
        public async Task<ActionResult<DescriptionDetailResponse>> PostDescription([FromBody] CreateDescriptionRequest createDescriptionRequest)
        {
            try
            {
                var description = await _DescriptionService.AddNewDescription(createDescriptionRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveDescriptionAdd", description);
                return Ok(_mapper.Map<DescriptionDetailResponse>(description));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete]
        public async Task<IActionResult> DeleteDescription([FromQuery] Guid? id)
        {
            try
            {
                var description = await _DescriptionService.DeleteDescription(id);
                await _hubContext.Clients.All.SendAsync("ReceiveDescriptionDelete", description);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
