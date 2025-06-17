using System.Net;
using System.Text.Json;
using CountryExplorer.Domain.Interfaces;
using CountryExplorer.Shared.Models;
using Microsoft.Extensions.Logging;

namespace CountryExplorer.Domain.Services;

public class CountryRestService(HttpClient httpClient, ILogger<CountryRestService> logger) : ICountryRestService
{
    private const string BaseUrl = "https://restcountries.com/v3.1";

    // Maximum 10 fields as per API limitation
    private readonly string _fields = "name,capital,region,subregion,population,languages,flags,currencies,cca2,area";

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<List<Country>> GetAllCountriesAsync()
    {
        try
        {
            // Use simple string concatenation to match the working format
            var url = $"{BaseUrl}/all?fields={_fields}";

            logger.LogInformation("Attempting to fetch countries from: {Url}", url);

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                logger.LogError("REST Countries API returned {StatusCode}: {ReasonPhrase}. Content: {ErrorContent}",
                    response.StatusCode, response.ReasonPhrase, errorContent);
                logger.LogError("Request URL was: {Url}", url);

                // Log all request headers
                logger.LogError("Request Headers:");
                foreach (var header in httpClient.DefaultRequestHeaders)
                    logger.LogError("{HeaderName}: {HeaderValue}", header.Key, string.Join(", ", header.Value));
            }

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var countries = JsonSerializer.Deserialize<List<Country>>(json, _jsonOptions);

            return countries ?? new List<Country>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching all countries");
            throw;
        }
    }

    public async Task<List<Country>> GetCountriesByRegionAsync(string region)
    {
        try
        {
            var url = $"{BaseUrl}/region/{region}?fields={_fields}";

            logger.LogInformation("Attempting to fetch countries from: {Url}", url);

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                logger.LogError("REST Countries API returned {StatusCode}: {ReasonPhrase}. Content: {ErrorContent}",
                    response.StatusCode, response.ReasonPhrase, errorContent);
                logger.LogError("Request URL was: {Url}", url);
            }

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var countries = JsonSerializer.Deserialize<List<Country>>(json, _jsonOptions);

            return countries ?? new List<Country>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching countries by region: {Region}", region);
            throw;
        }
    }

    public async Task<List<Country>> SearchCountriesByNameAsync(string name)
    {
        try
        {
            var url = $"{BaseUrl}/name/{name}?fields={_fields}";

            logger.LogInformation("Attempting to fetch countries from: {Url}", url);

            var response = await httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NotFound) return new List<Country>();

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                logger.LogError("REST Countries API returned {StatusCode}: {ReasonPhrase}. Content: {ErrorContent}",
                    response.StatusCode, response.ReasonPhrase, errorContent);
                logger.LogError("Request URL was: {Url}", url);
            }

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var countries = JsonSerializer.Deserialize<List<Country>>(json, _jsonOptions);

            return countries ?? new List<Country>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error searching countries by name: {Name}", name);
            throw;
        }
    }

    public async Task<Country> GetCountryByCodeAsync(string code)
    {
        try
        {
            var url = $"{BaseUrl}/alpha/{code}?fields={_fields}";

            logger.LogInformation("Attempting to fetch country from: {Url}", url);

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                logger.LogError("REST Countries API returned {StatusCode}: {ReasonPhrase}. Content: {ErrorContent}",
                    response.StatusCode, response.ReasonPhrase, errorContent);
                logger.LogError("Request URL was: {Url}", url);
            }

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var country = JsonSerializer.Deserialize<Country>(json, _jsonOptions);

            return country;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching country by code: {Code}", code);
            throw;
        }
    }

    public async Task<List<string>> GetAllRegionsAsync()
    {
        try
        {
            var countries = await GetAllCountriesAsync();
            var regions = countries
                .Where(c => !string.IsNullOrEmpty(c.Region))
                .Select(c => c.Region)
                .Distinct()
                .OrderBy(r => r)
                .ToList();

            return regions;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching all regions");
            throw;
        }
    }
}