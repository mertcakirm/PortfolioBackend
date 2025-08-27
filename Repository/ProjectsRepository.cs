using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ASPNetProject.data;
using Cors.DBO;
using Cors.DTO;
using ASPNetProject.Entities;

namespace ASPNetProject.Repositories;

    public class ProjectsRepository
    {
        private readonly AppDbContext _context;
        public ProjectsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ProjectsDBO.Projects>> GetProjectsPagedAsync(int page, int pageSize)
        {
            var query = _context.Projects.Where(p => p.IsDeleted==false).OrderByDescending(p => p.Id);
            var totalCount = await query.CountAsync();
            var projects = await query.Skip((page - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResult<ProjectsDBO.Projects>
            {
                Items = projects.Select(p => new ProjectsDBO.Projects
                {
                    id = p.Id,
                    title_tr = p.TitleTr,
                    description_tr = p.DescriptionTr,
                    title_en = p.TitleEn,
                    description_en = p.DescriptionEn,
                    image_base64 = p.ImageBase64,
                    href = p.Href,
                    Used_skills = p.UsedSkills
                }).ToList(),
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize
            };
        }

        public async Task<bool> UpdateProjectAsync(ProjectsDBO.Projects request)
        {
            var entity = await _context.Projects.FirstOrDefaultAsync(p => p.Id == request.id && p.IsDeleted==false);
            if (entity == null) return false;

            entity.TitleTr = request.title_tr;
            entity.DescriptionTr = request.description_tr;
            entity.TitleEn = request.title_en;
            entity.DescriptionEn = request.description_en;
            entity.ImageBase64 = request.image_base64;
            entity.Href = request.href;
            entity.UsedSkills = request.Used_skills;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            var entity = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null) return false;

            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddProjectAsync(ProjectsDBO.Projects request)
        {
            var entity = new Project
            {
                TitleTr = request.title_tr,
                DescriptionTr = request.description_tr,
                TitleEn = request.title_en,
                DescriptionEn = request.description_en,
                ImageBase64 = request.image_base64,
                Href = request.href,
                UsedSkills = request.Used_skills,
                IsDeleted = false
            };

            await _context.Projects.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
