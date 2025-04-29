using System.Security.Cryptography;
using System.Text;
using Cors.DBO;
using Dapper;
using MySql.Data.MySqlClient;

namespace Repositories
{
    public class UserRepository
    {
        private readonly MySqlConnection _connection;

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

        public bool AddUser(UserDBO.User request)
        {
            const string query = @"
            INSERT INTO User (Username, Password,Roleid)
            VALUES (@Username, @Password, @RoleId)";
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



        public List<UserDBO.User> GetUsers()
        {
            const string query = "SELECT Uid,Username,Roleid FROM User";
            {
                return _connection.Query<UserDBO.User>(query).ToList();
            }
        }
    }


}
