using System.Security.Cryptography;
using System.Text;

public static class Utils{
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return string.Join("", hash.Select(x => x.ToString("x2")));
            }
        }
}