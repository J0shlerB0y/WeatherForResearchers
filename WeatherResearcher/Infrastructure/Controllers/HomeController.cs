using Domain;
using Infrastructure.Builders;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UseCases.Mutations;
using UseCases.Services;

namespace Infrastructure.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, AccessTokenHandler accessTokenHandler)
        {
            _logger = logger;
        }

        public IActionResult Index(
            [FromServices] IReadWithAdditionalToolsRepository<City> repo, 
            [FromServices] IConfiguration configuration,
            int page = 0,
            FilterCityParametr filter = null,
            SortingEnum sortingState = SortingEnum.CityAsc)
        {
            return View(
                new IndexPageBuilder(repo).
                    Filter(filter).
                    Sort(sortingState).
                    Paginate(page).
                    FindWeather(new OpenWeatherMapApi(configuration)).
                    buildModel(page,
                        filter,
                        sortingState));
        }


        [HttpPost]
        public async Task<IActionResult> Subscribe(
            [FromServices] IWriteRepository<UsersCity> repo,
            [FromServices] IJwtManager resolver,
            int cityId = -1)
        {
            User user = resolver.GetUser();
            if (cityId == -1 || user == null)
            {
                return BadRequest("Can't get User");
            }
            repo.AddAsync(
                new UsersCity()
                {
                    CityId = cityId,
                    UserId = user.Id
                }
            );
            await repo.SaveAsync();

            return Ok(new { success = true, message = "Subscription completed" });
        }

        [HttpPost]
        public async Task<IActionResult> Snapshot(
            [FromServices] IWriteRepository<Snapshot> repo,
            [FromServices] IJwtManager resolver,
            int cityId,
            Weather weather)
        {
            User user = resolver.GetUser();
            if (!ModelState.IsValid || user == null)
            {
                return BadRequest("Can't get User");
            }

            Snapshot snapshot = weather.ParseToSnapshot();
            snapshot.Time = DateTime.Now;
            snapshot.CityId = cityId;
            snapshot.UserId = user.Id;

            repo.AddAsync(snapshot);
            await repo.SaveAsync();

            return Ok(new { success = true, message = "Snapshot completed" });
        }
    }
}