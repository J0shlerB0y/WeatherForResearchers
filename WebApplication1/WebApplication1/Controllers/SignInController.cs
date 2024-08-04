using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using WeatherResearcher.Models;
using WeatherResearcher.Services;

namespace WeatherResearcher.Controllers
{
	public class SignInController : Controller
	{
		private ApplicationContext db;
		private PasswordHandler passwordHandler;
		private readonly ILogger<SignInController> logger;
		private CookieOptions cookieOptions;
		private const int SaltSize = 128 / 8;
		byte[] salt = new byte[SaltSize];

		public SignInController(ILogger<SignInController> _logger, ApplicationContext ContextDb, PasswordHandler _passwordHandler)
		{
			passwordHandler = _passwordHandler;
			logger = _logger;
			db = ContextDb;
			cookieOptions = new CookieOptions()
			{
				Expires = DateTime.UtcNow.Add(TimeSpan.FromDays(7)),
				Secure = true,
				SameSite = SameSiteMode.Strict
			};
		}
		public IActionResult Authentication(DataToAuth dataToAuth)
		{

			if (!ModelState.IsValid || dataToAuth == null)
			{
				return View();
			}
			var cookiesResp = HttpContext.Response.Cookies;
			if (dataToAuth.Login != null && dataToAuth.Password != null)
			{
				var userList = db.users.Where(x => x.Login.Equals(dataToAuth.Login));
				if (!userList.IsNullOrEmpty() && userList.Count() == 1)
				{
					User user = userList.First();
					// Hash the password using the user's salt
					string hashedPassword = passwordHandler.HashPassword(dataToAuth.Password, user.Salt.Split('-').Select(hex => Convert.ToByte(hex)).ToArray());
					if (hashedPassword == user.Password)
					{

						cookiesResp.Delete("Login");
						cookiesResp.Append("Login", dataToAuth.Login, cookieOptions);
						cookiesResp.Delete("Password");
						cookiesResp.Append("Password", passwordHandler.EncryptString(hashedPassword), cookieOptions);

						return RedirectToAction("OwnWeather", "OwnCabinet");
					}
					else
					{
						ViewData["WrongLoginOrPasswordMessage"] = "<br /><div id=\"errorMessage\" class=\"error-message\">Password was wrong</div><br />";
						return View();
					}
				}
				else
				{
					ViewData["WrongLoginOrPasswordMessage"] = "<br /><div id=\"errorMessage\" class=\"error-message\"><h3>Login was wrong</h3></div><br />";
					return View();
				}
			}

			return View();
		}
		public IActionResult Registration(DataToRegistr dataToRegistr)
		{
			var cookies = HttpContext.Response.Cookies;
			if (!ModelState.IsValid || dataToRegistr == null)
			{
				return View();
			}
			if (dataToRegistr.Login != null && dataToRegistr.Password != null)
			{
				if (dataToRegistr.Password.Length >=8 && dataToRegistr.Password.Length <= 50)
				{
					if (db.users.Where(x => x.Login.Equals(dataToRegistr.Login)).IsNullOrEmpty())
					{
						new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);
						string passwordHash = passwordHandler
							.HashPassword(dataToRegistr.Password, salt);

						cookies.Delete("Login");
						cookies.Append("Login", dataToRegistr.Login, cookieOptions);
						cookies.Delete("Password");
						cookies.Append("Password",
							passwordHandler
							.EncryptString(passwordHash),
							cookieOptions);

						User newUser = new User();
						newUser.Login = dataToRegistr.Login;
						newUser.Password = passwordHash;

						string saltStr = "";
						for (int i = 0; i < SaltSize - 1; i++)
						{
							saltStr += salt[i].ToString() + "-";
						}
						saltStr += salt[SaltSize - 1].ToString();
						newUser.Salt = saltStr;
						db.users.Add(newUser);
						db.SaveChanges();
						return RedirectToAction("OwnWeather", "OwnCabinet");

					}
					else
					{
						ViewData["WrongLoginOrPasswordMessage"] = "<br /><div id=\"errorMessage\" class=\"error-message\"><h3>Login already in use</h3></div><br />";
						return View();
					}
				}
				else
				{
					ViewData["WrongLoginOrPasswordMessage"] = "<br /><div id=\"errorMessage\" class=\"error-message\"><h3>Line length must be between 8 and 50 characters</h3></div><br />";
					return View();

				}
			}
			else
			{
				ViewData["WrongLoginOrPasswordMessage"] = "<br /><div id=\"errorMessage\" class=\"error-message\"><h3>Login or Password is empty</h3></div><br />";
				return View();
			}
			return View(dataToRegistr);
		}

		[AcceptVerbs("Get", "Post")]
		public IActionResult CheckLogin(string login)
		{
			if (db.users.Where(x => EF.Functions.Collate(x.Login, "utf8mb4_bin").Equals(login)) is null)
			{
				return Json(true);
			}
			return Json(false);
		}
	}
}
