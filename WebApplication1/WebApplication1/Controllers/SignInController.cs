using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Text;
using WeatherResearcher.Models;
using WeatherResearcher.Services;

namespace WeatherResearcher.Controllers
{
    public class SignInController : Controller
	{
		private ApplicationContext db;
		private readonly ILogger<SignInController> _logger;

		public SignInController(ILogger<SignInController> logger, ApplicationContext ContextDb)
		{
			_logger = logger;
			db = ContextDb;
		}
		public IActionResult Authentication(string LoginToEnter = "", string PasswordToEnter = "")
		{
			var cookies = HttpContext.Response.Cookies;
			if (LoginToEnter != "" && PasswordToEnter != "")
			{
				if (db.users.Where(x => x.Login == LoginToEnter).FirstOrDefault(x => x.Password == PasswordToEnter) != null)
				{
					cookies.Delete("Login");
					cookies.Append("Login", LoginToEnter,new CookieOptions
					{
						Expires = DateTime.UtcNow.Add(TimeSpan.FromDays(7)),
						HttpOnly = true,
						Secure = true,
						SameSite = SameSiteMode.Strict
					});
					return RedirectToAction("OwnWeather", "OwnCabinet");
				}
			}

			return View();
		}
		public IActionResult Registration(string LoginToRegistr = "", string PasswordToRegistr = "")
		{
			var cookies = HttpContext.Response.Cookies;
			if (LoginToRegistr != "" && PasswordToRegistr != "")
			{
				if (db.users.Where(x => x.Login == LoginToRegistr).FirstOrDefault(x => x.Password == PasswordToRegistr) != null)
				{
					cookies.Delete("Login");
					cookies.Append("Login", LoginToRegistr, new CookieOptions
					{
						Expires = DateTime.UtcNow.Add(TimeSpan.FromDays(7)),
						HttpOnly = true,
						Secure = true,
						SameSite = SameSiteMode.Strict
					});
					return RedirectToAction("OwnWeather", "OwnCabinet");
				}
				User newUser = new User();
				newUser.Login = LoginToRegistr;
				newUser.Password = PasswordToRegistr;
				db.users.Add(newUser);
				db.SaveChanges();
			}

			return View();
		}
	}
}
