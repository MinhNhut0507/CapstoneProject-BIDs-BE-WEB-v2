using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data_Access.Entities;
using Business_Logic.Modules.DescriptionModule.Interface;
using Business_Logic.Modules.DescriptionModule.Request;
using Microsoft.AspNetCore.Authorization;

namespace BIDs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DescriptionController : ControllerBase
    {
        private readonly IDescriptionService _DescriptionService;

        public DescriptionController(IDescriptionService DescriptionService)
        {
            _DescriptionService = DescriptionService;
        }

        // GET api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Description>>> GetDescriptionsForAdmin()
        {
            try
            {
                var response = await _DescriptionService.GetAll();
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Description>> GetDescriptionByID([FromRoute] Guid? id)
        {
            var Description = await _DescriptionService.GetDescriptionByID(id);

            if (Description == null)
            {
                return NotFound();
            }

            return Description;
        }

        // GET api/<ValuesController>/abc
        [HttpGet("by_category_name/{name}")]
        public async Task<ActionResult<Description>> GetDescriptionByCategoryName([FromRoute] string name)
        {
            var Description = await _DescriptionService.GetDescriptionByCategoryName(name);

            if (Description == null)
            {
                return NotFound();
            }

            return Description;
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutDescription([FromBody] UpdateDescriptionRequest updateDescriptionRequest)
        {
            try
            {
                await _DescriptionService.UpdateDescription(updateDescriptionRequest);
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
        public async Task<ActionResult<Description>> PostDescription([FromBody] CreateDescriptionRequest createDescriptionRequest)
        {
            try
            {
                return Ok(await _DescriptionService.AddNewDescription(createDescriptionRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDescription([FromRoute] Guid? id)
        {
            try
            {
                await _DescriptionService.DeleteDescription(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
