using Cors.DBO;
using Cors.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories;

namespace Controllers
{
    [ApiController]
    [Route("api/roles")]
    [Authorize]

    public class RolesController : Controller
    {
        private readonly RoleRepository _rolesRepository;

        public RolesController(RoleRepository roleRepository)
        {
            _rolesRepository = roleRepository;
        }

        [HttpGet("get/all")]
        [AllowAnonymous]
        public IActionResult GetRoles()
        {
            try
            {
                var roles = _rolesRepository.GetRoles();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpPut("update")]
        public IActionResult UpdateRole([FromBody] RoleDTO.RoleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request body.");
            try
            {
                var role = new RoleDBO.Role
                {
                    Roleid = request.Roleid,
                    RoleName = request.RoleName,

                };
                var result = _rolesRepository.UpdateRole(role);
                if (result)
                    return Ok("Role updated successfully.");
                return BadRequest("Failed to update Role");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating data: {ex.Message}");
            }
        }


        [HttpDelete("delete/{id}")]
        public IActionResult DeleteRole(int id)
        {
            try
            {
                var result = _rolesRepository.DeleteRole(id);
                if (result)
                    return Ok("Role data deleted successfully.");
                return BadRequest("Failed to delete Role data.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting data: {ex.Message}");
            }
        }


        [HttpPost("add")]
        public IActionResult AddRole([FromBody] RoleDTO.RoleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request body.");
            try
            {
                var role = new RoleDBO.Role
                {
                    RoleName = request.RoleName,
                };
                var result = _rolesRepository.AddRole(role);
                if (result)
                    return Ok("Role data added successfully.");
                return BadRequest("Failed to add Role data.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding data: {ex.Message}");
            }
        }
    }


}