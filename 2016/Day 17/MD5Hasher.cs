using System.Security.Cryptography;
using System.Text;

namespace AoC_2016_Day_17
{
    internal static class MD5Hasher
    {
        public static string GetHash(string input)
        {
            // See https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithm.computehash

            // Convert the input string to a byte array and compute the hash.
            byte[] data = MD5.HashData(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
