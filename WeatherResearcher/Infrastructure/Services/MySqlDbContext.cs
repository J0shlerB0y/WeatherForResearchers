using Microsoft.EntityFrameworkCore;
using Infrastructure.Models;
using Domain;

namespace Infrastructure.Services
{
    public class MySqlDbContext : DbContext
    {
        public DbSet<Country> countries { get; set; } = null!;
        public DbSet<City> cities { get; set; } = null!;
        public DbSet<User> users { get; set; } = null!;
        public DbSet<UsersCity> userscities { get; set; } = null!;
		public DbSet<Snapshot> snapshots { get; set; } = null!;
        public DbSet<AccessToken> accesstoken { get; set; } = null!;

        string connectionString;
        public MySqlDbContext(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString"];
            if (connectionString == null) 
                throw new ArgumentNullException(nameof(connectionString));
            Database.EnsureCreated();
		}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString,
                new MySqlServerVersion(new Version(9, 0, 1)),
                mySqlOptions =>
                {
                    mySqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(3),
                        errorNumbersToAdd: null);
                }).EnableSensitiveDataLogging();
        }
    }
}
