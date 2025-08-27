using Cors.DBO;
using Cors.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ASPNetProject.Repositories;
using System.Threading.Tasks;

namespace ASPNetProject.Controllers;


    [ApiController]
    [Route("api/projects")]
    [Authorize]

    public class ProjectsController : Controller
    {
        private readonly ProjectsRepository _projectsRepository;

        public ProjectsController(ProjectsRepository projectsRepository)
        {
            _projectsRepository = projectsRepository;
        }

        [HttpGet("get/paged")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPagedProjects([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagedResult = await _projectsRepository.GetProjectsPagedAsync(page, pageSize);
                return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving paged project data: {ex.Message}");
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProjects([FromBody] ProjectsDTO.ProjectRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request body.");
            try
            {
                var project = new ProjectsDBO.Projects()
                {
                    id = request.id,
                    title_tr = request.title_tr,
                    description_tr = request.description_tr,
                    title_en = request.title_en,
                    description_en = request.description_en,
                    image_base64 = request.image_base64,
                    href = request.href,
                    Used_skills = request.Used_skills,
                };
                var result = await _projectsRepository.UpdateProjectAsync(project);
                if (result)
                    return Ok("Home page updated successfully.");
                return BadRequest("Failed to update home page.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating data: {ex.Message}");
            }
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                var result = await _projectsRepository.DeleteProjectAsync(id);
                if (result)
                    return Ok("Project data deleted successfully.");
                return BadRequest("Failed to delete Project data.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting data: {ex.Message}");
            }
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddProject([FromBody] ProjectsDTO.ProjectRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request body.");

            try
            {
                var newProject = new ProjectsDBO.Projects()
                {
                    title_tr = request.title_tr,
                    description_tr = request.description_tr,
                    title_en = request.title_en,
                    description_en = request.description_en,
                    image_base64 = request.image_base64,
                    href = request.href,
                    Used_skills = request.Used_skills,
                };

                var result = await _projectsRepository.AddProjectAsync(newProject);
                if (result)
                    return Ok("Project data added successfully.");
                return BadRequest("Failed to add project data.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding data: {ex.Message}");
            }
        }
    }