using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WeatherResearcher.MiddlewareTokens;
using WeatherResearcher.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<ApplicationContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(opt =>
{
	opt.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidIssuer = AuthOptions.ISSUER,
		ValidateAudience = true,
		ValidAudience = AuthOptions.AUDIENCE,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = AuthOptions.KEY
	};
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseMiddleware<AddingCity>();
app.UseMiddleware<DeletingCity>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "Cabinet",
	pattern: "{controller=OwnCabinet}/{action=OwnWeather}/{id?}");

app.MapControllerRoute(
	name: "SignIn",
	pattern: "{controller=SignIn}/{action=Authentication}/{id?}");

app.Run();

public struct AuthOptions
{
	public const string ISSUER = "Server"; // издатель токена
	public const string AUDIENCE = "Client"; // потребитель токена
	public static SymmetricSecurityKey KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("NatureIsOurHome"));   // ключ для шифрации
}