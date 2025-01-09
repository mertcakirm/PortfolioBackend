using MainPage.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MainPage.Controllers
{
    [ApiController]
    [Route("api/mainpage")]
    public class HomeController : Controller
    {
        private readonly MainpageRepository _homeRepository;

        public HomeController(MainpageRepository repository)
        {
            _homeRepository = repository;
        }

        [HttpGet("get/all")]
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

        [HttpPut("update")]
        public IActionResult UpdateMainpage([FromBody] RequestMain request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request body.");

            try
            {
                var homePage = new HomePage
                {
                    Id = request.Id,
                    header_tr = request.header_tr,
                    description_tr = request.description_tr,
                    header_en = request.header_en,
                    description_en = request.description_en,
                    main_image_base64 = request.main_image_base64
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

        [HttpPost("add")]
        public IActionResult AddHome([FromBody] RequestMain request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request body.");

            try
            {
                var homePage = new HomePage
                {
                    header_tr = request.header_tr,
                    description_tr = request.description_tr,
                    header_en = request.header_en,
                    description_en = request.description_en,
                    main_image_base64 = request.main_image_base64
                };

                var result = _homeRepository.AddHomeData(homePage);
                if (result)
                    return Ok("Home page data added successfully.");
                return BadRequest("Failed to add home page data.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding data: {ex.Message}");
            }
        }
    }

    public class RequestMain
    {
    public int Id { get; set; }
    public string header_tr { get; set; }
    public string description_tr { get; set; }
    public string header_en { get; set; }
    public string description_en { get; set; }
    public string main_image_base64 { get; set; }
    }
}
