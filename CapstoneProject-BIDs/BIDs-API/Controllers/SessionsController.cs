using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data_Access.Entities;
using Business_Logic.Modules.SessionModule.Interface;
using Business_Logic.Modules.SessionModule.Request;
using BIDs_API.SignalR;
using Microsoft.AspNetCore.SignalR;
using Business_Logic.Modules.SessionModule.Response;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionService _SessionService;
        private readonly IHubContext<SessionHub> _hubSessionContext;
        private readonly IMapper _mapper;

        public SessionsController(ISessionService SessionService
            , IHubContext<SessionHub> hubSessionContext
            , IMapper mapper)
        {
            _SessionService = SessionService;
            _hubSessionContext = hubSessionContext;
            _mapper = mapper;
            _ = RunTasksAtScheduledTimesForNotStart();
            _ = RunTasksAtScheduledTimesForHaventTranfer();
            _ = RunTasksAtScheduledTimesForInStage();
        }

        // GET api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionResponseStaffAndAdmin>>> GetSessionsForAdmin()
        {
            try
            {
                var list = await _SessionService.GetAll();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Session, SessionResponseStaffAndAdmin>(emp)
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
        public async Task<ActionResult<SessionResponseUser>> GetSessionByID([FromRoute] Guid? id)
        {
            var Session = _mapper.Map<SessionResponseUser>(await _SessionService.GetSessionByID(id));

            if (Session == null)
            {
                return NotFound();
            }

            return Session;
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_name/{name}")]
        public async Task<ActionResult<SessionResponseUser>> GetSessionByName([FromRoute] string name)
        {
            var Session = _mapper.Map<SessionResponseUser>(await _SessionService.GetSessionByName(name));

            if (Session == null)
            {
                return NotFound();
            }

            return Session;
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutSession([FromBody] UpdateSessionRequest updateSessionRequest)
        {
            try
            {
                var session = await _SessionService.UpdateSession(updateSessionRequest);
                await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", session);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("session_status_for_not_start")]
        public async Task<IActionResult> PutSessionStatusNotStart([FromBody] UpdateSessionStatusRequest updateSessionRequest)
        {
            try
            {
                var session = await _SessionService.UpdateSessionStatusNotStart(updateSessionRequest);
                await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", session);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("session_status_for_in_stage")]
        public async Task<IActionResult> PutSessionStatusInStage([FromBody] UpdateSessionStatusRequest updateSessionRequest)
        {
            try
            {
                var session = await _SessionService.UpdateSessionStatusInStage(updateSessionRequest);
                await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", session);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("session_status_for_havent_tranfer")]
        public async Task<IActionResult> PutSessionStatusHaventTranfer([FromBody] UpdateSessionStatusRequest updateSessionRequest)
        {
            try
            {
                var session = await _SessionService.UpdateSessionStatusHaventTranfer(updateSessionRequest);
                await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", session);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("session_status_to_complete")]
        public async Task<IActionResult> PutSessionStatusComplete([FromBody] UpdateSessionStatusRequest updateSessionRequest)
        {
            try
            {
                var session = await _SessionService.UpdateSessionStatusComplete(updateSessionRequest);
                await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionUpdate", session);
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
        public async Task<ActionResult<SessionResponseStaffAndAdmin>> PostSession([FromBody] CreateSessionRequest createSessionRequest)
        {
            try
            {
                var Session = await _SessionService.AddNewSession(createSessionRequest);
                await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionAdd", Session);
                return Ok(_mapper.Map<SessionResponseStaffAndAdmin>(Session));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSession([FromRoute] Guid? id)
        {
            try
            {
                var Session = await _SessionService.DeleteSession(id);
                await _hubSessionContext.Clients.All.SendAsync("ReceiveSessionDelete", Session);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("auto_update_status_for_not_start")]
        public async Task RunTasksAtScheduledTimesForNotStart()
        {
            var listSessionNotStart = await _SessionService.GetSessionsIsNotStart();
            var sortListSession = listSessionNotStart.OrderBy(s => s.BeginTime).ToList();
            var listTime = new List<DateTime>();
            for (int i = 0; i < sortListSession.Count; i++)
            {
                listTime.Add(sortListSession.ElementAt(i).BeginTime);
            }
            UpdateSessionStatusRequest updateSessionStatusRequest = new UpdateSessionStatusRequest();
            while (true)
            {
                DateTime now = DateTime.Now;

                for (int i = 0; i < listTime.Count; i++)
                {
                    if (now >= listTime[i])
                    {
                        // Thực hiện tác vụ tương ứng
                        updateSessionStatusRequest.SessionID = sortListSession.ElementAt(i).Id;
                        await PutSessionStatusNotStart(updateSessionStatusRequest);
                        // Xóa mốc thời gian và tác vụ đã hoàn thành
                        listTime.RemoveAt(i);
                        sortListSession.RemoveAt(i);
                        i--;
                    }
                }

                // Ngừng một khoảng thời gian trước khi kiểm tra lại
                await Task.Delay(1000);
            }
        }

        [HttpPut("auto_update_status_for_in_stage")]
        public async Task RunTasksAtScheduledTimesForInStage()
        {
            var listSessionNotStart = await _SessionService.GetSessionsIsInStage();
            var sortListSession = listSessionNotStart.OrderBy(s => s.EndTime).ToList();
            var listTime = new List<DateTime>();
            for (int i = 0; i < sortListSession.Count; i++)
            {
                listTime.Add(sortListSession.ElementAt(i).EndTime);
            }
            UpdateSessionStatusRequest updateSessionStatusRequest = new UpdateSessionStatusRequest();
            while (true)
            {
                DateTime now = DateTime.Now;

                for (int i = 0; i < listTime.Count; i++)
                {
                    if (now >= listTime[i])
                    {
                        // Thực hiện tác vụ tương ứng
                        updateSessionStatusRequest.SessionID = sortListSession.ElementAt(i).Id;
                        await PutSessionStatusInStage(updateSessionStatusRequest);
                        // Xóa mốc thời gian và tác vụ đã hoàn thành
                        listTime.RemoveAt(i);
                        sortListSession.RemoveAt(i);
                        i--;
                    }
                }

                // Ngừng một khoảng thời gian trước khi kiểm tra lại
                await Task.Delay(1000);
            }
        }

        [HttpPut("auto_update_status_for_havent_tranfer")]
        public async Task RunTasksAtScheduledTimesForHaventTranfer()
        {
            var listSessionNotStart = await _SessionService.GetSessionsIsHaventPay();
            var sortListSession = listSessionNotStart.OrderBy(s => s.EndTime).ToList();
            var listTime = new List<DateTime>();
            for (int i = 0; i < sortListSession.Count; i++)
            {
                listTime.Add((sortListSession.ElementAt(i).EndTime).AddDays(3));
            }
            UpdateSessionStatusRequest updateSessionStatusRequest = new UpdateSessionStatusRequest();
            while (true)
            {
                DateTime now = DateTime.Now;

                for (int i = 0; i < listTime.Count; i++)
                {
                    if (now >= listTime[i])
                    {
                        // Thực hiện tác vụ tương ứng
                        updateSessionStatusRequest.SessionID = sortListSession.ElementAt(i).Id;
                        await PutSessionStatusHaventTranfer(updateSessionStatusRequest);
                        // Xóa mốc thời gian và tác vụ đã hoàn thành
                        listTime.RemoveAt(i);
                        sortListSession.RemoveAt(i);
                        i--;
                    }
                }

                // Ngừng một khoảng thời gian trước khi kiểm tra lại
                await Task.Delay(1000);
            }
        }
    }
}
