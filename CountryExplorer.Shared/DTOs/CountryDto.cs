namespace CountryExplorer.Shared.DTOs;

public class CountryDto
{
    public string Name { get; set; }
    public string OfficialName { get; set; }
    public List<string> Capital { get; set; }
    public string Region { get; set; }
    public string Subregion { get; set; }
    public long Population { get; set; }
    public Dictionary<string, string> Languages { get; set; }
    public string FlagPng { get; set; }
    public string FlagSvg { get; set; }
    public double Area { get; set; }
    public Dictionary<string, CurrencyDto> Currencies { get; set; }
    public string CountryCode { get; set; }
}

public class CurrencyDto
{
    public string Name { get; set; }
    public string Symbol { get; set; }
}