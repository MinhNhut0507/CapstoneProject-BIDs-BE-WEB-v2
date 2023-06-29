using AutoMapper;
using BIDs_API.SignalR;
using Business_Logic.Modules.SessionDetailModule.Interface;
using Business_Logic.Modules.SessionDetailModule.Request;
using Business_Logic.Modules.SessionDetailModule.Response;
using Data_Access.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SessionDetailsController : ControllerBase
    {
        private readonly ISessionDetailService _SessionDetailService;
        private readonly IHubContext<SessionDetailHub> _hubSessionDetailContext;
        private readonly IMapper _mapper;

        public SessionDetailsController(ISessionDetailService SessionDetailService
            , IHubContext<SessionDetailHub> hubSessionDetailContext
            , IMapper mapper)
        {
            _SessionDetailService = SessionDetailService;
            _hubSessionDetailContext = hubSessionDetailContext;
            _mapper = mapper;
        }

        // GET api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionDetailResponseStaffAndAdmin>>> GetSessionDetailsForAdmin()
        {
            try
            {
                var list = await _SessionDetailService.GetAll();
                var response = list.Select
                           (
                             emp => _mapper.Map<SessionDetail, SessionDetailResponseStaffAndAdmin>(emp)
                           );
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
        public async Task<ActionResult<SessionDetailResponseStaffAndAdmin>> GetSessionDetailByID([FromRoute] Guid? id)
        {
            var SessionDetail = _mapper.Map<SessionDetailResponseStaffAndAdmin>(await _SessionDetailService.GetSessionDetailByID(id));

            if (SessionDetail == null)
            {
                return NotFound();
            }

            return SessionDetail;
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_user/{id}")]
        public async Task<ActionResult<IEnumerable<SessionDetail>>> GetSessionDetailByUser([FromRoute] Guid? id)
        {
            try
            {
                var SessionDetail = _mapper.Map<SessionDetailResponseUser>(await _SessionDetailService.GetSessionDetailByUser(id));
                if (SessionDetail == null)
                {
                    return NotFound();
                }
                return Ok(SessionDetail);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_session/{id}")]
        public async Task<ActionResult<IEnumerable<SessionDetail>>> GetSessionDetailBySession([FromRoute] Guid? id)
        {
            try
            {
                var SessionDetail = _mapper.Map<SessionDetailResponseUser>(await _SessionDetailService.GetSessionDetailBySession(id));
                if (SessionDetail == null)
                {
                    return NotFound();
                }
                return Ok(SessionDetail);
            }
            catch
            {
                return BadRequest();
            }
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
        [HttpPost]
        public async Task<ActionResult<SessionDetailResponseStaffAndAdmin>> PostSessionDetail([FromBody] CreateSessionDetailRequest createSessionDetailRequest)
        {
            try
            {
                var SessionDetail = await _SessionDetailService.AddNewSessionDetail(createSessionDetailRequest);
                await _hubSessionDetailContext.Clients.All.SendAsync("ReceiveSessionDetailAdd", SessionDetail);
                return Ok(_mapper.Map<SessionDetailResponseStaffAndAdmin>(SessionDetail));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSessionDetail([FromRoute] Guid? id)
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
