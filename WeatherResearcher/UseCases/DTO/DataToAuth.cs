using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace UseCases.Models
{
    public class DataToAuth
	{
		[Required(ErrorMessage = "Name not specified.")]
        public string Login { get; set; } = string.Empty;

		[Required(ErrorMessage = "Password not specified.")]
        public string Password { get; set; } = string.Empty;
	}
}
