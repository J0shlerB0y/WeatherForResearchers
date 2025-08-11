using Domain;
using Infrastructure.Mutations;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UseCases.Mutations;
using UseCases.Services;
using UseCases.DTO;
using UseCases.RepoSpecifications;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<MySqlDbContext>();

builder.Services.AddSingleton<IIncludesSpec<City>, CityWithCountrySpec>();
builder.Services.AddSingleton<IIncludesSpec<AccessToken>, AccessTokenyWithUserSpec>();
builder.Services.AddSingleton<IIncludesSpec<Snapshot>, SnapshotWithCountrySpec>();
builder.Services.AddSingleton<IIncludesSpec<UsersCity>, UsersCityWithCountrySpec>();
builder.Services.AddSingleton<IIncludesSpec<User>, UserIncludeSpec>();

builder.Services.AddScoped(typeof(IReadWithAdditionalToolsRepository<>), typeof(MySqlRepository<>));
builder.Services.AddScoped(typeof(IReadRepository<>), typeof(MySqlRepository<>));
builder.Services.AddScoped(typeof(IWriteRepository<>), typeof(MySqlRepository<>));

builder.Services.AddSingleton<IHasher, Pbkdf2Hasher>();
builder.Services.AddScoped<IAccountDataChecker, AccountDataChecker>();
builder.Services.AddSingleton<IStringGenerator, RandomStringGenerator>();

builder.Services.AddScoped<AccessTokenHandler>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtManager,JwtCookieManager>();

builder.Services.AddControllersWithViews();

AuthOptions.ConfigureOptions(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = AuthOptions.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.Key,
            ValidateIssuerSigningKey = true
        };
    });

var app = builder.Build();

//app.UseStatusCodePages(async (statusCodeContext) =>
//{
//	List<int> statusCodesToHandle = new List<int> { 310, 500, 408, 404, 401 };
//	var response = statusCodeContext.HttpContext.Response;
//	if (statusCodesToHandle.Contains(response.StatusCode))
//	{
//		response.ContentType = "text/plain; charset=UTF-8";
//		response.Redirect($"/Home/Error/{response.StatusCode}");
//	}
//});

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}");

app.MapControllerRoute(
	name: "Cabinet",
	pattern: "{controller=OwnCabinet}/{action=OwnWeather}");

app.MapControllerRoute(
	name: "SignIn",
	pattern: "{controller=SignIn}/{action=Authentication}");


app.Run();