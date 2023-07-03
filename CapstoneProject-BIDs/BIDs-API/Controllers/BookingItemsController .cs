using AutoMapper;
using BIDs_API.SignalR;
using Business_Logic.Modules.BookingItemModule.Interface;
using Business_Logic.Modules.BookingItemModule.Request;
using Business_Logic.Modules.BookingItemModule.Response;
using Data_Access.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingItemsController : ControllerBase
    {
        private readonly IBookingItemService _BookingItemService;
        public readonly IMapper _mapper;
        private readonly IHubContext<BookingItemHub> _hubContext;

        public BookingItemsController(IBookingItemService BookingItemService
            , IMapper mapper
            , IHubContext<BookingItemHub> hubContext)
        {
            _BookingItemService = BookingItemService;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        // GET api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingItemResponseAdminAndStaff>>> GetBookingItemsForAdmin()
        {
            try
            {
                var list = await _BookingItemService.GetAll();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<BookingItem, BookingItemResponseAdminAndStaff>(emp)
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
        public async Task<ActionResult<BookingItemResponseUser>> GetBookingItemByItem([FromRoute] Guid id)
        {
            var BookingItem = _mapper.Map<BookingItemResponseUser>(await _BookingItemService.GetBookingItemByItem(id));

            if (BookingItem == null)
            {
                return NotFound();
            }

            return BookingItem;
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_staff/{id}")]
        public async Task<ActionResult<BookingItemResponseAdminAndStaff>> GetBookingItemByStaff([FromRoute] Guid id)
        {
            try
            {
                var list = await _BookingItemService.GetBookingItemByStaff(id);
                if (list == null)
                {
                    return NotFound();
                }

                var response = list.Select
                           (
                             emp => _mapper.Map<BookingItem, BookingItemResponseAdminAndStaff>(emp)
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
        public async Task<ActionResult<BookingItemResponseAdminAndStaff>> GetBookingItemByID([FromRoute] Guid id)
        {
            var BookingItem = _mapper.Map<BookingItemResponseAdminAndStaff>(await _BookingItemService.GetBookingItemByID(id));

            if (BookingItem == null)
            {
                return NotFound();
            }

            return BookingItem;
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutBookingItem([FromBody] UpdateBookingItemRequest updateBookingItemRequest)
        {
            try
            {
                var BookingItem = await _BookingItemService.UpdateStatusBookingItem(updateBookingItemRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveBookingItemUpdate", BookingItem);
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
        public async Task<ActionResult<BookingItemResponseUser>> PostBookingItem([FromBody] CreateBookingItemRequest createBookingItemRequest)
        {
            try
            {
                await _BookingItemService.AddNewBookingItem(createBookingItemRequest);
                var bookingItem = await _BookingItemService.GetBookingItemByItem(createBookingItemRequest.ItemId);
                await _hubContext.Clients.All.SendAsync("ReceiveBookingItemAdd", bookingItem.First());

                return Ok(_mapper.Map<BookingItemResponseUser>(bookingItem));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("accept{id}")]
        public async Task<IActionResult> AcceptBookingItem([FromHeader] Guid id)
        {
            try
            {
                var BookingItem = await _BookingItemService.AcceptStatusBookingItem(id);
                await _hubContext.Clients.All.SendAsync("ReceiveBookingItemUpdate", BookingItem);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("deny{id}")]
        public async Task<IActionResult> DenyBookingItem([FromHeader] Guid id)
        {
            try
            {
                var BookingItem = await _BookingItemService.DenyStatusBookingItem(id);
                await _hubContext.Clients.All.SendAsync("ReceiveBookingItemUpdate", BookingItem);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
