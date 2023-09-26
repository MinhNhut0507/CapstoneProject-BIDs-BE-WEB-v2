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
using AutoMapper;
using Business_Logic.Modules.FeeModule.Response;
using Business_Logic.Modules.UserModule.Response;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FeeController : ControllerBase
    {
        private readonly IFeeService _FeeService;
        public readonly IMapper _mapper;
        private readonly IHubContext<FeeHub> _hubContext;

        public FeeController(IFeeService FeeService
            , IMapper mapper
            , IHubContext<FeeHub> hubContext)
        {
            _FeeService = FeeService;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        // GET api/<ValuesController>
        [HttpGet]       
        public async Task<ActionResult<IEnumerable<FeeResponse>>> GetFeesForAdmin()
        {
            try
            {
                var list = await _FeeService.GetAll();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Fee, FeeResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>
        [HttpGet("valid")]
        public async Task<ActionResult<IEnumerable<FeeResponse>>> GetFeesValid()
        {
            try
            {
                var list = await _FeeService.GetFeesIsValid();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Fee, FeeResponse>(emp)
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
        public async Task<ActionResult<FeeResponse>> GetFeeByID([FromQuery] int id)
        {
            var Fee = _mapper.Map<FeeResponse>(await _FeeService.GetFeeByID(id));

            if (Fee == null)
            {
                return NotFound();
            }

            return Fee;
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_name")]
        public async Task<ActionResult<FeeResponse>> GetFeeByName([FromQuery] string name)
        {
            var Fee = _mapper.Map<FeeResponse>(await _FeeService.GetFeeByName(name));

            if (Fee == null)
            {
                return NotFound();
            }

            return Fee;
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin,Dev")]
        [HttpPut]
        public async Task<IActionResult> PutFee([FromBody] UpdateFeeRequest updateFeeRequest)
        {
            try
            {
                var Fee = await _FeeService.UpdateFee(updateFeeRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveFeeUpdate", Fee);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin,Dev")]
        [HttpPost]
        public async Task<ActionResult<FeeResponse>> PostFee([FromBody] CreateFeeRequest createFeeRequest)
        {
            try
            {
                var Fee = await _FeeService.AddNewFee(createFeeRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveFeeAdd", Fee);
                return Ok(_mapper.Map<FeeResponse>(Fee));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ValuesController>/5
        [Authorize(Roles = "Admin,Dev")]
        [HttpDelete]
        public async Task<IActionResult> DeleteFee([FromQuery] int id)
        {
            try
            {
                var Fee = await _FeeService.DeleteFee(id);
                await _hubContext.Clients.All.SendAsync("ReceiveFeeDelete", Fee);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
