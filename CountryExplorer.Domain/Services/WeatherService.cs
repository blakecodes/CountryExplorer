using System.Text.Json;
using CountryExplorer.Domain.Interfaces;
using CountryExplorer.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CountryExplorer.Domain.Services;

public class WeatherService(HttpClient httpClient, ILogger<WeatherService> logger, IConfiguration configuration)
    : IWeatherService
{
    private const string BaseUrl = "https://api.openweathermap.org/data/2.5";
    private readonly string _apiKey = configuration["OpenWeatherApi:ApiKey"] ?? string.Empty;

    public async Task<Weather> GetWeatherByCityAsync(string cityName, string countryCode = null)
    {
        try
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                logger.LogWarning("OpenWeather API key is not configured");
                return null;
            }

            var query = string.IsNullOrEmpty(countryCode)
                ? cityName
                : $"{cityName},{countryCode}";

            var response = await httpClient.GetAsync($"{BaseUrl}/weather?q={query}&appid={_apiKey}&units=metric");

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Weather data not found for city: {City}", cityName);
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var weatherData = JsonDocument.Parse(json);

            var weather = new Weather
            {
                Temperature = weatherData.RootElement.GetProperty("main").GetProperty("temp").GetDouble(),
                Description = weatherData.RootElement.GetProperty("weather")[0].GetProperty("description").GetString(),
                Icon = weatherData.RootElement.GetProperty("weather")[0].GetProperty("icon").GetString(),
                Humidity = weatherData.RootElement.GetProperty("main").GetProperty("humidity").GetInt32(),
                WindSpeed = weatherData.RootElement.GetProperty("wind").GetProperty("speed").GetDouble(),
                CityName = weatherData.RootElement.GetProperty("name").GetString(),
                Country = weatherData.RootElement.GetProperty("sys").GetProperty("country").GetString()
            };

            return weather;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching weather for city: {City}", cityName);
            return null;
        }
    }
}