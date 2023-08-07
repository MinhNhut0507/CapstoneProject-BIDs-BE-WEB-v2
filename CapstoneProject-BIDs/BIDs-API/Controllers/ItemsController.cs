using AutoMapper;
using BIDs_API.SignalR;
using Business_Logic.Modules.BookingItemModule.Interface;
using Business_Logic.Modules.CommonModule.Interface;
using Business_Logic.Modules.ItemModule.Interface;
using Business_Logic.Modules.ItemModule.Request;
using Business_Logic.Modules.ItemModule.Response;
using Data_Access.Entities;
using Data_Access.Enum;
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
        public readonly IMapper _mapper;
        private readonly IHubContext<ItemHub> _hubContext;
        private readonly IHubContext<BookingItemHub> _hubBookingContext;
        private readonly IBookingItemService _BookingItemService;
        private readonly IItemService _ItemService;
        private readonly IHubContext<NotificationHub> _notiHubContext;
        private readonly IHubContext<UserNotificationDetailHub> _userNotiHubContext;
        public readonly ICommon _common;

        public ItemsController(IItemService ItemService
            , IMapper mapper
            , IHubContext<BookingItemHub> hubBookingContext
            , IBookingItemService BookingItemService
            , IHubContext<ItemHub> hubContext
            , IHubContext<NotificationHub> notiHubContext
            , IHubContext<UserNotificationDetailHub> userNotiHubContext
            , ICommon common)
        {
            _mapper = mapper;
            _hubContext = hubContext;
            _hubBookingContext = hubBookingContext;
            _BookingItemService = BookingItemService;
            _common = common;
            _notiHubContext = notiHubContext;
            _userNotiHubContext = userNotiHubContext;
            _ItemService = ItemService;
        }

        // GET api/<ValuesController>
        [Authorize(Roles = "Staff,Admin,Dev")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemResponse>>> GetItemsForAdmin()
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
                             emp => _mapper.Map<Item, ItemResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/5
        [Authorize(Roles = "Staff,Admin,Dev")]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ItemResponse>>> GetItemByID([FromQuery] Guid? id)
        {
            try
            {
                var list = await _ItemService.GetItemByID(id);
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Item, ItemResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [Authorize(Roles = "Staff,Admin,Dev")]
        [HttpGet("by_name/{name}")]
        public async Task<ActionResult<IEnumerable<ItemResponse>>> GetItemByName([FromQuery] string name)
        {
            try
            {
                var list = await _ItemService.GetItemByName(name);
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Item, ItemResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [Authorize(Roles = "Staff,Admin,Dev")]
        [HttpGet("by_category_name/{name}")]
        public async Task<ActionResult<IEnumerable<ItemResponse>>> GetItemByCategoryName([FromQuery] string name)
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
                             emp => _mapper.Map<Item, ItemResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [Authorize(Roles = "Bidder,Auctioneer,Dev")]
        [HttpGet("by_user/{id}")]
        public async Task<ActionResult<IEnumerable<ItemResponse>>> GetItemByUser([FromQuery] Guid? id)
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
                             emp => _mapper.Map<Item, ItemResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [Authorize(Roles = "Bidder,Auctioneer,Dev")]
        [HttpGet("by_name_and_user")]
        public async Task<ActionResult<IEnumerable<ItemResponse>>> GetItemByNameAndUser([FromQuery] string name, Guid id)
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
                             emp => _mapper.Map<Item, ItemResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [Authorize(Roles = "Bidder,Auctioneer,Dev")]
        [HttpGet("by_category_name_and_user")]
        public async Task<ActionResult<IEnumerable<ItemResponse>>> GetItemByCategoryNameAndUser([FromQuery] string name, Guid id)
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
                             emp => _mapper.Map<Item, ItemResponse>(emp)
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
        [Authorize(Roles = "Auctioneer,Dev")]
        [HttpPut]
        public async Task<IActionResult> PutItem([FromBody] UpdateItemRequest updateItemRequest)
        {
            try
            {
                var Item = await _ItemService.UpdateItem(updateItemRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveItemUpdate", Item);
                string message = "Bạn vừa cập nhập thông tinh vật phẩm có tên là " + Item.Name + " thành công. Bạn có thể xem lại thông tin sản phẩm ở chi tiết sản phẩm.";
                var userNoti = await _common.UserNotification(10, (int)NotificationTypeEnum.Item, message, Item.UserId);
                await _notiHubContext.Clients.All.SendAsync("ReceiveNotificationAdd", userNoti.Notification);
                await _userNotiHubContext.Clients.All.SendAsync("ReceiveUserNotificationDetailAdd", userNoti.UserNotificationDetail);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Auctioneer,Dev")]
        [HttpPost]
        public async Task<ActionResult<ItemResponse>> PostItem([FromBody] CreateItemRequest createItemRequest)
        {
            try
            {
                var Item = await _ItemService.AddNewItem(createItemRequest);
                var BookingItem = await _BookingItemService.GetBookingItemByItem(Item.ElementAt(0).Id);
                await _hubContext.Clients.All.SendAsync("ReceiveItemAdd", Item);
                await _hubBookingContext.Clients.All.SendAsync("ReceiveBookingItemAdd", BookingItem.First());

                var response = Item.Select
                           (
                             emp => _mapper.Map<Item, ItemResponse>(emp)
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
        //public async Task<IActionResult> DeleteItem([FromQuery] Guid id)
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
