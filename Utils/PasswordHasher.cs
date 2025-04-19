using System.Security.Cryptography;
using System.Text;

namespace VirchowAspNetApi.Utils
{ 

    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            // Gera um salt aleatório
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            // Deriva a chave usando PBKDF2
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32); // Tamanho do hash

            // Combina salt + hash
            byte[] hashBytes = new byte[48]; // 16 (salt) + 32 (hash)
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 32);

            // Retorna em Base64 para salvar no banco
            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            for (int i = 0; i < 32; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    return false;
            }

            return true;
        }
    }

}
