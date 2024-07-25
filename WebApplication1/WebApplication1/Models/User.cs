﻿using System.ComponentModel.DataAnnotations;

namespace WeatherResearcher.Models
{
	public class User
	{
		[Key]
		public int Id { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public string Salt { get; set; }
	}
}
