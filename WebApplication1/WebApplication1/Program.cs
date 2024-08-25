using WeatherResearcher.MiddlewareTokens;
using WeatherResearcher.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<ApplicationContext>();
builder.Services.AddSingleton<PasswordHandler>();
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseStatusCodePages(async (statusCodeContext) =>
{
	List<int> statusCodesToHandle = new List<int> { 310, 500, 408, 404, 401 };
	var response = statusCodeContext.HttpContext.Response;
	if (statusCodesToHandle.Contains(response.StatusCode))
	{
		response.ContentType = "text/plain; charset=UTF-8";
		response.Redirect($"/Home/Error/{response.StatusCode}");
	}
});

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseMiddleware<AddingCity>();
app.UseMiddleware<DeletingCity>();

app.UseMiddleware<AddingSnapshot>();
app.UseMiddleware<DeletingSnapshot>();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}");

app.MapControllerRoute(
	name: "Cabinet",
	pattern: "{controller=OwnCabinet}/{action=OwnWeather}");

app.MapControllerRoute(
	name: "SignIn",
	pattern: "{controller=SignIn}/{action=Authentication}");

app.MapControllerRoute(
	name: "Error",
	pattern: "{controller=Home}/{action=Error}/{statusCode?}");

app.Run();