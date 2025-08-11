using System.ComponentModel.DataAnnotations;

namespace Domain
{
	public class UsersCity : IEntity
    {
		[Key]
		public int Id { get; set; }
		public int CityId { get; set; }
		public int UserId { get; set; }

        public City City { get; set; }
        public User User { get; set; }
    }
}
