using Domain;
using Infrastructure.Builders;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Security.Claims;
using UseCases.Mutations;
using UseCases.RepoSpecifications;
using UseCases.Services;

namespace Infrastructure.Controllers
{
    public class OwnCabinetController : Controller
    {
        private readonly ILogger<OwnCabinetController> _logger;
        private IJwtManager jwtManager;

        public OwnCabinetController(ILogger<OwnCabinetController> logger, IJwtManager jwtManager)
        {
            this.jwtManager = jwtManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult OwnWeather(
            [FromServices] IReadWithAdditionalToolsRepository<UsersCity> repo,
            [FromServices] IConfiguration configuration,
            int page = 0,
            FilterCityParametr filter = null,
            SortingEnum sortingState = SortingEnum.CityAsc)
        {
            User user = jwtManager.GetUser();
            if (user == null)
            {
                return RedirectToAction("Index","Home");
            }
            return View(
                new SubscriptionPageBuilder(repo, user).
                    Filter(filter).
                    Sort(sortingState).
                    Paginate(page).
                    FindWeather(new OpenWeatherMapApi(configuration)).
                    buildModel(page,
                        filter,
                        sortingState)
                );
        }

        [HttpGet]
        public IActionResult OwnSnapshots(
            [FromServices] IReadWithAdditionalToolsRepository<Snapshot> repo,
            int page = 0,
            FilterSnapshotParametr filter = null,
            SortingEnum sortingState = SortingEnum.CityAsc)
        {
            User user = jwtManager.GetUser();
            if (!ModelState.IsValid || user == null)
            {
                return BadRequest("Can't get User");
            }

            return View(
                new SnapshotPageBuilder(repo, user).
                    Filter(filter).
                    Sort(sortingState).
                    Paginate(page).
                    buildModel(page,
                        filter,
                        sortingState));
        }

        [HttpPost]
        public async Task<IActionResult> Unsubsribe(
            [FromServices] IWriteRepository<UsersCity> repo,
            int cityId = -1)
        {
            User user = jwtManager.GetUser();
            if (cityId == -1 || user == null)
            {
                return BadRequest("Can't get User");
            }
            repo.Delete(new UsersCityCriteriaSpec(user.Id, cityId));
            await repo.SaveAsync();

            return Ok(new { success = true, message = "Unsubscription completed" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSnapshot(
            [FromServices] IWriteRepository<Snapshot> repo,
            int snapshotId = -1)
        {
            if (snapshotId == -1)
            {
                return BadRequest("Can't get User");
            }
            repo.Delete(new SnapshotCriteriaSpec(snapshotId));
            await repo.SaveAsync();

            return Ok(new { success = true, message = "Snapshot Deleted" });
        }
    }
}
