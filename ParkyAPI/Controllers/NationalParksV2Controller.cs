using AutoMapper;
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
    [ApiVersion("2.0")]
    //[Route("api/[controller]")]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ApiController]

    //400 badrequest may response of all method
    [ProducesResponseType(StatusCodes.Status400BadRequest)]


    public class NationalParksV2Controller : ControllerBase
    {
        private INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationalParksV2Controller(INationalParkRepository npRepo,IMapper mapper)
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
            var obj = _npRepo.GetNationalParks().FirstOrDefault();
            return Ok(_mapper.Map<NationalParkDto>(obj));
        }

    }
}
