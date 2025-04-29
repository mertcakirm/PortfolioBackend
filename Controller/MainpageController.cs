using Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using Cors.DBO;
using Cors.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Controllers
{
    [ApiController]
    [Route("api/mainpage")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly MainpageRepository _homeRepository;

        public HomeController(MainpageRepository repository)
        {
            _homeRepository = repository;
        }

        [HttpGet("get/all")]
        [AllowAnonymous]
        public IActionResult GetHome()
        {
            try
            {
                var homeData = _homeRepository.GetHomeData();
                return Ok(homeData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpPut("update/contents")]
        public IActionResult UpdateMainpage([FromBody] MainPageDTO.RequestMain request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request body.");
            try
            {
                var homePage = new MainPageDBO.HomePage
                {
                    Id = request.Id,
                    header_tr = request.header_tr,
                    description_tr = request.description_tr,
                    header_en = request.header_en,
                    description_en = request.description_en,
                };
                var result = _homeRepository.UpdateHomeData(homePage);
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
        public IActionResult DeleteHome(int id)
        {
            try
            {
                var result = _homeRepository.DeleteHomeData(id);
                if (result)
                    return Ok("Home page data deleted successfully.");
                return BadRequest("Failed to delete home page data.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting data: {ex.Message}");
            }
        }


        [HttpPut("update/image")]
            public IActionResult UpdateImage([FromBody] MainPageDTO.Image_base64 request)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid request body.");
                }
                try
                {
                    bool isUpdated = _homeRepository.ImageUpdate(request.main_image_base64);
                    if (isUpdated)
                    {
                        return Ok("Image updated successfully.");
                    }
                    return BadRequest("Image update failed.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal Server Error: {ex.Message}");
                }
            }
    }

}
