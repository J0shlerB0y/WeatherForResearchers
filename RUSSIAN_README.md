# Weather Researcher

Weather Researcher - это веб-приложение, в котором пользователи могут просматривать текущую погоду и исторические снапшоты для всех городов в мире. Он предоставляет как общий доступ, так и персонализированные функции для зарегистрированных пользователей.

## Интересные техники.

- **Asynchronous Fetch API:** Приложение использует [`fetch` API](https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API) для асинхронных запросов к серверу для получения и управления погодными данными.
- **Управление куками:** Аутентификация пользователей и их предпочтения управляются с помощью [HTTP-куков](https://developer.mozilla.org/en-US/docs/Web/HTTP/Cookies).
- **Удаленная проверка:** Процесс регистрации включает в себя [удаленную проверку](https://developer.mozilla.org/en-US/docs/Web/API/HTMLFormElement/submit) для проверки существующих имен пользователей на стороне сервера.
- **Хеширование и шифрование паролей:** Пароли пользователей [хешируются](https://developer.mozilla.org/en-US/docs/Web/API/SubtleCrypto/digest) и [шифруются](https://developer.mozilla.org/en-US/docs/Web/API/Aes/encrypt) для обеспечения безопасности.

## Используемые технологии и библиотеки.

- **ASP.NET Core MVC:** Приложение построено с использованием фреймворка [ASP.NET Core MVC](https://learn.microsoft.com/en-us/aspnet/core/mvc/overview).
- **Entity Framework Core:** [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) используется для взаимодействия с базами данных и управления данными.
- **База данных MySQL:** Приложение использует базу данных [MySQL](https://www.mysql.com/) для хранения данных.
- **API OpenWeatherMap:** Данные о погоде берутся из [OpenWeatherMap API](https://openweathermap.org/api).

## Структура проекта

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

- **Controllers:** Содержит контроллеры, отвечающие за обработку пользовательских запросов и взаимодействий.
- **MiddlewareTokens:** Содержит пользовательское промежуточное ПО для добавления и удаления городов и снимков погоды.
- **Models:** Определяет структуры данных, используемые во всем приложении.
- **Services:** Содержит контекст базы данных и обработчик паролей.

## Как использовать

1. **Установка зависимостей:** Убедитесь, что у вас установлены необходимые [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0), [docker](https://www.docker.com/products/docker-desktop/) и [docker compose](https://docs.docker.com/compose/install/).
2. **Запуск приложения:** Соберите и запустите приложение из командной строки, перейдя в директорию с dockerfile.
   ```
   docker-compose build
   docker-compose up
   ```
3. **Доступ к приложению:** Перейдите к приложению в веб-браузере.
   ```
   http://localhost:8080/
   ```
  
## Особенности

**Weather Researcher** предлагает ряд возможностей как для случайных посетителей, так и для зарегистрированных пользователей:

![](WebApplication1/WebApplication1/result.gif)

**Для всех:**

- **Глобальный обзор погоды:** Изучите текущие данные о погоде в городах по всему миру.
- **Динамическая фильтрация:** Уточняйте список городов с помощью фильтров по названию города и стране.
- **Гибкая сортировка:** Упорядочивайте список городов по названию города, стране или различным погодным атрибутам, таким как температура, скорость ветра и т. д., в порядке возрастания или убывания.
- **Подробная информация о погоде:** Получите исчерпывающую информацию о погоде, включая температуру, температуру по ощущениям, минимальную и максимальную температуру, влажность, давление, скорость ветра и описание текущих условий.
- **Визуальные значки погоды:** Легко понять погодные условия с помощью визуально привлекательных значков.

**Для зарегистрированных пользователей:**

- **Персонализированная панель погоды:** Доступ к персонализированной панели погоды, показывающей текущие условия для выбранных вами городов.
- **Управление городами:** Добавляйте и удаляйте города из личного списка погоды для удобства отслеживания.
- **История снимков погоды:** Захватывайте и сохраняйте снимки погоды для выбранных вами городов, обеспечивая историческую запись погодных условий.
- **Фильтрация и сортировка снимков:** Углубляйтесь в историю снимков погоды, фильтруя их по дате, времени и различным погодным параметрам. Сортируйте снимки по различным критериям.
- **Безопасная аутентификация:** Зарегистрируйте учетную запись и войдите в систему, используя безопасное хэширование и шифрование паролей. 