using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TFU_Building_API.Configure;
using TFU_Building_API.Dto;
using TFU_Building_API.Service;
using TFU_Building_API.Service.impl;

namespace TFU_Building_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomFilter]
    public class ApartmentTypeController : ControllerBase
    {
        private readonly IApartmentType _apartmentType;
        
        public ApartmentTypeController(IApartmentType apartmentType)
        {
            _apartmentType = apartmentType;
        }
        [HttpGet]
        [Route("GetAllApartmentType")]
        public async Task<IActionResult> GetAllApartmentType() 
        {
            var listApartmentType = await _apartmentType.GetApartmentType();
            return Ok(listApartmentType);
        }
        [HttpPost]
        [Route("Add")]

        public async Task<IActionResult> AddApartmentType([FromBody]ApartmentTypeDto apartmentType)
        {
            var response = await _apartmentType.AddApartmentType(apartmentType);
            if(response.Success) 
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
