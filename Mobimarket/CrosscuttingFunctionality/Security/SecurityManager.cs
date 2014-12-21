using System.Linq;

namespace Mobimarket.CrosscuttingFunctionality.Security
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class SecurityManager
    {
        private const string salt = "sdlkjfsdjkfkjdhfssdjkfhsdahjfgs,fkdghsdfhgks,adfjgsdfkhsfhgd&*&8766$$$$%65s";

        public static string GetHashString(string s)
        {
            try
            {
                var securedString = String.Concat(s, salt);
                byte[] bytes = Encoding.Unicode.GetBytes(securedString);
                var csp = new MD5CryptoServiceProvider();
                byte[] byteHash = csp.ComputeHash(bytes);
                return byteHash.Aggregate(string.Empty, (current, b) => current + string.Format("{0:x2}", b));
            }
            catch
            {
                return "";
            }
        }
    }
}