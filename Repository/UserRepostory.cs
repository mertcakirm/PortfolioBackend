using System.Security.Cryptography;
using System.Text;
using Dapper;
using MySql.Data.MySqlClient;

namespace UserRepository.Repositories
{
    public class UserRepository
    {
        private readonly MySqlConnection _connection;

        // Constructor doğru şekilde tanımlandı
        public UserRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public bool ValidateUser(string Username, string Password)
        {
            var query = "SELECT COUNT(*) FROM User WHERE Username = @Username AND Password = @Password";

            using (var command = new MySqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@Username", Username);
                command.Parameters.AddWithValue("@Password", Utils.HashPassword(Password));

                var result = command.ExecuteScalar();
                return int.TryParse(result?.ToString(), out int count) && count > 0;
            }
        }

        public bool AddUser(User request)
        {
            const string query = @"
            INSERT INTO User (Username, Password)
            VALUES (@Username, @Password)";
            var affectedRows = _connection.Execute(query, request);
            return affectedRows > 0;
        }


        public bool DeleteUser(int id)
        {
            const string query = "DELETE FROM User WHERE Uid = @id";
            {
                var affectedRows = _connection.Execute(query, new { Id = id });
                return affectedRows > 0;
            }
        }



        public List<User> GetUsers()
        {
            const string query = "SELECT Uid,Username FROM User";
            {
                return _connection.Query<User>(query).ToList();
            }
        }
    }

    public class User
    {
        public int Uid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
