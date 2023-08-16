using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class City
    {
        [Key]
        public long City_id { get; set; }
        public string Title_ru { get; set; } = "";
        public long Country_id { get; set; }
    }
}
