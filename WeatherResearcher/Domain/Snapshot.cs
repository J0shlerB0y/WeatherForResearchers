using System.ComponentModel.DataAnnotations;

namespace Domain
{
	public class Snapshot: WeatherWithTimeAndCity, IEntity
	{
		[Key]
		public int Id { get; set; }

		public int UserId { get; set; }
        public User User { get; set; }
    }
}
