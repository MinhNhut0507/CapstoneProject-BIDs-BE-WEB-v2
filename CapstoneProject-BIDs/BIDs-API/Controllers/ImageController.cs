using AutoMapper;
using BIDs_API.SignalR;
using Business_Logic.Modules.ImageModule.Interface;
using Business_Logic.Modules.ImageModule.Request;
using Business_Logic.Modules.ImageModule.Response;
using Data_Access.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _ImageService;
        public readonly IMapper _mapper;
        private readonly IHubContext<ImageHub> _hubContext;

        public ImagesController(IImageService ImageService
            , IMapper mapper
            , IHubContext<ImageHub> hubContext)
        {
            _ImageService = ImageService;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        // GET api/<ValuesController>
        [Authorize(Roles = "Staff,Admin,Dev")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageResponse>>> GetImagesForAdmin()
        {
            try
            {
                var list = await _ImageService.GetAll();
                if (list == null)
                {
                    return NotFound();
                }
                var response = list.Select
                           (
                             emp => _mapper.Map<Image, ImageResponse>(emp)
                           );
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("by_id")]
        public async Task<ActionResult<ImageResponse>> GetImageByID([FromQuery] Guid id)
        {
            var Image = _mapper.Map<ImageResponse>(await _ImageService.GetImageByID(id));

            if (Image == null)
            {
                return NotFound();
            }

            return Image;
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutImage([FromBody] UpdateImageRequest updateImageRequest)
        {
            try
            {
                var Image = await _ImageService.UpdateImage(updateImageRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveImageUpdate", Image);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[Authorize(Roles = "Auctioneer")]
        [HttpPost]
        public async Task<ActionResult<ImageResponse>> PostImage([FromBody] CreateImageRequest createImageRequest)
        {
            try
            {
                var Image = await _ImageService.AddNewImage(createImageRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveImageAdd", Image);
                return Ok(_mapper.Map<ImageResponse>(Image));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //// DELETE api/<ValuesController>/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteImage([FromQuery] Guid id)
        //{
        //    try
        //    {
        //        var Image = await _ImageService.DeleteImage(id);
        //        var BookingImage = await _BookingImageService.GetBookingImageByImage(Image.Id);
        //        await _hubContext.Clients.All.SendAsync("ReceiveImageDelete", Image);
        //        await _hubBookingContext.Clients.All.SendAsync("ReceiveBookingImageUpdate", BookingImage.First());
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
