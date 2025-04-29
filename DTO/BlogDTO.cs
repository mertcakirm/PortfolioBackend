namespace Cors.DTO;

public class BlogDTO
{
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
    
        public List<Blog_ContentsReq> blog_Contents { get; set; }

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