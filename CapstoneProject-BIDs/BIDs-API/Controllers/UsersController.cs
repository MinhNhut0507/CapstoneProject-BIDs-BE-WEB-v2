using AutoMapper;
using BIDs_API.SignalR;
using Business_Logic.Modules.LoginModule.Request;
using Business_Logic.Modules.UserModule.Interface;
using Business_Logic.Modules.UserModule.Request;
using Business_Logic.Modules.UserModule.Response;
using Data_Access.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

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
        public async Task<ActionResult<ReturnUserController>> GetUserByID([FromRoute] Guid id)
        {
            var returnUser = await _userService.GetUserByID(id);
            if (returnUser.Users == null)
            {
                return NotFound();
            }
            var UserDTO = _mapper.Map<UserResponseStaffAndAdmin>(returnUser.Users.ElementAt(0));
            var response = new ReturnUserController();
            response.Success = true;
            response.Error = null;
            response.User = new List<UserResponseStaffAndAdmin>()
            {
                UserDTO
            };
            //response.User.Add(UserDTO);
            return response;
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_name/{name}")]
        public async Task<ActionResult<ReturnUserController>> GetUserByName([FromRoute] string name)
        {
            var returnUser = await _userService.GetUserByName(name);
            if (returnUser.Users == null)
            {
                return NotFound();
            }
            var UserDTO = _mapper.Map<UserResponseStaffAndAdmin>(returnUser.Users.ElementAt(0));
            var response = new ReturnUserController();
            response.Success = true;
            response.Error = null;
            response.User = new List<UserResponseStaffAndAdmin>()
            {
                UserDTO
            };
            //response.User.Add(UserDTO);
            return response;
        }

        // GET api/<ValuesController>/abc
        [Authorize(Roles = "Bidder,Auctioneer")]
        [HttpGet("by_email/{email}")]
        public async Task<ActionResult<ReturnUserController>> GetUserByEmail([FromRoute] string email)
        {
            var returnUser = await _userService.GetUserByEmail(email);
            if (returnUser.Users == null)
            {
                return NotFound();
            }
            var UserDTO = _mapper.Map<UserResponseStaffAndAdmin>(returnUser.Users.ElementAt(0));
            var response = new ReturnUserController();
            response.Success = true;
            response.Error = null;
            response.User = new List<UserResponseStaffAndAdmin>()
            {
                UserDTO
            };
            //response.User.Add(UserDTO);
            return response;
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Bidder,Auctioneer")]
        [HttpPut]
        public async Task<ActionResult<ReturnUserController>> PutUser([FromBody] UpdateUserRequest updateUserRequest)
        {
            try
            {
                var returnUser = await _userService.UpdateUser(updateUserRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveUserUpdate", returnUser.Users);
                var UserDTO = _mapper.Map<UserResponseStaffAndAdmin>(returnUser.Users);
                var response = new ReturnUserController();
                response.Success = true;
                response.Error = null;
                response.User = new List<UserResponseStaffAndAdmin>()
                {
                    UserDTO
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Bidder")]
        [HttpPut("update_role_account/{id}")]
        public async Task<ActionResult<ReturnUserController>> PutRoleUser([FromRoute] Guid id)
        {
            try
            {
                var returnUser = await _userService.UpdateRoleAccount(id);
                await _hubContext.Clients.All.SendAsync("ReceiveUserUpdate", returnUser.Users);
                var UserDTO = _mapper.Map<UserResponseStaffAndAdmin>(returnUser.Users);
                var response = new ReturnUserController();
                response.Success = true;
                response.Error = null;
                response.User = new List<UserResponseStaffAndAdmin>()
                {
                    UserDTO
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Bidder,Auctioneer")]
        [HttpPut("update_password/{id}")]
        public async Task<ActionResult<ReturnUserController>> PutPasswordUser([FromBody] UpdatePasswordRequest updatePasswordRequest)
        {
            try
            {
                var returnUser = await _userService.UpdatePassword(updatePasswordRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveUserUpdate", returnUser.Users);
                var UserDTO = _mapper.Map<UserResponseStaffAndAdmin>(returnUser.Users);
                var response = new ReturnUserController();
                response.Success = true;
                response.Error = null;
                response.User = new List<UserResponseStaffAndAdmin>()
                {
                    UserDTO
                };
                return Ok(response);
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
        public async Task<ActionResult<ReturnUserController>> PostUser([FromBody] CreateUserRequest createUserRequest)
        {
            try
            {
                var returnUser = await _userService.AddNewUser(createUserRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveUserAdd", returnUser.Users);
                var UserDTO = _mapper.Map<UserResponseStaffAndAdmin>(returnUser.Users);
                var response = new ReturnUserController();
                response.Success = true;
                response.Error = null;
                response.User = new List<UserResponseStaffAndAdmin>()
                {
                    UserDTO
                };
                return Ok(response);
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
