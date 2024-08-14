using Microsoft.EntityFrameworkCore;
using WeatherResearcher.Models;

namespace WeatherResearcher.Services
{
    public class ApplicationContext : DbContext
    {
        public DbSet<CitiesAndCountries> citiesandcountries { get; set; } = null!;
        public DbSet<User> users { get; set; } = null!;
        public DbSet<UsersCity> userscities { get; set; } = null!;
		public DbSet<Snapshot> snapshots { get; set; } = null!;
		public ApplicationContext()
        {
            Database.EnsureCreated();
		}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(@"Server=webapplication1-db-1;Port=3306;Database=weather_researcher;Uid=root;Pwd=localroot1289host;",
                new MySqlServerVersion(new Version(9, 0, 1)),
        mySqlOptions =>
        {
            mySqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(3),
                errorNumbersToAdd: null);
        });
        }
    }
}
