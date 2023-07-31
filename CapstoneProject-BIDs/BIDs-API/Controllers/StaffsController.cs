using AutoMapper;
using BIDs_API.SignalR;
using Business_Logic.Modules.LoginModule.Request;
using Business_Logic.Modules.StaffModule.Interface;
using Business_Logic.Modules.StaffModule.Request;
using Business_Logic.Modules.StaffModule.Response;
using Data_Access.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Staff,Admin")]
    public class StaffsController : ControllerBase
    {
        private readonly IStaffService _StaffService;
        private readonly IMapper _mapper;
        private readonly IHubContext<UserHub> _hubUserContext;
        private readonly IHubContext<StaffHub> _hubStaffContext;
        public StaffsController(IStaffService StaffService
            , IMapper mapper
            , IHubContext<UserHub> hubUserContext
            , IHubContext<StaffHub> hubStaffContext)
        {
            _StaffService = StaffService;
            _mapper = mapper;
            _hubUserContext = hubUserContext;
            _hubStaffContext = hubStaffContext;
        }

        // GET api/<ValuesController>
        [Authorize(Roles = "Admin,Staff")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffResponse>>> GetStaffsForAdmin()
        {
            try
            {
                
                var list = await _StaffService.GetAll();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Staff, StaffResponse>(emp)
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
        public async Task<ActionResult<StaffResponse>> GetStaffByID([FromRoute] Guid id)
        {
            var Staff = _mapper.Map<StaffResponse>( await _StaffService.GetStaffByID(id));

            if (Staff == null)
            {
                return NotFound();
            }

            return Staff;
        }

        // GET api/<ValuesController>/abc
        [Authorize(Roles = "Admin")]
        [HttpGet("by_name/{name}")]
        public async Task<ActionResult<StaffResponse>> GetStaffByName([FromRoute] string name)
        {
            var Staff = _mapper.Map<StaffResponse>(await _StaffService.GetStaffByName(name));

            if (Staff == null)
            {
                return NotFound();
            }

            return Staff;
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_email/{email}")]
        public async Task<ActionResult<StaffResponse>> GetStaffByEmail([FromRoute] string email)
        {
            var Staff = _mapper.Map<StaffResponse>(await _StaffService.GetStaffByEmail(email));

            if (Staff == null)
            {
                return NotFound();
            }

            return Staff;
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Staff")]
        [HttpPut]
        public async Task<IActionResult> PutStaff([FromBody] UpdateStaffRequest updateStaffRequest)
        {
            try
            {
                var staff = await _StaffService.UpdateStaff(updateStaffRequest);
                await _hubStaffContext.Clients.All.SendAsync("ReceiveStaffUpdate", staff);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Staff")]
        [HttpPut("update_password")]
        public async Task<IActionResult> PutPassword([FromBody] UpdatePasswordRequest updateStaffRequest)
        {
            try
            {
                var staff = await _StaffService.UpdatePassword(updateStaffRequest);
                await _hubStaffContext.Clients.All.SendAsync("ReceiveStaffUpdate", staff);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<StaffResponse>> PostStaff([FromBody] CreateStaffRequest createStaffRequest)
        {
            try
            {
                var staff = await _StaffService.AddNewStaff(createStaffRequest);
                await _hubStaffContext.Clients.All.SendAsync("ReceiveStaffAdd", staff);
                return Ok(_mapper.Map<StaffResponse>(staff));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ValuesController>/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaff([FromRoute] Guid id)
        {
            try
            {
                var staff = await _StaffService.DeleteStaff(id);
                await _hubStaffContext.Clients.All.SendAsync("ReceiveStaffDelete", staff);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("accept_user/{AcceptID}")]
        public async Task<IActionResult> AcceptAccountCreate([FromRoute] Guid AcceptID)
        {
            try
            {
                var user = await _StaffService.AcceptCreateAccount(AcceptID);
                await _hubUserContext.Clients.All.SendAsync("ReceiveUserActive", user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("deny_user/{DenyID}")]
        public async Task<IActionResult> DenyAccountCreate([FromRoute] Guid DenyID)
        {
            try
            {
                var user = await _StaffService.DenyCreate(DenyID);
                await _hubUserContext.Clients.All.SendAsync("ReceiveUserDeny", user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=212375
        [HttpPut("ban/{BanID}")]
        public async Task<IActionResult> BanUser([FromRoute] Guid BanID)
        {
            try
            {
                var user = await _StaffService.BanUser(BanID);
                await _hubUserContext.Clients.All.SendAsync("ReceiveUserBan", user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("unban/{UnbanID}")]
        public async Task<IActionResult> UnbanUser([FromRoute] Guid UnbanID)
        {
            try
            {
                var user = await _StaffService.UnbanUser(UnbanID);
                await _hubUserContext.Clients.All.SendAsync("ReceiveUserUnban", user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //private bool StaffExists(Guid id)
        //{
        //    return (_context.Staffs?.Any(e => e.StaffId == id)).GetValueOrDefault();
        //}
    }
}
