using System.Security.Cryptography;
using EcommerceBehzad.Application.Common.Interfaces;

namespace EcommerceBehzad.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int Keysize = 32; // 256 bits
        private const int Iterations = 350000; // High iteration count for PBKDF2 OWASP recommendation
        private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA512;

        public (string Hash, string Salt) HashPassword(string password)
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(Keysize);
            byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, Iterations, HashAlgorithm, Keysize);

            return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltBytes));
        }

        public bool VerifyPassword(string password, string hash, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] originalHashBytes = Convert.FromBase64String(hash);
            byte[] testHashBytes = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, Iterations, HashAlgorithm, Keysize);

            return CryptographicOperations.FixedTimeEquals(originalHashBytes, testHashBytes);
        }
    }
}
