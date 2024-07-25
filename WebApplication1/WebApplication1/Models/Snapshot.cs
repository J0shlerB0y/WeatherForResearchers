using System.ComponentModel.DataAnnotations;

namespace WeatherResearcher.Models
{
	public class Snapshot: WeatherSnapshotModel
	{
		[Key]
		public int Id { get; set; }
		public int UserId { get; set; }
		public int CityId { get; set; }
	}
}
