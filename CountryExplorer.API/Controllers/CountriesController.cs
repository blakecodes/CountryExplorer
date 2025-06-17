using CountryExplorer.Domain.Interfaces;
using CountryExplorer.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace CountryExplorer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountriesController(
    ICountryService countryService,
    IWeatherService weatherService,
    ILogger<CountriesController> logger)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Country>>> GetAllCountries()
    {
        try
        {
            var countries = await countryService.GetAllCountriesAsync();
            return Ok(countries);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting all countries");
            return StatusCode(500, "An error occurred while fetching countries");
        }
    }

    [HttpGet("region/{region}")]
    public async Task<ActionResult<List<Country>>> GetCountriesByRegion(string region)
    {
        try
        {
            var countries = await countryService.GetCountriesByRegionAsync(region);
            return Ok(countries);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting countries by region: {Region}", region);
            return StatusCode(500, "An error occurred while fetching countries");
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<Country>>> SearchCountries([FromQuery] string name)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name parameter is required");

            var countries = await countryService.SearchCountriesByNameAsync(name);
            return Ok(countries);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error searching countries by name: {Name}", name);
            return StatusCode(500, "An error occurred while searching countries");
        }
    }

    [HttpGet("code/{code}")]
    public async Task<ActionResult<Country>> GetCountryByCode(string code)
    {
        try
        {
            var country = await countryService.GetCountryByCodeAsync(code);
            if (country == null) return NotFound();
            return Ok(country);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting country by code: {Code}", code);
            return StatusCode(500, "An error occurred while fetching country");
        }
    }

    [HttpGet("regions")]
    public async Task<ActionResult<List<string>>> GetAllRegions()
    {
        try
        {
            var regions = await countryService.GetAllRegionsAsync();
            return Ok(regions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting all regions");
            return StatusCode(500, "An error occurred while fetching regions");
        }
    }

    [HttpGet("{countryCode}/weather")]
    public async Task<ActionResult<Weather>> GetCountryWeather(string countryCode)
    {
        try
        {
            var country = await countryService.GetCountryByCodeAsync(countryCode);
            if (country == null) return NotFound("Country not found");

            if (country.Capital == null || !country.Capital.Any()) return NotFound("Country has no capital city");

            var weather = await weatherService.GetWeatherByCityAsync(country.Capital.First(), countryCode);
            if (weather == null) return NotFound("Weather data not available");

            return Ok(weather);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting weather for country: {CountryCode}", countryCode);
            return StatusCode(500, "An error occurred while fetching weather");
        }
    }
}