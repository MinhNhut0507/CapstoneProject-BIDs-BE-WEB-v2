using AutoMapper;
using BIDs_API.SignalR;
using Business_Logic.Modules.ItemDescriptionModule.Interface;
using Business_Logic.Modules.ItemDescriptionModule.Request;
using Business_Logic.Modules.ItemDescriptionModule.Response;
using Data_Access.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemDescriptionsController : ControllerBase
    {
        private readonly IItemDescriptionService _ItemDescriptionService;
        public readonly IMapper _mapper;
        private readonly IHubContext<ItemDescriptionHub> _hubContext;

        public ItemDescriptionsController(IItemDescriptionService ItemDescriptionService
            , IMapper mapper
            , IHubContext<ItemDescriptionHub> hubContext)
        {
            _ItemDescriptionService = ItemDescriptionService;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        // GET api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDescriptionDetailResponse>>> GetItemDescriptionsForAdmin()
        {
            try
            {
                var list = await _ItemDescriptionService.GetAll();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<ItemDescription, ItemDescriptionDetailResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("by_item/{id}")]
        public async Task<ActionResult<ItemDescriptionDetailResponse>> GetItemDescriptionByItem([FromRoute] Guid id)
        {
            var ItemDescription = _mapper.Map<ItemDescriptionDetailResponse>(await _ItemDescriptionService.GetItemDescriptionByItem(id));

            if (ItemDescription == null)
            {
                return NotFound();
            }

            return ItemDescription;
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_staff/{id}")]
        public async Task<ActionResult<ItemDescriptionDetailResponse>> GetItemDescriptionByDescription([FromRoute] Guid id)
        {
            try
            {
                var list = await _ItemDescriptionService.GetItemDescriptionByDescription(id);
                if (list == null)
                {
                    return NotFound();
                }

                var response = list.Select
                           (
                             emp => _mapper.Map<ItemDescription, ItemDescriptionDetailResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }


        // GET api/<ValuesController>/abc
        [HttpGet("by_id/{id}")]
        public async Task<ActionResult<ItemDescriptionDetailResponse>> GetItemDescriptionByID([FromRoute] Guid id)
        {
            var ItemDescription = _mapper.Map<ItemDescriptionDetailResponse>(await _ItemDescriptionService.GetItemDescriptionByID(id));

            if (ItemDescription == null)
            {
                return NotFound();
            }

            return ItemDescription;
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutItemDescription([FromBody] UpdateItemDescriptionRequest updateItemDescriptionRequest)
        {
            try
            {
                var ItemDescription = await _ItemDescriptionService.UpdateStatusItemDescription(updateItemDescriptionRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveItemDescriptionUpdate", ItemDescription);
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
        public async Task<ActionResult<ItemDescriptionDetailResponse>> PostItemDescription([FromBody] CreateItemDescriptionRequest createItemDescriptionRequest)
        {
            try
            {
                var ItemDescription = await _ItemDescriptionService.AddNewItemDescription(createItemDescriptionRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveItemDescriptionAdd", ItemDescription);

                return Ok(_mapper.Map<ItemDescriptionDetailResponse>(ItemDescription));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
