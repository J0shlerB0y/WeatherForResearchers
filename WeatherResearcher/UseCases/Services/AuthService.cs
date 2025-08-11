using Domain;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using UseCases.DTO;
using UseCases.Models;
using UseCases.Mutations;
using UseCases.RepoSpecifications;
using UseCases.Services;

namespace UseCases.Services
{
	public class AuthService : IAuthService
	{
		private AccessTokenHandler tokenHandler;
		private IHasher hasher;

        private const int SaltSize = 128 / 8;
		byte[] salt = new byte[SaltSize];

        IReadRepository<User> readUserRepo;
        IWriteRepository<User> writeUserRepo;

        public AuthService(AccessTokenHandler accessTokenHandler, IAccountDataChecker accountDataChecker,IHasher hasher, IReadRepository<User> readUserRepo, IWriteRepository<User> writeUserRepo)
		{
			this.tokenHandler = accessTokenHandler;
			this.hasher = hasher;

            this.readUserRepo = readUserRepo;
            this.writeUserRepo = writeUserRepo;
        }

        public async Task<AuthResult> LoginAsync(DataToAuth credentials)
        {
            var spec = new UserCriteriaSpec(credentials.Login);
            User user = readUserRepo.GetByName(spec);

            if (user == null)
            {
                return AuthResult.Failure("Invalid login or password");
            }

            byte[] saltBytes = user.Salt.Split('-').Select(hex => Convert.ToByte(hex)).ToArray();
            string providedPasswordHash = hasher.Hash(credentials.Password, saltBytes);

            if (providedPasswordHash != user.Password)
            {
                return AuthResult.Failure("Invalid login or password");
            }

            string jwtToken = await HandleUserToJwt(user);

            return AuthResult.Success(jwtToken);
        }

        public async Task<AuthResult> RegisterAsync(DataToRegistr dataToRegistr)
        {
            if (dataToRegistr.Password.Length < 8 || dataToRegistr.Password.Length > 50)
            {
                return AuthResult.Failure("Password length must be between 8 and 50 characters");
            }

            if (!IsLoginAvailable(dataToRegistr.Login))
            {
                return AuthResult.Failure("This login is already in use");
            }

            var salt = hasher.GenerateSalt();
            var passwordHash = hasher.Hash(dataToRegistr.Password, salt);
            var saltString = hasher.MakeSaltStr(salt);

            var newUser = new User
            {
                Login = dataToRegistr.Login,
                Password = passwordHash,
                Salt = saltString
            };

            await writeUserRepo.AddAsync(newUser);
            await writeUserRepo.SaveAsync();

            string jwtToken = await HandleUserToJwt(newUser);

            return AuthResult.Success(jwtToken);
        }

        public async Task<string> HandleUserToJwt(User user)
        {
            AccessToken accessToken = tokenHandler.MakeAccessToken(user);

            await tokenHandler.SaveAccessTokenAsync(accessToken);
            return tokenHandler.GetJwtWithAccessToken(accessToken);
        }

        public bool IsLoginAvailable(string login)
        {
            var spec = new UserCriteriaSpec(login);
            var user = readUserRepo.GetByName(spec);
            return user == null;
        }


        public async Task<AuthResult> LogOutAsync(string accessTokenHash)
        {
            bool isSuccess = await tokenHandler.DeleteAccessTokenAsync(accessTokenHash);
            if (!isSuccess)
            {
                return AuthResult.Failure("This access token invalid");
            }
            return AuthResult.Success();
        }
    }
}
