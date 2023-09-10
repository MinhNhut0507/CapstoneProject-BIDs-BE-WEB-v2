using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data_Access.Entities;
using Business_Logic.Modules.UserPaymentInformationModule.Interface;
using Business_Logic.Modules.UserPaymentInformationModule.Request;
using BIDs_API.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using AutoMapper;
using Business_Logic.Modules.UserPaymentInformationModule.Response;
using Business_Logic.Modules.UserModule.Response;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserPaymentInformationController : ControllerBase
    {
        private readonly IUserPaymentInformationService _UserPaymentInformationService;
        public readonly IMapper _mapper;
        private readonly IHubContext<UserPaymentInformationHub> _hubContext;

        public UserPaymentInformationController(IUserPaymentInformationService UserPaymentInformationService
            , IMapper mapper
            , IHubContext<UserPaymentInformationHub> hubContext)
        {
            _UserPaymentInformationService = UserPaymentInformationService;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        // GET api/<ValuesController>
        [HttpGet]       
        public async Task<ActionResult<IEnumerable<UserPaymentInformationResponse>>> GetUserPaymentInformationsForAdmin()
        {
            try
            {
                var list = await _UserPaymentInformationService.GetAll();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<UserPaymentInformation, UserPaymentInformationResponse>(emp)
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
        public async Task<IActionResult> PutUserPaymentInformation([FromBody] UpdateUserPaymentInformationRequest updateUserPaymentInformationRequest)
        {
            try
            {
                var UserPaymentInformation = await _UserPaymentInformationService.UpdateUserPaymentInformation(updateUserPaymentInformationRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveUserPaymentInformationUpdate", UserPaymentInformation);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserPaymentInformationResponse>> PostUserPaymentInformation([FromBody] CreateUserPaymentInformationRequest createUserPaymentInformationRequest)
        {
            try
            {
                var UserPaymentInformation = await _UserPaymentInformationService.AddNewUserPaymentInformation(createUserPaymentInformationRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveUserPaymentInformationAdd", UserPaymentInformation);
                return Ok(_mapper.Map<UserPaymentInformationResponse>(UserPaymentInformation));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
