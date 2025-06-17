using CountryExplorer.Domain.Interfaces;
using CountryExplorer.Domain.Services;
using CountryExplorer.Shared.Models;
using Moq;

namespace CountryExplorer.Tests;

[TestFixture]
public class CountryServiceTests
{
    [SetUp]
    public void Setup()
    {
        _mockCountryRestService = new Mock<ICountryRestService>();
        _countryService = new CountryService(_mockCountryRestService.Object);
        _testCountries = CreateTestCountries();
    }

    private Mock<ICountryRestService> _mockCountryRestService;
    private CountryService _countryService;
    private List<Country> _testCountries;

    [Test]
    public async Task GetAllCountriesAsync_ReturnsCountriesSortedByName()
    {
        _mockCountryRestService.Setup(x => x.GetAllCountriesAsync())
            .ReturnsAsync(_testCountries);

        var result = await _countryService.GetAllCountriesAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(5));
        Assert.That(result[0].Name.Common, Is.EqualTo("Argentina"));
        Assert.That(result[1].Name.Common, Is.EqualTo("Brazil"));
        Assert.That(result[2].Name.Common, Is.EqualTo("Canada"));
        Assert.That(result[3].Name.Common, Is.EqualTo("Germany"));
        Assert.That(result[4].Name.Common, Is.EqualTo("United States"));
    }

    [Test]
    public async Task GetAllCountriesAsync_WithPaging_ReturnsPagedResult()
    {
        _mockCountryRestService.Setup(x => x.GetAllCountriesAsync())
            .ReturnsAsync(_testCountries);

        var result = await _countryService.GetAllCountriesAsync(1, 2);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Items.Count, Is.EqualTo(2));
        Assert.That(result.TotalCount, Is.EqualTo(5));
        Assert.That(result.PageNumber, Is.EqualTo(1));
        Assert.That(result.PageSize, Is.EqualTo(2));
        Assert.That(result.TotalPages, Is.EqualTo(3));
        Assert.That(result.HasPreviousPage, Is.False);
        Assert.That(result.HasNextPage, Is.True);
        Assert.That(result.Items[0].Name.Common, Is.EqualTo("Argentina"));
        Assert.That(result.Items[1].Name.Common, Is.EqualTo("Brazil"));
    }

    [Test]
    public async Task GetAllCountriesAsync_WithPaging_SecondPage_ReturnsCorrectItems()
    {
        _mockCountryRestService.Setup(x => x.GetAllCountriesAsync())
            .ReturnsAsync(_testCountries);

        var result = await _countryService.GetAllCountriesAsync(2, 2);

        Assert.That(result.Items.Count, Is.EqualTo(2));
        Assert.That(result.PageNumber, Is.EqualTo(2));
        Assert.That(result.HasPreviousPage, Is.True);
        Assert.That(result.HasNextPage, Is.True);
        Assert.That(result.Items[0].Name.Common, Is.EqualTo("Canada"));
        Assert.That(result.Items[1].Name.Common, Is.EqualTo("Germany"));
    }

    [Test]
    public async Task GetAllCountriesAsync_WithPaging_LastPage_ReturnsRemainingItems()
    {
        _mockCountryRestService.Setup(x => x.GetAllCountriesAsync())
            .ReturnsAsync(_testCountries);

        var result = await _countryService.GetAllCountriesAsync(3, 2);

        Assert.That(result.Items.Count, Is.EqualTo(1));
        Assert.That(result.PageNumber, Is.EqualTo(3));
        Assert.That(result.HasPreviousPage, Is.True);
        Assert.That(result.HasNextPage, Is.False);
        Assert.That(result.Items[0].Name.Common, Is.EqualTo("United States"));
    }

    [Test]
    public async Task GetAllCountriesAsync_WithPaging_InvalidPageNumber_ReturnsFirstPage()
    {
        _mockCountryRestService.Setup(x => x.GetAllCountriesAsync())
            .ReturnsAsync(_testCountries);

        var result = await _countryService.GetAllCountriesAsync(0, 2);

        Assert.That(result.PageNumber, Is.EqualTo(1));
        Assert.That(result.Items.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllCountriesAsync_WithPaging_PageSizeExceedsLimit_LimitsTo100()
    {
        _mockCountryRestService.Setup(x => x.GetAllCountriesAsync())
            .ReturnsAsync(_testCountries);

        var result = await _countryService.GetAllCountriesAsync(1, 150);

        Assert.That(result.PageSize, Is.EqualTo(100));
    }

    [Test]
    public async Task GetCountriesByRegionAsync_ReturnsFilteredAndSortedCountries()
    {
        _mockCountryRestService.Setup(x => x.GetAllCountriesAsync())
            .ReturnsAsync(_testCountries);

        var result = await _countryService.GetCountriesByRegionAsync("Americas");

        Assert.That(result.Count, Is.EqualTo(4));
        Assert.That(result[0].Name.Common, Is.EqualTo("Argentina"));
        Assert.That(result[1].Name.Common, Is.EqualTo("Brazil"));
        Assert.That(result[2].Name.Common, Is.EqualTo("Canada"));
        Assert.That(result[3].Name.Common, Is.EqualTo("United States"));
    }

    [Test]
    public async Task GetCountriesByRegionAsync_CaseInsensitive_ReturnsCorrectResults()
    {
        _mockCountryRestService.Setup(x => x.GetAllCountriesAsync())
            .ReturnsAsync(_testCountries);

        var result = await _countryService.GetCountriesByRegionAsync("americas");

        Assert.That(result.Count, Is.EqualTo(4));
    }

    [Test]
    public async Task GetCountriesByRegionAsync_NoMatches_ReturnsEmptyList()
    {
        _mockCountryRestService.Setup(x => x.GetAllCountriesAsync())
            .ReturnsAsync(_testCountries);

        var result = await _countryService.GetCountriesByRegionAsync("Asia");

        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GetCountriesByRegionAsync_WithPaging_ReturnsPagedResult()
    {
        _mockCountryRestService.Setup(x => x.GetAllCountriesAsync())
            .ReturnsAsync(_testCountries);

        var result = await _countryService.GetCountriesByRegionAsync("Americas", 1, 2);

        Assert.That(result.Items.Count, Is.EqualTo(2));
        Assert.That(result.TotalCount, Is.EqualTo(4));
        Assert.That(result.Items[0].Name.Common, Is.EqualTo("Argentina"));
        Assert.That(result.Items[1].Name.Common, Is.EqualTo("Brazil"));
    }

    private List<Country> CreateTestCountries()
    {
        return new List<Country>
        {
            new()
            {
                Name = new Name { Common = "United States", Official = "United States of America" },
                Cca2 = "US",
                Region = "Americas",
                Population = 331002651,
                Capital = new List<string> { "Washington, D.C." }
            },
            new()
            {
                Name = new Name { Common = "Brazil", Official = "Federative Republic of Brazil" },
                Cca2 = "BR",
                Region = "Americas",
                Population = 212559417,
                Capital = new List<string> { "Brasília" }
            },
            new()
            {
                Name = new Name { Common = "Germany", Official = "Federal Republic of Germany" },
                Cca2 = "DE",
                Region = "Europe",
                Population = 83783942,
                Capital = new List<string> { "Berlin" }
            },
            new()
            {
                Name = new Name { Common = "Canada", Official = "Canada" },
                Cca2 = "CA",
                Region = "Americas",
                Population = 37742154,
                Capital = new List<string> { "Ottawa" }
            },
            new()
            {
                Name = new Name { Common = "Argentina", Official = "Argentine Republic" },
                Cca2 = "AR",
                Region = "Americas",
                Population = 45195774,
                Capital = new List<string> { "Buenos Aires" }
            }
        };
    }
}