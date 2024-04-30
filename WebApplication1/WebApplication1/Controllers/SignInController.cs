﻿using Microsoft.AspNetCore.Mvc;
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
				HttpOnly = true,
				Secure = true,
				SameSite = SameSiteMode.Strict
			};
		}
		public IActionResult Authentication(string LoginToEnter = "", string PasswordToEnter = "")
		{
			var cookiesResp = HttpContext.Response.Cookies;
			var cookiesReq = HttpContext.Request.Cookies;
			if (LoginToEnter != "" && PasswordToEnter != "")
			{
				if (db.users.Where(x => x.Login == LoginToEnter) != null)
				{
					salt = (db.users
						.Where(x => x.Login == cookiesReq["Login"])
						.FirstOrDefault(x => x.Password == passwordHandler.DecryptString(cookiesReq["Password"]))
						.Salt.Split("-"))
							.Select(hex => Convert.ToByte(hex))
							.ToArray(); 
					string passwordHash = passwordHandler.HashPassword(PasswordToEnter, salt);

					cookiesResp.Delete("Login");
					cookiesResp.Append("Login", LoginToEnter, cookieOptions);
					cookiesResp.Delete("Password");
					cookiesResp.Append("Password", passwordHandler.EncryptString(passwordHash), cookieOptions);

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
				if (db.users.FirstOrDefault(x => x.Login == LoginToRegistr) == null)
				{
					new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);
					string passwordHash = passwordHandler.HashPassword(PasswordToRegistr, salt);

					cookies.Delete("Login");
					cookies.Append("Login", LoginToRegistr, cookieOptions);
					cookies.Delete("Password");
					cookies.Append("Password", passwordHandler.EncryptString(passwordHash), cookieOptions);

					User newUser = new User();
					newUser.Login = LoginToRegistr;
					newUser.Password = passwordHash;

					string saltStr = "";
					for (int i = 0; i < SaltSize-1; i++)
					{
						saltStr += salt[i].ToString() + "-";
					}
					saltStr += salt[SaltSize - 1].ToString();
					newUser.Salt = saltStr;
					db.users.Add(newUser);
					db.SaveChanges();
					return RedirectToAction("OwnWeather", "OwnCabinet");
				}
			}

			return View();
		}
	}
}
