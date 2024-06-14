using System.Security.Cryptography;

namespace Tools
{
    public static class CalculateHash
    {
        public static string CalculateSHA256Hash(string filePath)
        {
            using (FileStream stream = File.OpenRead(filePath))
            {
                SHA256 sha = SHA256.Create();
                byte[] hash = sha.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }
    }
}
