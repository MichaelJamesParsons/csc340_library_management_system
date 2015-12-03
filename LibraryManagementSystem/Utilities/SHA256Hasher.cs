using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LibraryManagementSystem.Utilities
{
    public class Sha256Hasher
    {
        /// <summary>
        /// Generates a SHA 256 hash from the given key.
        /// </summary>
        /// <param name="key">The string to hash</param>
        /// <returns>The hashed string.</returns>
        public static string Create(string key)
        {
            var bytes = Encoding.UTF8.GetBytes(key);
            var hashstring = new SHA256Managed();
            var hash = hashstring.ComputeHash(bytes);
            return hash.Aggregate(string.Empty, (current, x) => current + $"{x:x2}");
        }
    }
}