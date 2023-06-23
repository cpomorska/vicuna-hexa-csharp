using System;
using System.Security.Cryptography;
using System.Text;

namespace vicuna_ddd.Shared.Util
{
    public class HashUtil
    {
        public static string CalculateCustomerHash(string password, string salt)
        {
            byte[] tmpHash = new SHA256CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(password + salt));
            return Convert.ToBase64String(tmpHash);
        }

        public static string GetRandomSalt(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }
    }
}
