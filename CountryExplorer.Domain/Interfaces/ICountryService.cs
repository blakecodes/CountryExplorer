using CountryExplorer.Shared.Models;

namespace CountryExplorer.Domain.Interfaces;

public interface ICountryService
{
    Task<List<Country>> GetAllCountriesAsync();
    Task<PagedResult<Country>> GetAllCountriesAsync(int pageNumber, int pageSize);
    Task<List<Country>> GetCountriesByRegionAsync(string region);
    Task<PagedResult<Country>> GetCountriesByRegionAsync(string region, int pageNumber, int pageSize);
    Task<List<Country>> SearchCountriesByNameAsync(string name);
    Task<PagedResult<Country>> SearchCountriesByNameAsync(string name, int pageNumber, int pageSize);
    Task<Country> GetCountryByCodeAsync(string code);
    Task<List<string>> GetAllRegionsAsync();
}