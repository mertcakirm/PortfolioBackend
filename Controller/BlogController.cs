using Microsoft.AspNetCore.Mvc;
using Repositories;
using System.Linq;

namespace Controllers
{
    [ApiController]
    [Route("api/blogs")]
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
                var blogs = await _blogRepository.GetBlog(); // Asenkron çağrı
                return Ok(blogs);
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
                    blog_Contents = request.blog_Contents.Select(content => new Blog_Contents
                    {
                        id = content.id,
                        title_en = content.title_en,
                        title_tr = content.title_tr,
                        content_en = content.content_en,
                        content_tr = content.content_tr,
                        image_base64 = content.image_base64,
                        Blogid = content.Blogid
                    }).ToList()
                };

                await _blogRepository.AddBlogWithContentsAsync(blog);
                return Ok("Blog and its content added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding data: {ex.Message}");
            }
        }

    }

    public class BlogReq
    {
        public int Blogid { get; set; }
        public string BlogName { get; set; }
        public string Blog_image_base64 { get; set; }
        public List<Blog_Contents> blog_Contents { get; set; }

    }
    public class Blog_ContentsReq
    {
        public int id { get; set; }
        public string title_en { get; set; }
        public string title_tr { get; set; }
        public string content_en { get; set; }
        public string content_tr { get; set; }
        public string image_base64 { get; set; }
        public int Blogid { get; set; }
    }
}
