using Microsoft.AspNetCore.Mvc;
using WeatherResearcher.Models;
using WeatherResearcher.Services;

namespace WeatherResearcher.MiddlewareTokens
{
	public class DeletingCity
	{
		private class CityId
		{
			public int Id { get; set; }
		}
		private PasswordHandler passwordHandler;
		private ApplicationContext db;
		private RequestDelegate next;
		public DeletingCity(RequestDelegate next, ApplicationContext db, PasswordHandler _passwordHandler)
		{
			passwordHandler = _passwordHandler;
			this.db = db;
			this.next = next;
		}
		[HttpPost]
		public async Task InvokeAsync(HttpContext context)
		{
			if (context.Request.Path.Value.ToString() == "/api/delete/user")
			{
				var cookies = context.Request.Cookies;
				CityId cityId = await context.Request.ReadFromJsonAsync<CityId>();
				if (cookies["Login"] != null && cookies["Password"] != null)
				{
					UsersCity usersCityToDelete = db.userscities.Where(
						z => z.UserId == db.users
							.Where(x => x.Login == cookies["Login"]).FirstOrDefault(x => x.Password == passwordHandler.DecryptString(cookies["Password"])).Id
						).FirstOrDefault(
						x => x.CityId == cityId.Id
						);
					if (usersCityToDelete != null)
					{
						db.userscities.Remove(usersCityToDelete);
						db.SaveChanges();
					}
				}
				else
				{
					foreach (var cooke in cookies.ToList())
					{
						if (cooke.Key != "Login")
						{
							if (cooke.Value == cityId.Id.ToString())
							{
								context.Response.Cookies.Delete(cooke.Key);
							}
						}
					}
				}
			}
			await next.Invoke(context);
		}
	}
}
