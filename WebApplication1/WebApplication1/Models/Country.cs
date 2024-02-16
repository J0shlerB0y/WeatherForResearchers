using System.ComponentModel.DataAnnotations;

namespace WeatherResearcher.Models
{
    public class Country
    {
        [Key]
        public long Country_id { get; set; }
        public string Title_ru { get; set; } = "";
        public string Title_en { get; set; } = "";
    }
}
