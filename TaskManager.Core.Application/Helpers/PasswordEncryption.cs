using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Application.Helpers
{
    public class PasswordEncryption
    {
        public static string ComputeShad256Hash(string password)
        {
            using (SHA256 shad256Hash = SHA256.Create())
            {
                byte[] bytes = shad256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));


                StringBuilder builder = new();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
