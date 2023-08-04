using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data_Access.Entities;
using Business_Logic.Modules.SessionModule.Interface;
using Business_Logic.Modules.SessionModule.Request;
using BIDs_API.SignalR;
using Microsoft.AspNetCore.SignalR;
using Business_Logic.Modules.SessionModule.Response;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Business_Logic.Modules.CommonModule.Interface;
using System.Runtime.InteropServices;
using Data_Access.Enum;
using Business_Logic.Modules.ItemModule.Interface;
using Business_Logic.Modules.ItemModule;
using System.Reflection;
using static System.Collections.Specialized.BitVector32;
using Business_Logic.Modules.UserModule.Response;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionService _SessionService;
        private readonly IHubContext<SessionHub> _hubSessionContext;
        private readonly IMapper _mapper;
        private readonly ICommon _Common;
        private readonly IItemService _ItemService;
        private readonly IHubContext<NotificationHub> _notiHubContext;
        private readonly IHubContext<UserNotificationDetailHub> _userNotiHubContext;

        public SessionsController(ISessionService SessionService
            , IHubContext<SessionHub> hubSessionContext
            , IMapper mapper
            , ICommon Common
            , IItemService ItemService
            , IHubContext<ItemHub> hubContext
            , IHubContext<NotificationHub> notiHubContext
            , IHubContext<UserNotificationDetailHub> userNotiHubContext)
        {
            _SessionService = SessionService;
            _hubSessionContext = hubSessionContext;
            _mapper = mapper;
            _Common = Common;
            _notiHubContext = notiHubContext;
            _userNotiHubContext = userNotiHubContext;
            _ItemService = ItemService;
        }

        // GET api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionResponse>>> GetSessionsForAdmin()
        {
            try
            {
                var list = await _SessionService.GetAll();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<SessionResponse>>> GetSessionByID([FromRoute] Guid? id)
        {
            try
            {
                var list = await _SessionService.GetSessionByID(id);
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_name/{name}")]
        public async Task<ActionResult<IEnumerable<SessionResponse>>> GetSessionByName([FromRoute] string name)
        {
            try
            {
                var list = await _SessionService.GetSessionByName(name);
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [AllowAnonymous]
        [HttpGet("by_not_start")]
        public async Task<ActionResult<IEnumerable<SessionResponse>>> GetSessionNotStart()
        {
            try
            {
                var list = await _SessionService.GetSessionsIsNotStart();
                if (list == null)
                {
                    return NotFound();
                }
                var date = DateTime.UtcNow.AddHours(7);
                for (int i = 0; i < list.Count(); i++)
                {
                    if (list.ElementAt(i).BeginTime <= date)
                    {
                        var request = new UpdateSessionStatusRequest()
                        {
                            SessionID = list.ElementAt(i).Id
                        };
                        await PutSessionStatusNotStart(request);
                        list.Remove(list.ElementAt(i));
                    }
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_in_stage")]
        public async Task<ActionResult<IEnumerable<SessionResponse>>> GetSessionInStage()
        {
            try
            {
                var list = await _SessionService.GetSessionsIsInStage();
                if (list == null)
                {
                    return NotFound();
                }
                for (int i = 0; i < list.Count(); i++)
                {
                    if (list.ElementAt(i).EndTime <= DateTime.UtcNow.AddHours(7))
                    {
                        var request = new UpdateSessionStatusRequest()
                        {
                            SessionID = list.ElementAt(i).Id
                        };
                        await PutSessionStatusInStage(request);
                        list.Remove(list.ElementAt(i));
                    }
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_in_stage_by_auctioneer/{id}")]
        public async Task<ActionResult<IEnumerable<SessionResponse>>> GetSessionInStageByAuctioneer([FromRoute]Guid id)
        {
            try
            {
                var list = await _Common.GetSessionInStageByAuctioneer(id);
                if (list == null)
                {
                    return NotFound();
                }
                for (int i = 0; i < list.Count(); i++)
                {
                    if (list.ElementAt(i).EndTime <= DateTime.UtcNow.AddHours(7))
                    {
                        var request = new UpdateSessionStatusRequest()
                        {
                            SessionID = list.ElementAt(i).Id
                        };
                        await PutSessionStatusInStage(request);
                        list.Remove(list.ElementAt(i));
                    }
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_complete_by_auctioneer/{id}")]
        public async Task<ActionResult<IEnumerable<SessionResponse>>> GetSessionCompleteByAuctioneer([FromRoute] Guid id)
        {
            try
            {
                var list = await _Common.GetSessionCompleteByAuctioneer(id);
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_havent_tranfer_yet_by_auctioneer/{id}")]
        public async Task<ActionResult<IEnumerable<SessionResponse>>> GetSessionHaventTranferByAuctioneer([FromRoute] Guid id)
        {
            try
            {
                var list = await _Common.GetSessionHaventTranferByAuctioneer(id);
                if (list == null)
                {
                    return NotFound();
                }
                for (int i = 0; i < list.Count(); i++)
                {
                    if (list.ElementAt(i).EndTime <= DateTime.UtcNow.AddHours(79))
                    {
                        list.ElementAt(i).Status = (int)SessionStatusEnum.Fail;
                        list.Remove(list.ElementAt(i));
                    }
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_havent_pay")]
        public async Task<ActionResult<IEnumerable<SessionResponseComplete>>> GetSessionHaventPay()
        {
            try
            {
                var list = await _SessionService.GetSessionsIsHaventPay();
                if (list == null)
                {
                    return NotFound();
                }
                for (int i = 0; i < list.Count(); i++)
                {
                    if (list.ElementAt(i).EndTime <= DateTime.UtcNow.AddHours(79))
                    {
                        list.ElementAt(i).Status = (int)SessionStatusEnum.Fail;
                        list.Remove(list.ElementAt(i));
                    }
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                for (int i = 0; i < response.Count(); i++)
                {
                    var check = await _Common.CheckSessionIncrease(response.ElementAt(i).SessionId);
                    if(check == true)
                    {
                        user = await _Common.GetUserWinning(response.ElementAt(i).SessionId);
                        response.ElementAt(i).Winner = user.Email;
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);
                        response.ElementAt(i).Winner = user.Email;
                    }
                }
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_fail")]
        public async Task<ActionResult<IEnumerable<SessionResponseComplete>>> GetSessionFail()
        {
            try
            {
                var list = await _SessionService.GetSessionsIsFail();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                for (int i = 0; i < response.Count(); i++)
                {
                    var checkJoing = await _Common.CheckSessionJoining(response.ElementAt(i).SessionId);
                    if( checkJoing == false)
                    {
                        continue;
                    }
                    var check = await _Common.CheckSessionIncrease(response.ElementAt(i).SessionId);
                    if (check == true)
                    {
                        user = await _Common.GetUserWinning(response.ElementAt(i).SessionId);
                        response.ElementAt(i).Winner = user.Email;
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);
                        response.ElementAt(i).Winner = user.Email;
                    }
                }
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_complete")]
        public async Task<ActionResult<IEnumerable<SessionResponseComplete>>> GetSessionComplete()
        {
            try
            {
                var list = await _SessionService.GetSessionsIsComplete();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                for (int i = 0; i < response.Count(); i++)
                {
                    var check = await _Common.CheckSessionIncrease(response.ElementAt(i).SessionId);
                    if (check == true)
                    {
                        user = await _Common.GetUserWinning(response.ElementAt(i).SessionId);
                        response.ElementAt(i).Winner = user.Email;
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);
                        response.ElementAt(i).Winner = user.Email;
                    }
                }
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_complete_user/{id}")]
        public async Task<ActionResult<IEnumerable<SessionResponseComplete>>> GetSessionCompleteByUser([FromRoute] Guid id)
        {
            try
            {
                var list = await _SessionService.GetSessionsIsCompleteByUser(id);
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                for (int i = 0; i < response.Count(); i++)
                {
                    var check = await _Common.CheckSessionIncrease(response.ElementAt(i).SessionId);
                    if (check == true)
                    {
                        user = await _Common.GetUserWinning(response.ElementAt(i).SessionId);
                        response.ElementAt(i).Winner = user.Email;
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);
                        response.ElementAt(i).Winner = user.Email;
                    }
                }
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_havent_pay_user/{id}")]
        public async Task<ActionResult<IEnumerable<SessionResponseComplete>>> GetSessionHaventPayByUser([FromRoute] Guid id)
        {
            try
            {
                var list = await _SessionService.GetSessionsIsHaventPayByUser(id);
                if (list == null)
                {
                    return NotFound();
                }
                for (int i = 0; i < list.Count(); i++)
                {
                    if (list.ElementAt(i).EndTime <= DateTime.UtcNow.AddHours(79))
                    {
                        list.ElementAt(i).Status = (int)SessionStatusEnum.Fail;
                        list.Remove(list.ElementAt(i));
                    }
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                for (int i = 0; i < response.Count(); i++)
                {
                    var check = await _Common.CheckSessionIncrease(response.ElementAt(i).SessionId);
                    if (check == true)
                    {
                        user = await _Common.GetUserWinning(response.ElementAt(i).SessionId);
                        response.ElementAt(i).Winner = user.Email;
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);
                        response.ElementAt(i).Winner = user.Email;
                    }
                }
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_fail_user/{id}")]
        public async Task<ActionResult<IEnumerable<SessionResponseComplete>>> GetSessionFailByUser([FromRoute] Guid id)
        {
            try
            {
                var list = await _SessionService.GetSessionsIsFailByUser(id);
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                for (int i = 0; i < response.Count(); i++)
                {
                    var checkJoing = await _Common.CheckSessionJoining(response.ElementAt(i).SessionId);
                    if (checkJoing == false)
                    {
                        continue;
                    }
                    var check = await _Common.CheckSessionIncrease(response.ElementAt(i).SessionId);
                    if (check == true)
                    {
                        user = await _Common.GetUserWinning(response.ElementAt(i).SessionId);
                        response.ElementAt(i).Winner = user.Email;
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);
                        response.ElementAt(i).Winner = user.Email;
                    }
                }
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_not_start_user/{id}")]
        public async Task<ActionResult<IEnumerable<SessionResponse>>> GetSessionNotStartByUser([FromRoute] Guid id)
        {
            try
            {
                var list = await _SessionService.GetSessionsIsNotStartByUser(id);
                if (list == null)
                {
                    return NotFound();
                }
                for (int i = 0; i < list.Count(); i++)
                {
                    if (list.ElementAt(i).BeginTime <= DateTime.UtcNow.AddHours(7))
                    {
                        var request = new UpdateSessionStatusRequest()
                        {
                            SessionID = list.ElementAt(i).Id
                        };
                        await PutSessionStatusNotStart(request);
                        list.Remove(list.ElementAt(i));
                    }
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_in_stage_user/{id}")]
        public async Task<ActionResult<IEnumerable<SessionResponse>>> GetSessionInStageByUser([FromRoute] Guid id)
        {
            try
            {
                var list = await _SessionService.GetSessionsIsInStageByUser(id);
                if (list == null)
                {
                    return NotFound();
                }
                for (int i = 0; i < list.Count(); i++)
                {
                    if (list.ElementAt(i).EndTime <= DateTime.UtcNow.AddHours(7))
                    {
                        var request = new UpdateSessionStatusRequest()
                        {
                            SessionID = list.ElementAt(i).Id
                        };
                        await PutSessionStatusInStage(request);
                        list.Remove(list.ElementAt(i));
                    }
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponse>(emp)
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
        [Authorize(Roles = "Admin,Staff")]
        [HttpPut]
        public async Task<IActionResult> PutSession([FromBody] UpdateSessionRequest updateSessionRequest)
        {
            try
            {
                var session = await _SessionService.UpdateSession(updateSessionRequest);
                await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", session);
                var item = await _ItemService.GetItemByID(session.ItemId);
                string message = "Phiên đấu giá vật phẩm " + item.ElementAt(0).Name + " của bạn vừa được cập nhập thành công. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.";
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

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [AllowAnonymous]
        [HttpPut("session_status_to_in_stage")]
        public async Task<IActionResult> PutSessionStatusNotStart([FromBody] UpdateSessionStatusRequest updateSessionRequest)
        {
            try
            {
                var session = await _SessionService.UpdateSessionStatusNotStart(updateSessionRequest);
                await _Common.SendEmailBeginAuction(session);
                await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", session);
                var item = await _ItemService.GetItemByID(session.ItemId);
                string message = "Phiên đấu giá vật phẩm " + item.ElementAt(0).Name + " của bạn đã bắt đầu. Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.";
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

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [AllowAnonymous]
        [HttpPut("session_status_to_haven't_pay")]
        public async Task<ActionResult<SessionResponseComplete>> PutSessionStatusInStage([FromBody] UpdateSessionStatusRequest updateSessionRequest)
        {
            try
            {
                var checkSession = await _Common.CheckSessionJoining(updateSessionRequest.SessionID);
                if(checkSession == true)
                {
                    var session = await _SessionService.UpdateSessionStatusInStage(updateSessionRequest);
                    var winner = await _Common.SendEmailWinnerAuction(session);
                    await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", session);
                    var item = await _ItemService.GetItemByID(session.ItemId);
                    string message = "Phiên đấu giá vật phẩm " + item.ElementAt(0).Name + " của bạn đã kết thúc. Người thắng cuộc là tài khoản có email là " + winner.Email + " với mức giá cuối cùng được đưa ra là " + session.FinalPrice + ". Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.";
                    var userNoti = await _Common.UserNotification(10, (int)NotificationTypeEnum.Item, message, item.ElementAt(0).UserId);
                    await _notiHubContext.Clients.All.SendAsync("ReceiveNotificationAdd", userNoti.Notification);
                    await _userNotiHubContext.Clients.All.SendAsync("ReceiveUserNotificationDetailAdd", userNoti.UserNotificationDetail);
                    string messageForWinner = "Bạn đã đấu giá thành công sản phẩm " + item.ElementAt(0).Name + " với mức giá đưa ra là " + session.FinalPrice + ". Bạn hãy thanh toán trong vòng 3 ngày sau khi nhận được thông báo này. Nếu sau 3 ngày vẫn chưa thanh toán bạn sẽ bị khóa tài khoản và sẽ không được nhận lại phí đặt cọc khi tham gia đấu giá. Bạn có thể từ chối thanh toán ngay bằng cách từ chối thanh toán trong mục các phiên đấu giá thắng cuộc.";
                    var response = _mapper.Map<SessionResponseComplete>(session);
                    response.Winner = winner.Email;
                    return Ok(response);
                }
                else
                {
                    var session = await _SessionService.UpdateSessionStatusFail(updateSessionRequest);
                    await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", session);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [AllowAnonymous]
        [HttpPut("session_status_to_fail")]
        public async Task<IActionResult> PutSessionStatusHaventTranfer([FromBody] UpdateSessionStatusRequest updateSessionRequest)
        {
            try
            {
                var session = await _SessionService.UpdateSessionStatusFail(updateSessionRequest);
                await _Common.SendEmailFailAuction(session);
                await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", session);
                var item = await _ItemService.GetItemByID(session.ItemId);
                string message = "Phiên đấu giá vật phẩm " + item.ElementAt(0).Name + " của bạn đã thất bại do không có người tham gia trả giá hoặc người trúng đấu giá đã từ chối thanh toán sản phẩm. Bạn có thể đăng ký lại phiên đấu giá trong mục phiên đấu của tôi -> phiên đấu giá thất bại.";
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

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [AllowAnonymous]
        [HttpPut("session_status_to_complete")]
        public async Task<IActionResult> PutSessionStatusComplete([FromBody] UpdateSessionStatusRequest updateSessionRequest)
        {
            try
            {
                var session = await _SessionService.UpdateSessionStatusComplete(updateSessionRequest);
                await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", session);
                var item = await _ItemService.GetItemByID(session.ItemId);
                string message = "Phiên đấu giá vật phẩm " + item.ElementAt(0).Name + " của bạn đã thành công, người trúng giải đã thanh toán cho hệ thống. Sau khi xác nhận sản phẩm, hệ thống sẽ thanh toán cho bạn(đã trừ phí phụ thu).";
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

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin,Staff,Dev")]
        [HttpPost]
        public async Task<ActionResult<SessionResponse>> PostSession([FromBody] CreateSessionRequest createSessionRequest)
        {
            try
            {
                var Session = await _SessionService.AddNewSession(createSessionRequest);
                await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionAdd", Session);
                var item = await _ItemService.GetItemByID(Session.ItemId);
                string message = "Phiên đấu giá vật phẩm " + item.ElementAt(0).Name + " của bạn đã được tạo thành công. Bạn có thể xem thông tin chi tiết ở phiên đấu giá của tôi.";
                var userNoti = await _Common.UserNotification(10, (int)NotificationTypeEnum.Item, message, item.ElementAt(0).UserId);
                await _notiHubContext.Clients.All.SendAsync("ReceiveNotificationAdd", userNoti.Notification);
                await _userNotiHubContext.Clients.All.SendAsync("ReceiveUserNotificationDetailAdd", userNoti.UserNotificationDetail);
                return Ok(_mapper.Map<SessionResponse>(Session));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ValuesController>/5
        [Authorize(Roles = "Admin,Staff,Dev")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSession([FromRoute] Guid? id)
        {
            try
            {
                var Session = await _SessionService.DeleteSession(id);
                await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionDelete", Session);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
