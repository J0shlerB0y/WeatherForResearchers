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
				CityId cityId = await context.Request.ReadFromJsonAsync<CityId>();
				UsersCity usersCityToDelete = db.userscities.FirstOrDefault(x => x.CityId == cityId.Id);
				db.userscities.Remove(usersCityToDelete);
				db.SaveChanges();
			}
			await next.Invoke(context);
		}
	}
}
