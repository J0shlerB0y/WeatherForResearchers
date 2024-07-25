using Microsoft.AspNetCore.Mvc;
using WeatherResearcher.Models;
using WeatherResearcher.Services;

namespace WeatherResearcher.MiddlewareTokens
{
	public class DeletingSnapshot
	{
		//It doesn't work without this class
		private class SnapshotId
		{
			public int Id { get; set; }
		}
		private PasswordHandler passwordHandler;
		private ApplicationContext db;
		private RequestDelegate next;
		public DeletingSnapshot(RequestDelegate next, ApplicationContext db, PasswordHandler _passwordHandler)
		{
			passwordHandler = _passwordHandler;
			this.db = db;
			this.next = next;
		}
		[HttpPost]
		public async Task InvokeAsync(HttpContext context)
		{
			if (context.Request.Path.Value.ToString() == ("/api/delete/snapshot"))
			{
				var cookies = context.Request.Cookies;
				if (cookies["Login"] != null && cookies["Password"] != null)
				{
					SnapshotId snapshotId = await context.Request.ReadFromJsonAsync<SnapshotId>();
					int userIdForWeatherSnapshot = db.users.Where(
						(x) => (x.Login == cookies["Login"]) &&
						(x.Password == passwordHandler.DecryptString(cookies["Password"]))
						).FirstOrDefault().Id;
					Snapshot snapshotToDelete = db.snapshots.Where(x =>
					x.UserId == userIdForWeatherSnapshot 
					).Where(
						z => z.Id == snapshotId.Id
						).First();
					if (snapshotToDelete != null)
					{
						db.snapshots.Remove(snapshotToDelete);
						db.SaveChanges();
					}
				}
			}
			await next.Invoke(context);
		}
	}
}
