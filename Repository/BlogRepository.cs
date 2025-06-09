using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cors.DBO;
using Cors.DTO;

namespace Repositories
{
    public class BlogRepository
    {
        private readonly MySqlConnection _connection;

        public BlogRepository(MySqlConnection connection)
        {
            _connection = connection;
        }
            public async Task<PagedResult<BlogDBO.Blog>> GetBlogsPagedAsync(int page, int pageSize)
            {
                var offset = (page - 1) * pageSize;

                var dataQuery = @"
                    SELECT Blogid, BlogName, Blog_image_base64, Blog_description, BLOG_desc_tr, BLOG_Name_tr, ShowBlog, CreatedBy, CreatedDate
                    FROM Blogs
                    WHERE isDeleted = false
                    ORDER BY CreatedDate DESC
                    LIMIT @PageSize OFFSET @Offset;
                ";

                var countQuery = @"SELECT COUNT(*) FROM Blogs WHERE isDeleted = false;";

                var blogs = await _connection.QueryAsync<BlogDBO.Blog>(dataQuery, new { PageSize = pageSize, Offset = offset });
                var totalCount = await _connection.ExecuteScalarAsync<int>(countQuery);

                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                return new PagedResult<BlogDBO.Blog>
                {
                    Items = blogs.ToList(),
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    CurrentPage = page,
                    PageSize = pageSize
                };
            }

            public async Task<PagedResult<BlogDBO.Blog>> GetBlogsActivePagedAsync(int page, int pageSize)
            {
                var offset = (page - 1) * pageSize;

                var dataQuery = @"
                    SELECT Blogid, BlogName, Blog_image_base64, Blog_description, BLOG_desc_tr, BLOG_Name_tr, ShowBlog, CreatedBy, CreatedDate
                    FROM Blogs
                    WHERE ShowBlog = 1 AND isDeleted = false
                    ORDER BY CreatedDate DESC
                    LIMIT @PageSize OFFSET @Offset;
                ";

                var countQuery = @"SELECT COUNT(*) FROM Blogs WHERE ShowBlog = 1 AND isDeleted = false;";

                var blogs = await _connection.QueryAsync<BlogDBO.Blog>(dataQuery, new { PageSize = pageSize, Offset = offset });
                var totalCount = await _connection.ExecuteScalarAsync<int>(countQuery);

                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                return new PagedResult<BlogDBO.Blog>
                {
                    Items = blogs.ToList(),
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    CurrentPage = page,
                    PageSize = pageSize
                };
            }

            public async Task<BlogDBO.Blog> GetBlog(int id)
            {
                const string query = @"
                    SELECT 
                        b.Blogid, b.BlogName, b.Blog_description, b.BLOG_desc_tr, b.BLOG_Name_tr,b.CreatedBy,b.CreatedDate,
                        bc.id, bc.title_en, bc.title_tr, bc.content_en, bc.content_tr, bc.image_base64
                    FROM Blogs b
                    LEFT JOIN Blog_Contents bc ON b.Blogid = bc.Blogid
                    WHERE b.Blogid = @id AND b.isDeleted = false";

                var blogDictionary = new Dictionary<int, BlogDBO.Blog>();
                var result = await _connection.QueryAsync<BlogDBO.Blog, BlogDBO.Blog_Contents, BlogDBO.Blog>(
                    query,
                    (blog, content) =>
                    {
                        if (!blogDictionary.TryGetValue(blog.Blogid, out var currentBlog))
                        {
                            currentBlog = blog;
                            currentBlog.blog_Contents = new List<BlogDBO.Blog_Contents>();
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
                    blog.blog_Contents = new List<BlogDBO.Blog_Contents>();
                }
                return blog;
            }

            public async Task AddBlogWithContentsAsync(BlogDBO.Blog blog)
            {
                using (var transaction = await _connection.BeginTransactionAsync())
                {
                    try
                    {
                        string insertBlogQuery = @"
                            INSERT INTO Blogs (BlogName, Blog_image_base64, Blog_description, BLOG_Name_tr, BLOG_desc_tr, ShowBlog, CreatedBy, isDeleted)
                            VALUES (@BlogName, @Blog_image_base64, @Blog_description, @BLOG_Name_tr, @BLOG_desc_tr, @ShowBlog, @CreatedBy, false);
                            SELECT LAST_INSERT_ID();";

                        var blogId = await _connection.ExecuteScalarAsync<int>(
                            insertBlogQuery,
                            new
                            {
                                BlogName = blog.BlogName ?? "No Name",
                                Blog_image_base64 = blog.Blog_image_base64 ?? "",
                                Blog_description = blog.Blog_description ?? "No description",
                                BLOG_Name_tr = blog.BLOG_Name_tr ?? blog.BlogName,
                                BLOG_desc_tr = blog.BLOG_desc_tr ?? blog.Blog_description,
                                ShowBlog = blog.ShowBlog,
                                CreatedBy = blog.CreatedBy ?? "Belirtilmemiş"
                            },
                            transaction
                        );

                        if (blog.blog_Contents != null && blog.blog_Contents.Any())
                        {
                            string insertContentQuery = @"
                                INSERT INTO Blog_Contents (title_en, title_tr, content_en, content_tr, image_base64, Blogid)
                                VALUES (@title_en, @title_tr, @content_en, @content_tr, @image_base64, @Blogid);";
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
                var query = @"UPDATE Blogs SET isDeleted = true WHERE Blogid = @id";
                var affectedRows = await _connection.ExecuteAsync(query, new { id });
                return affectedRows > 0;
            }

            public async Task<bool> ShowVisibility(int id)
            {
                var query = @"UPDATE Blogs SET ShowBlog = 1 WHERE Blogid = @id AND isDeleted = false";
                var affectedRows = await _connection.ExecuteAsync(query, new { id });
                return affectedRows > 0;
            }

            public async Task<bool> HiddenVisibility(int id)
            {
                var query = @"UPDATE Blogs SET ShowBlog = 0 WHERE Blogid = @id AND isDeleted = false";
                var affectedRows = await _connection.ExecuteAsync(query, new { id });
                return affectedRows > 0;
            }
    }

  
}
