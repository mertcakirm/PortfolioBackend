using Cors.DBO;
using Cors.DTO;
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
            var query = @"SELECT COUNT(*) FROM User 
                          WHERE Username = @Username AND Password = @Password AND isDeleted = false";
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
                INSERT INTO User (Username, Password, Roleid, isDeleted)
                VALUES (@Username, @Password, @RoleId, false)";
            var affectedRows = _connection.Execute(query, request);
            return affectedRows > 0;
        }

        public bool DeleteUser(int id)
        {
            const string query = "UPDATE User SET isDeleted = true WHERE Uid = @id";
            var affectedRows = _connection.Execute(query, new { id });
            return affectedRows > 0;
        }

        public PagedResult<UserDBO.User> GetUsersPaged(int page, int pageSize)
        {
            var offset = (page - 1) * pageSize;

            var dataQuery = @"SELECT Uid, Username, Roleid 
                              FROM User 
                              WHERE isDeleted = false 
                              ORDER BY Uid DESC 
                              LIMIT @PageSize OFFSET @Offset;";
            var countQuery = @"SELECT COUNT(*) FROM User WHERE isDeleted = false;";

            var users = _connection.Query<UserDBO.User>(dataQuery, new { PageSize = pageSize, Offset = offset }).ToList();
            var totalCount = _connection.ExecuteScalar<int>(countQuery);
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResult<UserDBO.User>
            {
                Items = users,
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize
            };
        }
    }
}
