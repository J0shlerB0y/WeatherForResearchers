using Domain;
using UseCases.DTO;
using UseCases.Models;

namespace UseCases.Services
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(DataToRegistr dataToRegistr);
        Task<AuthResult> LoginAsync(DataToAuth dataToAuth);
        Task<AuthResult> LogOutAsync(string accessTokenHash);
        bool IsLoginAvailable(string login);
    }
}
