using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
				if (cookies["Login"] != null && cookies["Password"] != null && !db.users.Where(x => x.Login
							== cookies["Login"] && passwordHandler.DecryptString(cookies["Password"]) == x.Password).IsNullOrEmpty())
				{
					SnapshotId snapshotId = await context.Request.ReadFromJsonAsync<SnapshotId>();
					int userIdForWeatherSnapshot = db.users.FirstOrDefault(x => x.Login
							== cookies["Login"] &&
							passwordHandler.DecryptString(cookies["Password"]) == x.Password)
								.Id;
					Snapshot snapshotToDelete = db.snapshots.Where(x =>
					x.UserId == userIdForWeatherSnapshot 
					).Where(
						z => z.Id == snapshotId.Id
						).First();
					if (snapshotToDelete is not null)
					{
						db.snapshots.Remove(snapshotToDelete);
						db.SaveChanges();
					}
				}
				else
				{
					var cookiesToDelete = context.Response.Cookies;
					cookiesToDelete.Delete("Login");
					cookiesToDelete.Delete("Password");
					context.Response.StatusCode = 401;
				}
			}
			await next.Invoke(context);
		}
	}
}
