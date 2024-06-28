using Bogus;
using dataGenerator.Config;
using dataGenerator.Data.WeatherData;
using Microsoft.Extensions.Options;

namespace dataGenerator.Data;

public class WeatherGenerator : IDataGenerator<WeatherModel>
{
    private readonly Faker<WeatherModel> _weatherModelFaker;
    private readonly WeatherConfig _weatherConfig;

    public WeatherGenerator(IOptions<WeatherConfig> weatherConfig)
    {
        _weatherConfig = weatherConfig.Value;
        Randomizer.Seed = new Random(123);
        _weatherModelFaker = GenerateFakeWeather();
    }

    public WeatherModel GenerateData()
    {
        return _weatherModelFaker.Generate();
    }

    private Faker<WeatherModel> GenerateFakeWeather()
    {
        return new Faker<WeatherModel>()
            .RuleFor(w => w.Timestamp, f => f.Date.RecentOffset(30).UtcDateTime)
            .RuleFor(w => w.Longitude,
                f => Math.Round(f.Address.Longitude(), _weatherConfig.Information.LongitudeDecimalPlaces))
            .RuleFor(w => w.Latitude,
                f => Math.Round(f.Address.Latitude(), _weatherConfig.Information.LatitudeDecimalPlaces))
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