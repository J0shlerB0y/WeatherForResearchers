using Domain;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Cryptography;
using UseCases.DTO;
using UseCases.Models;
using UseCases.Services;

namespace Infrastructure.Controllers
{
	public class SignInController : Controller
	{

        private readonly ILogger<OwnCabinetController> _logger;
        private IJwtManager jwtManager;

        public SignInController(ILogger<OwnCabinetController> logger, IJwtManager jwtManager)
        {
            _logger = logger;
            this.jwtManager = jwtManager;
		}

		[HttpGet]
		public IActionResult Authentication()
        { return View(); }

        [HttpGet]
        public IActionResult Registration()
		{ return View(); }

		[HttpPost]
        public async Task<IActionResult> Login(
			[FromServices] IAuthService authService,
			DataToAuth dataToAuth
			)
		{
            if (!ModelState.IsValid)
            {
                return View("Authentication", dataToAuth);
            }

            AuthResult result = await authService.LoginAsync(dataToAuth);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Errors.First());
                return View("Authentication", dataToAuth);
            }

            jwtManager.SetJwt(result.Token);
            return RedirectToAction("OwnWeather", "OwnCabinet");
        }

        [HttpPost]
        public async Task<IActionResult> Register(
            [FromServices] IAuthService authService, 
            DataToRegistr dataToRegistr)
		{
            if (!ModelState.IsValid)
            {
                return View("Registration", dataToRegistr);
            }

            AuthResult result = await authService.RegisterAsync(dataToRegistr);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Errors.First());
                return View("Registration", dataToRegistr);
            }

            jwtManager.SetJwt(result.Token);
            return RedirectToAction("OwnWeather", "OwnCabinet");
        }

        public async Task<IActionResult> LogOut(
            [FromServices] IAuthService authService
            )
        {
            AuthResult result = await authService.LogOutAsync(jwtManager.GetJwt());

            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors.First());
            }


            if (
            !jwtManager.DeleteJwt()
                )
            {
                return BadRequest("Can't get User");
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpGet("check-login")]
        public IActionResult CheckLogin(
            [FromServices] IAuthService authService,
            [FromQuery] string login)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                return Json(false);
            }
            var isAvailable = authService.IsLoginAvailable(login);
            return Json(isAvailable);
        }
    }
}
