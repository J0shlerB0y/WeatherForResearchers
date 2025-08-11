using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using UseCases.DTO;
using UseCases.Mutations;
using UseCases.RepoSpecifications;
using UseCases.Services;

namespace UseCases.Services
{
    public class AccessTokenHandler
    {
        IHasher hasher;
        IStringGenerator stringGenerator;
        IWriteRepository<AccessToken> writeRepo;
        IReadRepository<AccessToken> readRepo;
        public AccessTokenHandler(IHasher hasher, IStringGenerator stringGenerator, IReadRepository<AccessToken> readRepo, IWriteRepository<AccessToken> writeRepo)
        {
            this.hasher = hasher;
            this.stringGenerator = stringGenerator;
            this.writeRepo = writeRepo;
            this.readRepo = readRepo;
        }

        public User GetUserByAccessToken(string accessToken)
        {
            return readRepo.GetByName(new AccessTokenCriteriaSpec(accessToken))?
                .User;
        }

        public AccessToken MakeAccessToken(User user)
        {
            byte[] salt;
            int SaltSize = hasher.SaltSize;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);
            string accessTokenStr = hasher.Hash(stringGenerator.GenerateString(), salt);
            DateTime time = DateTime.UtcNow;

            return new AccessToken()
            {
                Hash = accessTokenStr,
                Salt = hasher.MakeSaltStr(salt),
                Time = time,
                UserId = user.Id,
            };
        }

        public async Task SaveAccessTokenAsync(AccessToken accessToken)
        {
            await writeRepo.AddAsync(accessToken);
            await writeRepo.SaveAsync();
        }

        public async Task<bool> DeleteAccessTokenAsync(string accessTokenHash)
        {
            AccessToken accessTokenToDelete = readRepo.GetByName(new AccessTokenCriteriaSpec(accessTokenHash));
            if (accessTokenToDelete == null)
            {
                return false;
            }
            writeRepo.Delete(accessTokenToDelete);
            await writeRepo.SaveAsync();
            return true;
        }

        public string GetJwtWithAccessToken(AccessToken accessToken)
        {
            var claims = new List<Claim> { new Claim("access token", accessToken.Hash), new Claim("salt", accessToken.Salt) };
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.Issuer,
                audience: AuthOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(AuthOptions.ExpiresDays),
                signingCredentials: new SigningCredentials(AuthOptions.Key, SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public List<Claim> GetClaimsOfJwt(string encodedJwt)
        {
            JwtSecurityToken decodedJwt = new JwtSecurityTokenHandler().ReadJwtToken(encodedJwt);
            return decodedJwt.Claims.ToList();
        }
    }
}
