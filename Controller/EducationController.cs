using Repositories;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using System;
using Cors.DBO;
using Cors.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Controllers
{
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
        public IActionResult GetEducationsReq()
        {
            try
            {
                var EducationData = _educationRepository.GetEducations();
                return Ok(EducationData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpPut("update")]
        public IActionResult UpdateEducationReq([FromBody] EducationDTO.Education request)
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
                var result = _educationRepository.UpdateEducation(education);
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
        public IActionResult DeleteEducation(int id)
        {
            try
            {
                var result = _educationRepository.DeleteEducation(id);
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
        public IActionResult AddEducation([FromBody] EducationDTO.Education request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request body.");
            try
            {
                var education = new EducationDBO.EducationQuery()
                {
                    EducationText = request.EducationText,
                };
                var result = _educationRepository.AddEducations(education);
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


}
