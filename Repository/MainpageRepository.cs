using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;

namespace Repositories
{
    public class MainpageRepository
    {
        private readonly MySqlConnection _connection;

        public MainpageRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public List<HomePage> GetHomeData()
        {
            const string query = "SELECT * FROM Homepage";
            {
                return _connection.Query<HomePage>(query).ToList();
            }
        }

        public bool UpdateHomeData(HomePage request)
        {
            const string query = @"
            UPDATE Homepage 
            SET header_tr = @header_tr, 
                description_tr = @description_tr, 
                header_en = @header_en, 
                description_en = @description_en
            WHERE id = @Id";

            {
                var affectedRows = _connection.Execute(query, request);
                return affectedRows > 0;
            }
        }

        public bool DeleteHomeData(int id)
        {
            const string query = "DELETE FROM Homepage WHERE Id = @Id";
            {
                var affectedRows = _connection.Execute(query, new { Id = id });
                return affectedRows > 0;
            }
        }


    public bool ImageUpdate(string base64_image)
    {
        const string query = @"UPDATE Homepage SET main_image_base64 = @base64_image";
        var affectedRows = _connection.Execute(query, new { base64_image });
        return affectedRows > 0;
    }

    }
    public class HomePage
    {
        public int Id { get; set; }
        public string header_tr { get; set; }
        public string description_tr { get; set; }
        public string header_en { get; set; }
        public string description_en { get; set; }
        public string main_image_base64 { get; set; }
    }

}
