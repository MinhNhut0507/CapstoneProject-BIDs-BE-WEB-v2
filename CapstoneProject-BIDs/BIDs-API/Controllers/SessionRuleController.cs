using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data_Access.Entities;
using Business_Logic.Modules.SessionRuleModule.Interface;
using Business_Logic.Modules.SessionRuleModule.Request;
using BIDs_API.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using AutoMapper;
using Business_Logic.Modules.SessionRuleModule.Response;
using Business_Logic.Modules.UserModule.Response;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SessionRuleController : ControllerBase
    {
        private readonly ISessionRuleService _SessionRuleService;
        public readonly IMapper _mapper;
        private readonly IHubContext<SessionRuleHub> _hubContext;

        public SessionRuleController(ISessionRuleService SessionRuleService
            , IMapper mapper
            , IHubContext<SessionRuleHub> hubContext)
        {
            _SessionRuleService = SessionRuleService;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        // GET api/<ValuesController>
        [HttpGet]       
        public async Task<ActionResult<IEnumerable<SessionRuleResponseAdmin>>> GetSessionRulesForAdmin()
        {
            try
            {
                var list = await _SessionRuleService.GetAll();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<SessionRule, SessionRuleResponseAdmin>(emp)
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
        public async Task<ActionResult<ReturnSessionRuleController>> GetSessionRuleByID([FromRoute] Guid id)
        {
            var returnSessionRule = await _SessionRuleService.GetSessionRuleByID(id);
            var SessionRuleDTO = _mapper.Map<SessionRuleResponseAdmin>(returnSessionRule.SessionRule);
            if (SessionRuleDTO == null)
            {
                return NotFound();
            }
            var response = new ReturnSessionRuleController()
            {
                Success = returnSessionRule.Success,
                Error = returnSessionRule.Error,
                SessionRules = SessionRuleDTO
            };
            return response;
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_name/{name}")]
        public async Task<ActionResult<ReturnSessionRuleController>> GetSessionRuleByName([FromRoute] string name)
        {
            var returnSessionRule = await _SessionRuleService.GetSessionRuleByName(name);
            var SessionRuleDTO = _mapper.Map<SessionRuleResponseAdmin>(returnSessionRule.SessionRule);
            if (SessionRuleDTO == null)
            {
                return NotFound();
            }
            var response = new ReturnSessionRuleController()
            {
                Success = returnSessionRule.Success,
                Error = returnSessionRule.Error,
                SessionRules = SessionRuleDTO
            };
            return response;
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<ReturnSessionRuleController>> PutSessionRule([FromBody] UpdateSessionRuleRequest updateSessionRuleRequest)
        {
            try
            {
                var returnSessionRule = await _SessionRuleService.UpdateSessionRule(updateSessionRuleRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveSessionRuleUpdate", returnSessionRule.SessionRule);
                var SessionRuleDTO = _mapper.Map<SessionRuleResponseAdmin>(returnSessionRule.SessionRule);
                var response = new ReturnSessionRuleController()
                {
                    Success = returnSessionRule.Success,
                    Error = returnSessionRule.Error,
                    SessionRules = SessionRuleDTO
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ReturnSessionRuleController>> PostSessionRule([FromBody] CreateSessionRuleRequest createSessionRuleRequest)
        {
            try
            {
                var returnSessionRule = await _SessionRuleService.AddNewSessionRule(createSessionRuleRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveSessionRuleAdd", returnSessionRule.SessionRule);
                var SessionRuleDTO = _mapper.Map<SessionRuleResponseAdmin>(returnSessionRule.SessionRule);
                var response = new ReturnSessionRuleController()
                {
                    Success = returnSessionRule.Success,
                    Error = returnSessionRule.Error,
                    SessionRules = SessionRuleDTO
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ValuesController>/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ReturnSessionRuleController>> DeleteSessionRule([FromRoute] Guid id)
        {
            try
            {
                var returnSessionRule = await _SessionRuleService.DeleteSessionRule(id);
                await _hubContext.Clients.All.SendAsync("ReceiveSessionRuleDelete", returnSessionRule.SessionRule);
                var SessionRuleDTO = _mapper.Map<SessionRuleResponseAdmin>(returnSessionRule.SessionRule);
                var response = new ReturnSessionRuleController()
                {
                    Success = returnSessionRule.Success,
                    Error = returnSessionRule.Error,
                    SessionRules = SessionRuleDTO
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
