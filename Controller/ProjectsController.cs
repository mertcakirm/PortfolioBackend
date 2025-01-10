
using Microsoft.AspNetCore.Mvc;
using Repositories;

namespace Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController : Controller
    {
        private readonly ProjectsRepository _projectsRepository;

        public ProjectsController(ProjectsRepository projectsRepository)
        {
            _projectsRepository = projectsRepository;
        }

        [HttpGet("get/all")]
        public IActionResult GetProjects()
        {
            try{
                var projects = _projectsRepository.GetProjects();
                return Ok(projects);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpPut("update")]
        public IActionResult UpdateProjects([FromBody] ProjectRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request body.");

            try
            {
                var project = new Projects
                {
                    id=request.id,
                    title_tr=request.title_tr,
                    description_tr=request.description_tr,
                    title_en=request.title_en,
                    description_en=request.description_en,
                    image_base64=request.image_base64,
                    href=request.href,
                    Used_skills=request.Used_skills,
                };
                var result=_projectsRepository.UpdateProject(project);
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
        public IActionResult DeleteProject(int id)
        {
            try
            {
                var result = _projectsRepository.DeleteProject(id);
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
        public IActionResult AddProject([FromBody] ProjectRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request body.");

            try
            {
                var newProject = new Projects
                {
                    title_tr=request.title_tr,
                    description_tr=request.description_tr,
                    title_en=request.title_en,
                    description_en=request.description_en,
                    image_base64=request.image_base64,
                    href=request.href,
                    Used_skills=request.Used_skills,
                };

                var result = _projectsRepository.AddProjects(newProject);
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



        public class ProjectRequest
    {
        public int id { get; set; }
        public string title_tr { get; set; }
        public string description_tr { get; set; }
        public string title_en { get; set; }
        public string description_en { get; set; }
        public string image_base64 { get; set; }
        public string href { get; set; }
        public string Used_skills { get; set; }
    }
}