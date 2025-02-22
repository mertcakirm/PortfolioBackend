using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                    Blogid, BlogName, Blog_image_base64, Blog_description, BLOG_desc_tr, BLOG_Name_tr
                FROM Blogs";

            var blogs = await _connection.QueryAsync<Blog>(query);
            return blogs.ToList();
        }

        public async Task<Blog> GetBlog(int id)
        {
            const string query = @"
        SELECT 
            b.Blogid, b.BlogName, b.Blog_description, b.BLOG_desc_tr, b.BLOG_Name_tr,
            bc.id, bc.title_en, bc.title_tr, bc.content_en, bc.content_tr, bc.image_base64
        FROM Blogs b
        LEFT JOIN Blog_Contents bc ON b.Blogid = bc.Blogid
        WHERE b.Blogid = @id";

            var blogDictionary = new Dictionary<int, Blog>();

            var result = await _connection.QueryAsync<Blog, Blog_Contents, Blog>(
                query,
                (blog, content) =>
                {
                    if (!blogDictionary.TryGetValue(blog.Blogid, out var currentBlog))
                    {
                        currentBlog = blog;
                        currentBlog.blog_Contents = new List<Blog_Contents>();
                        blogDictionary.Add(blog.Blogid, currentBlog);
                    }

                    if (content != null)
                    {
                        currentBlog.blog_Contents.Add(content);
                    }

                    return currentBlog;
                },
                new { id },
                splitOn: "id"
            );

            var blog = blogDictionary.Values.FirstOrDefault();
            if (blog != null && blog.blog_Contents == null)
            {
                blog.blog_Contents = new List<Blog_Contents>();
            }
            return blog;
        }


        public async Task AddBlogWithContentsAsync(Blog blog)
        {
            using (var transaction = await _connection.BeginTransactionAsync())
            {
                try
                {
                    string insertBlogQuery = @"
                        INSERT INTO Blogs (BlogName, Blog_image_base64, Blog_description, BLOG_Name_tr, BLOG_desc_tr)
                        VALUES (@BlogName, @Blog_image_base64, @Blog_description, @BLOG_Name_tr, @BLOG_desc_tr);
                        SELECT LAST_INSERT_ID();";

                    var blogId = await _connection.ExecuteScalarAsync<int>(
                        insertBlogQuery, 
                        new
                        {
                            BlogName = blog.BlogName ?? "No Name",
                            Blog_image_base64 = blog.Blog_image_base64 ?? "",
                            Blog_description = blog.Blog_description ?? "No description",
                            BLOG_Name_tr = blog.BLOG_Name_tr ?? blog.BlogName,
                            BLOG_desc_tr = blog.BLOG_desc_tr ?? blog.Blog_description
                        },
                        transaction
                    );


                    if (blog.blog_Contents != null && blog.blog_Contents.Any())
                    {
                        string insertContentQuery = @"
                            INSERT INTO Blog_Contents (title_en, title_tr, content_en, content_tr, image_base64,Blogid)
                            VALUES (@title_en, @title_tr, @content_en, @content_tr, @image_base64,@Blogid);";

                        foreach (var content in blog.blog_Contents)
                        {
                            await _connection.ExecuteAsync(insertContentQuery, new
                            {   
                                Blogid = blogId,
                                title_en = content.title_en ?? "",
                                title_tr = content.title_tr ?? "",
                                content_en = content.content_en ?? "",
                                content_tr = content.content_tr ?? "",
                                image_base64 = content.image_base64 ?? ""
                            }, transaction);
                        }

                    }

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"Veritabanı hatası: {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<bool> DeleteBlog(int id)
        {
            var query = @"DELETE FROM Blogs WHERE Blogid = @id";
            var affectedRows = await _connection.ExecuteAsync(query, new { id });
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
        public List<Blog_Contents> blog_Contents { get; set; } = new();
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
