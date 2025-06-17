using CountryExplorer.Domain.Interfaces;
using CountryExplorer.Shared.Models;

namespace CountryExplorer.Domain.Services;

public class CountryService(ICountryRestService countryRestService)
    : ICountryService
{
    public async Task<List<Country>> GetAllCountriesAsync()
    {
        var allCountries = await countryRestService.GetAllCountriesAsync();
        return allCountries;
    }

    public async Task<PagedResult<Country>> GetAllCountriesAsync(int pageNumber, int pageSize)
    {
        var allCountries = await countryRestService.GetAllCountriesAsync();
        return CreatePagedResult(allCountries, pageNumber, pageSize);
    }

    public async Task<List<Country>> GetCountriesByRegionAsync(string region)
    {
        var allCountries = await countryRestService.GetAllCountriesAsync();
        return allCountries.Where(c => c.Region?.Equals(region, StringComparison.OrdinalIgnoreCase) == true).ToList();
    }

    public async Task<PagedResult<Country>> GetCountriesByRegionAsync(string region, int pageNumber, int pageSize)
    {
        var countries = await GetCountriesByRegionAsync(region);
        return CreatePagedResult(countries, pageNumber, pageSize);
    }

    public async Task<List<Country>> SearchCountriesByNameAsync(string name)
    {
        var allCountries = await countryRestService.GetAllCountriesAsync();
        return allCountries.Where(c =>
            c.Name?.Common?.Contains(name, StringComparison.OrdinalIgnoreCase) == true ||
            c.Name?.Official?.Contains(name, StringComparison.OrdinalIgnoreCase) == true
        ).ToList();
    }

    public async Task<PagedResult<Country>> SearchCountriesByNameAsync(string name, int pageNumber, int pageSize)
    {
        var countries = await SearchCountriesByNameAsync(name);
        return CreatePagedResult(countries, pageNumber, pageSize);
    }

    public async Task<Country> GetCountryByCodeAsync(string code)
    {
        var allCountries = await countryRestService.GetAllCountriesAsync();
        return allCountries.FirstOrDefault(c => c.Cca2?.Equals(code, StringComparison.OrdinalIgnoreCase) == true);
    }

    public async Task<List<string>> GetAllRegionsAsync()
    {
        var allCountries = await countryRestService.GetAllCountriesAsync();
        return allCountries
            .Where(c => !string.IsNullOrEmpty(c.Region))
            .Select(c => c.Region)
            .Distinct()
            .OrderBy(r => r)
            .ToList();
    }

    private PagedResult<Country> CreatePagedResult(List<Country> allItems, int pageNumber, int pageSize)
    {
        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Max(1, Math.Min(100, pageSize)); // Limit page size to 100

        var totalCount = allItems.Count;
        var items = allItems
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResult<Country>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}