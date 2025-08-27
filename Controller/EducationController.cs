using ASPNetProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using Cors.DBO;
using Cors.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace ASPNetProject.Controllers;


    [ApiController]
    [Route("api/education")]
    [Authorize]
    public class EducationController : Controller
    {
        private readonly EducationRepository _educationRepository;

        public EducationController(EducationRepository repository)
        {
            _educationRepository = repository;
        }

        [HttpGet("get/all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetEducationsReq()
        {
            try
            {
                var EducationData = await _educationRepository.GetEducationsAsync();
                return Ok(EducationData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateEducationReq([FromBody] EducationDTO.Education request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request body.");

            try
            {
                var education = new EducationDBO.EducationQuery()
                {
                    id = request.id,
                    EducationText = request.EducationText,
                };

                var result = await _educationRepository.UpdateEducationAsync(education);

                if (result)
                    return Ok("Education page updated successfully.");

                return BadRequest("Failed to update Education page.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating data: {ex.Message}");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteEducation(int id)
        {
            try
            {
                var result = await _educationRepository.DeleteEducationAsync(id);

                if (result)
                    return Ok("Education page data deleted successfully.");

                return BadRequest("Failed to delete Education page data.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting data: {ex.Message}");
            }
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddEducation([FromBody] EducationDTO.Education request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request body.");

            try
            {
                var education = new EducationDBO.EducationQuery()
                {
                    EducationText = request.EducationText,
                };

                var result = await _educationRepository.AddEducationAsync(education);

                if (result)
                    return Ok("Education page data added successfully.");

                return BadRequest("Failed to add Education page data.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding data: {ex.Message}");
            }
        }
    }