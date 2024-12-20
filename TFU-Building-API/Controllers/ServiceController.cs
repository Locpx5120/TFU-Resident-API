﻿using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TFU_Building_API.Configure;
using TFU_Building_API.Dto;
using TFU_Building_API.Service;

namespace TFU_Building_API.Controllers
{
    [ApiController]
    [Route("api/apartment-services")]
    [CustomFilter]
    public class ApartmentServiceController : ControllerBase
    {
        private readonly IServiceApartment _apartmentService;
        private readonly IService _service;

        public ApartmentServiceController(
            IServiceApartment apartmentService, 
            IService service)
        {
            _apartmentService = apartmentService;
            _service = service;
        }

        [HttpGet("get-services")]
        public async Task<IActionResult> GetServices()
        {
            var response = await _service.GetServices();

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }


        [HttpGet("summary")]
        public async Task<IActionResult> GetApartmentServiceSummary(int pageSize = 10, [FromQuery] int pageNumber = 1)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Lấy userId từ token
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            {
                return Unauthorized("User ID not found in token.");
            }

            var response = await _apartmentService.GetApartmentServiceSummaryByUserId(userGuid, pageSize, pageNumber);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("details")]
        public async Task<IActionResult> GetServiceDetailsByApartmentId(ServiceDetailRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _apartmentService.GetServiceDetailsByApartmentId(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        //[HttpGet("unpaid-summary")]
        //public async Task<IActionResult> GetUnpaidServiceSummary([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Lấy userId từ token
        //    if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
        //    {
        //        return Unauthorized("User ID not found in token.");
        //    }

        //    var response = await _apartmentService.GetUnpaidServiceSummaryByUserId(userGuid, pageSize, pageNumber);
        //    if (!response.Success)
        //    {
        //        return BadRequest(response.Message);
        //    }

        //    return Ok(response.Data);
        //}

        /// <summary>
        /// Get a summary of apartment services for the user with filters and pagination.
        /// </summary>
        /// <param name="request">Service summary request with filters and pagination.</param>
        /// <returns>Paginated response with apartment service summaries.</returns>
        /// <summary>
        /// Get a summary of apartment services for the authenticated user with filters and pagination.
        /// </summary>
        /// <param name="request">Service summary request with filters and pagination.</param>
        /// <returns>Paginated response with apartment service summaries.</returns>
        [HttpGet("payment-summary")]
        public async Task<IActionResult> GetServiceSummary([FromQuery] ServiceSummaryRequestDto request)
        {
            // Retrieve UserId from token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Lấy userId từ token
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            {
                return Unauthorized("User ID not found in token.");
            }


            var response = await _apartmentService.GetServiceSummaryByUserId(userGuid, request);

            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpPost("unpaid-details")]
        public async Task<IActionResult> GetUnpaidServiceDetailsByApartmentId([FromBody] UnpaidServiceDetailRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _apartmentService.GetUnpaidServiceDetailsByApartmentId(request);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }

        /// <summary>
        /// Lấy danh sách dịch vụ theo ServiceCategoryId.
        /// </summary>
        /// <param name="serviceCategoryId">ID của danh mục dịch vụ.</param>
        /// <returns>Danh sách các dịch vụ thuộc danh mục dịch vụ.</returns>
        [HttpGet]
        [Route("GetByCategory/{serviceCategoryId}")]
        public async Task<IActionResult> GetServicesByCategoryId(Guid serviceCategoryId)
        {
            var result = await _service.GetServicesByCategoryIdAsync(serviceCategoryId);
            return StatusCode(result.Code, result);
        }
    }


}
