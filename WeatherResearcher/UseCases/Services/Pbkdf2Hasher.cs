using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Services
{
    public class Pbkdf2Hasher : IHasher
    {
        public int SaltSize { get; private set; } = 128 / 8;
        public string Hash(string password, byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
        public string MakeSaltStr(byte[] salt)
        {
            string saltStr = "";
            for (int i = 0; i < SaltSize - 1; i++)
            {
                saltStr += salt[i].ToString() + "-";
            }
            return saltStr + salt[SaltSize - 1].ToString();

        }
        public byte[] GenerateSalt()
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);
            return salt;
        }
    }
}
