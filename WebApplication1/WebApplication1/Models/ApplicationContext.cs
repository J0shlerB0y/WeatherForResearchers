using Microsoft.EntityFrameworkCore;
namespace WebApplication1.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<City> _cities { get; set; } = null!;
        public DbSet<Country> _countries { get; set; } = null!;
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(@"Server=localhost;database=geodata;user=root;password=localroot1289host;",
                new MariaDbServerVersion(new Version(10, 3, 22)));
        }
    }
}
