using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data_Access.Entities;
using Business_Logic.Modules.StaffNotificationDetailModule.Interface;
using Business_Logic.Modules.StaffNotificationDetailModule.Request;
using BIDs_API.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using AutoMapper;
using Business_Logic.Modules.StaffNotificationDetailModule.Response;
using Business_Logic.Modules.UserModule.Response;
using Business_Logic.Modules.NotificationModule.Interface;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StaffNotificationDetailController : ControllerBase
    {
        private readonly IStaffNotificationDetailService _StaffNotificationDetailService;
        public readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IHubContext<StaffNotificationDetailHub> _hubContext;

        public StaffNotificationDetailController(IStaffNotificationDetailService StaffNotificationDetailService
            , IMapper mapper
            , IHubContext<StaffNotificationDetailHub> hubContext
            , INotificationService notificationService)
        {
            _StaffNotificationDetailService = StaffNotificationDetailService;
            _mapper = mapper;
            _hubContext = hubContext;
            _notificationService = notificationService;
        }

        // GET api/<ValuesController>
        [Authorize(Roles = "Admin,Dev")]
        [HttpGet]       
        public async Task<ActionResult<IEnumerable<StaffNotificationDetailResponse>>> GetStaffNotificationDetailsForAdmin()
        {
            try
            {
                var list = await _StaffNotificationDetailService.GetAll();
                if (list == null)
                {
                    return NotFound();
                }
                var sort = list.OrderByDescending(x => x.Notification.CreateDate);
                var response = sort.Select
                           (
                             emp => _mapper.Map<StaffNotificationDetail, StaffNotificationDetailResponse>(emp)
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
        public async Task<ActionResult<StaffNotificationDetailResponse>> GetStaffNotificationDetailByStaff([FromQuery] Guid staffId)
        {
            try
            {
                var list = await _StaffNotificationDetailService.GetStaffNotificationDetailByStaff(staffId);
                if (list == null)
                {
                    return NotFound();
                }
                var listAfterRemove = new List<StaffNotificationDetail>();
                foreach (var staffNoti in list)
                {
                    if (staffNoti.Notification.ExpireDate <= DateTime.UtcNow.AddHours(7))
                    {
                        await _StaffNotificationDetailService.Delete(staffNoti.Notification.Id, staffId);
                        await _notificationService.DeleteNotification(staffNoti.Notification.Id);
                    }
                    else
                    {
                        listAfterRemove.Add(staffNoti);
                    }
                }
                var response = listAfterRemove.Select
                           (
                             emp => _mapper.Map<StaffNotificationDetail, StaffNotificationDetailResponse>(emp)
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
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> PostStaffNotificationDetail([FromBody] CreateStaffNotificationDetailRequest createStaffNotificationDetailRequest)
        {
            try
            {
                var StaffNotificationDetail = await _StaffNotificationDetailService.AddNewStaffNotificationDetail(createStaffNotificationDetailRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveStaffNotificationDetailAdd", StaffNotificationDetail);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
