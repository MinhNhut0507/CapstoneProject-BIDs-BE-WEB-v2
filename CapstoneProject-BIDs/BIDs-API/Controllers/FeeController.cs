using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data_Access.Entities;
using Business_Logic.Modules.FeeModule.Interface;
using Business_Logic.Modules.FeeModule.Request;
using BIDs_API.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class FeeController : ControllerBase
    {
        private readonly IFeeService _FeeService;
        private readonly IHubContext<FeeHub> _hubFeeContext;
        public FeeController(IFeeService FeeService
            , IHubContext<FeeHub> hubFeeContext)
        {
            _FeeService = FeeService;
            _hubFeeContext = hubFeeContext;
        }

        // GET api/<ValuesController>
        [HttpGet]       
        public async Task<ActionResult<IEnumerable<Fee>>> GetFeesForAdmin()
        {
            try
            {
                var response = await _FeeService.GetAll();
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
        public async Task<ActionResult<Fee>> GetFeeByID([FromRoute] int id)
        {
            var Fee = await _FeeService.GetFeeByID(id);

            if (Fee == null)
            {
                return NotFound();
            }

            return Fee;
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_name/{name}")]
        public async Task<ActionResult<Fee>> GetFeeByName([FromRoute] string name)
        {
            var Fee = await _FeeService.GetFeeByName(name);

            if (Fee == null)
            {
                return NotFound();
            }

            return Fee;
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutFee([FromBody] UpdateFeeRequest updateFeeRequest)
        {
            try
            {
                var Fee = await _FeeService.UpdateFee(updateFeeRequest);
                await _hubFeeContext.Clients.All.SendAsync("ReceiveFeeUpdate", Fee);
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
        public async Task<ActionResult<Fee>> PostFee([FromBody] CreateFeeRequest createFeeRequest)
        {
            try
            {
                var Fee = await _FeeService.AddNewFee(createFeeRequest);
                await _hubFeeContext.Clients.All.SendAsync("ReceiveFeeAdd", Fee);
                return Ok(Fee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFee([FromRoute] int id)
        {
            try
            {
                var Fee = await _FeeService.DeleteFee(id);
                await _hubFeeContext.Clients.All.SendAsync("ReceiveFeeDelete", Fee);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
