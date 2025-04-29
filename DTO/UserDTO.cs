namespace Cors.DTO;

public class UserDTO
{
    public class UserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
}