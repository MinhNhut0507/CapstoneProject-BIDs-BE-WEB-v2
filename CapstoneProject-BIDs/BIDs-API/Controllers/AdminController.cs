using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data_Access.Entities;
using Business_Logic.Modules.AdminModule.Interface;
using Business_Logic.Modules.AdminModule.Request;
using BIDs_API.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _AdminService;
        private readonly IHubContext<AdminHub> _hubAdminContext;
        public AdminController(IAdminService AdminService
            , IHubContext<AdminHub> hubAdminContext)
        {
            _AdminService = AdminService;
            _hubAdminContext = hubAdminContext;
        }

        // GET api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdminsForAdmin()
        {
            try
            {
                var response = await _AdminService.GetAll();
                if(response == null)
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
        public async Task<ActionResult<Admin>> GetAdminByID([FromRoute] Guid? id)
        {
            var Admin = await _AdminService.GetAdminByID(id);

            if (Admin == null)
            {
                return NotFound();
            }

            return Admin;
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_name/{name}")]
        public async Task<ActionResult<Admin>> GetAdminByName([FromRoute] string name)
        {
            var Admin = await _AdminService.GetAdminByName(name);

            if (Admin == null)
            {
                return NotFound();
            }

            return Admin;
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_email/{email}")]
        public async Task<ActionResult<Admin>> GetAdminByEmail([FromRoute] string email)
        {
            var Admin = await _AdminService.GetAdminByEmail(email);

            if (Admin == null)
            {
                return NotFound();
            }

            return Admin;
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutAdmin([FromBody] UpdateAdminRequest updateAdminRequest)
        {
            try
            {
                var Admin = await _AdminService.UpdateAdmin(updateAdminRequest);
                await _hubAdminContext.Clients.All.SendAsync("ReceiveAdminUpdate", Admin);
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
        public async Task<ActionResult<Admin>> PostAdmin([FromBody] CreateAdminRequest createAdminRequest)
        {
            try
            {
                var Admin = await _AdminService.AddNewAdmin(createAdminRequest);
                await _hubAdminContext.Clients.All.SendAsync("ReceiveAdminAdd", Admin);
                return Ok(Admin);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
