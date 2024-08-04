using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WeatherResearcher.Models;
using WeatherResearcher.Services;

namespace WeatherResearcher.MiddlewareTokens
{
	public class AddingCity
	{
		//It doesn't work without this class
		private class CityId
		{
			public int Id { get; set; }
		}
		private PasswordHandler passwordHandler;
		private ApplicationContext db;
		private RequestDelegate next;
		public AddingCity(RequestDelegate next, ApplicationContext db, PasswordHandler _passwordHandler)
		{
			passwordHandler = _passwordHandler;
			this.db = db;
			this.next = next;
		}
		[HttpPost]
		public async Task InvokeAsync(HttpContext context)
		{
			if (context.Request.Path.Value.ToString() == ("/api/add/user"))
			{
				var cookies = context.Request.Cookies;
				if (cookies["Login"] != null && cookies["Password"] != null && !db.users.Where(x => x.Login
							== cookies["Login"] && passwordHandler.DecryptString(cookies["Password"]) == x.Password).IsNullOrEmpty())
				{
					CityId cityId = await context.Request.ReadFromJsonAsync<CityId>();
					string decrPasHash = passwordHandler.DecryptString(cookies["Password"]);
					UsersCity userCity = new UsersCity()
					{
						CityId = cityId.Id,
						UserId = db.users.FirstOrDefault(x => x.Login
							== cookies["Login"] &&
							passwordHandler.DecryptString(cookies["Password"]) == x.Password)
								.Id
					};
					if (userCity is not null)
					{
						db.userscities.Add(userCity);
						db.SaveChanges();
					}
				}
				else
				{
					var cookiesToDelete = context.Response.Cookies;
					cookiesToDelete.Delete("Login");
					cookiesToDelete.Delete("Password");
				}
			}
			await next.Invoke(context);
		}
	}
}