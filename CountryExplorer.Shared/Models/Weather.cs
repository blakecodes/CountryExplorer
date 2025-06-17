namespace CountryExplorer.Shared.Models;

public class Weather
{
    public double Temperature { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
    public int Humidity { get; set; }
    public double WindSpeed { get; set; }
    public string CityName { get; set; }
    public string Country { get; set; }
}