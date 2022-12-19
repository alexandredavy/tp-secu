using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Hello_World
{
    public class Chiffrement
    {

        public static string Hash(string text, int salt, int iterations)
        {
            
            byte[] bplainText = Encoding.UTF8.GetBytes(text);
            byte[] bsalt = Encoding.UTF8.GetBytes(salt.ToString());
            byte[] plainTextWithSaltBytes = new byte[bplainText.Length + bsalt.Length];

            for (int i = 0; i < bplainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = bplainText[i];
            }
            for (int i = 0; i < bsalt.Length; i++)
            {
                plainTextWithSaltBytes[bplainText.Length + i] = bsalt[i];
            }

            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(plainTextWithSaltBytes);

                for (int i = 1; i<iterations;  i++)
                {
                    bytes = sha256Hash.ComputeHash(bytes);
                }

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
}
