using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/nationalpark")]
    //[Route("api/[controller]")]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ApiController]

    //400 badrequest may response of all method
    [ProducesResponseType(StatusCodes.Status400BadRequest)]


    public class NationalParksController : ControllerBase
    {
        private INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository npRepo,IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }

        //for create xml from document Active "XML Documention File"
        //in Build tab from project properties
        //and delete absolote address to just file name

        //for disable missing xmlcomment warning from other .... 
        //in Treat warning as error in Build tab from project properties
        //add warning code 1591 to specific warnings

        /// <summary>
        /// Get list of national park
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type =typeof(List<NationalParkDto>))]
        public IActionResult GetNationalParks()
        {
            var objList = _npRepo.GetNationalParks();
            var objDto = new List<NationalParkDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<NationalParkDto>(obj));
            }
            //return Ok(objList);
            return Ok(objDto);
        }


        /// <summary>
        /// Get Individual national park
        /// </summary>
        /// <param name="nationalParkId">The Id of national park</param>
        /// <returns></returns>
        [HttpGet("{nationalParkId:int}",  Name = "GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound )]
        [Authorize]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var obj = _npRepo.GetNationalPark(nationalParkId);

            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<NationalParkDto>(obj);
            
            //Automapper Do This
            //var objDto = new NationalParkDto()
            //{
            //    Created=obj.Created,
            //    Id=obj.Id,
            //    Name=obj.Name,
            //    State= obj.State
            //};

            return Ok(objDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_npRepo.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Exists!");
                return StatusCode(404, ModelState);
            }
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            var nationalparkObj = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_npRepo.CreateNationalPark(nationalparkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {nationalparkObj.Name}");
                return StatusCode(500, ModelState);
            }
            //change for automatic return add value with call GetNationalPark api
            //return Ok();
            //return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalparkObj.Id }, nationalparkObj);
            //Change for Api Version lost roue
            return CreatedAtRoute("GetNationalPark", new { Version = HttpContext.GetRequestedApiVersion().ToString(),
                            nationalParkId = nationalparkObj.Id }, nationalparkObj);
        }

        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateNationalPark(int nationalParkId,[FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null ||nationalParkId !=nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }
            var nationalparkObj = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_npRepo.UpdateNationalPark(nationalparkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {nationalparkObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_npRepo.NationalParkExists(nationalParkId))
            {
                return NotFound();
            }
            var nationalparkObj = _npRepo.GetNationalPark(nationalParkId);
            if (!_npRepo.DeleteNationalPark(nationalparkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {nationalparkObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
