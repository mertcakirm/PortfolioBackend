
using Cors.DBO;
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


        public List<ProjectsDBO.Projects> GetProjects()
        {
            const string query = "SELECT * FROM Projects";
            {
                return _connection.Query<ProjectsDBO.Projects>(query).ToList();
            }
        }


        public bool UpdateProject(ProjectsDBO.Projects request)
        {
            const string query = @"
        UPDATE Projects
        SET title_tr=@title_tr
            description_tr=@description_tr
            title_en=@title_en
            description_en=@description_en
            image_base64=@image_base64
            href=@href
            Used_skills=@Used_skills 
        WHERE id = @id";
            {
                var affectedRows = _connection.Execute(query, request);
                return affectedRows > 0;
            }
        }


        public bool DeleteProject(int Id)
        {
            const string query = "DELETE FROM Projects WHERE id = @Id";
            {
                var affectedRows = _connection.Execute(query, new { id = Id });
                return affectedRows > 0;
            }
        }

        public bool AddProjects(ProjectsDBO.Projects request)
        {
            try{
            const string query = @" 
        INSERT INTO Projects (title_tr, description_tr, title_en, description_en, image_base64,href,Used_skills) 
            VALUES (@title_tr, @description_tr, @title_en, @description_en, @image_base64, @href, @Used_skills)";
            {
                var affectedRows = _connection.Execute(query, request);
                return affectedRows > 0;
            }
            }catch(Exception ex){
                
                Console.WriteLine(ex);
                return false;
            }
        }
    }





}