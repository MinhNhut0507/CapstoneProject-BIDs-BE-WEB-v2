using AutoMapper;
using BIDs_API.SignalR;
using Business_Logic.Modules.UserNotificationDetailModule.Interface;
using Business_Logic.Modules.UserNotificationDetailModule.Request;
using Business_Logic.Modules.UserNotificationDetailModule.Response;
using Data_Access.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Data;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserNotificationDetailController : ControllerBase
    {
        private readonly IUserNotificationDetailService _UserNotificationDetailService;
        public readonly IMapper _mapper;
        private readonly IHubContext<UserNotificationDetailHub> _hubContext;

        public UserNotificationDetailController(IUserNotificationDetailService UserNotificationDetailService
            , IMapper mapper
            , IHubContext<UserNotificationDetailHub> hubContext)
        {
            _UserNotificationDetailService = UserNotificationDetailService;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        // GET api/<ValuesController>
        [Authorize(Roles = "Admin,Staff,Dev")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserNotificationDetailResponse>>> GetUserNotificationDetailsForAdmin()
        {
            try
            {
                var list = await _UserNotificationDetailService.GetAll();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<UserNotificationDetail, UserNotificationDetailResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/5
        [Authorize(Roles = "Bidder,Auction,Admin,Staff,Dev")]
        [HttpGet("by_id")]
        public async Task<ActionResult<UserNotificationDetailResponse>> GetUserNotificationDetailByUser([FromQuery] Guid id)
        {
            try
            {
                var list = await _UserNotificationDetailService.GetUserNotificationDetailByUser(id);
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<UserNotificationDetail, UserNotificationDetailResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        public async Task<ActionResult> PostUserNotificationDetail([FromBody] CreateUserNotificationDetailRequest createUserNotificationDetailRequest)
        {
            try
            {
                var UserNotificationDetail = await _UserNotificationDetailService.AddNewUserNotificationDetail(createUserNotificationDetailRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveUserNotificationDetailAdd", UserNotificationDetail);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
