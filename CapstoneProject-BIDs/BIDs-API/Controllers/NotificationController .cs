using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data_Access.Entities;
using Business_Logic.Modules.NotificationModule.Interface;
using Business_Logic.Modules.NotificationModule.Request;
using BIDs_API.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using AutoMapper;
using Business_Logic.Modules.NotificationModule.Response;
using Business_Logic.Modules.UserModule.Response;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _NotificationService;
        public readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationController(INotificationService NotificationService
            , IMapper mapper
            , IHubContext<NotificationHub> hubContext)
        {
            _NotificationService = NotificationService;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        // GET api/<ValuesController>
        [HttpGet]       
        public async Task<ActionResult<IEnumerable<NotificationResponseAdmin>>> GetNotificationsForAdmin()
        {
            try
            {
                var list = await _NotificationService.GetAll();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Notification, NotificationResponseAdmin>(emp)
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
        public async Task<IActionResult> PutNotification([FromBody] UpdateNotificationRequest updateNotificationRequest)
        {
            try
            {
                var Notification = await _NotificationService.UpdateNotification(updateNotificationRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveNotificationUpdate", Notification);
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
        public async Task<ActionResult<NotificationResponseAdmin>> PostNotification([FromBody] CreateNotificationRequest createNotificationRequest)
        {
            try
            {
                var Notification = await _NotificationService.AddNewNotification(createNotificationRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveNotificationAdd", Notification);
                return Ok(_mapper.Map<NotificationResponseAdmin>(Notification));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification([FromRoute] Guid id)
        {
            try
            {
                var Notification = await _NotificationService.DeleteNotification(id);
                await _hubContext.Clients.All.SendAsync("ReceiveNotificationDelete", Notification);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
