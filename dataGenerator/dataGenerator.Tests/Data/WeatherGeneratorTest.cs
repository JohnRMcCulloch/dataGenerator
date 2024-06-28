using dataGenerator.Config;
using dataGenerator.Data;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace dataGenerator.Tests.Data;

public class WeatherGeneratorTests
{
    private readonly WeatherConfig _weatherConfig;
    private readonly WeatherGenerator _weatherGenerator;

    public WeatherGeneratorTests()
    {
        _weatherConfig = new WeatherConfig
        {
            Information = new Information()
        };

        var mockWeatherConfig = new Mock<IOptions<WeatherConfig>>();
        mockWeatherConfig.Setup(x => x.Value).Returns(_weatherConfig);

        _weatherGenerator = new WeatherGenerator(mockWeatherConfig.Object);
    }

    [Fact]
    public void GenerateDataGenerate_Success()
    {
        //Arrange
        var expectedTimestampLower = _weatherConfig.StartTimestampUtc.Date.AddDays(-30);
        var expectedTimestampUpper = _weatherConfig.StartTimestampUtc.Date.AddDays(+30);

        // Act
        var weatherModel = _weatherGenerator.GenerateData();

        // Assert
        Assert.NotNull(weatherModel);
        Assert.True(
            weatherModel.Timestamp >= expectedTimestampLower && weatherModel.Timestamp <= expectedTimestampUpper);

        Assert.InRange(weatherModel.Longitude, -180, 180);
        Assert.InRange(weatherModel.Latitude, -90, 90);
        AssertDecimalPlaces(_weatherConfig.Information.LongitudeDecimalPlaces, weatherModel.Longitude);
        AssertDecimalPlaces(_weatherConfig.Information.LatitudeDecimalPlaces, weatherModel.Latitude);
        
        Assert.Contains(weatherModel.TemperatureUnit,
            _weatherConfig.Information.TemperatureUnit);
        if (weatherModel.TemperatureUnit == "C")
        {
            Assert.InRange(weatherModel.TemperatureValue, -30, 50);
        }
        else
        {
            Assert.InRange(weatherModel.TemperatureValue, -22, 122);
        }

        Assert.InRange(weatherModel.WindSpeedValue, 0, 150);
        Assert.Contains(weatherModel.WindSpeedUnit,
            _weatherConfig.Information.WindSpeedUnit);
        Assert.Contains(weatherModel.WindDirection,
            _weatherConfig.Information.WindDirection);
        Assert.InRange(weatherModel.PrecipitationChance, 0,
            _weatherConfig.Information
                .PrecipitationChancePercentageUpperLimit);
    }
    
    private void AssertDecimalPlaces(int expectedDecimalPlaces, double actual)
    {
        var valueStr = actual.ToString("0.###############");
        var actualDecimalPlaces = valueStr.Length - valueStr.IndexOf('.') - 1;

        Assert.Equal(expectedDecimalPlaces, actualDecimalPlaces);
    }
}