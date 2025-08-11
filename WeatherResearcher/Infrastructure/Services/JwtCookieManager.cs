using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using UseCases.DTO;
using UseCases.Mutations;
using UseCases.Services;

namespace Infrastructure.Services
{
    public class JwtCookieManager : IJwtManager
    {
        private readonly HttpContext httpContext;
        private AccessTokenHandler accessTokenHandler;
        public JwtCookieManager(IHttpContextAccessor httpContext, AccessTokenHandler accessTokenHandler)
        {
            this.httpContext = httpContext.HttpContext ?? throw new ArgumentNullException(nameof(httpContext.HttpContext));
            this.accessTokenHandler = accessTokenHandler ?? throw new ArgumentNullException(nameof(accessTokenHandler));
        }

        public bool DeleteJwt()
        {
            if (httpContext.Request.Cookies[AccessToken.deffaultAccessTokenName].IsNullOrEmpty())
            {
                return false;
            }
            httpContext.Response.Cookies.Delete(AccessToken.deffaultAccessTokenName);
            return true;

        }

        public string GetJwt()
        {
            string encodedJwt = httpContext.Request.Cookies[AccessToken.deffaultAccessTokenName];

            List<Claim> claims = accessTokenHandler.GetClaimsOfJwt(encodedJwt);
            return claims[0].Value;
        }

        public User GetUser()
        {
            string encodedJwt = httpContext.Request.Cookies[AccessToken.deffaultAccessTokenName];

            if (encodedJwt == null)
            {
                return null;
            }
            List<Claim> claims = accessTokenHandler.GetClaimsOfJwt(encodedJwt);
            string accessToken = claims[0].Value;
            User user = accessTokenHandler.GetUserByAccessToken(accessToken);
            if (user == null)
            {
                httpContext.Response.Cookies.Delete(AccessToken.deffaultAccessTokenName);
            }
            return user;
        }

        public void SetJwt(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.Add(AuthOptions.ExpiresDays),
                Secure = true,
                SameSite = SameSiteMode.Strict
            };
            httpContext.Response.Cookies.Append(AccessToken.deffaultAccessTokenName, token, cookieOptions);
        }

    }
}

