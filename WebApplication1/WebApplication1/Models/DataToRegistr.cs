using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WeatherResearcher.Models
{
	public class DataToRegistr
	{
		[Remote(action: "CheckLogin", controller: "SignIn", ErrorMessage = "Login already in use.")]
		[Required(ErrorMessage = "Name not specified.")]
		public string Login { get; set; } = string.Empty;

		[StringLength(50, MinimumLength = 8, ErrorMessage = "Line length must be between 8 and 50 characters.")]
		[Required(ErrorMessage = "Password not specified.")]
		public string Password { get; set; } = string.Empty;

		//[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Incorrect address.")]
		//[Required(ErrorMessage = "Email not specified")]
		//public string Email { get; set; } = string.Empty;
	}
}
