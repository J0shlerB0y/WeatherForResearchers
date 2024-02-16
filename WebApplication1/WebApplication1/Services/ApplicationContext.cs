﻿using Microsoft.EntityFrameworkCore;
using WeatherResearcher.Models;

namespace WeatherResearcher.Services
{
    public class ApplicationContext : DbContext
    {
        public DbSet<CityAndCountry> citiesAndCountries { get; set; } = null!;
        public DbSet<User> users { get; set; } = null!;
        public DbSet<UsersCity> userscities { get; set; } = null!;
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(@"Server=localhost;database=geodatamaincopy;user=root;password=localroot1289host;",
                new MariaDbServerVersion(new Version(10, 3, 22)));
        }
    }
}