using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class City : IEntity
    {
        [Key]
        public int Id { get; set; } = 0;
        public string CityTitle_en { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}