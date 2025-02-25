using Repositories;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Controllers
{
    [ApiController]
    [Route("api/skills")]
    [Authorize]

    public class SkillController : Controller
    {
        private readonly SkillsRepository _skillsRepository;

        public SkillController(SkillsRepository repository)
        {
            _skillsRepository = repository;
        }

        [HttpGet("get/all")]
        [AllowAnonymous]
        public IActionResult GetSkills()
        {
            try
            {
                var SkillData = _skillsRepository.GetSkills();
                return Ok(SkillData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpPut("update")]
        public IActionResult UpdateSkill([FromBody] RequestSkill request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request body.");
            try
            {
                var skill = new Skills
                {
                    id = request.id,
                    SkillName = request.SkillName,
                    proficiency = request.proficiency,
                };

                var result = _skillsRepository.UpdateSkill(skill);
                if (result)
                    return Ok("Skill page updated successfully.");
                return BadRequest("Failed to update Skill page.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating data: {ex.Message}");
            }
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteSkill(int id)
        {
            try
            {
                var result = _skillsRepository.DeleteSkill(id);
                if (result)
                    return Ok("Skill page data deleted successfully.");
                return BadRequest("Failed to delete Skill page data.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting data: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public IActionResult AddSkill([FromBody] RequestSkill request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request body.");
            try
            {
                var skill = new Skills
                {
                    SkillName = request.SkillName,
                    proficiency = request.proficiency,
                };
                var result = _skillsRepository.AddSkill(skill);
                if (result)
                    return Ok("Skill page data added successfully.");
                return BadRequest("Failed to add Skill page data.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding data: {ex.Message}");
            }
        }
    }

    public class RequestSkill
    {
        public int id { get; set; }
        public string SkillName { get; set; }
        public string proficiency { get; set; }
    }
}
