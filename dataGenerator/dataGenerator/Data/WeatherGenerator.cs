using Bogus;
using dataGenerator.Config;
using dataGenerator.Data.WeatherData;
using Microsoft.Extensions.Options;

namespace dataGenerator.Data;

/// <summary>
/// Generates weather data models based on the provided configuration.
/// </summary>
public class WeatherGenerator : IDataGenerator<WeatherModel>
{
    private readonly Faker<WeatherModel> _weatherModelFaker;
    private readonly WeatherConfig _weatherConfig;

    /// <summary>
    /// Initializes a new instance of the <see cref="WeatherGenerator"/> class.
    /// </summary>
    /// <param name="weatherConfig">The weather configuration options.</param>
    public WeatherGenerator(IOptions<WeatherConfig> weatherConfig)
    {
        _weatherConfig = weatherConfig.Value;
        Randomizer.Seed = new Random(123);
        _weatherModelFaker = GenerateFakeWeather();
    }

    /// <summary>
    /// Generates a single instance of <see cref="WeatherModel"/>.
    /// </summary>
    /// <returns>A generated <see cref="WeatherModel"/> instance.</returns>
    public WeatherModel GenerateData()
    {
        return _weatherModelFaker.Generate();
    }

    /// <summary>
    /// Configures the rules for generating fake weather data.
    /// </summary>
    /// <returns>A configured <see cref="Faker{WeatherModel}"/> instance.</returns>
    private Faker<WeatherModel> GenerateFakeWeather()
    {
        var randomizer = new Randomizer();
        
        return new Faker<WeatherModel>()
            .RuleFor(w => w.Longitude,
                f => Math.Round(randomizer.Double(-180, 180), _weatherConfig.Information.LongitudeDecimalPlaces))
            .RuleFor(w => w.Latitude,
                f => Math.Round(randomizer.Double(-90, 0), _weatherConfig.Information.LatitudeDecimalPlaces))
            .RuleFor(w => w.TemperatureUnit, f =>
                f.PickRandom(_weatherConfig.Information.TemperatureUnit))
            .RuleFor(w => w.TemperatureValue, (f, w) =>
                w.TemperatureUnit == "C"
                    ? Math.Round(f.Random.Double(-30, 50),
                        _weatherConfig.Information.TemperatureDecimalPlaces) // Celsius range
                    : Math.Round(f.Random.Double(-22, 122),
                        _weatherConfig.Information.TemperatureDecimalPlaces)) // Fahrenheit range
            .RuleFor(w => w.WindSpeedValue,
                f => Math.Round(f.Random.Double(0, 150), _weatherConfig.Information.WindSpeedDecimalPlaces))
            .RuleFor(w => w.WindSpeedUnit, f =>
                f.PickRandom(_weatherConfig.Information.WindSpeedUnit))
            .RuleFor(w => w.WindDirection, f =>
                f.PickRandom(_weatherConfig.Information.WindDirection))
            .RuleFor(w => w.PrecipitationChance,
                f => f.Random.Int(0, _weatherConfig.Information.PrecipitationChancePercentageUpperLimit));
    }
}