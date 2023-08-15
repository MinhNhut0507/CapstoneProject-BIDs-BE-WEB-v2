using Microsoft.AspNetCore.Mvc;
using Data_Access.Entities;
using Business_Logic.Modules.UserModule.Interface;
using Business_Logic.Modules.UserModule.Request;
using AutoMapper;
using Business_Logic.Modules.UserModule.Response;
using Microsoft.AspNetCore.SignalR;
using BIDs_API.SignalR;
using Microsoft.AspNetCore.Authorization;
using Business_Logic.Modules.LoginModule.Request;
using Business_Logic.Modules.CommonModule.Interface;
using Data_Access.Enum;
using Business_Logic.Modules.CommonModule.Data;
using Microsoft.Extensions.Caching.Memory;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public readonly IMapper _mapper;
        private readonly IHubContext<UserHub> _hubContext;
        private readonly IHubContext<NotificationHub> _notiHubContext;
        private readonly IHubContext<UserNotificationDetailHub> _userNotiHubContext;
        public readonly ICommon _common;
        private static readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        public UsersController(IUserService userService
            , IMapper mapper
            , IHubContext<UserHub> hubContext
            , IHubContext<NotificationHub> notiHubContext
            , IHubContext<UserNotificationDetailHub> userNotiHubContext
            , ICommon common)
        {
            _userService = userService;
            _mapper = mapper;
            _hubContext = hubContext;
            _notiHubContext = notiHubContext;
            _common = common;
            _userNotiHubContext = userNotiHubContext;
        }

        // GET api/<ValuesController>
        [Authorize(Roles = "Staff,Admin,Dev")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsersForAdmin()
        {
            try
            {
                var list = await _userService.GetAll();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Users, UserResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>
        [Authorize(Roles = "Staff,Admin,Dev")]
        [HttpGet("get_active")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsersActive()
        {
            try
            {
                var list = await _userService.GetUsersIsActive();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Users, UserResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>
        [Authorize(Roles = "Staff,Admin,Dev")]
        [HttpGet("get_join_session")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsersJoinSession([FromQuery] Guid sessionId)
        {
            try
            {
                var list = await _common.GetUserJoinSession(sessionId);
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Users, UserResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>
        [Authorize(Roles = "Staff,Admin,Dev")]
        [HttpGet("get_waitting")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsersWaitting()
        {
            try
            {
                var list = await _userService.GetUsersIsWaitting();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Users, UserResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>
        [Authorize(Roles = "Staff,Admin,Dev")]
        [HttpGet("get_ban")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsersBan()
        {
            try
            {
                var list = await _userService.GetUsersIsBan();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Users, UserResponse>(emp)
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
        public async Task<ActionResult<UserResponse>> GetUserByID([FromQuery] Guid id)
        {
            var user = _mapper.Map<UserResponse>(await _userService.GetUserByID(id));

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_name")]
        public async Task<ActionResult<UserResponse>> GetUserByName([FromQuery] string name)
        {
            var user = _mapper.Map<UserResponse>(await _userService.GetUserByName(name));

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // GET api/<ValuesController>/abc
        [Authorize(Roles = "Bidder,Auctioneer,Dev")]
        [HttpGet("by_email")]
        public async Task<ActionResult<UserResponse>> GetUserByEmail([FromQuery] string email)
        {
            var user = _mapper.Map<UserResponse>(await _userService.GetUserByEmail(email));

            if (User == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Bidder,Auctioneer,Dev")]
        [HttpPut]
        public async Task<IActionResult> PutUser([FromBody] UpdateUserRequest updateUserRequest)
        {
            try
            {
                var user = await _userService.UpdateUser(updateUserRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveUserUpdate", user);
                string message = "Tài khoản " + user.Name + " vừa cập nhập thông tin tài khoản thành công. Bạn có thể xem chi tiết ở thông tin cá nhân.";
                var userNoti = await _common.UserNotification(10, (int)NotificationTypeEnum.Account, message, user.Id);
                await _notiHubContext.Clients.All.SendAsync("ReceiveNotificationAdd", userNoti.Notification);
                await _userNotiHubContext.Clients.All.SendAsync("ReceiveUserNotificationDetailAdd", userNoti.UserNotificationDetail);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPut("confirm_email")]
        public async Task<IActionResult> ConfirmEmailUser([FromQuery] string email)
        {
            try
            {
                var codeResponse = await _common.ConfirmEmail(email);
                _cache.Set(email, codeResponse.code, TimeSpan.FromMinutes(1));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Bidder,Dev")]
        [HttpPut("update_role_user")]
        public async Task<IActionResult> PutRoleUser([FromBody] UTCCode code)
        {
            try
            {
                var codeCheck = _cache.Get(code.email).ToString();
                if(codeCheck == null)
                {
                    return BadRequest();
                };
                var check = await _common.CheckUTCCode(code.code, codeCheck);
                if(check == true)
                {
                    var user = await _userService.UpdateRoleAccount(code.email);
                    await _hubContext.Clients.All.SendAsync("ReceiveUserUpdate", user);
                    string message = "Tài khoản " + user.Name + " có email là " + user.Email + " vừa cập nhập được nâng cấp thành người bán. Từ giờ bạn có thêm chức năng đăng bán sản phẩm đấu giá.";
                    var userNoti = await _common.UserNotification(10, (int)NotificationTypeEnum.Account, message, user.Id);
                    await _notiHubContext.Clients.All.SendAsync("ReceiveNotificationAdd", userNoti.Notification);
                    await _userNotiHubContext.Clients.All.SendAsync("ReceiveUserNotificationDetailAdd", userNoti.UserNotificationDetail);
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Bidder,Auctioneer,Dev")]
        [HttpPut("update_password")]
        public async Task<IActionResult> PutPasswordUser([FromBody] UpdatePasswordRequest updatePasswordRequest)
        {
            try
            {
                var user = await _userService.UpdatePassword(updatePasswordRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveUserUpdate", user);
                string message = "Tài khoản " + user.Name + " vừa cập nhập mật khẩu thành công. Bạn có thể xem chi tiết ở thông tin cá nhân.";
                var userNoti = await _common.UserNotification(10, (int)NotificationTypeEnum.Account, message, user.Id);
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
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserResponse>> PostUser([FromBody] CreateUserRequest createUserRequest)
        {
            try
            {
                var user = await _userService.AddNewUser(createUserRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveUserAdd", user);
                return Ok(_mapper.Map<UserResponse>(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
