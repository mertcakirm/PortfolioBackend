
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


    public List<Projects> GetProjects()
    {
        const string query = "SELECT * FROM Projects";
        {
            return _connection.Query<Projects>(query).ToList();
        }
    }
    

    public bool UpdateProject(Projects request)
    {
        const string query= @"
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


    public bool DeleteProject(int id)
    {
            const string query = "DELETE FROM Projects WHERE id = @id";
            {
                var affectedRows = _connection.Execute(query, new { Id = id });
                return affectedRows > 0;
            }
    }

    public bool AddProjects(Projects request)
    {
        const string query= @" 
        INSERT INTO Projects (title_tr, description_tr, title_en, description_en, image_base64,href,Used_skills) 
            VALUES (@title_tr, @description_tr, @title_en, @description_en, @image_base64, @href, @Used_skills)";
    

            {
                var affectedRows = _connection.Execute(query, request);
                return affectedRows > 0;
            }

    }

    }



    public class Projects
    {
        public int id { get; set; }
        public string title_tr { get; set; }
        public string description_tr { get; set; }
        public string title_en { get; set; }
        public string description_en { get; set; }
        public string image_base64 { get; set; }
        public string href { get; set; }
        public string Used_skills { get; set; }
    }

}