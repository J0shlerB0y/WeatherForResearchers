using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace UseCases.DTO
{
    public static class AuthOptions
    {
        private static SymmetricSecurityKey key;
        public static SymmetricSecurityKey Key
        { 
            get
            {
                return key ?? throw new ArgumentNullException("SecretKey not configured.");
            }
            private set
            {
                key = value;
            }
        }
        private static string issuer;
        public static string Issuer
        {
            get
            {
                return issuer ?? throw new ArgumentNullException("Issuer not configured.");
            }
            private set
            {
                issuer = value;
            }
        }
        private static string audience;
        public static string Audience
        {
            get
            {
                return audience ?? throw new ArgumentNullException("Audience not configured.");
            }
            private set
            {
                audience = value;
            }
        }
        public static TimeSpan ExpiresDays { get; private set; } = TimeSpan.FromDays(1);

        public static void ConfigureOptions(IConfiguration configuration)
        {
            var secret = configuration["JwtSettings:SecretKey"] ?? throw new ArgumentNullException("JwtSettings:SecretKey not configured.");
            issuer = configuration["JwtSettings:Issuer"] ?? throw new ArgumentNullException("JwtSettings:Issuer not configured.");
            audience = configuration["JwtSettings:Audience"] ?? throw new ArgumentNullException("JwtSettings:Audience not configured.");
            ExpiresDays = TimeSpan.FromDays(double.Parse(configuration["JwtSettings:ExpiryDays"] ?? "1"));

            if (string.IsNullOrEmpty(secret) || secret.Length < 32) 
            {
                throw new ArgumentException("JWT Secret Key is missing or too short in configuration.");
            }

            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }
    }
}
