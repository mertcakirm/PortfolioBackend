using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ASPNetProject.data;
using Cors.DBO;
using Cors.DTO;
using ASPNetProject.Entities;

namespace ASPNetProject.Repositories;

    public class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            var hashedPassword = Utils.HashPassword(password);
            return await _context.Users.AnyAsync(u => u.Username == username 
                                                     && u.Password == hashedPassword 
                                                     && u.IsDeleted==false);
        }

        public async Task<bool> AddUserAsync(UserDBO.User request)
        {
            var entity = new User
            {
                Username = request.Username,
                Password = Utils.HashPassword(request.Password),
                Roleid = request.RoleId,
                IsDeleted = false
            };

            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(u => u.Uid == id);
            if (entity == null) return false;

            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<UserDBO.User>> GetUsersPagedAsync(int page, int pageSize)
        {
            var query = _context.Users.Where(u => u.IsDeleted==false).OrderByDescending(u => u.Uid);

            var totalCount = await query.CountAsync();
            var users = await query.Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            var resultUsers = users.Select(u => new UserDBO.User
            {
                Uid = u.Uid,
                Username = u.Username,
                RoleId = u.Roleid
            }).ToList();

            var totalPages = (int)System.Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResult<UserDBO.User>
            {
                Items = resultUsers,
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize
            };
        }
    }

