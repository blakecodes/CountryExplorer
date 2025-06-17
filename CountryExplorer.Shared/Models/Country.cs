using System.Text.Json.Serialization;

namespace CountryExplorer.Shared.Models;

public class Country
{
    [JsonPropertyName("name")] public Name Name { get; set; }

    [JsonPropertyName("capital")] public List<string> Capital { get; set; }

    [JsonPropertyName("region")] public string Region { get; set; }

    [JsonPropertyName("subregion")] public string Subregion { get; set; }

    [JsonPropertyName("population")] public long Population { get; set; }

    [JsonPropertyName("languages")] public Dictionary<string, string> Languages { get; set; }

    [JsonPropertyName("flags")] public Flags Flags { get; set; }

    [JsonPropertyName("area")] public double Area { get; set; }

    [JsonPropertyName("currencies")] public Dictionary<string, Currency> Currencies { get; set; }

    [JsonPropertyName("cca2")] public string Cca2 { get; set; }
}

public class Name
{
    [JsonPropertyName("common")] public string Common { get; set; }

    [JsonPropertyName("official")] public string Official { get; set; }

    [JsonPropertyName("nativeName")] public Dictionary<string, NativeName> NativeName { get; set; }
}

public class NativeName
{
    [JsonPropertyName("official")] public string Official { get; set; }

    [JsonPropertyName("common")] public string Common { get; set; }
}

public class Flags
{
    [JsonPropertyName("png")] public string Png { get; set; }

    [JsonPropertyName("svg")] public string Svg { get; set; }

    [JsonPropertyName("alt")] public string Alt { get; set; }
}

public class Currency
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("symbol")] public string Symbol { get; set; }
}