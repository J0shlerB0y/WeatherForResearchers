using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class CityAndCountry
    {
        [Key]
        public int Id { get; set; } = 0;
        public string CityTitle_en { get; set; } = "";
        public string CountryTitle_en { get; set; } = "";
    }
}