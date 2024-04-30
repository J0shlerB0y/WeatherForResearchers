using WeatherResearcher.MiddlewareTokens;
using WeatherResearcher.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<ApplicationContext>();
builder.Services.AddSingleton<PasswordHandler>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

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