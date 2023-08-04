using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data_Access.Entities;
using Business_Logic.Modules.NotificationTypeModule.Interface;
using Business_Logic.Modules.NotificationTypeModule.Request;
using BIDs_API.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using AutoMapper;
using Business_Logic.Modules.NotificationTypeModule.Response;
using Business_Logic.Modules.UserModule.Response;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Dev")]
    public class NotificationTypeController : ControllerBase
    {
        private readonly INotificationTypeService _NotificationTypeService;
        public readonly IMapper _mapper;
        private readonly IHubContext<NotificationTypeHub> _hubContext;

        public NotificationTypeController(INotificationTypeService NotificationTypeService
            , IMapper mapper
            , IHubContext<NotificationTypeHub> hubContext)
        {
            _NotificationTypeService = NotificationTypeService;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        // GET api/<ValuesController>
        [HttpGet]       
        public async Task<ActionResult<IEnumerable<NotificationTypeResponse>>> GetNotificationTypesForAdmin()
        {
            try
            {
                var list = await _NotificationTypeService.GetAll();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<NotificationType, NotificationTypeResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutNotificationType([FromBody] UpdateNotificationTypeRequest updateNotificationTypeRequest)
        {
            try
            {
                var NotificationType = await _NotificationTypeService.UpdateNotificationType(updateNotificationTypeRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveNotificationTypeUpdate", NotificationType);
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
        public async Task<ActionResult<NotificationTypeResponse>> PostNotificationType([FromBody] CreateNotificationTypeRequest createNotificationTypeRequest)
        {
            try
            {
                var NotificationType = await _NotificationTypeService.AddNewNotificationType(createNotificationTypeRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveNotificationTypeAdd", NotificationType);
                return Ok(_mapper.Map<NotificationTypeResponse>(NotificationType));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
