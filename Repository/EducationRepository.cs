using System.Collections.Generic;
using System.Linq;
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
            const string query = "SELECT * FROM Educations WHERE isDeleted = false";
            return _connection.Query<EducationDBO.EducationQuery>(query).ToList();
        }

        public bool UpdateEducation(EducationDBO.EducationQuery request)
        {
            const string query = @"
                UPDATE Educations 
                SET EducationText = @EducationText, 
                    Egitim = @Egitim
                WHERE id = @id AND isDeleted = false";
            
            var affectedRows = _connection.Execute(query, request);
            return affectedRows > 0;
        }

        public bool DeleteEducation(int id)
        {
            const string query = "UPDATE Educations SET isDeleted = true WHERE id = @id";
            var affectedRows = _connection.Execute(query, new { id });
            return affectedRows > 0;
        }

        public bool AddEducations(EducationDBO.EducationQuery request)
        {
            const string query = @"
                INSERT INTO Educations (EducationText, Egitim, isDeleted) 
                VALUES (@EducationText, @Egitim, false)";

            var affectedRows = _connection.Execute(query, request);
            return affectedRows > 0;
        }
    }
}