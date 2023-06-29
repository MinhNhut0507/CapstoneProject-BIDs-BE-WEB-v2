using AutoMapper;
using BIDs_API.SignalR;
using Business_Logic.Modules.ItemModule.Interface;
using Business_Logic.Modules.ItemModule.Request;
using Business_Logic.Modules.ItemModule.Response;
using Data_Access.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _ItemService;
        public readonly IMapper _mapper;
        private readonly IHubContext<ItemHub> _hubContext;

        public ItemsController(IItemService ItemService
            , IMapper mapper
            , IHubContext<ItemHub> hubContext)
        {
            _ItemService = ItemService;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        // GET api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemResponseStaffAndAdmin>>> GetItemsForAdmin()
        {
            try
            {
                var list = await _ItemService.GetAll();
                var response = list.Select
                           (
                             emp => _mapper.Map<Item, ItemResponseStaffAndAdmin>(emp)
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
        public async Task<ActionResult<ItemResponseStaffAndAdmin>> GetItemByID([FromRoute] Guid? id)
        {
            var Item = _mapper.Map<ItemResponseStaffAndAdmin>(await _ItemService.GetItemByID(id));

            if (Item == null)
            {
                return NotFound();
            }

            return Item;
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_name/{name}")]
        public async Task<ActionResult<ItemResponseUser>> GetItemByName([FromRoute] string name)
        {
            var Item = _mapper.Map<ItemResponseUser>(await _ItemService.GetItemByName(name));

            if (Item == null)
            {
                return NotFound();
            }

            return Item;
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_type_name/{name}")]
        public async Task<ActionResult<IEnumerable<ItemResponseStaffAndAdmin>>> GetItemByTypeName([FromRoute] string name)
        {
            try
            {
                var list = await _ItemService.GetItemByTypeName(name);
                var response = list.Select
                           (
                             emp => _mapper.Map<Item, ItemResponseStaffAndAdmin>(emp)
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

        // GET api/<ValuesController>/abc
        [HttpGet("by_user/{id}")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemByUser([FromRoute] Guid? id)
        {
            try
            {
                var list = await _ItemService.GetItemByUserID(id);
                var response = list.Select
                           (
                             emp => _mapper.Map<Item, ItemResponseUser>(emp)
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

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutItem([FromBody] UpdateItemRequest updateItemRequest)
        {
            try
            {
                var Item = await _ItemService.UpdateItem(updateItemRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveItemUpdate", Item);
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
        public async Task<ActionResult<ItemResponseUser>> PostItem([FromBody] CreateItemRequest createItemRequest)
        {
            try
            {
                var Item = await _ItemService.AddNewItem(createItemRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveItemAdd", Item);
                return Ok(_mapper.Map<ItemResponseUser>(Item));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem([FromRoute] Guid? id)
        {
            try
            {
                var Item = await _ItemService.DeleteItem(id);
                await _hubContext.Clients.All.SendAsync("ReceiveItemDelete", Item);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
