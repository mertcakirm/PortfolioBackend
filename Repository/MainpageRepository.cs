using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ASPNetProject.data;
using Cors.DBO;
using ASPNetProject.Entities;

namespace ASPNetProject.Repositories;

    public class MainpageRepository
    {
        private readonly AppDbContext _context;

        public MainpageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MainPageDBO.HomePage>> GetHomeDataAsync()
        {
            return await _context.Homepages
                                 .Select(h => new MainPageDBO.HomePage
                                 {
                                     Id = h.Id,
                                     header_tr = h.HeaderTr,
                                     description_tr = h.DescriptionTr,
                                     header_en = h.HeaderEn,
                                     description_en = h.DescriptionEn,
                                     main_image_base64 = h.MainImageBase64
                                 })
                                 .ToListAsync();
        }

        public async Task<bool> UpdateHomeDataAsync(MainPageDBO.HomePage request)
        {
            var entity = await _context.Homepages.FirstOrDefaultAsync(h => h.Id == request.Id);
            if (entity == null) return false;

            entity.HeaderTr = request.header_tr;
            entity.DescriptionTr = request.description_tr;
            entity.HeaderEn = request.header_en;
            entity.DescriptionEn = request.description_en;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteHomeDataAsync(int id)
        {
            var entity = await _context.Homepages.FirstOrDefaultAsync(h => h.Id == id);
            if (entity == null) return false;

            _context.Homepages.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ImageUpdateAsync(string base64_image)
        {
            var entity = await _context.Homepages.FirstOrDefaultAsync();
            if (entity == null) return false;

            entity.MainImageBase64 = base64_image;
            await _context.SaveChangesAsync();
            return true;
        }
    }