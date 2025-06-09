using Cors.DBO;
using Dapper;
using MySql.Data.MySqlClient;

namespace Repositories
{
    public class RoleRepository
    {
        private readonly MySqlConnection _connection;
        public RoleRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public List<RoleDBO.Role> GetRoles()
        {
            const string query = "SELECT * FROM Roles WHERE isDeleted = false";
            return _connection.Query<RoleDBO.Role>(query).ToList();
        }

        public bool UpdateRole(RoleDBO.Role request)
        {
            const string query = @"
                UPDATE Roles
                SET RoleName = @RoleName
                WHERE Roleid = @Roleid AND isDeleted = false";
            
            var affectedRows = _connection.Execute(query, request);
            return affectedRows > 0;
        }

        public bool DeleteRole(int id)
        {
            const string query = "UPDATE Roles SET isDeleted = true WHERE Roleid = @id";
            var affectedRows = _connection.Execute(query, new { id });
            return affectedRows > 0;
        }

        public bool AddRole(RoleDBO.Role request)
        {
            const string query = @" 
                INSERT INTO Roles (RoleName, isDeleted) 
                VALUES (@RoleName, false)";
            
            var affectedRows = _connection.Execute(query, request);
            return affectedRows > 0;
        }

        public string GetRole(int roleId)
        {
            const string query = "SELECT RoleName FROM Roles WHERE Roleid = @roleid AND isDeleted = false";
            var result = _connection.ExecuteScalar(query, new { roleid = roleId });
            return result?.ToString() ?? string.Empty;
        }
    }
}