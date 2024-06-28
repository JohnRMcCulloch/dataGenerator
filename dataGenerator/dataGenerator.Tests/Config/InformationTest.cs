using dataGenerator.Config;
using Xunit;

namespace dataGenerator.Tests.Config;

public class InformationTest
{
    [Fact]
    public void TestInformationDefaultValues()
    {
        var temperatureUnitExpected = new[] {"C", "F"};
        var windSpeedUnitExpected = new[] {"km/h"};
        var windDirectionExpected = new[] {"N", "NE", "E", "SE", "S", "SW", "W", "NW"};

        var information = new Information();

        Assert.Equal(6, information.LongitudeDecimalPlaces);
        Assert.Equal(6, information.LatitudeDecimalPlaces);
        Assert.Equal(1, information.TemperatureDecimalPlaces);
        Assert.Equal(temperatureUnitExpected, information.TemperatureUnit);
        Assert.Equal(1, information.WindSpeedDecimalPlaces);
        Assert.Equal(windSpeedUnitExpected, information.WindSpeedUnit);
        Assert.Equal(windDirectionExpected, information.WindDirection);
        Assert.Equal(100, information.PrecipitationChancePercentageUpperLimit);
    }

    [Fact]
    public void TestInformationSetter()
    {
        var temperatureUnit = new[] {"Celsius", "Fahrenheit"};
        var windSpeedUnit = new[] {"m/s", "mph"};
        var windDirection = new[] {"North", "East", "South", "West"};

        // Arrange
        var information = new Information
        {
            LongitudeDecimalPlaces = 5,
            LatitudeDecimalPlaces = 4,
            TemperatureDecimalPlaces = 2,
            TemperatureUnit = temperatureUnit,
            WindSpeedDecimalPlaces = 2,
            WindSpeedUnit = windSpeedUnit,
            WindDirection = windDirection,
            PrecipitationChancePercentageUpperLimit = 80
        };

        // Assert
        Assert.Equal(5, information.LongitudeDecimalPlaces);
        Assert.Equal(4, information.LatitudeDecimalPlaces);
        Assert.Equal(2, information.TemperatureDecimalPlaces);
        Assert.Equal(temperatureUnit, information.TemperatureUnit);
        Assert.Equal(2, information.WindSpeedDecimalPlaces);
        Assert.Equal(windSpeedUnit, information.WindSpeedUnit);
        Assert.Equal(windDirection, information.WindDirection);
        Assert.Equal(80, information.PrecipitationChancePercentageUpperLimit);
    }
}