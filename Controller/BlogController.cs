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

        [HttpGet("get/all")]
        public async Task<IActionResult> GetBlogs()
        {
            try
            {
                var blogs = await _blogRepository.GetBlogs();
                return Ok(blogs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving data: {ex.Message}");
            }
        }
        
        [HttpGet("get/active")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBlogsActiveCont()
        {
            try
            {
                var blogs = await _blogRepository.GetBlogsActive();
                return Ok(blogs);
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
        public async Task<IActionResult> AddBlog([FromBody] Blog request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request body.");
            try
            {
                var blog = new Blog
                {
                    Blogid = request.Blogid,
                    BlogName = request.BlogName,
                    Blog_image_base64 = request.Blog_image_base64,
                    Blog_description = request.Blog_description,
                    BLOG_desc_tr = request.BLOG_desc_tr,
                    BLOG_Name_tr = request.BLOG_Name_tr,
                    CreatedBy=request.CreatedBy,
                    ShowBlog = request.ShowBlog,
                    blog_Contents = request.blog_Contents.Select(content => new Blog_Contents
                    {
                        id = content.id,
                        title_en = content.title_en,
                        title_tr = content.title_tr,
                        content_en = content.content_en,
                        content_tr = content.content_tr,
                        image_base64 = content.image_base64,
                        Blogid = content.Blogid
                    }).ToList() ?? new List<Blog_Contents>()
                };

                await _blogRepository.AddBlogWithContentsAsync(blog);
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

    public class BlogReq
    {
        public int Blogid { get; set; }
        public string BlogName { get; set; }
        public string? Blog_image_base64 { get; set; }
        public string Blog_description { get; set; }
        public string BLOG_Name_tr { get; set; }
        public string BLOG_desc_tr { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool ShowBlog { get; set; }
        public string CreatedBy { get; set; }
    
        public List<Blog_Contents> blog_Contents { get; set; }

    }
    public class Blog_ContentsReq
    {
        public int id { get; set; }
        public string title_en { get; set; }
        public string title_tr { get; set; }
        public string content_en { get; set; }
        public string content_tr { get; set; }
        public string? image_base64 { get; set; }
        public int Blogid { get; set; }
    }
}
