using AutoMapper;
using BIDs_API.SignalR;
using Business_Logic.Modules.BookingItemModule.Interface;
using Business_Logic.Modules.ItemModule.Interface;
using Business_Logic.Modules.ItemModule.Request;
using Business_Logic.Modules.ItemModule.Response;
using Data_Access.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

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
        private readonly IHubContext<BookingItemHub> _hubBookingContext;
        private readonly IBookingItemService _BookingItemService;

        public ItemsController(IItemService ItemService
            , IMapper mapper
            , IHubContext<ItemHub> hubContext
            , IHubContext<BookingItemHub> hubBookingContext
            , IBookingItemService BookingItemService)
        {
            _ItemService = ItemService;
            _mapper = mapper;
            _hubContext = hubContext;
            _hubBookingContext = hubBookingContext;
            _BookingItemService = BookingItemService;
        }

        // GET api/<ValuesController>
        [Authorize(Roles = "Staff,Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemResponseStaffAndAdmin>>> GetItemsForAdmin()
        {
            try
            {
                var list = await _ItemService.GetAll();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Item, ItemResponseStaffAndAdmin>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/5
        [Authorize(Roles = "Staff,Admin")]
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
        [Authorize(Roles = "Staff,Admin")]
        [HttpGet("by_name/{name}")]
        public async Task<ActionResult<ItemResponseStaffAndAdmin>> GetItemByName([FromRoute] string name)
        {
            var Item = _mapper.Map<ItemResponseStaffAndAdmin>(await _ItemService.GetItemByName(name));

            if (Item == null)
            {
                return NotFound();
            }

            return Item;
        }

        // GET api/<ValuesController>/abc
        [Authorize(Roles = "Staff,Admin")]
        [HttpGet("by_category_name/{name}")]
        public async Task<ActionResult<IEnumerable<ItemResponseStaffAndAdmin>>> GetItemByCategoryName([FromRoute] string name)
        {
            try
            {
                var list = await _ItemService.GetItemByCategoryName(name);
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Item, ItemResponseStaffAndAdmin>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [Authorize(Roles = "Bidder,Auctioneer")]
        [HttpGet("by_user/{id}")]
        public async Task<ActionResult<IEnumerable<ItemResponseStaffAndAdmin>>> GetItemByUser([FromRoute] Guid? id)
        {
            try
            {
                var list = await _ItemService.GetItemByUserID(id);
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Item, ItemResponseStaffAndAdmin>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [Authorize(Roles = "Bidder,Auctioneer")]
        [HttpGet("by_name_and_user")]
        public async Task<ActionResult<IEnumerable<ItemResponseStaffAndAdmin>>> GetItemByNameAndUser([FromRoute] string name, Guid id)
        {
            try
            {
                var list = await _ItemService.GetItemByNameAndUser(name, id);
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Item, ItemResponseStaffAndAdmin>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [Authorize(Roles = "Bidder,Auctioneer")]
        [HttpGet("by_category_name_and_user")]
        public async Task<ActionResult<IEnumerable<ItemResponseStaffAndAdmin>>> GetItemByCategoryNameAndUser([FromRoute] string name, Guid id)
        {
            try
            {
                var list = await _ItemService.GetItemByCategoryNameAndUser(name, id);
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Item, ItemResponseStaffAndAdmin>(emp)
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
        [Authorize(Roles = "Auctioneer")]
        [HttpPost]
        public async Task<ActionResult<ItemResponseStaffAndAdmin>> PostItem([FromBody] CreateItemRequest createItemRequest)
        {
            try
            {
                var Item = await _ItemService.AddNewItem(createItemRequest);
                var BookingItem = await _BookingItemService.GetBookingItemByItem(Item.ElementAt(0).Id);
                await _hubContext.Clients.All.SendAsync("ReceiveItemAdd", Item);
                await _hubBookingContext.Clients.All.SendAsync("ReceiveBookingItemAdd", BookingItem.First());

                var response = Item.Select
                           (
                             emp => _mapper.Map<Item, ItemResponseStaffAndAdmin>(emp)
                           );
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //// DELETE api/<ValuesController>/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteItem([FromRoute] Guid id)
        //{
        //    try
        //    {
        //        var Item = await _ItemService.DeleteItem(id);
        //        var BookingItem = await _BookingItemService.GetBookingItemByItem(Item.Id);
        //        await _hubContext.Clients.All.SendAsync("ReceiveItemDelete", Item);
        //        await _hubBookingContext.Clients.All.SendAsync("ReceiveBookingItemUpdate", BookingItem.First());
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
