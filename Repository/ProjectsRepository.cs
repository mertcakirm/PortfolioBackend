using Cors.DBO;
using Cors.DTO;
using Dapper;
using MySql.Data.MySqlClient;

namespace Repositories
{
    public class ProjectsRepository
    {
        private readonly MySqlConnection _connection;
        public ProjectsRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public PagedResult<ProjectsDBO.Projects> GetProjectsPaged(int page, int pageSize)
        {
            var offset = (page - 1) * pageSize;

            const string dataQuery = @"
        SELECT * FROM Projects
        WHERE isDeleted = false
        ORDER BY id DESC
        LIMIT @PageSize OFFSET @Offset;
    ";

            const string countQuery = @"SELECT COUNT(*) FROM Projects WHERE isDeleted = false;";

            var projects = _connection.Query<ProjectsDBO.Projects>(dataQuery, new { PageSize = pageSize, Offset = offset }).ToList();
            var totalCount = _connection.ExecuteScalar<int>(countQuery);
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResult<ProjectsDBO.Projects>
            {
                Items = projects,
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize
            };
        }
        public bool UpdateProject(ProjectsDBO.Projects request)
        {
            const string query = @"
                UPDATE Projects
                SET title_tr = @title_tr,
                    description_tr = @description_tr,
                    title_en = @title_en,
                    description_en = @description_en,
                    image_base64 = @image_base64,
                    href = @href,
                    Used_skills = @Used_skills
                WHERE id = @id AND isDeleted = false";
            
            var affectedRows = _connection.Execute(query, request);
            return affectedRows > 0;
        }

        public bool DeleteProject(int id)
        {
            const string query = "UPDATE Projects SET isDeleted = true WHERE id = @id";
            var affectedRows = _connection.Execute(query, new { id });
            return affectedRows > 0;
        }

        public bool AddProjects(ProjectsDBO.Projects request)
        {
            try
            {
                const string query = @"
                    INSERT INTO Projects 
                    (title_tr, description_tr, title_en, description_en, image_base64, href, Used_skills, isDeleted) 
                    VALUES 
                    (@title_tr, @description_tr, @title_en, @description_en, @image_base64, @href, @Used_skills, false)";
                
                var affectedRows = _connection.Execute(query, request);
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}