using AutoMapper;
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
    [Authorize(Roles = "Admin")]
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
        public async Task<ActionResult<IEnumerable<CategoryResponseAdmin>>> GetCategorysForAdmin()
        {
            try
            {
                var list = await _CategoryService.GetAll();
                var response = list.Select
                           (
                             emp => _mapper.Map<Category, CategoryResponseAdmin>(emp)
                           );
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponseAdmin>> GetCategoryByID([FromRoute] Guid? id)
        {
            var Category = _mapper.Map<CategoryResponseAdmin>(await _CategoryService.GetCategoryByID(id));

            if (Category == null)
            {
                return NotFound();
            }

            return Category;
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_name/{name}")]
        public async Task<ActionResult<CategoryResponseAdmin>> GetCategoryByName([FromRoute] string name)
        {
            var Category = _mapper.Map<CategoryResponseAdmin>(await _CategoryService.GetCategoryByName(name));

            if (Category == null)
            {
                return NotFound();
            }

            return Category;
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
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
        public async Task<ActionResult<CategoryResponseAdmin>> PostCategory([FromBody] CreateCategoryRequest createCategoryRequest)
        {
            try
            {
                var Category = await _CategoryService.AddNewCategory(createCategoryRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveCategoryAdd", Category);
                return Ok(_mapper.Map<CategoryResponseAdmin>(Category));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid? id)
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
