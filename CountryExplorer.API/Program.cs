using CountryExplorer.Domain.Interfaces;
using CountryExplorer.Domain.Services;

namespace CountryExplorer.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Configure CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAngularApp",
                policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });

        // Register HttpClient for external API calls
        builder.Services.AddHttpClient<ICountryService, CountryService>(client =>
        {
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        builder.Services.AddHttpClient<IWeatherService, WeatherService>(client =>
        {
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        // Register services
        builder.Services.AddScoped<ICountryService, CountryService>();
        builder.Services.AddScoped<IWeatherService, WeatherService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors("AllowAngularApp");

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}