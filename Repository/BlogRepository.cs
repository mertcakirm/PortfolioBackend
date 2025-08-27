using Microsoft.EntityFrameworkCore;
using Cors.DBO;
using Cors.DTO;
using ASPNetProject.data;
using AutoMapper; // <- IMapper için
using Microsoft.EntityFrameworkCore.Metadata;
using ASPNetProject.Entities;

namespace ASPNetProject.Repositories;

    public class BlogRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper; // <- MappingProfile yerine IMapper

        public BlogRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // Sayfalama ile tüm bloglar
        public async Task<PagedResult<BlogDBO.Blog>> GetBlogsPagedAsync(int page, int pageSize)
        {
            var query = _context.Blogs.OrderByDescending(b => b.CreatedDate);

            var totalCount = await query.CountAsync();
            var blogs = await query.Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            Console.WriteLine("test : ",blogs);

            // Mapper ile dönüşüm
            var blogsDBO = _mapper.Map<List<BlogDBO.Blog>>(blogs);
            return new PagedResult<BlogDBO.Blog>
            {
                Items = blogsDBO,
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize
            };
        }

        // Aktif bloglar
        public async Task<PagedResult<BlogDBO.Blog>> GetBlogsActivePagedAsync(int page, int pageSize)
        {
            var query = _context.Blogs
                                .Where(b => b.ShowBlog == true)
                                .OrderByDescending(b => b.CreatedDate)
                                .Include(b => b.BlogContents);

            var totalCount = await query.CountAsync();
            var blogsEntity = await query.Skip((page - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToListAsync();

            // Mapper ile Entity → DBO dönüşümü
            var blogsDBO = _mapper.Map<List<BlogDBO.Blog>>(blogsEntity);

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResult<BlogDBO.Blog>
            {
                Items = blogsDBO,
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize
            };
        }

        // Tek blog + içerikleri
        public async Task<BlogDBO.Blog?> GetBlog(int id)
        {
            var blogEntity = await _context.Blogs
                                           .Include(b => b.BlogContents)
                                           .FirstOrDefaultAsync(b => b.Blogid == id);

            if (blogEntity == null) return null;

            return _mapper.Map<BlogDBO.Blog>(blogEntity);
        }

        // Blog + içerik ekleme
        public async Task AddBlogWithContentsAsync(BlogDBO.Blog blog)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                blog.CreatedDate = DateTime.Now;

                var blogEntity = _mapper.Map<Blog>(blog); // DBO → Entity dönüşümü

                await _context.Blogs.AddAsync(blogEntity);
                await _context.SaveChangesAsync();

                if (blog.blog_Contents != null && blog.blog_Contents.Any())
                {
                    foreach (var content in blog.blog_Contents)
                    {
                        var contentEntity = _mapper.Map<BlogContent>(content);
                        contentEntity.Blogid = blogEntity.Blogid;
                        await _context.BlogContents.AddAsync(contentEntity);
                    }

                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Blog silme
        public async Task<bool> DeleteBlog(int id)
        {
            var blog = await _context.Blogs
                                     .Include(b => b.BlogContents)
                                     .FirstOrDefaultAsync(b => b.Blogid == id);
            if (blog == null) return false;

            _context.BlogContents.RemoveRange(blog.BlogContents);
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return true;
        }

        // Göster
        public async Task<bool> ShowVisibility(int id)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Blogid == id);
            if (blog == null) return false;

            blog.ShowBlog = true;
            await _context.SaveChangesAsync();
            return true;
        }

        // Gizle
        public async Task<bool> HiddenVisibility(int id)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Blogid == id);
            if (blog == null) return false;

            blog.ShowBlog = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }