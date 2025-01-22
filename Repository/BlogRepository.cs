using Dapper;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using MySql.Data.MySqlClient;

namespace Repositories
{
    public class BlogRepository
    {
        private readonly MySqlConnection _connection;

        public BlogRepository(MySqlConnection connection)
        {
            _connection = connection;
        }



        public async Task<List<Blog>> GetBlogs()
        {
            const string query = @"
                    SELECT 
                        b.Blogid AS Blogid,
                        b.BlogName AS BlogName,
                        b.Blog_image_base64 AS Blog_image_base64,
                        b.Blog_description AS Blog_description,
                        b.BLOG_desc_tr AS BLOG_desc_tr,
                        b.BLOG_Name_tr AS BLOG_Name_tr
                    FROM 
                        Blogs b
                    ";

            return _connection.Query<Blog>(query).ToList();
        }

                public async Task<Blog> GetBlog(int id)
                {
                    const string query = @"
                        SELECT 
                            b.Blogid AS Blogid,
                            b.BlogName AS BlogName,
                            b.Blog_description AS Blog_description,
                            b.BLOG_desc_tr AS BLOG_desc_tr,
                            b.BLOG_Name_tr AS BLOG_Name_tr,
                            bc.id AS id,
                            bc.title_en AS title_en,
                            bc.title_tr AS title_tr,
                            bc.content_en AS content_en,
                            bc.content_tr AS content_tr,
                            bc.image_base64 AS image_base64,
                            bc.Blogid AS Blogid
                        FROM 
                            Blogs b
                        LEFT JOIN 
                            Blog_Contents bc
                        ON 
                            b.Blogid = bc.Blogid
                        WHERE
                            b.Blogid = @id;
                    ";

                    var blogDictionary = new Dictionary<int, Blog>();
                    var result = await _connection.QueryAsync<Blog, Blog_Contents, Blog>(
                        query,
                        (blog, content) =>
                        {
                            // Blog zaten dictionary'de mevcutsa ekleme
                            if (!blogDictionary.TryGetValue(blog.Blogid, out var currentBlog))
                            {
                                currentBlog = blog;
                                currentBlog.blog_Contents = new List<Blog_Contents>();
                                blogDictionary.Add(blog.Blogid, currentBlog);
                            }

                            // İçerik varsa ekleme
                            if (content != null)
                            {
                                currentBlog.blog_Contents.Add(content);
                            }

                            return currentBlog;
                        },
                        new { id }
                    );

                    return blogDictionary.Values.FirstOrDefault();
                }



        public async Task AddBlogWithContentsAsync(Blog blog)
        {
            const string insertBlogQuery = @"
                INSERT INTO Blogs (BlogName, Blog_image_base64,Blog_description)
                VALUES (@BlogName, @Blog_image_base64 ,@Blog_description);
                SELECT LAST_INSERT_ID();";

            const string insertBlogContentQuery = @"
                INSERT INTO Blog_Contents (title_en, title_tr, content_en, content_tr, image_base64, Blogid)
                VALUES (@title_en, @title_tr, @content_en, @content_tr, @image_base64, @Blogid);";

            using (var transaction = _connection.BeginTransaction())
            {
                try
                {
                    var blogId = await _connection.ExecuteScalarAsync<int>(insertBlogQuery, new
                    {
                        blog.BlogName,
                        blog.Blog_image_base64
                    }, transaction);

                    foreach (var content in blog.blog_Contents)
                    {
                        content.Blogid = blogId; 

                        await _connection.ExecuteAsync(insertBlogContentQuery, new
                        {
                            content.title_en,
                            content.title_tr,
                            content.content_en,
                            content.content_tr,
                            content.image_base64,
                            content.Blogid
                        }, transaction);
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }


        public bool DeleteBlog(int id){
        var query = @"DELETE FROM Blogs WHERE Blogid = @id";
        var affectedRows = _connection.Execute(query, new { id });
        return affectedRows > 0;

        }
        


    }

    public class Blog
    {
        public int Blogid { get; set; }
        public string BlogName { get; set; }
        public string Blog_image_base64 { get; set; }
        public string BLOG_Name_tr { get; set; }
        public string BLOG_desc_tr { get; set; }
        public string Blog_description { get; set; }
        public List<Blog_Contents> blog_Contents { get; set; }

    }
    public class Blog_Contents
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