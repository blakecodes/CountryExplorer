using CountryExplorer.Shared.DTOs;
using CountryExplorer.Shared.Models;

namespace CountryExplorer.Domain.Mappers;

public static class CountryMapper
{
    public static CountryDto ToDto(Country country)
    {
        if (country == null) return null;

        return new CountryDto
        {
            Name = country.Name?.Common,
            OfficialName = country.Name?.Official,
            Capital = country.Capital,
            Region = country.Region,
            Subregion = country.Subregion,
            Population = country.Population,
            Languages = country.Languages,
            FlagPng = country.Flags?.Png,
            FlagSvg = country.Flags?.Svg,
            Area = country.Area,
            Currencies = country.Currencies?.ToDictionary(
                kvp => kvp.Key,
                kvp => new CurrencyDto
                {
                    Name = kvp.Value.Name,
                    Symbol = kvp.Value.Symbol
                }
            ),
            CountryCode = country.Cca2
        };
    }
}