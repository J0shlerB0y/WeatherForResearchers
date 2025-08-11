using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Services
{
    public interface IStringGenerator
    {
        public string GenerateString(int length = 16, char[]? allowedChars = null);
    }
}
