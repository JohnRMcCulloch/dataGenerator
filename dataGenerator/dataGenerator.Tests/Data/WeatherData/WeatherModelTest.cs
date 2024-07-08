using System;
using dataGenerator.Data.WeatherData;
using Xunit;

namespace dataGenerator.Tests.Data.WeatherData;

public class WeatherModelTest
{
    [Fact]
    public void TestWeatherModelConstructorSetter()
    {
        // Arrange
        const double longitude = 123.45;
        const double latitude = 54.32;
        const double temperatureValue = 25.5;
        const string temperatureUnit = "Celsius";
        const double windSpeedValue = 15.2;
        const string windSpeedUnit = "km/h";
        const string windDirection = "NW";
        const int precipitationChance = 80;

        // Act
        var weatherModel = new WeatherModel
        {
            Longitude = longitude,
            Latitude = latitude,
            TemperatureValue = temperatureValue,
            TemperatureUnit = temperatureUnit,
            WindSpeedValue = windSpeedValue,
            WindSpeedUnit = windSpeedUnit,
            WindDirection = windDirection,
            PrecipitationChance = precipitationChance
        };

        // Assert
        Assert.Equal(longitude, weatherModel.Longitude);
        Assert.Equal(latitude, weatherModel.Latitude);
        Assert.Equal(temperatureValue, weatherModel.TemperatureValue);
        Assert.Equal(temperatureUnit, weatherModel.TemperatureUnit);
        Assert.Equal(windSpeedValue, weatherModel.WindSpeedValue);
        Assert.Equal(windSpeedUnit, weatherModel.WindSpeedUnit);
        Assert.Equal(windDirection, weatherModel.WindDirection);
        Assert.Equal(precipitationChance, weatherModel.PrecipitationChance);
    }
}