using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
	public class AuthorizationController : Controller
	{
		private ApplicationContext db;
		private ForAuthorizationViewModel viewModel;
		private readonly ILogger<AuthorizationController> _logger;

		public AuthorizationController(ILogger<AuthorizationController> logger, ApplicationContext ContextDb)
		{
			_logger = logger;
			db = ContextDb;
		}
		public IActionResult Authorization(string LoginToRegistr = "",string PasswordToRegistr = "", string LoginToEnter = "", string PasswordToEnter = "")
		{
			var cookies = HttpContext.Response.Cookies;
			if (LoginToEnter != "" && PasswordToEnter != "")
			{
				if (db.users.Where(x => x.Login == LoginToEnter).Where(x => x.Password == PasswordToEnter).FirstOrDefault() != null)
				{
					cookies.Append("Login", LoginToEnter);
					cookies.Append("Password", PasswordToEnter);
					return RedirectToAction("OwnCabinet", "Other");
				}
			}

			if (LoginToRegistr != "" && PasswordToRegistr != "")
			{
				User newUser = new User();
				newUser.Login = LoginToRegistr;
				newUser.Password = PasswordToRegistr;
				db.users.Add(newUser);
				db.SaveChanges();
				cookies.Append("Login", LoginToRegistr);
				cookies.Append("Password", PasswordToRegistr);
			}

			return View(viewModel);
		}
	}
}
