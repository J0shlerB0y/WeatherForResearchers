using Microsoft.AspNetCore.Mvc;
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
		private ApplicationContext db;
		private RequestDelegate next;
		public AddingCity(RequestDelegate next, ApplicationContext db)
		{
			this.db = db;
			this.next = next;
		}
		[HttpPost]
		public async Task InvokeAsync(HttpContext context)
		{
			if (context.Request.Path.Value.ToString() == ("/api/add/user"))
			{
				var cookies = context.Request.Cookies;
				if (cookies["Login"] != null && cookies["Password"] != null)
				{
					CityId cityId = await context.Request.ReadFromJsonAsync<CityId>();
					UsersCity userCity = new UsersCity()
					{
						CityId = cityId.Id,
						UserId =
								db.users.Where(x => x.Login == cookies["Login"])
							.Where(x => x.Password == cookies["Password"]).FirstOrDefault().Id
					};
					db.userscities.Add(userCity);
					db.SaveChanges();
				}
			}
			await next.Invoke(context);
		}
	}
}
