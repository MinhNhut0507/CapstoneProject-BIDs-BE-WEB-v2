using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data_Access.Entities;
using Business_Logic.Modules.StaffModule.Interface;
using Business_Logic.Modules.StaffModule.Request;
using Business_Logic.Modules.StaffModule.Response;
using AutoMapper;
using BIDs_API.SignalR;
using Microsoft.AspNetCore.SignalR;
using Business_Logic.Modules.UserModule.Request;
using System.Collections;
using Microsoft.AspNetCore.Authorization;

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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffResponseAdmin>>> GetStaffsForAdmin()
        {
            try
            {
                
                var list = await _StaffService.GetAll();
                var response = list.Select
                           (
                             emp => _mapper.Map<Staff, StaffResponseAdmin>(emp)
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
        public async Task<ActionResult<StaffResponseStaff>> GetStaffByID([FromRoute] Guid id)
        {
            var Staff = _mapper.Map<StaffResponseStaff>( await _StaffService.GetStaffByID(id));

            if (Staff == null)
            {
                return NotFound();
            }

            return Staff;
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_name/{name}")]
        public async Task<ActionResult<StaffResponseStaff>> GetStaffByName([FromRoute] string name)
        {
            var Staff = _mapper.Map<StaffResponseStaff>(await _StaffService.GetStaffByName(name));

            if (Staff == null)
            {
                return NotFound();
            }

            return Staff;
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_email/{email}")]
        public async Task<ActionResult<Staff>> GetStaffByEmail([FromRoute] string email)
        {
            var Staff = await _StaffService.GetStaffByEmail(email);

            if (Staff == null)
            {
                return NotFound();
            }

            return Staff;
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StaffResponseAdmin>> PostStaff([FromBody] CreateStaffRequest createStaffRequest)
        {
            try
            {
                var staff = await _StaffService.AddNewStaff(createStaffRequest);
                await _hubStaffContext.Clients.All.SendAsync("ReceiveStaffAdd", staff);
                return Ok(_mapper.Map<StaffResponseAdmin>(staff));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ValuesController>/5
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
