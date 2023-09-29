using AutoMapper;
using BIDs_API.SignalR;
using Business_Logic.Modules.CommonModule.Interface;
using Business_Logic.Modules.SessionDetailModule.Interface;
using Business_Logic.Modules.SessionDetailModule.Request;
using Business_Logic.Modules.SessionDetailModule.Response;
using Business_Logic.Modules.SessionModule.Interface;
using Business_Logic.Modules.UserModule.Response;
using Data_Access.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Immutable;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SessionDetailsController : ControllerBase
    {
        private readonly ISessionDetailService _SessionDetailService;
        private readonly IHubContext<SessionDetailHub> _hubSessionDetailContext;
        private readonly IHubContext<SessionHub> _hubSessionContext;
        private readonly IMapper _mapper;
        private readonly ISessionService _SessionService;
        private readonly ICommon _common;

        public SessionDetailsController(ISessionDetailService SessionDetailService
            , IHubContext<SessionDetailHub> hubSessionDetailContext
            , IHubContext<SessionHub> hubSessionContext
            , IMapper mapper
            , ISessionService SessionService
            , ICommon common)
        {
            _SessionDetailService = SessionDetailService;
            _hubSessionDetailContext = hubSessionDetailContext;
            _mapper = mapper;
            _SessionService = SessionService;
            _hubSessionContext = hubSessionContext;
            _common = common;
        }

        // GET api/<ValuesController>
        [Authorize(Roles = "Staff,Admin,Dev")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionDetailResponse>>> GetSessionDetailsForAdminAndStaff()
        {
            try
            {
                var list = await _SessionDetailService.GetAll();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<SessionDetail, SessionDetailResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/5
        [Authorize(Roles = "Staff,Admin,Dev")]
        [HttpGet("by_id")]
        public async Task<ActionResult<SessionDetailResponse>> GetSessionDetailByID([FromQuery] Guid? id)
        {
            try
            {
                var list = await _SessionDetailService.GetSessionDetailByID(id);
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<SessionDetail, SessionDetailResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_user")]
        public async Task<ActionResult<IEnumerable<SessionDetailResponse>>> GetSessionDetailByUser([FromQuery] Guid? id)
        {
            try
            {
                var list = await _SessionDetailService.GetSessionDetailByUser(id);
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<SessionDetail, SessionDetailResponse>(emp)
                           );
                var sort = response.OrderByDescending(s => s.Price);
                return Ok(sort);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_session")]
        public async Task<ActionResult<IEnumerable<SessionDetailResponse>>> GetSessionDetailBySession([FromQuery] Guid? id)
        {
            try
            {
                var list = await _SessionDetailService.GetSessionDetailBySession(id);
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<SessionDetail, SessionDetailResponse>(emp)
                           );
                var responseSort = response.OrderByDescending(s => s.CreateDate);
                return Ok(responseSort);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_session_for_bidder")]
        public async Task<ActionResult<IEnumerable<SessionDetailResponse>>> GetSessionDetailBySessionForBidder( Guid? id, Guid? userId)
        {
            try
            {
                var list = await _SessionDetailService.GetSessionDetailBySessionForBidder(id, userId);
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<SessionDetail, SessionDetailResponse>(emp)
                           );
                var sort = response.OrderByDescending(s => s.Price);
                return Ok(sort);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("winner")]
        public async Task<ActionResult<Users>> GetWinner([FromQuery]Guid sessionId)
        {
            try
            {
                var checkJoin = await _common.CheckSessionJoining(sessionId);
                if (checkJoin == false)
                {
                    return Ok();
                }
                var checkIncrease = await _common.CheckSessionIncrease(sessionId);
                if (checkIncrease == false)
                {
                    var list = await _common.GetUserWinningByJoining(sessionId);
                    return Ok(_mapper.Map<UserResponse>(list));
                }
                else
                {
                    var list = await _common.GetUserWinning(sessionId);
                    return Ok(_mapper.Map<UserResponse>(list));
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Staff,Admin,Dev")]
        [HttpPut]
        public async Task<IActionResult> PutSessionDetail([FromBody] UpdateSessionDetailRequest updateSessionDetailRequest)
        {
            try
            {
                var SessionDetail = await _SessionDetailService.UpdateSessionDetail(updateSessionDetailRequest);
                await _hubSessionDetailContext.Clients.All.SendAsync("ReceiveSessionDetailUpdate", SessionDetail);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "User,Dev")]
        [HttpPost("increase_price")]
        public async Task<ActionResult<SessionDetailResponse>> IncreasePrice([FromBody] CreateSessionDetailRequest createSessionDetailRequest)
        {
            try
            {
                var SessionDetail = await _SessionDetailService.IncreasePrice(createSessionDetailRequest);
                if (SessionDetail == null)
                {
                    return Ok();
                }
                var Session = await _SessionService.GetSessionByID(SessionDetail.SessionId);
                await _hubSessionDetailContext.Clients.All.SendAsync("ReceiveSessionDetailAdd", SessionDetail);
                await _hubSessionDetailContext.Clients.All.SendAsync("ReceiveSessionUpdate", Session.ElementAt(0));
                return Ok(_mapper.Map<SessionDetailResponse>(SessionDetail));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "User,Dev")]
        [HttpPost("joinning")]
        public async Task<ActionResult<SessionDetailResponse>> Joinning([FromBody] CreateSessionDetailRequest createSessionDetailRequest)
        {
            try
            {
                var SessionDetail = await _SessionDetailService.Joinning(createSessionDetailRequest);
                if (SessionDetail == null)
                {
                    return Ok();
                }
                var Session = await _SessionService.GetSessionByID(SessionDetail.SessionId);
                await _hubSessionDetailContext.Clients.All.SendAsync("ReceiveSessionDetailAdd", SessionDetail);
                await _hubSessionDetailContext.Clients.All.SendAsync("ReceiveSessionUpdate", Session.ElementAt(0));
                return Ok(_mapper.Map<SessionDetailResponse>(SessionDetail));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "User,Dev")]
        [HttpPost("joinning_in_stage")]
        public async Task<ActionResult<SessionDetailResponse>> JoinningInStage([FromBody] CreateSessionDetailRequest createSessionDetailRequest)
        {
            try
            {
                var SessionDetail = await _SessionDetailService.JoinningInStage(createSessionDetailRequest);
                if(SessionDetail == null)
                {
                    return Ok();
                }
                var Session = await _SessionService.GetSessionByID(SessionDetail.SessionId);
                await _hubSessionDetailContext.Clients.All.SendAsync("ReceiveSessionDetailAdd", SessionDetail);
                await _hubSessionDetailContext.Clients.All.SendAsync("ReceiveSessionUpdate", Session.ElementAt(0));
                return Ok(_mapper.Map<SessionDetailResponse>(SessionDetail));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ValuesController>/5
        [Authorize(Roles = "Staff,Admin,Dev")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSessionDetail([FromQuery] Guid? id)
        {
            try
            {
                var SessionDetail = await _SessionDetailService.DeleteSessionDetail(id);
                await _hubSessionDetailContext.Clients.All.SendAsync("ReceiveSessionDetailDelete", SessionDetail);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
