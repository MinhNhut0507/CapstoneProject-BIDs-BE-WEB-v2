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

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public readonly IMapper _mapper;
        private readonly IHubContext<UserHub> _hubContext;

        public UsersController(IUserService userService
            , IMapper mapper
            , IHubContext<UserHub> hubContext)
        {
            _userService = userService;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        // GET api/<ValuesController>
        [Authorize(Roles = "Staff,Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseStaffAndAdmin>>> GetUsersForAdmin()
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
                             emp => _mapper.Map<Users, UserResponseStaffAndAdmin>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>
        [Authorize(Roles = "Staff,Admin")]
        [HttpGet("get-active")]
        public async Task<ActionResult<IEnumerable<UserResponseStaffAndAdmin>>> GetUsersActive()
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
                             emp => _mapper.Map<Users, UserResponseStaffAndAdmin>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>
        [Authorize(Roles = "Staff,Admin")]
        [HttpGet("get-waitting")]
        public async Task<ActionResult<IEnumerable<UserResponseStaffAndAdmin>>> GetUsersWaitting()
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
                             emp => _mapper.Map<Users, UserResponseStaffAndAdmin>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>
        [Authorize(Roles = "Staff,Admin")]
        [HttpGet("get-ban")]
        public async Task<ActionResult<IEnumerable<UserResponseStaffAndAdmin>>> GetUsersBan()
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
                             emp => _mapper.Map<Users, UserResponseStaffAndAdmin>(emp)
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
        public async Task<ActionResult<UserResponseUser>> GetUserByID([FromRoute] Guid id)
        {
            var user = _mapper.Map<UserResponseUser>(await _userService.GetUserByID(id));

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // GET api/<ValuesController>/abc
        [Authorize(Roles = "Staff,Admin")]
        [HttpGet("by_name/{name}")]
        public async Task<ActionResult<UserResponseStaffAndAdmin>> GetUserByName([FromRoute] string name)
        {
            var user = _mapper.Map<UserResponseStaffAndAdmin>(await _userService.GetUserByName(name));

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // GET api/<ValuesController>/abc
        [Authorize(Roles = "Staff,Admin")]
        [HttpGet("by_email/{email}")]
        public async Task<ActionResult<UserResponseStaffAndAdmin>> GetUserByEmail([FromRoute] string email)
        {
            var user = _mapper.Map<UserResponseStaffAndAdmin>(await _userService.GetUserByEmail(email));

            if (User == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Bidder,Auctioneer")]
        [HttpPut]
        public async Task<IActionResult> PutUser([FromBody] UpdateUserRequest updateUserRequest)
        {
            try
            {
                var user = await _userService.UpdateUser(updateUserRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveUserUpdate", user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Bidder,Auctioneer")]
        [HttpPut("update_role_account/{id}")]
        public async Task<IActionResult> PutRoleUser([FromRoute] Guid id)
        {
            try
            {
                var user = await _userService.UpdateRoleAccount(id);
                await _hubContext.Clients.All.SendAsync("ReceiveUserUpdate", user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Bidder,Auctioneer")]
        [HttpPut("update_password/{id}")]
        public async Task<IActionResult> PutPasswordUser([FromBody] UpdatePasswordRequest updatePasswordRequest)
        {
            try
            {
                var user = await _userService.UpdatePassword(updatePasswordRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveUserUpdate", user);
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
        public async Task<ActionResult<UserResponseUser>> PostUser([FromBody] CreateUserRequest createUserRequest)
        {
            try
            {
                var user = await _userService.AddNewUser(createUserRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveUserAdd", user);
                return Ok(_mapper.Map<UserResponseUser>(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //// DELETE api/<ValuesController>/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        //{
        //    try
        //    {
        //        await _userService.DeleteUser(id);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //private bool UserExists(Guid id)
        //{
        //    return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        //}
    }
}
