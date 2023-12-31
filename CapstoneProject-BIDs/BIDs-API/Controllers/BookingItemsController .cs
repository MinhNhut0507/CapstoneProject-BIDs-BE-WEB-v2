﻿using AutoMapper;
using BIDs_API.SignalR;
using Business_Logic.Modules.BookingItemModule.Interface;
using Business_Logic.Modules.BookingItemModule.Request;
using Business_Logic.Modules.BookingItemModule.Response;
using Business_Logic.Modules.CommonModule.Interface;
using Business_Logic.Modules.ItemModule.Interface;
using Business_Logic.Modules.ItemModule.Request;
using Data_Access.Entities;
using Data_Access.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingItemsController : ControllerBase
    {
        private readonly IBookingItemService _BookingItemService;
        private readonly IItemService _ItemService;
        private readonly ICommon _Common;
        public readonly IMapper _mapper;
        private readonly IHubContext<BookingItemHub> _hubContext;
        private readonly IHubContext<ItemHub> _itemHubContext;
        private readonly IHubContext<UserHub> _userHubContext;
        private readonly IHubContext<NotificationHub> _notiHubContext;
        private readonly IHubContext<UserNotificationDetailHub> _userNotiHubContext;
        private readonly IHubContext<StaffNotificationDetailHub> _staffNotiHubContext;

        public BookingItemsController(IBookingItemService BookingItemService
            , IMapper mapper
            , IHubContext<BookingItemHub> hubContext
            , IHubContext<ItemHub> itemHubContext
            , IItemService ItemService
            , ICommon Common
            , IHubContext<UserHub> userHubContext
            , IHubContext<NotificationHub> notiHubContext
            , IHubContext<UserNotificationDetailHub> userNotiHubContext
            , IHubContext<StaffNotificationDetailHub> staffNotiHubContext)
        {
            _BookingItemService = BookingItemService;
            _mapper = mapper;
            _hubContext = hubContext;
            _ItemService = ItemService;
            _itemHubContext = itemHubContext;
            _Common = Common;
            _userHubContext = userHubContext;
            _notiHubContext = notiHubContext;
            _userNotiHubContext = userNotiHubContext;
            _staffNotiHubContext = staffNotiHubContext;
        }

        // GET api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingItemResponse>>> GetBookingItemsForAdmin()
        {
            try
            {
                var list = await _BookingItemService.GetAll();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<BookingItem, BookingItemResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("by_item")]
        public async Task<ActionResult<ICollection<BookingItemResponse>>> GetBookingItemByItem([FromQuery] Guid id)
        {
            try
            {
                var list = await _BookingItemService.GetBookingItemByItem(id);
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<BookingItem, BookingItemResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [Authorize(Roles = "Staff,Dev")]
        [HttpGet("by_staff")]
        public async Task<ActionResult<IEnumerable<BookingItemResponse>>> GetBookingItemByStaff([FromQuery] Guid id)
        {
            try
            {
                var list = await _BookingItemService.GetBookingItemByStaff(id);
                if (list == null)
                {
                    return NotFound();
                }

                var response = list.Select
                           (
                             emp => _mapper.Map<BookingItem, BookingItemResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [Authorize(Roles = "Staff,Dev")]
        [HttpGet("by_staff_watting")]
        public async Task<ActionResult<IEnumerable<BookingItemResponse>>> GetBookingItemByStaffIsWatting([FromQuery] string email)
        {
            try
            {
                var list = await _BookingItemService.GetBookingItemByStaffIsWatting(email);
                if (list == null)
                {
                    return NotFound();
                }

                var response = list.Select
                           (
                             emp => _mapper.Map<BookingItem, BookingItemResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [Authorize(Roles = "Staff,Dev")]
        [HttpGet("by_staff_begin_watting")]
        public async Task<ActionResult<IEnumerable<BookingItemResponse>>> GetBookingItemByStaffIsBeginWatting([FromQuery] string email)
        {
            try
            {
                var list = await _BookingItemService.GetBookingItemByStaffIsBeginWatting(email);
                if (list == null)
                {
                    return NotFound();
                }

                var response = list.Select
                           (
                             emp => _mapper.Map<BookingItem, BookingItemResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [Authorize(Roles = "User")]
        [HttpGet("by_user_watting")]
        public async Task<ActionResult<IEnumerable<BookingItemResponse>>> GetBookingItemByUserIsWatting([FromQuery] Guid id)
        {
            try
            {
                var list = await _BookingItemService.GetBookingItemByUserIsWaiting(id);
                if (list == null)
                {
                    return NotFound();
                }

                var response = list.Select
                           (
                             emp => _mapper.Map<BookingItem, BookingItemResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [Authorize(Roles = "User")]
        [HttpGet("by_user_begin_now_is_watting")]
        public async Task<ActionResult<IEnumerable<BookingItemResponse>>> GetBookingItemByUserIsBeignWatting([FromQuery] Guid id)
        {
            try
            {
                var list = await _BookingItemService.GetBookingItemByUserIsBeginWaiting(id);
                if (list == null)
                {
                    return NotFound();
                }

                var response = list.Select
                           (
                             emp => _mapper.Map<BookingItem, BookingItemResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [Authorize(Roles = "User")]
        [HttpGet("by_user_waiting_create_session")]
        public async Task<ActionResult<IEnumerable<BookingItemResponse>>> GetBookingItemByUserIsNotCreateSessionYet([FromQuery] Guid id)
        {
            try
            {
                var list = await _BookingItemService.GetBookingItemByUserIsNotCreateSession(id);
                if (list == null)
                {
                    return NotFound();
                }

                var response = list.Select
                           (
                             emp => _mapper.Map<BookingItem, BookingItemResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [Authorize(Roles = "User")]
        [HttpGet("by_user_accepted")]
        public async Task<ActionResult<IEnumerable<BookingItemResponse>>> GetBookingItemByUserIsAccepted([FromQuery] Guid id)
        {
            try
            {
                var list = await _BookingItemService.GetBookingItemByUserIsAccepted(id);
                if (list == null)
                {
                    return NotFound();
                }

                var response = list.Select
                           (
                             emp => _mapper.Map<BookingItem, BookingItemResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [Authorize(Roles = "User")]
        [HttpGet("by_user_denied")]
        public async Task<ActionResult<IEnumerable<BookingItemResponse>>> GetBookingItemByUserIsDenied([FromQuery] Guid id)
        {
            try
            {
                var list = await _BookingItemService.GetBookingItemByUserIsDenied(id);
                if (list == null)
                {
                    return NotFound();
                }

                var response = list.Select
                           (
                             emp => _mapper.Map<BookingItem, BookingItemResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [Authorize(Roles = "Staff,Dev")]
        [HttpGet("by_staff_to_create_session")]
        public async Task<ActionResult<IEnumerable<BookingItemResponse>>> GetBookingItemByStaffToCreateSession([FromQuery] string email)
        {
            try
            {
                var list = await _BookingItemService.GetBookingItemByStaffToCreateSession(email);
                if (list == null)
                {
                    return NotFound();
                }

                var response = list.Select
                           (
                             emp => _mapper.Map<BookingItem, BookingItemResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }


        // GET api/<ValuesController>/abc
        [HttpGet("by_id")]
        public async Task<ActionResult<IEnumerable<BookingItemResponse>>> GetBookingItemByID([FromQuery] Guid id)
        {
            try
            {
                var list = await _BookingItemService.GetBookingItemByID(id);
                if (list == null)
                {
                    return NotFound();
                }

                var response = list.Select
                           (
                             emp => _mapper.Map<BookingItem, BookingItemResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        //// PUT api/<ValuesController>/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut]
        //public async Task<IActionResult> PutBookingItem([FromBody] UpdateBookingItemRequest updateBookingItemRequest)
        //{
        //    try
        //    {
        //        var BookingItem = await _BookingItemService.UpdateStatusBookingItem(updateBookingItemRequest);
        //        await _hubContext.Clients.All.SendAsync("ReceiveBookingItemUpdate", BookingItem);
        //        var item = await _ItemService.GetItemByID(BookingItem.ItemId);
        //        string message = "Bạn vừa cập nhập thành công vật phẩm có tên là " + item.ElementAt(0).Name + ". Bạn có thể xem lại thông tin sản phẩm ở chi tiết sản phẩm.";
        //        var userNoti = await _Common.UserNotification(10, (int)NotificationTypeEnum.Item, message, item.ElementAt(0).UserId);
        //        await _notiHubContext.Clients.All.SendAsync("ReceiveNotificationAdd", userNoti.Notification);
        //        await _userNotiHubContext.Clients.All.SendAsync("ReceiveUserNotificationDetailAdd", userNoti.UserNotificationDetail);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookingItemResponse>> PostBookingItem([FromBody] CreateBookingItemRequest createBookingItemRequest)
        {
            try
            {
                await _BookingItemService.AddNewBookingItem(createBookingItemRequest);
                var bookingItem = await _BookingItemService.GetBookingItemByItem(createBookingItemRequest.ItemId);
                await _hubContext.Clients.All.SendAsync("ReceiveBookingItemAdd", bookingItem.First());
                var item = await _ItemService.GetItemByID(bookingItem.ElementAt(0).ItemId);
                string message = "Bạn vừa tạo mới thành công vật phẩm có tên là " + item.ElementAt(0).Name + ". Sản phẩm của bạn đang được nhân viên hệ thống xác nhận và sẽ có kết quả trong vòng 3 ngày kể từ ngày tạo. Bạn có thể xem lại thông tin sản phẩm ở chi tiết sản phẩm.";
                var userNoti = await _Common.UserNotification(10, (int)NotificationTypeEnum.Item, message, item.ElementAt(0).UserId);
                await _notiHubContext.Clients.All.SendAsync("ReceiveNotificationAdd", userNoti.Notification);
                await _userNotiHubContext.Clients.All.SendAsync("ReceiveUserNotificationDetailAdd", userNoti.UserNotificationDetail);
                string messageStaff = "Bạn có đơn đăng ký sản phẩm mới cần được duyệt. Vui lòng truy cập mục đơn đăng ký sản phẩm để duyệt đơn.";
                var staffNoti = await _Common.StaffNotification(10, (int)NotificationTypeEnum.Item, messageStaff, bookingItem.ElementAt(0).StaffId);
                await _notiHubContext.Clients.All.SendAsync("ReceiveNotificationAdd", staffNoti.Notification);
                await _staffNotiHubContext.Clients.All.SendAsync("ReceiveStaffNotificationDetailAdd", staffNoti.StaffNotificationDetail);

                return Ok(_mapper.Map<BookingItemResponse>(bookingItem));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Staff,Dev")]
        [HttpPut("accept")]
        public async Task<IActionResult> AcceptBookingItem([FromQuery] Guid id)
        {
            try
            {
                var BookingItem = await _BookingItemService.AcceptStatusBookingItem(id);
                await _hubContext.Clients.All.SendAsync("ReceiveBookingItemUpdate", BookingItem);
                var item = await _ItemService.GetItemByID(BookingItem.ItemId);
                string message = "Sản phẩm của bạn có tên là " + item.ElementAt(0).Name + " đã được chấp thuận bán đấu giá trên hệ thống. Bạn có thể theo dõi thông tin cuộc đấu giá ở phiên đấu giá của tôi.";
                var userNoti = await _Common.UserNotification(10, (int)NotificationTypeEnum.Item, message, item.ElementAt(0).UserId);
                await _notiHubContext.Clients.All.SendAsync("ReceiveNotificationAdd", userNoti.Notification);
                await _userNotiHubContext.Clients.All.SendAsync("ReceiveUserNotificationDetailAdd", userNoti.UserNotificationDetail);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Staff,Dev")]
        [HttpPut("deny")]
        public async Task<IActionResult> DenyBookingItem([FromQuery] Guid id, [FromQuery] string reason)
        {
            try
            {
                var BookingItem = await _BookingItemService.DenyStatusBookingItem(id,reason);
                await _hubContext.Clients.All.SendAsync("ReceiveBookingItemUpdate", BookingItem);
                var item = await _ItemService.GetItemByID(BookingItem.ItemId);
                string message = "Sản phẩm của bạn có tên là " + item.ElementAt(0).Name + " KHÔNG được chấp thuận bán đấu giá trên hệ thống. Nếu bạn vẫn muốn đấu giá vật phẩm đó, hãy tạo mới sản phẩm và cung cấp thông tin chính xác, cụ thể và chi tiết hơn.";
                var userNoti = await _Common.UserNotification(10, (int)NotificationTypeEnum.Item, message, item.ElementAt(0).UserId);
                await _notiHubContext.Clients.All.SendAsync("ReceiveNotificationAdd", userNoti.Notification);
                await _userNotiHubContext.Clients.All.SendAsync("ReceiveUserNotificationDetailAdd", userNoti.UserNotificationDetail);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
