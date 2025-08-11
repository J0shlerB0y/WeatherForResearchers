using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Services
{
    public interface IHasher
    {
        public int SaltSize { get;}
        public string Hash(string password, byte[] salt);
        public string MakeSaltStr(byte[] salt);
        public byte[] GenerateSalt();
    }
}
