using ASPNetProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using Cors.DBO;
using Cors.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace ASPNetProject.Controllers;

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
        public async Task<IActionResult> GetHome()
        {
            try
            {
                var homeData = await _homeRepository.GetHomeDataAsync();
                return Ok(homeData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpPut("update/contents")]
        public async Task<IActionResult> UpdateMainpage([FromBody] MainPageDTO.RequestMain request)
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

                var result = await _homeRepository.UpdateHomeDataAsync(homePage);

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
        public async Task<IActionResult> DeleteHome(int id)
        {
            try
            {
                var result = await _homeRepository.DeleteHomeDataAsync(id);

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
        public async Task<IActionResult> UpdateImage([FromBody] MainPageDTO.Image_base64 request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request body.");

            try
            {
                var isUpdated = await _homeRepository.ImageUpdateAsync(request.main_image_base64);

                if (isUpdated)
                    return Ok("Image updated successfully.");

                return BadRequest("Image update failed.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
