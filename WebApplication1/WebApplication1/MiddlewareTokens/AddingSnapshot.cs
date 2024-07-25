using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using WeatherResearcher.Models;
using WeatherResearcher.Services;

namespace WeatherResearcher.MiddlewareTokens
{
	public class AddingSnapshot
	{
		//It doesn't work without this class
		private class WeatherSnapshotForAddingModel : WeatherSnapshotModel
		{
			public int CityId { get; set; }
		}
		private PasswordHandler passwordHandler;
		private ApplicationContext db;
		private RequestDelegate next;
		public AddingSnapshot(RequestDelegate next, ApplicationContext db, PasswordHandler _passwordHandler)
		{
			passwordHandler = _passwordHandler;
			this.db = db;
			this.next = next;
		}
		[HttpPost]
		public async Task InvokeAsync(HttpContext context)
		{
			if (context.Request.Path.Value.ToString() == ("/api/add/snapshot"))
			{
				var cookies = context.Request.Cookies;
				if (cookies["Login"] != null && cookies["Password"] != null)
				{
					WeatherSnapshotForAddingModel weatherSnapshotForAddingModel = await context.Request.ReadFromJsonAsync<WeatherSnapshotForAddingModel>();
					string decrPasHash = passwordHandler.DecryptString(cookies["Password"]);
					Snapshot snapshot = new Snapshot()
					{
						CityId = weatherSnapshotForAddingModel.CityId,
						UserId = db.users
								.Where(x => x.Login == cookies["Login"])
								.FirstOrDefault(x => x.Password == passwordHandler.DecryptString(cookies["Password"]))
								.Id,
						Time = weatherSnapshotForAddingModel.Time,
						weather = weatherSnapshotForAddingModel.weather,
						icon = weatherSnapshotForAddingModel.icon,
						temp = weatherSnapshotForAddingModel.temp,
						temp_feels_like = weatherSnapshotForAddingModel.temp_feels_like,
						temp_min = weatherSnapshotForAddingModel.temp_min,
						temp_max = weatherSnapshotForAddingModel.temp_max,
						pressure = weatherSnapshotForAddingModel.pressure,
						humidity = weatherSnapshotForAddingModel.humidity,
						wind_speed = weatherSnapshotForAddingModel.wind_speed
					};
					db.snapshots.Add(snapshot);
					db.SaveChanges();
				}
			}
			await next.Invoke(context);
		}
	}
}
