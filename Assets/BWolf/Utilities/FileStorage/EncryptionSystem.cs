// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System.Security.Cryptography;
using System.Text;

namespace BWolf.Utilities.FileStorage
{
    /// <summary>Provides functionality for encrypting data</summary>
    public static class EncryptionSystem
    {
        /// <summary>Computes a hash given the data provided using a cryptographic hash function</summary>
        public static string Hash(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            SHA256Managed SHA256 = new SHA256Managed();

            byte[] hash = SHA256.ComputeHash(bytes);
            return HashToHexString(hash);
        }

        /// <summary>Creates a string of hexadecimal numerals based on given hash</summary>
        public static string HashToHexString(byte[] hash)
        {
            int length = hash.Length;
            StringBuilder sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}