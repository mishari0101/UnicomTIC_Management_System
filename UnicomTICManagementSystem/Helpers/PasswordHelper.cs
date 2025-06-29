// Helpers/PasswordHelper.cs
using System;
using System.Security.Cryptography;
using System.Text;

namespace UnicomTICManagementSystem.Helpers
{
    public static class PasswordHelper
    {
        // This method takes a plain text password and returns its SHA256 hash.
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return string.Empty;
            }

            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash from the password bytes
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert the byte array to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2")); // "x2" for lowercase hex
                }
                return builder.ToString();
            }
        }
    }
}