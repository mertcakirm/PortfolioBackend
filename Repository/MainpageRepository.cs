using System.Collections.Generic;
using System.Linq;
using Cors.DBO;
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

        public List<MainPageDBO.HomePage> GetHomeData()
        {
            const string query = "SELECT * FROM Homepage";
            {
                return _connection.Query<MainPageDBO.HomePage>(query).ToList();
            }
        }

        public bool UpdateHomeData(MainPageDBO.HomePage request)
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


}
