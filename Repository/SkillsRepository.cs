using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ASPNetProject.data;
using Cors.DBO;
using ASPNetProject.Entities;

namespace ASPNetProject.Repositories;

    public class SkillsRepository
    {
        private readonly AppDbContext _context;

        public SkillsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SkillsDBO.Skills>> GetSkillsAsync()
        {
            var entities = await _context.Skills
                                         .Where(p => p.IsDeleted==false)
                                         .ToListAsync();
        Console.WriteLine(entities);

            var result = entities.Select(e => new SkillsDBO.Skills
            {
                id = e.Id,
                SkillName = e.SkillName,
                proficiency = e.Proficiency,
            }).ToList();

            return result;
        }

        public async Task<bool> UpdateSkillAsync(SkillsDBO.Skills request)
        {
            var entity = await _context.Skills.FirstOrDefaultAsync(s => s.Id == request.id && s.IsDeleted==false);
            if (entity == null) return false;

            entity.SkillName = request.SkillName;
            entity.Proficiency = request.proficiency;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSkillAsync(int id)
        {
            var entity = await _context.Skills.FirstOrDefaultAsync(s => s.Id == id);
            if (entity == null) return false;

            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddSkillAsync(SkillsDBO.Skills request)
        {
            var entity = new Skill
            {
                SkillName = request.SkillName,
                Proficiency = request.proficiency,
                IsDeleted = false
            };

            await _context.Skills.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
