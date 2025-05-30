using System.Collections.Generic;
using System.Linq;
using Controllers;
using Cors.DBO;
using Dapper;
using MySql.Data.MySqlClient;

namespace Repositories
{
    public class EducationRepository
    {
        private readonly MySqlConnection _connection;

        public EducationRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public List<EducationDBO.EducationQuery> GetEducations()
        {
            const string query = "SELECT * FROM Educations";
            {
                return _connection.Query<EducationDBO.EducationQuery>(query).ToList();
            }
        }

        public bool UpdateEducation(EducationDBO.EducationQuery request)
        {
            const string query = @"
                UPDATE Educations 
                SET EducationText = @EducationText, 
                WHERE id = @id";
            {
                var affectedRows = _connection.Execute(query, request);
                return affectedRows > 0;
            }
        }

        public bool DeleteEducation(int id)
        {
            const string query = "DELETE FROM Educations WHERE id = @id";
            {
                var affectedRows = _connection.Execute(query, new { Id = id });
                return affectedRows > 0;
            }
        }

        public bool AddEducations(EducationDBO.EducationQuery request)
        {
            const string query = @"
                INSERT INTO Educations (EducationText,Egitim) 
                VALUES (@EducationText,@Egitim)";

            {
                var affectedRows = _connection.Execute(query, request);
                return affectedRows > 0;
            }
        }
    }

}
