using MySql.Data.MySqlClient;

namespace Repositories
{
    public class BlogRepository
    {
        private readonly MySqlConnection _connection;
        
        public BlogRepository(MySqlConnection connection)
        {
            _connection = connection;
        }


    




    }
}