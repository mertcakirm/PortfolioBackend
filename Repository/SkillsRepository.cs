using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;

namespace Repositories
{
    public class SkillsRepository
    {
        private readonly MySqlConnection _connection;

        public SkillsRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public List<Skills> GetSkills()
        {
            const string query = "SELECT * FROM Skills";
            {
                return _connection.Query<Skills>(query).ToList();
            }
        }

        public bool UpdateSkill(Skills request)
        {
            const string query = @"
                UPDATE Skills 
                SET SkillName = @SkillName, 
                    proficiency = @proficiency
                WHERE id = @id";

            {
                var affectedRows = _connection.Execute(query, request);
                return affectedRows > 0;
            }
        }

        public bool DeleteSkill(int id)
        {
            const string query = "DELETE FROM Skills WHERE id = @id";
            {
                var affectedRows = _connection.Execute(query, new { Id = id });
                return affectedRows > 0;
            }
        }

        public bool AddSkill(Skills request)
        {
            const string query = @"
                INSERT INTO Skills (SkillName, proficiency) 
                VALUES (@SkillName, @proficiency)";

            {
                var affectedRows = _connection.Execute(query, request);
                return affectedRows > 0;
            }
        }
    }
    public class Skills
{
    public int id { get; set; }
    public string SkillName { get; set; }
    public string proficiency { get; set; }
}

}
