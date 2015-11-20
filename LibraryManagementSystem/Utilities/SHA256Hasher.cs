using System.Security.Cryptography;
using System.Text;

namespace LibraryManagementSystem.Utilities
{
    public class SHA256Hasher
    {
        public static string Create(string key)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(key);
            SHA256Managed hashstring = new SHA256Managed();

            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;

            foreach (byte x in hash)
            {
                hashString += $"{x:x2}";
            }

            return hashString;
        }
    }
}