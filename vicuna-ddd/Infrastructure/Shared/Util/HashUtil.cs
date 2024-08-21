using System.Security.Cryptography;
using System.Text;

namespace vicuna_ddd.Shared.Util;

public class HashUtil
{
    protected HashUtil() { /*NOSONAR*/ }
    public static string CalculateCustomerHash(string password, string salt)
    {
        byte[] tmpHash = SHA256.HashData(Encoding.ASCII.GetBytes(password + salt));
        return Convert.ToBase64String(tmpHash);
    }

    public static string GetRandomSalt(int maximumSaltLength)
    {
        var salt = new byte[maximumSaltLength];
        using (var random = RandomNumberGenerator.Create())
        {
            random.GetNonZeroBytes(salt);
        }

        return Convert.ToBase64String(salt);
    }
}
