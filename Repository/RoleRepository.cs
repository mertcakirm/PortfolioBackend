using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ASPNetProject.data;
using Cors.DBO;
using ASPNetProject.Entities;

namespace ASPNetProject.Repositories;

    public class RoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoleDBO.Role>> GetRolesAsync()
        {
            var entities = await _context.Roles
                                         .Where(r => r.IsDeleted==false)
                                         .ToListAsync();

            return entities.Select(r => new RoleDBO.Role
            {
                Roleid = r.Roleid,
                RoleName = r.RoleName
            }).ToList();
        }

        public async Task<bool> UpdateRoleAsync(RoleDBO.Role request)
        {
            var entity = await _context.Roles.FirstOrDefaultAsync(r => r.Roleid == request.Roleid && r.IsDeleted==false);
            if (entity == null) return false;

            entity.RoleName = request.RoleName;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var entity = await _context.Roles.FirstOrDefaultAsync(r => r.Roleid == id);
            if (entity == null) return false;

            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddRoleAsync(RoleDBO.Role request)
        {
            var entity = new Role
            {
                RoleName = request.RoleName,
                IsDeleted = false
            };

            await _context.Roles.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> GetRoleAsync(int roleId)
        {
            var entity = await _context.Roles.FirstOrDefaultAsync(r => r.Roleid == roleId && r.IsDeleted==false);
            return entity?.RoleName ?? string.Empty;
        }
    }
