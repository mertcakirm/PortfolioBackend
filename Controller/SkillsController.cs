using ASPNetProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using Cors.DBO;
using Cors.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace ASPNetProject.Controllers;


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
        public async Task<IActionResult> GetSkills()
        {
            try
            {
                var SkillData = await _skillsRepository.GetSkillsAsync();
                return Ok(SkillData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateSkill([FromBody] SkillsDTO.RequestSkill request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request body.");
            try
            {
                var skill = new SkillsDBO.Skills()
                {
                    id = request.id,
                    SkillName = request.SkillName,
                    proficiency = request.proficiency,
                };

                var result = await _skillsRepository.UpdateSkillAsync(skill);
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
        public async Task<IActionResult> DeleteSkill(int id)
        {
            try
            {
                var result =await _skillsRepository.DeleteSkillAsync(id);
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
        public async Task<IActionResult> AddSkill([FromBody] SkillsDTO.RequestSkill request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request body.");
            try
            {
                var skill = new SkillsDBO.Skills()
                {
                    SkillName = request.SkillName,
                    proficiency = request.proficiency,
                };
                var result = await _skillsRepository.AddSkillAsync(skill);
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