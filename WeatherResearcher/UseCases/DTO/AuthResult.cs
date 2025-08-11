using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.DTO
{
    public class AuthResult
    {
        public bool IsSuccess { get; private set; }
        public string? Token { get; private set; }
        public IEnumerable<string> Errors { get; private set; }

        public AuthResult(bool isSuccesss, string? token, IEnumerable<string> errors)
        {
            IsSuccess = isSuccesss;
            Token = token;
            Errors = errors;
        }

        public static AuthResult Success(string token)
        {
            return new(true, token, Enumerable.Empty<string>());
        }

        public static AuthResult Success()
        {
            return new(true, null, Enumerable.Empty<string>());
        }

        public static AuthResult Failure(IEnumerable<string> errors)
        {
            return new(false, null, errors);
        }

        public static AuthResult Failure(string error)
        {
            return new(false, null, new[] {error});
        }
    }
}
