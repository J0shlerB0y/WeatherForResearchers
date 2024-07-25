using System.ComponentModel.DataAnnotations;

namespace WeatherResearcher.Models
{
	public class UsersCity
	{
		[Key]
		public int Id { get; set; }
		public int CityId { get; set; }
		public int UserId { get; set; }
	}
}
