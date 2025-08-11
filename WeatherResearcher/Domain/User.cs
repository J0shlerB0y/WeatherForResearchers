using System.ComponentModel.DataAnnotations;

namespace Domain
{
	public class User : IEntity
    {
		[Key]
		public int Id { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public string Salt { get; set; }

		public User IfNotNull()
		{
			if ((this is null) || (Login == "") || (Password == "") || (Salt == ""))
			{
				throw new Exception(" User doesn't exist or incomplete. ");
			}
			return this;
		}
	}
}
