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
using BIDs_API.PaymentPayPal.Interface;
using Business_Logic.Modules.PaymentUserModule.Interface;
using Business_Logic.Modules.StaffModule.Interface;
using Business_Logic.Modules.ItemModule.Request;

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
        private readonly IHubContext<StaffNotificationDetailHub> _staffNotiHubContext;
        private readonly IHubContext<ItemHub> _itemHubContext;
        private readonly IPayPalPayment _payPal;
        private readonly IPaymentUserService _paymentUserService;
        private readonly IStaffService _StaffService;

        public SessionsController(ISessionService SessionService
            , IHubContext<SessionHub> hubSessionContext
            , IMapper mapper
            , ICommon Common
            , IItemService ItemService
            , IHubContext<ItemHub> hubContext
            , IHubContext<NotificationHub> notiHubContext
            , IHubContext<UserNotificationDetailHub> userNotiHubContext
            , IHubContext<StaffNotificationDetailHub> staffNotiHubContext
            , IHubContext<ItemHub> itemHubContext
            , IPayPalPayment payPal
            , IPaymentUserService paymentUserService
            , IStaffService StaffService)
        {
            _SessionService = SessionService;
            _hubSessionContext = hubSessionContext;
            _mapper = mapper;
            _Common = Common;
            _notiHubContext = notiHubContext;
            _userNotiHubContext = userNotiHubContext;
            _ItemService = ItemService;
            _payPal = payPal;
            _paymentUserService = paymentUserService;
            _StaffService = StaffService;
            _staffNotiHubContext = staffNotiHubContext;
            _itemHubContext = itemHubContext;
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

        // GET api/<ValuesController>
        [HttpGet("by_item")]
        public async Task<ActionResult<IEnumerable<SessionResponse>>> GetSessionsByItem([FromQuery] Guid id)
        {
            try
            {
                var list = await _SessionService.GetSessionsByItem(id);
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
        [HttpGet("by_id")]
        public async Task<ActionResult<IEnumerable<SessionWinnerResponse>>> GetSessionByID([FromQuery] Guid? id)
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
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                var Response = new List<SessionWinnerResponse>();
                for (int i = 0; i < response.Count(); i++)
                {
                    if (response.ElementAt(i).Status == (int)SessionStatusEnum.InStage || response.ElementAt(i).Status == (int)SessionStatusEnum.NotStart)
                    {
                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = "Không có người thắng cuộc"
                        };
                        Response.Add(test);
                        break;
                    }
                    var checkFail = await _Common.CheckSessionJoining(response.ElementAt(i).SessionId);
                    if(checkFail == false)
                    {
                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = "Không có người thắng cuộc"
                        };
                        Response.Add(test);
                        break;
                    }
                    var check = await _Common.CheckSessionIncrease(response.ElementAt(i).SessionId);
                    if (check == true)
                    {

                        user = await _Common.GetUserWinning(response.ElementAt(i).SessionId);
                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);
                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                }
                return Ok(Response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_name")]
        public async Task<ActionResult<IEnumerable<SessionResponse>>> GetSessionByName([FromQuery] string name)
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
        [HttpGet("by_not_start_in_stage")]
        public async Task<ActionResult<IEnumerable<SessionResponse>>> GetSessionNotStartAndInstage()
        {
            try
            {
                var list = await _SessionService.GetSessionsIsNotStartAndInStage();
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionResponse>();
                    return Empty;
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
                        var sessionUpdate = await PutSessionStatusNotStart(request);
                        await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", sessionUpdate);
                    }
                    if (list.ElementAt(i).EndTime <= date)
                    {
                        var request = new UpdateSessionStatusRequest()
                        {
                            SessionID = list.ElementAt(i).Id
                        };
                        var sessionUpdate = await PutSessionStatusInStage(request);
                        await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", sessionUpdate);
                        list.Remove(list.ElementAt(i));
                        i--;
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
        [AllowAnonymous]
        [HttpGet("by_not_start")]
        public async Task<ActionResult<IEnumerable<SessionResponse>>> GetSessionNotStart()
        {
            try
            {
                var list = await _SessionService.GetSessionsIsNotStart();
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionResponse>();
                    return Empty;
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
                        var sessionUpdate = await PutSessionStatusNotStart(request);
                        await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", sessionUpdate);
                        list.Remove(list.ElementAt(i));
                        i--;
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
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionResponse>();
                    return Empty;
                }
                var date = DateTime.UtcNow.AddHours(7);
                for (int i = 0; i < list.Count(); i++)
                {

                    if (list.ElementAt(i).EndTime <= date)
                    {
                        var request = new UpdateSessionStatusRequest()
                        {
                            SessionID = list.ElementAt(i).Id
                        };
                        var sessionUpdate = await PutSessionStatusInStage(request);
                        await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", sessionUpdate);
                        list.Remove(list.ElementAt(i));
                        i--;
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
        [AllowAnonymous]
        [HttpGet("by_not_start_by_category")]
        public async Task<ActionResult<IEnumerable<SessionResponse>>> GetSessionNotStartByCategory([FromRoute] Guid SessionId)
        {
            try
            {
                var list = await _SessionService.GetSessionsIsNotStartByCategory(SessionId);
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionResponse>();
                    return Empty;
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
                        var sessionUpdate = await PutSessionStatusNotStart(request);
                        await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", sessionUpdate);
                        list.Remove(list.ElementAt(i));
                        i--;
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
        [HttpGet("by_in_stage_by_category")]
        public async Task<ActionResult<IEnumerable<SessionResponse>>> GetSessionInStageByCategory([FromRoute] Guid SessionId)
        {
            try
            {
                var list = await _SessionService.GetSessionsIsInStageByCategory(SessionId);
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionResponse>();
                    return Empty;
                }
                var date = DateTime.UtcNow.AddHours(7);
                for (int i = 0; i < list.Count(); i++)
                {

                    if (list.ElementAt(i).EndTime <= date)
                    {
                        var request = new UpdateSessionStatusRequest()
                        {
                            SessionID = list.ElementAt(i).Id
                        };
                        var sessionUpdate = await PutSessionStatusInStage(request);
                        await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", sessionUpdate);
                        list.Remove(list.ElementAt(i));
                        i--;
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
        [HttpGet("by_in_stage_by_auctioneer")]
        public async Task<ActionResult<IEnumerable<SessionResponse>>> GetSessionInStageByAuctioneer([FromQuery] Guid id)
        {
            try
            {
                var list = await _Common.GetSessionInStageByAuctioneer(id);
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionResponse>();
                    return Empty;
                }
                var date = DateTime.UtcNow.AddHours(7);
                for (int i = 0; i < list.Count(); i++)
                {

                    if (list.ElementAt(i).EndTime <= date)
                    {
                        var request = new UpdateSessionStatusRequest()
                        {
                            SessionID = list.ElementAt(i).Id
                        };
                        var sessionUpdate = await PutSessionStatusInStage(request);
                        await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", sessionUpdate);
                        list.Remove(list.ElementAt(i));
                        i--;
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
        [HttpGet("by_user_for_payment")]
        public async Task<ActionResult<IEnumerable<SessionResponse>>> GetSessionByUserForPayment([FromQuery] Guid id)
        {
            try
            {
                var list = await _Common.GetSessionNeedToPayByUser(id);
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionResponse>();
                    return Empty;
                }
                for (int i = 0; i < list.Count(); i++)
                {
                    var date = list.ElementAt(i).EndTime.AddDays(3);
                    if (date <= DateTime.UtcNow.AddHours(7))
                    {
                        var updateRequest = new UpdateSessionStatusRequest()
                        {
                            SessionID = list.ElementAt(i).Id
                        };
                        var sessionUpdate = await _SessionService.UpdateSessionStatusToFail(updateRequest);
                        await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", sessionUpdate);
                        list.Remove(list.ElementAt(i));
                        i--;
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
        public async Task<ActionResult<IEnumerable<SessionWinnerResponse>>> GetSessionHaventPay()
        {
            try
            {
                var list = await _SessionService.GetSessionsIsHaventPay();
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionWinnerResponse>();
                    return Empty;
                }
                for (int i = 0; i < list.Count(); i++)
                {
                    var date = list.ElementAt(i).EndTime.AddDays(3);
                    if (date <= DateTime.UtcNow.AddHours(7))
                    {
                        var updateRequest = new UpdateSessionStatusRequest()
                        {
                            SessionID = list.ElementAt(i).Id
                        };
                        var sessionUpdate = await PutSessionStatusToFail(updateRequest);
                        list.Remove(list.ElementAt(i));
                        i--;
                    }
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                var Response = new List<SessionWinnerResponse>();
                for (int i = 0; i < response.Count(); i++)
                {
                    var check = await _Common.CheckSessionIncrease(response.ElementAt(i).SessionId);
                    if (check == true)
                    {

                        user = await _Common.GetUserWinning(response.ElementAt(i).SessionId);
                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);
                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                }
                return Ok(Response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_fail")]
        public async Task<ActionResult<IEnumerable<SessionWinnerResponse>>> GetSessionFail()
        {
            try
            {
                var list = await _SessionService.GetSessionsIsFail();
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionWinnerResponse>();
                    return Empty;
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                var Response = new List<SessionWinnerResponse>();
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

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                }
                return Ok(Response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_fail_had_join")]
        public async Task<ActionResult<IEnumerable<SessionWinnerResponse>>> GetSessionFailHadJoin()
        {
            try
            {
                var list = await _Common.GetSessionFailHadJoin();
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionWinnerResponse>();
                    return Empty;
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                var Response = new List<SessionWinnerResponse>();
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

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                }
                return Ok(Response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_complete")]
        public async Task<ActionResult<IEnumerable<SessionWinnerResponse>>> GetSessionComplete()
        {
            try
            {
                var list = await _SessionService.GetSessionsIsComplete();
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionWinnerResponse>();
                    return Empty;
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                var Response = new List<SessionWinnerResponse>();
                for (int i = 0; i < response.Count(); i++)
                {
                    var check = await _Common.CheckSessionIncrease(response.ElementAt(i).SessionId);
                    if (check == true)
                    {
                        user = await _Common.GetUserWinning(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                }
                return Ok(Response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_complete_by_winner")]
        public async Task<ActionResult<IEnumerable<SessionWinnerResponse>>> GetSessionCompleteByWinner([FromQuery] Guid userId)
        {
            try
            {
                var list = await _Common.GetSessionCompleteByWinner(userId);
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionWinnerResponse>();
                    return Empty;
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                var Response = new List<SessionWinnerResponse>();
                for (int i = 0; i < response.Count(); i++)
                {
                    var check = await _Common.CheckSessionIncrease(response.ElementAt(i).SessionId);
                    if (check == true)
                    {
                        user = await _Common.GetUserWinning(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                }
                return Ok(Response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_received_by_auctioneer")]
        public async Task<ActionResult<IEnumerable<SessionWinnerResponse>>> GetSessionReceivedByAuctioneer([FromQuery] Guid userId)
        {
            try
            {
                var list = await _Common.GetSessionReceivedByAuctioneer(userId);
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionWinnerResponse>();
                    return Empty;
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                var Response = new List<SessionWinnerResponse>();
                for (int i = 0; i < response.Count(); i++)
                {
                    var check = await _Common.CheckSessionIncrease(response.ElementAt(i).SessionId);
                    if (check == true)
                    {
                        user = await _Common.GetUserWinning(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                }
                return Ok(Response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_complete_by_auctioneer")]
        public async Task<ActionResult<IEnumerable<SessionWinnerResponse>>> GetSessionCompleteByAuctioneer([FromQuery] Guid userId)
        {
            try
            {
                var list = await _Common.GetSessionCompleteByAuctioneer(userId);
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionWinnerResponse>();
                    return Empty;
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                var Response = new List<SessionWinnerResponse>();
                for (int i = 0; i < response.Count(); i++)
                {
                    var check = await _Common.CheckSessionIncrease(response.ElementAt(i).SessionId);
                    if (check == true)
                    {
                        user = await _Common.GetUserWinning(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                }
                return Ok(Response);
            }
            catch
            {
                return BadRequest();
            }
        }


        // GET api/<ValuesController>/abc
        [HttpGet("by_havent_tranfer_by_auctioneer")]
        public async Task<ActionResult<IEnumerable<SessionWinnerResponse>>> GetSessionHaventTranferByAuctioneer([FromQuery] Guid userId)
        {
            try
            {
                var list = await _Common.GetSessionHaventTranferByAuctioneer(userId);
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionWinnerResponse>();
                    return Empty;
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                var Response = new List<SessionWinnerResponse>();
                for (int i = 0; i < response.Count(); i++)
                {
                    var check = await _Common.CheckSessionIncrease(response.ElementAt(i).SessionId);
                    if (check == true)
                    {
                        user = await _Common.GetUserWinning(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                }
                return Ok(Response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_fail_by_auctioneer")]
        public async Task<ActionResult<IEnumerable<SessionWinnerResponse>>> GetSessionFailByAuctioneer([FromQuery] Guid userId)
        {
            try
            {
                var list = await _Common.GetSessionFailByAuctioneer(userId);
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionWinnerResponse>();
                    return Empty;
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                var Response = new List<SessionWinnerResponse>();
                for (int i = 0; i < response.Count(); i++)
                {
                    var check = await _Common.CheckSessionIncrease(response.ElementAt(i).SessionId);
                    if (check == true)
                    {
                        user = await _Common.GetUserWinning(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                }
                return Ok(Response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_error_item_by_auctioneer")]
        public async Task<ActionResult<IEnumerable<SessionWinnerResponse>>> GetSessionErrorItemByAuctioneer([FromQuery] Guid userId)
        {
            try
            {
                var list = await _Common.GetSessionErrorItemByAuctioneer(userId);
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionWinnerResponse>();
                    return Empty;
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                var Response = new List<SessionWinnerResponse>();
                for (int i = 0; i < response.Count(); i++)
                {
                    var check = await _Common.CheckSessionIncrease(response.ElementAt(i).SessionId);
                    if (check == true)
                    {
                        user = await _Common.GetUserWinning(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                }
                return Ok(Response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_received_by_winner")]
        public async Task<ActionResult<IEnumerable<SessionWinnerResponse>>> GetSessionReceivedByWinner([FromQuery] Guid userId)
        {
            try
            {
                var list = await _Common.GetSessionReceivedByWinner(userId);
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionWinnerResponse>();
                    return Empty;
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                var Response = new List<SessionWinnerResponse>();
                for (int i = 0; i < response.Count(); i++)
                {
                    var check = await _Common.CheckSessionIncrease(response.ElementAt(i).SessionId);
                    if (check == true)
                    {
                        user = await _Common.GetUserWinning(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                }
                return Ok(Response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_error_item_by_winner")]
        public async Task<ActionResult<IEnumerable<SessionWinnerResponse>>> GetSessionErrorItemByWinner([FromQuery] Guid userId)
        {
            try
            {
                var list = await _Common.GetSessionErrorItemByWinner(userId);
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionWinnerResponse>();
                    return Empty;
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                var Response = new List<SessionWinnerResponse>();
                for (int i = 0; i < response.Count(); i++)
                {
                    var check = await _Common.CheckSessionIncrease(response.ElementAt(i).SessionId);
                    if (check == true)
                    {
                        user = await _Common.GetUserWinning(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                }
                return Ok(Response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_received")]
        public async Task<ActionResult<IEnumerable<SessionWinnerResponse>>> GetSessionReceived()
        {
            try
            {
                var list = await _SessionService.GetSessionsIsReceived();
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionWinnerResponse>();
                    return Empty;
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                var Response = new List<SessionWinnerResponse>();
                for (int i = 0; i < response.Count(); i++)
                {
                    var check = await _Common.CheckSessionIncrease(response.ElementAt(i).SessionId);
                    if (check == true)
                    {
                        user = await _Common.GetUserWinning(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                }
                return Ok(Response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_error_item")]
        public async Task<ActionResult<IEnumerable<SessionWinnerResponse>>> GetSessionErrorItem()
        {
            try
            {
                var list = await _SessionService.GetSessionsIsErrorItem();
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionWinnerResponse>();
                    return Empty;
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                var Response = new List<SessionWinnerResponse>();
                for (int i = 0; i < response.Count(); i++)
                {
                    var check = await _Common.CheckSessionIncrease(response.ElementAt(i).SessionId);
                    if (check == true)
                    {
                        user = await _Common.GetUserWinning(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                }
                return Ok(Response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_complete_user")]
        public async Task<ActionResult<IEnumerable<SessionWinnerResponse>>> GetSessionCompleteByUser([FromQuery] Guid id)
        {
            try
            {
                var list = await _SessionService.GetSessionsIsCompleteByUser(id);
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionWinnerResponse>();
                    return Empty;
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                var Response = new List<SessionWinnerResponse>();
                for (int i = 0; i < response.Count(); i++)
                {
                    var check = await _Common.CheckSessionIncrease(response.ElementAt(i).SessionId);
                    if (check == true)
                    {
                        user = await _Common.GetUserWinning(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                }
                return Ok(Response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_havent_pay_user")]
        public async Task<ActionResult<IEnumerable<SessionWinnerResponse>>> GetSessionHaventPayByUser([FromQuery] Guid id)
        {
            try
            {
                var list = await _SessionService.GetSessionsIsHaventPayByUser(id);
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionWinnerResponse>();
                    return Empty;
                }

                for (int i = 0; i < list.Count(); i++)
                {
                    var date = list.ElementAt(i).EndTime.AddDays(3);
                    if (date <= DateTime.UtcNow.AddHours(7))
                    {
                        var updateRequest = new UpdateSessionStatusRequest()
                        {
                            SessionID = list.ElementAt(i).Id
                        };
                        var sessionUpdate = await PutSessionStatusToFail(updateRequest);
                        list.Remove(list.ElementAt(i));
                        i--;
                    }
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                var Response = new List<SessionWinnerResponse>();
                for (int i = 0; i < response.Count(); i++)
                {
                    var check = await _Common.CheckSessionIncrease(response.ElementAt(i).SessionId);
                    if (check == true)
                    {
                        user = await _Common.GetUserWinning(response.ElementAt(i).SessionId);

                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);
                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                }
                return Ok(Response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_fail_user")]
        public async Task<ActionResult<IEnumerable<SessionWinnerResponse>>> GetSessionFailByUser([FromQuery] Guid id)
        {
            try
            {
                var list = await _SessionService.GetSessionsIsFailByUser(id);
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionWinnerResponse>();
                    return Empty;
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                var Response = new List<SessionWinnerResponse>();
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
                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);
                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                }
                return Ok(Response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_received_user")]
        public async Task<ActionResult<IEnumerable<SessionWinnerResponse>>> GetSessionReceivedByUser([FromQuery] Guid id)
        {
            try
            {
                var list = await _SessionService.GetSessionsIsReceivedByUser(id);
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionWinnerResponse>();
                    return Empty;
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                var Response = new List<SessionWinnerResponse>();
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
                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);
                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                }
                return Ok(Response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_error_item_user")]
        public async Task<ActionResult<IEnumerable<SessionWinnerResponse>>> GetSessionErrorItemByUser([FromQuery] Guid id)
        {
            try
            {
                var list = await _SessionService.GetSessionsIsErrorItemByUser(id);
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionWinnerResponse>();
                    return Empty;
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseComplete>(emp)
                           );
                var user = new Users();
                var Response = new List<SessionWinnerResponse>();
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
                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                    else
                    {
                        user = await _Common.GetUserWinningByJoining(response.ElementAt(i).SessionId);
                        var test = new SessionWinnerResponse()
                        {
                            sessionResponseCompletes = response.ElementAt(i),
                            winner = user.Email
                        };
                        Response.Add(test);
                    }
                }
                return Ok(Response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_not_start_user")]
        public async Task<ActionResult<IEnumerable<SessionResponse>>> GetSessionNotStartByUser([FromQuery] Guid id)
        {
            try
            {
                var list = await _SessionService.GetSessionsIsNotStartByUser(id);
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionResponse>();
                    return Empty;
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
                        var sessionUpdate = await PutSessionStatusNotStart(request);
                        await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", sessionUpdate);
                        list.Remove(list.ElementAt(i));
                        i--;
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
        [HttpGet("by_in_stage_user")]
        public async Task<ActionResult<IEnumerable<SessionResponse>>> GetSessionInStageByUser([FromQuery] Guid id)
        {
            try
            {
                var list = await _SessionService.GetSessionsIsInStageByUser(id);
                if (list.Count() == 0)
                {
                    var Empty = new List<SessionResponse>();
                    return Empty;
                }
                var date = DateTime.UtcNow.AddHours(7);
                for (int i = 0; i < list.Count(); i++)
                {

                    if (list.ElementAt(i).EndTime <= date)
                    {
                        var request = new UpdateSessionStatusRequest()
                        {
                            SessionID = list.ElementAt(i).Id
                        };
                        var sessionUpdate = await PutSessionStatusInStage(request);
                        await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", sessionUpdate);
                        list.Remove(list.ElementAt(i));
                        i--;
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
        [Authorize(Roles = "Admin,Staff,Dev")]
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
                var session = await _SessionService.UpdateSessionStatusToInStage(updateSessionRequest);
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
        public async Task<ActionResult<IEnumerable<SessionWinnerResponse>>> PutSessionStatusInStage([FromBody] UpdateSessionStatusRequest updateSessionRequest)
        {
            try
            {
                var checkSession = await _Common.CheckSessionJoining(updateSessionRequest.SessionID);
                if (checkSession == true)
                {
                    var session = await _SessionService.UpdateSessionStatusToHaventTranfer(updateSessionRequest);
                    var winner = await _Common.SendEmailWinnerAuction(session);
                    await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", session);
                    var item = await _ItemService.GetItemByID(session.ItemId);
                    string message = "Phiên đấu giá vật phẩm " + item.ElementAt(0).Name + " của bạn đã kết thúc. Người thắng cuộc là tài khoản có email là " + winner.Email + " với mức giá cuối cùng được đưa ra là " + session.FinalPrice + ". Bạn có thể xem lại thông tin chi tiết ở phiên đấu giá của tôi.";
                    var userNoti = await _Common.UserNotification(10, (int)NotificationTypeEnum.Item, message, item.ElementAt(0).UserId);
                    await _notiHubContext.Clients.All.SendAsync("ReceiveNotificationAdd", userNoti.Notification);
                    await _userNotiHubContext.Clients.All.SendAsync("ReceiveUserNotificationDetailAdd", userNoti.UserNotificationDetail);
                    string messageForWinner = "Bạn đã đấu giá thành công sản phẩm " + item.ElementAt(0).Name + " với mức giá đưa ra là " + session.FinalPrice + ". Bạn hãy thanh toán trong vòng 3 ngày sau khi nhận được thông báo này. Nếu sau 3 ngày vẫn chưa thanh toán bạn sẽ bị khóa tài khoản và sẽ không được nhận lại phí đặt cọc khi tham gia đấu giá. Bạn có thể từ chối thanh toán ngay bằng cách từ chối thanh toán trong mục các phiên đấu giá thắng cuộc.";
                    var response = _mapper.Map<SessionResponseComplete>(session);
                    var Response = new List<SessionWinnerResponse>();
                    var test = new SessionWinnerResponse()
                    {
                        sessionResponseCompletes = response,
                        winner = winner.Email
                    };
                    Response.Add(test);
                    return Ok(Response);
                }
                else
                {
                    var session = await _SessionService.UpdateSessionStatusToFail(updateSessionRequest);
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
        public async Task<IActionResult> PutSessionStatusToFail([FromBody] UpdateSessionStatusRequest updateSessionRequest)
        {
            try
            {
                var session = await _SessionService.UpdateSessionStatusToFail(updateSessionRequest);
                await _Common.SendEmailFailAuction(session);
                await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", session);
                var item = await _ItemService.GetItemByID(session.ItemId);
                string message = "Phiên đấu giá vật phẩm " + item.ElementAt(0).Name + " của bạn đã thất bại do không có người tham gia trả giá hoặc người trúng đấu giá đã từ chối thanh toán sản phẩm. Bạn có thể đăng ký lại phiên đấu giá trong mục phiên đấu của tôi -> phiên đấu giá thất bại.";
                var userNoti = await _Common.UserNotification(10, (int)NotificationTypeEnum.Item, message, item.ElementAt(0).UserId);
                await _notiHubContext.Clients.All.SendAsync("ReceiveNotificationAdd", userNoti.Notification);
                await _userNotiHubContext.Clients.All.SendAsync("ReceiveUserNotificationDetailAdd", userNoti.UserNotificationDetail);
                await _payPal.PaymentStaffReturnDeposit(session.Id);
                var checkJoining = await _Common.CheckSessionJoining(session.Id);
                var winner = new Users();
                if (checkJoining == true)
                {
                    var checkIncrease = await _Common.CheckSessionIncrease(session.Id);
                    if (checkIncrease == true)
                    {
                        winner = await _Common.GetUserWinning(session.Id);
                    }
                    else
                    {
                        winner = await _Common.GetUserWinningByJoining(session.Id);
                    }
                    var banReason = "";
                    await _StaffService.BanUser(winner.Id, banReason);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("reject_payment")]
        public async Task<IActionResult> RejectPayment([FromBody] UpdateSessionStatusRequest updateSessionRequest)
        {
            try
            {
                var session = await _SessionService.UpdateSessionStatusToFail(updateSessionRequest);
                await _Common.SendEmailFailAuction(session);
                await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", session);
                var item = await _ItemService.GetItemByID(session.ItemId);
                string message = "Phiên đấu giá vật phẩm " + item.ElementAt(0).Name + " của bạn đã thất bại do người đấu giá thành công từ chối thanh toán sản phẩm. Bạn sẽ được nhận 30% phí đặt cọc nếu sản phẩm có yêu cầu đặt cọc. Bạn có thể đăng ký lại phiên đấu giá trong mục phiên đấu của tôi -> phiên đấu giá thất bại.";
                var userNoti = await _Common.UserNotification(10, (int)NotificationTypeEnum.Item, message, item.ElementAt(0).UserId);
                await _notiHubContext.Clients.All.SendAsync("ReceiveNotificationAdd", userNoti.Notification);
                await _userNotiHubContext.Clients.All.SendAsync("ReceiveUserNotificationDetailAdd", userNoti.UserNotificationDetail);
                await _payPal.PaymentStaffToUserRejectPayment(session.Id);
                await _payPal.PaymentStaffReturnDeposit(session.Id);
                var checkIncrease = await _Common.CheckSessionIncrease(session.Id);
                var winner = new Users();
                if (checkIncrease == true)
                {
                    winner = await _Common.GetUserWinning(session.Id);
                }
                else
                {
                    winner = await _Common.GetUserWinningByJoining(session.Id);
                }
                var banReason = "";
                await _StaffService.BanUser(winner.Id, banReason);
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
                var session = await _SessionService.UpdateSessionStatusToComplete(updateSessionRequest);
                await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", session);
                await _Common.SendEmailCompleteAuction(session);
                var item = await _ItemService.GetItemByID(session.ItemId);
                string message = "Phiên đấu giá vật phẩm " + item.ElementAt(0).Name + " của bạn đã thành công, người trúng giải đã thanh toán cho hệ thống. Sau khi xác nhận sản phẩm, hệ thống sẽ thanh toán cho bạn(đã trừ phí phụ thu).";
                var userNoti = await _Common.UserNotification(10, (int)NotificationTypeEnum.Item, message, item.ElementAt(0).UserId);
                await _notiHubContext.Clients.All.SendAsync("ReceiveNotificationAdd", userNoti.Notification);
                await _userNotiHubContext.Clients.All.SendAsync("ReceiveUserNotificationDetailAdd", userNoti.UserNotificationDetail);
                await _payPal.PaymentStaffReturnDeposit(session.Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPut("session_status_to_received")]
        public async Task<IActionResult> PutSessionStatusReceived([FromBody] UpdateSessionStatusRequest updateSessionRequest)
        {
            try
            {
                var session = await _SessionService.UpdateSessionStatusToReceived(updateSessionRequest);
                await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", session);
                var item = await _ItemService.GetItemByID(session.ItemId);
                string message = "Bạn đã xác nhận đã nhận sản phẩm " + item.ElementAt(0).Name + ". Cảm ơn bạn đã sử dụng hệ thống đấu giá trực tuyến BIDs.";
                var userNoti = await _Common.UserNotification(10, (int)NotificationTypeEnum.Item, message, item.ElementAt(0).UserId);
                await _notiHubContext.Clients.All.SendAsync("ReceiveNotificationAdd", userNoti.Notification);
                await _userNotiHubContext.Clients.All.SendAsync("ReceiveUserNotificationDetailAdd", userNoti.UserNotificationDetail);
                await _payPal.PaymentStaffToUserSuccessSession(session.Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPut("session_status_to_error_item")]
        public async Task<IActionResult> PutSessionStatusErrorItem([FromQuery] string reason, [FromBody] UpdateSessionStatusRequest updateSessionRequest)
        {
            try
            {
                var session = await _SessionService.UpdateSessionStatusToErrorItem(updateSessionRequest);
                await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", session);
                var item = await _ItemService.GetItemByID(session.ItemId);
                string message = "Bạn đã xác nhận đã nhận được sản phẩm LỖI " + item.ElementAt(0).Name + " với lỗi là " + reason + ". Chúng tôi sẽ liên lạc với bạn trong thời gian sớm nhất để xác nhận và làm các thủ tục hoàn trả nếu đúng sự thật. Xin vui lòng kiểm tra email và số điện thoại để không bỏ lỡ liên lạc từ hệ thống";
                var userNoti = await _Common.UserNotification(10, (int)NotificationTypeEnum.Item, message, item.ElementAt(0).UserId);
                var staff = await _StaffService.GetAll();
                string messageStaff = "Sản phẩm " + item.ElementAt(0).Name + " đã được thông báo là hàng lỗi" + " với lỗi là " + reason + " . Xin vui lòng kiểm tra xác nhận và hoàn tiền nếu cần thiết.";
                foreach (var x in staff)
                {
                    var staffNoti = await _Common.StaffNotification(10, (int)NotificationTypeEnum.Item, messageStaff, x.Id);
                    await _notiHubContext.Clients.All.SendAsync("ReceiveNotificationAdd", staffNoti.Notification);
                    await _staffNotiHubContext.Clients.All.SendAsync("ReceiveStaffNotificationDetailAdd", staffNoti.StaffNotificationDetail);
                }
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
        [HttpPost("add_session")]
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

        [Authorize(Roles = "Admin,Staff,Dev")]
        [HttpPost("add_begin_now_session")]
        public async Task<ActionResult<SessionResponse>> PostBeginSession([FromBody] CreateBeginSessionRequest createSessionRequest)
        {
            try
            {
                var Session = await _SessionService.AddNewBeginSession(createSessionRequest);
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

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("re_auction")]
        public async Task<IActionResult> ReAuctionItem([FromBody] UpdateItemRequest updateItemRequest)
        {
            try
            {
                var item = await _Common.ReAuction(updateItemRequest);
                await _itemHubContext.Clients.All.SendAsync("ReceiveItemUpdate", item);
                var session = await _SessionService.GetSessionsByItem(item.Id);
                await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", session.ElementAt(0));
                string message = "Bạn vừa đăng bán lại thành công sản phẩm có tên là " + item.Name + ". Sản phẩm của bạn đã được lên sàn đấu giá với tên phiên đấu giá là " + session.ElementAt(0).Name + ", bạn có thể xem phiên đấu giá sản phẩm của bạn ở trang chủ.";
                var userNoti = await _Common.UserNotification(10, (int)NotificationTypeEnum.Session, message, item.UserId);
                await _notiHubContext.Clients.All.SendAsync("ReceiveNotificationAdd", userNoti.Notification);
                await _userNotiHubContext.Clients.All.SendAsync("ReceiveUserNotificationDetailAdd", userNoti.UserNotificationDetail);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // DELETE api/<ValuesController>/5
        [Authorize(Roles = "Admin,Staff,Dev")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSession([FromQuery] Guid? id)
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

        [HttpPut("check_and_update_order")]
        public async Task<IActionResult> CheckAndUpdateOrder([FromQuery] Guid userId)
        {
            try
            {
                var response = await _payPal.CheckAndUpdateOrderComplete(userId);
                var session = await _SessionService.GetSessionByID(response.SessionID);
                var deposit = session.ElementAt(0).Fee.DepositFee * session.ElementAt(0).Item.FirstPrice;
                var listPayment = await _paymentUserService.GetPaymentUserBySessionAndUser(response.SessionID, userId);
                //if (response.Status != "APPROVED")
                //{
                //    return Ok(response.Status);
                //}
                if (session.ElementAt(0).Status != (int)SessionStatusEnum.HaventTranferYet)
                {
                    return Ok(response.SessionID);
                }
                else
                {
                    var sortPayment = listPayment.OrderByDescending(x => x.Amount);
                    if ((session.ElementAt(0).FinalPrice - deposit) == sortPayment.ElementAt(0).Amount)
                    {
                        var updateSessionStatus = new UpdateSessionStatusRequest()
                        {
                            SessionID = session.ElementAt(0).Id
                        };
                        await PutSessionStatusComplete(updateSessionStatus);
                        await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", session.ElementAt(0));
                    }
                }
                return Ok(response.SessionID);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
