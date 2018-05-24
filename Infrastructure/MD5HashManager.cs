using System;
using System.Security.Cryptography;
using System.Text;

namespace Nop.Plugin.Payments.Tpay.Infrastructure
{
    static class MD5HashManager
    {
        internal static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            string hashOfInput = GetMd5Hash(md5Hash, input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static string GetMd5Hash(MD5 md5Hash, string input)
        {
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (var hashedByte in hashBytes)
                {
                    sb.Append(hashedByte.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
