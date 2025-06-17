using CountryExplorer.Shared.Models;

namespace CountryExplorer.Domain.Interfaces;

public interface IWeatherService
{
    Task<Weather> GetWeatherByCityAsync(string cityName, string countryCode = null);
}