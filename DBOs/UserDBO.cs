namespace Cors.DBO;

public class UserDBO
{
    public class User
    {
        public int Uid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? RoleId { get; set; }
    }
}