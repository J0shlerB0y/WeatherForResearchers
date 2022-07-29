using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class CityAndCountry
    {
        [Key]
        public string CityTitle_ru { get; set; } = "";
        public string CountryTitle_ru { get; set; } = "";
        public string CountryTitle_en { get; set; } = "";
        public CityAndCountry(string CityTitle_ru, string CountryTitle_ru, string CountryTitle_en)
        {
            this.CityTitle_ru = CityTitle_ru;
            this.CountryTitle_ru = CountryTitle_ru;
            this.CountryTitle_en = CountryTitle_en;  
        }
    }
}
