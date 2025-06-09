using Cors.DBO;
using Cors.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories;

namespace Controllers
{
    [ApiController]
    [Route("api/blogs")]
    [Authorize]
    public class BlogController : Controller

    {
        private readonly BlogRepository _blogRepository;
        public BlogController(BlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        [HttpGet("paged")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPagedBlogs([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagedResult = await _blogRepository.GetBlogsPagedAsync(page, pageSize);
                return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving data: {ex.Message}");
            }
        }
        
        [HttpGet("public")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBlogsActiveCont([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _blogRepository.GetBlogsActivePagedAsync(page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving data: {ex.Message}");
            }
        }
        
            [HttpGet("get/{id}")]
             [AllowAnonymous]
  			public async Task<IActionResult> GetBlogSimple(int id)
            {
                try
                {
                    var blog = await _blogRepository.GetBlog(id);

                    if (blog == null)
                    {
                        return NotFound($"Blog with ID {id} not found.");
                    }

                    return Ok(blog);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error retrieving data: {ex.Message}");
                }
            }


            [HttpPost("add")]
            public async Task<IActionResult> AddBlog([FromBody] BlogDTO.BlogReq request)
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid request body.");

                try
                {
                    var blogEntity = new BlogDBO.Blog
                    {
                        Blogid = request.Blogid,
                        BlogName = request.BlogName,
                        Blog_image_base64 = request.Blog_image_base64,
                        Blog_description = request.Blog_description,
                        BLOG_Name_tr = request.BLOG_Name_tr,
                        BLOG_desc_tr = request.BLOG_desc_tr,
                        CreatedDate = request.CreatedDate ?? DateTime.Now,
                        CreatedBy = request.CreatedBy,
                        ShowBlog = request.ShowBlog,
                        blog_Contents = request.blog_Contents?.Select(content => new BlogDBO.Blog_Contents
                        {
                            id = content.id,
                            title_en = content.title_en,
                            title_tr = content.title_tr,
                            content_en = content.content_en,
                            content_tr = content.content_tr,
                            image_base64 = content.image_base64,
                            Blogid = content.Blogid
                        }).ToList() ?? new List<BlogDBO.Blog_Contents>()
                    };

                    await _blogRepository.AddBlogWithContentsAsync(blogEntity);
                    return Ok("Blog and its content added successfully.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error adding data: {ex.Message}");
                }
            }

            [HttpDelete("delete/{id}")]
            public async Task<IActionResult> DeleteBlog(int id)
            {
                try
                {
                    var result = await _blogRepository.DeleteBlog(id);
                    if (result)
                        return Ok("Blog page data deleted successfully.");
                    return BadRequest("Failed to delete Blog page data.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error deleting data: {ex.Message}");
                }
            }
            
            [HttpPut("show/{id}")]
            public async Task<IActionResult> ShowBlog(int id)
            {
                try
                {
                    var result = await _blogRepository.ShowVisibility(id);
                    if (result)
                        return Ok("Blog page data updated successfully.");
                    return BadRequest("Failed to update Blog page data.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error updating data: {ex.Message}");
                }
            }
            
            [HttpPut("hidden/{id}")]
            public async Task<IActionResult> HiddenBlog(int id)
            {
                try
                {
                    var result = await _blogRepository.HiddenVisibility(id);
                    if (result)
                        return Ok("Blog page data updated successfully.");
                    return BadRequest("Failed to update Blog page data.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error updating data: {ex.Message}");
                }
            }  
    }


}
