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
    [Authorize(Roles = "Admin")]
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
        public async Task<ActionResult<IEnumerable<FeeResponseAdmin>>> GetFeesForAdmin()
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
                             emp => _mapper.Map<Fee, FeeResponseAdmin>(emp)
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
        public async Task<ActionResult<FeeResponseAdmin>> GetFeeByID([FromRoute] int id)
        {
            var Fee = _mapper.Map<FeeResponseAdmin>(await _FeeService.GetFeeByID(id));

            if (Fee == null)
            {
                return NotFound();
            }

            return Fee;
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_name/{name}")]
        public async Task<ActionResult<FeeResponseStaff>> GetFeeByName([FromRoute] string name)
        {
            var Fee = _mapper.Map<FeeResponseStaff>(await _FeeService.GetFeeByName(name));

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
        [HttpPost]
        public async Task<ActionResult<FeeResponseAdmin>> PostFee([FromBody] CreateFeeRequest createFeeRequest)
        {
            try
            {
                var Fee = await _FeeService.AddNewFee(createFeeRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveFeeAdd", Fee);
                return Ok(_mapper.Map<FeeResponseAdmin>(Fee));
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
