using System.Security.Cryptography;
using System.Text;

namespace MyLaptopWebsite.Utils
{
    public static class SHA256Hasher
    {
        public static string Hash(string org)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(org);
                byte[] hash = sha256.ComputeHash(bytes);

                string hashedInput = Convert.ToBase64String(hash);
                return hashedInput;
            }
        }
    }
}
