# Weather Researcher

Weather Researcher is a web application where users can view the current weather and historical snapshots for various cities around the world. It provides both general access and personalized features for registered users.

## Interesting Techniques

- **Asynchronous Fetch API:**  The application uses the [`fetch` API](https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API) to make asynchronous requests to the server to retrieve and manage weather data.
- **Cookie Management:**  User authentication and preferences are managed using [HTTP cookies](https://developer.mozilla.org/en-US/docs/Web/HTTP/Cookies).
- **Remote Validation:** The registration process incorporates [remote validation](https://developer.mozilla.org/en-US/docs/Web/API/HTMLFormElement/submit) to check for existing usernames on the server-side.
- **Password Hashing and Encryption:**  User passwords are [hashed](https://developer.mozilla.org/en-US/docs/Web/API/SubtleCrypto/digest) and [encrypted](https://developer.mozilla.org/en-US/docs/Web/API/Aes/encrypt) for security.

## Technologies and Libraries Used

- **ASP.NET Core MVC:** The application is built using the [ASP.NET Core MVC framework](https://learn.microsoft.com/en-us/aspnet/core/mvc/overview).
- **Entity Framework Core:**  [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) is used for database interaction and data management.
- **MySQL Database:** The application uses a [MySQL database](https://www.mysql.com/) for data persistence.
- **OpenWeatherMap API:** Weather data is fetched from the [OpenWeatherMap API](https://openweathermap.org/api).

## Project Structure

```
├── Controllers
│   ├── HomeController.cs
│   ├── OwnCabinetController.cs
│   └── SignInController.cs
├── MiddlewareTokens
│   ├── AddingCity.cs
│   ├── AddingSnapshot.cs
│   ├── DeletingCity.cs
│   └── DeletingSnapshot.cs
├── Models
│   ├── CitiesAndCountries.cs
│   ├── City.cs
│   ├── Country.cs
│   ├── DataToAuth.cs
│   ├── DataToRegistr.cs
│   ├── ErrorViewModel.cs
│   ├── FilterForSnapshotViewModel.cs
│   ├── FilterViewModel.cs
│   ├── ForWeatherModel.cs
│   ├── ForWeatherSnapshotModel.cs
│   ├── PageViewModel.cs
│   ├── Snapshot.cs
│   ├── SnapshotWithCityModel.cs
│   ├── SortingEnum.cs
│   ├── UsersCity.cs
│   ├── User.cs
│   └── WeatherModel.cs
└── Services
    ├── ApplicationContext.cs
    └── PasswordHandler.cs

```

- **Controllers:**  Contains the controllers responsible for handling user requests and interactions.
- **MiddlewareTokens:** Includes custom middleware for adding and deleting cities and weather snapshots.
- **Models:** Defines the data structures used throughout the application.
- **Services:** Contains the database context and password handler.

## How to Use

1. **Installing dependencies:** Make sure you have the required [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0), [docker](https://www.docker.com/products/docker-desktop/), and [docker compose](https://docs.docker.com/compose/install/) installed.
2. **Starting the application:** Compose and run the application from the command line by navigating to the directory with the dockerfile.
   ```
   docker-compose build
   docker-compose up
   ```
3. **Access the application:** Navigate to the application in a web 
    ```
   http://localhost:8080/
   ```

## Features

**Weather Researcher** offers a range of features for both casual visitors and registered users:

![](WebApplication1/WebApplication1/result.gif)

**For Everyone:**

- **Global Weather Overview:**  Explore current weather data for cities around the world.
- **Dynamic Filtering:**  Refine the city list using filters for city name and country.
- **Flexible Sorting:** Arrange the city list based on city name, country, or various weather attributes like temperature, wind speed, etc. in ascending or descending order.
- **Detailed Weather Information:** Get comprehensive weather details including temperature, feels-like temperature, minimum and maximum temperatures, humidity, pressure, wind speed, and a description of the current conditions.
- **Visual Weather Icons:** Easily understand weather conditions with visually appealing icons.

**For Registered Users:**

- **Personalized Weather Dashboard:** Access a customized weather dashboard showing current conditions for your selected cities.
- **City Management:** Add and remove cities from your personal weather list for easy tracking.
- **Weather Snapshot History:** Capture and store weather snapshots for your chosen cities, providing a historical record of weather patterns.
- **Snapshot Filtering and Sorting:** Dive deeper into your weather snapshot history by filtering based on date, time, and various weather parameters. Sort snapshots by a variety of criteria.
- **Secure Authentication:**  Register for an account and sign in using secure password hashing and encryption. 

