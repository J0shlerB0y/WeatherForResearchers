﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WeatherResearcher.Models;
using WeatherResearcher.Services;

namespace WeatherResearcher.Controllers
{
	public class OwnCabinetController : AbstractWeatherShowerController
	{
		private readonly ILogger<HomeController> _logger;
		private PasswordHandler passwordHandler;

		public OwnCabinetController(ILogger<HomeController> logger, ApplicationContext ContextDb, PasswordHandler _passwordHandler)
		{
			passwordHandler = _passwordHandler;
			_logger = logger;
			db = ContextDb;
			this.passwordHandler = passwordHandler;
		}
		public async Task<IActionResult> OwnWeather(int page = 0,
			FilterViewModel filter = null,
			SortingEnum sortingState = SortingEnum.CityAsc)
		{
			citiesAndCountries = Enumerable.Empty<CitiesAndCountries>().AsQueryable();
			//fetching and recycling cookies

			var cookies = HttpContext.Request.Cookies;

			if (cookies["Login"] != null && cookies["Password"] != null && !db.users.Where(x => x.Login
							== cookies["Login"] && passwordHandler.DecryptString(cookies["Password"]) == x.Password).IsNullOrEmpty())
			{
				isLogedIn = true;
				citiesAndCountries = db.userscities.Join(
					db.users.Where(x => x.Login
							== cookies["Login"] && 
							passwordHandler.DecryptString(cookies["Password"]) == x.Password),
					x => x.UserId,
					y => y.Id,
					(x, y) => x.CityId
					).Join(db.citiesandcountries,
					x => x,
					y => y.Id,
					(x, y) => 
					new CitiesAndCountries { Id = x, CityTitle_en = y.CityTitle_en, CountryTitle_en = y.CountryTitle_en }
					);
			}
			else
			{
				var cookiesToDelete = HttpContext.Response.Cookies;
				cookiesToDelete.Delete("Login");
				cookiesToDelete.Delete("Password");
				isLogedIn = false;
				foreach (var cooke in cookies.ToList())
				{
					if (cooke.Key != "Login" && cooke.Key != " Login" && cooke.Key != "Password" && cooke.Key != " Password")
					{
						citiesAndCountries = citiesAndCountries.Append<CitiesAndCountries>(db.citiesandcountries.FirstOrDefault(x => x.Id.ToString() == cooke.Value));
					}
				}
			}

			return await PostWeather(page,filter,sortingState);
		}
		public async Task<IActionResult> OwnSnapshots(int page = 0,
			FilterForSnapshotViewModel filter = null,
			WeatherSnapshotModel topWeather = null,
			WeatherSnapshotModel bottomWeather = null,
			SortingEnum sortingState = SortingEnum.CityAsc)
		{
			filter.TopWeather = topWeather;
			filter.BottomWeather = bottomWeather;

			IQueryable<SnapshotWithCityModel> snapshotsWithCitiesAndCountries = Enumerable.Empty<SnapshotWithCityModel>().AsQueryable();

			//fetching and recycling cookies

			var cookies = HttpContext.Request.Cookies;

			if (cookies["Login"] != null && cookies["Password"] != null)
			{
				snapshotsWithCitiesAndCountries = db.snapshots.Join(
					db.users.Where(x => x.Login
							== cookies["Login"] &&
							passwordHandler.DecryptString(cookies["Password"]) == x.Password),
					x => x.UserId,
					y => y.Id,
					(x, y) => new { x.CityId, x.weather, x.icon, 
						x.temp, x.temp_feels_like, x.temp_min, x.temp_max, 
						x.pressure, x.humidity, x.wind_speed, x.Id, x.Time}
					).Join(db.citiesandcountries,
					x => x.CityId,
					y => y.Id,
					(x, y) =>
					new SnapshotWithCityModel { CityId = x.CityId, 
						weather = x.weather, icon = x.icon, 
						temp = x.temp, temp_feels_like = x.temp_feels_like, temp_min = x.temp_min, temp_max = x.temp_max, 
						pressure = x.pressure, humidity = x.humidity, wind_speed = x.wind_speed, 
						Id = x.Id, 
						CityTitle_en = y.CityTitle_en, CountryTitle_en = y.CountryTitle_en,Time = x.Time}
					);
			}
			else
			{
				RedirectToAction("Authentication", "SignIn");
			}
			snapshotsWithCitiesAndCountries = snapshotsWithCitiesAndCountries.Where(c =>
				(String.IsNullOrEmpty(filter.City) || filter.City == c.CityTitle_en) &&
				(String.IsNullOrEmpty(filter.Country) || filter.Country == c.CountryTitle_en) &&

				(filter.TopWeather.weather == "" || filter.TopWeather.weather == c.weather) &&

				(filter.TopWeather.Time == null || filter.TopWeather.Time <= c.Time) &&
				(filter.TopWeather.temp == null || filter.TopWeather.temp <= c.temp) &&
				(filter.TopWeather.temp_feels_like == null || filter.TopWeather.temp_feels_like <= c.temp_feels_like) &&
				(filter.TopWeather.temp_min == null || filter.TopWeather.temp_min <= c.temp_min) &&
				(filter.TopWeather.temp_max == null || filter.TopWeather.temp_max <= c.temp_max) &&
				(filter.TopWeather.pressure == null || filter.TopWeather.pressure <= c.pressure) &&
				(filter.TopWeather.humidity == null || filter.TopWeather.humidity <= c.humidity) &&
				(filter.TopWeather.wind_speed == null || filter.TopWeather.wind_speed <= c.wind_speed) &&

				(filter.BottomWeather.Time == null || filter.BottomWeather.Time >= c.Time) &&
				(filter.BottomWeather.temp == null || filter.BottomWeather.temp >= c.temp) &&
				(filter.BottomWeather.temp_feels_like == null || filter.BottomWeather.temp_feels_like >= c.temp_feels_like) &&
				(filter.BottomWeather.temp_min == null || filter.BottomWeather.temp_min >= c.temp_min) &&
				(filter.BottomWeather.temp_max == null || filter.BottomWeather.temp_max >= c.temp_max) &&
				(filter.BottomWeather.pressure == null || filter.BottomWeather.pressure >= c.pressure) &&
				(filter.BottomWeather.humidity == null || filter.BottomWeather.humidity >= c.humidity) &&
				(filter.BottomWeather.wind_speed == null || filter.BottomWeather.wind_speed >= c.wind_speed)
			);



			//sorting
			switch (sortingState)
			{
				case SortingEnum.CityAsc:
					snapshotsWithCitiesAndCountries = snapshotsWithCitiesAndCountries.OrderBy(s => s.CityTitle_en);
					break;
				case SortingEnum.CountryAsc:
					snapshotsWithCitiesAndCountries = snapshotsWithCitiesAndCountries.OrderBy(s => s.CountryTitle_en);
					break;

				case SortingEnum.TempAsc:
					snapshotsWithCitiesAndCountries = snapshotsWithCitiesAndCountries.OrderBy(s => s.temp);
					break;
				case SortingEnum.TempFeelsLikeAsc:
					snapshotsWithCitiesAndCountries = snapshotsWithCitiesAndCountries.OrderBy(s => s.temp_feels_like);
					break;
				case SortingEnum.TempMinAsc:
					snapshotsWithCitiesAndCountries = snapshotsWithCitiesAndCountries.OrderBy(s => s.temp_min);
					break;
				case SortingEnum.TempMaxAsc:
					snapshotsWithCitiesAndCountries = snapshotsWithCitiesAndCountries.OrderBy(s => s.temp_max);
					break;
				case SortingEnum.PressureAsc:
					snapshotsWithCitiesAndCountries = snapshotsWithCitiesAndCountries.OrderBy(s => s.pressure);
					break;
				case SortingEnum.HumidityAsc:
					snapshotsWithCitiesAndCountries = snapshotsWithCitiesAndCountries.OrderBy(s => s.humidity);
					break;
				case SortingEnum.WindSpeedAsc:
					snapshotsWithCitiesAndCountries = snapshotsWithCitiesAndCountries.OrderBy(s => s.wind_speed);
					break;


				case SortingEnum.CityDesc:
					snapshotsWithCitiesAndCountries = snapshotsWithCitiesAndCountries.OrderByDescending(s => s.CityTitle_en);
					break;
				case SortingEnum.CountryDesc:
					snapshotsWithCitiesAndCountries = snapshotsWithCitiesAndCountries.OrderByDescending(s => s.CountryTitle_en);
					break;

				case SortingEnum.TempDesc:
					snapshotsWithCitiesAndCountries = snapshotsWithCitiesAndCountries.OrderByDescending(s => s.temp);
					break;
				case SortingEnum.TempFeelsLikeDesc:
					snapshotsWithCitiesAndCountries = snapshotsWithCitiesAndCountries.OrderByDescending(s => s.temp_feels_like);
					break;
				case SortingEnum.TempMinDesc:
					snapshotsWithCitiesAndCountries = snapshotsWithCitiesAndCountries.OrderByDescending(s => s.temp_min);
					break;
				case SortingEnum.TempMaxDesc:
					snapshotsWithCitiesAndCountries = snapshotsWithCitiesAndCountries.OrderByDescending(s => s.temp_max);
					break;
				case SortingEnum.PressureDesc:
					snapshotsWithCitiesAndCountries = snapshotsWithCitiesAndCountries.OrderByDescending(s => s.pressure);
					break;
				case SortingEnum.HumidityDesc:
					snapshotsWithCitiesAndCountries = snapshotsWithCitiesAndCountries.OrderByDescending(s => s.humidity);
					break;
				case SortingEnum.WindSpeedDesc:
					snapshotsWithCitiesAndCountries = snapshotsWithCitiesAndCountries.OrderByDescending(s => s.wind_speed);
					break;
			}

			count = snapshotsWithCitiesAndCountries.Count();

			List<SnapshotWithCityModel> pageSnapshotsWithCitiesAndCountries = snapshotsWithCitiesAndCountries.Skip(page * pageSize).Take(pageSize).ToList();

			ForWeatherSnapshotModel forWeatherSnapshotModel;

			forWeatherSnapshotModel = new ForWeatherSnapshotModel(
				pageSnapshotsWithCitiesAndCountries.ToList(),
				new PageViewModel(count, page, pageSize),
				sortingState,
				isLogedIn
			)
			{
				filter = filter
			};

			return View(forWeatherSnapshotModel);
		}
	}
}
