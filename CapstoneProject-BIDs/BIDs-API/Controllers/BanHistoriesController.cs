using AutoMapper;
using BIDs_API.SignalR;
using Business_Logic.Modules.BanHistoryModule.Interface;
using Business_Logic.Modules.BanHistoryModule.Request;
using Business_Logic.Modules.BanHistoryModule.Response;
using Data_Access.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Staff,Admin,Dev")]
    public class BanHistoriesController : ControllerBase
    {
        private readonly IBanHistoryService _BanHistoryService;
        public readonly IMapper _mapper;
        private readonly IHubContext<BanHistoryHub> _hubContext;

        public BanHistoriesController(IBanHistoryService BanHistoryService
            , IMapper mapper
            , IHubContext<BanHistoryHub> hubContext)
        {
            _BanHistoryService = BanHistoryService;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        // GET api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BanHistoryResponse>>> GetBanHistorysForAdmin()
        {
            try
            {
                var list = await _BanHistoryService.GetAll();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<BanHistory, BanHistoryResponse>(emp)
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
        public async Task<ActionResult<IEnumerable<BanHistoryResponse>>> GetBanHistoryByUserID([FromRoute] Guid? id)
        {
            try
            {
                var list = await _BanHistoryService.GetBanHistoryByUserID(id);
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<BanHistory, BanHistoryResponse>(emp)
                           );
                
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_name/{name}")]
        public async Task<ActionResult<IEnumerable<BanHistoryResponse>>> GetBanHistoryByUserName([FromRoute] string name)
        {
            try
            {
                var list = await _BanHistoryService.GetBanHistoryByUserName(name);
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<BanHistory, BanHistoryResponse>(emp)
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
        public async Task<IActionResult> PutBanHistory([FromBody] UpdateBanHistoryRequest updateBanHistoryRequest)
        {
            try
            {
                var BanHistory = await _BanHistoryService.UpdateBanHistory(updateBanHistoryRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveBanHistoryUpdate", BanHistory);
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
        public async Task<ActionResult<BanHistoryResponse>> PostBanHistory([FromBody] CreateBanHistoryRequest createBanHistoryRequest)
        {
            try
            {
                var BanHistory = await _BanHistoryService.AddNewBanHistory(createBanHistoryRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveBanHistoryAdd", BanHistory);
                return Ok(_mapper.Map<BanHistoryResponse>(BanHistory));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //// DELETE api/<ValuesController>/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteBanHistory([FromRoute] Guid? id)
        //{
        //    try
        //    {
        //        await _BanHistoryService.d(id);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
