using CountryExplorer.Shared.Models;

namespace CountryExplorer.Domain.Interfaces;

public interface ICountryRestService
{
    Task<List<Country>> GetAllCountriesAsync();
    Task<List<Country>> GetCountriesByRegionAsync(string region);
    Task<List<Country>> SearchCountriesByNameAsync(string name);
    Task<Country> GetCountryByCodeAsync(string code);
    Task<List<string>> GetAllRegionsAsync();
} 