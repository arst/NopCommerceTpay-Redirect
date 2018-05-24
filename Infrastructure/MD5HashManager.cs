using System.Security.Cryptography;
using System.Text;

namespace Nop.Plugin.Payments.Tpay.Infrastructure
{
    internal static class Md5HashManager
    {
        internal static string GetMd5Hash(string input)
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
