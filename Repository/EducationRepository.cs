using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ASPNetProject.data;
using Cors.DBO;
using ASPNetProject.Entities;

namespace ASPNetProject.Repositories;


    public class EducationRepository
    {
        private readonly AppDbContext _context;

        public EducationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<EducationDBO.EducationQuery>> GetEducationsAsync()
        {
            return await _context.Educations
                                 .Select(e => new EducationDBO.EducationQuery
                                 {
                                     id = e.Id,
                                     EducationText = e.EducationText,
                                     Egitim = e.Egitim
                                 })
                                 .ToListAsync();
        }

        public async Task<bool> UpdateEducationAsync(EducationDBO.EducationQuery request)
        {
            var entity = await _context.Educations
                                       .FirstOrDefaultAsync(e => e.Id == request.id);
            if (entity == null) return false;

            entity.EducationText = request.EducationText;
            entity.Egitim = request.Egitim;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEducationAsync(int id)
        {
            var entity = await _context.Educations.FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null) return false;

            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddEducationAsync(EducationDBO.EducationQuery request)
        {
            var entity = new Education
            {
                EducationText = request.EducationText,
                Egitim = request.Egitim,
                IsDeleted = false
            };

            await _context.Educations.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
