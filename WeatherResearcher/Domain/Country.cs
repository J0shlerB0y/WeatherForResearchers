using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    [Table("countries")]
    public class Country : IEntity
    {
        [Key]
        public int Id { get; set; } = 0;
        public string CountryTitle_en { get; set; } = "";

        public List<City> Cities { get; set; }
    }
}