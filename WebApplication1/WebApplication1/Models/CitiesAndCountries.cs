using System.ComponentModel.DataAnnotations;

namespace WeatherResearcher.Models
{
    public class CitiesAndCountries
    {
        [Key]
        public int Id { get; set; } = 0;
        public string CityTitle_en { get; set; } = "";
        public string CountryTitle_en { get; set; } = "";
    }
}