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
		private ApplicationContext db;
		private RequestDelegate next;
		public DeletingCity(RequestDelegate next, ApplicationContext db)
		{
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
				UsersCity usersCityToDelete = db.userscities.Where(
					z => z.UserId == db.users
						.FirstOrDefault(x => x.Login == cookies["Login"]).Id
					).FirstOrDefault(
					x => x.CityId == cityId.Id
					);
				if (usersCityToDelete != null)
				{
					db.userscities.Remove(usersCityToDelete);
					db.SaveChanges();
				}
			}
			await next.Invoke(context);
		}
	}
}
