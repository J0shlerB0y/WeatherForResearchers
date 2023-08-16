using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Country
    {
        [Key]
        public long Country_id { get; set; }
        public string Title_ru { get; set; } = "";
        public string Title_en { get; set; } = "";
    }
}
