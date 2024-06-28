using System;
using dataGenerator.Config;
using dataGenerator.Data;
using dataGenerator.Data.WeatherData;
using dataGenerator.Factory;
using dataGenerator.FileWriter;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using static System.Globalization.CultureInfo;

namespace dataGenerator.Tests.Factory;

public class WeatherFactoryTests
{
    private readonly Mock<ILogger<WeatherFactory>> _mockLogger;
    private readonly Mock<IDataGenerator<WeatherModel>> _mockDataGenerator;
    private readonly Mock<IFileWriter> _mockFileWriter;
    private readonly IOptions<WeatherConfig> _mockWeatherConfig;

    public WeatherFactoryTests()
    {
        _mockLogger = new Mock<ILogger<WeatherFactory>>();
        _mockDataGenerator = new Mock<IDataGenerator<WeatherModel>>();
        _mockFileWriter = new Mock<IFileWriter>();

        _mockWeatherConfig = Options.Create(new WeatherConfig
        {
            StartTimestampUtc = DateTime.UtcNow,
            WeatherTimestampQuantity = 2,
            NumberOfWeatherInformationRecords = 3,
            FileName = "TestWeatherData"
        });
    }

    [Fact]
    public void WeatherFactoryGenerate_Success()
    {
        // Arrange
        var weatherFactory = new WeatherFactory(
            _mockLogger.Object,
            _mockWeatherConfig,
            _mockDataGenerator.Object,
            _mockFileWriter.Object
        );

        var mockWeatherData = new WeatherModel
        {
            Longitude = 10.0,
            Latitude = 20.0,
            TemperatureValue = 25.5,
            TemperatureUnit = "C",
            WindSpeedValue = 15.0,
            WindSpeedUnit = "km/h",
            WindDirection = "N",
            PrecipitationChance = 2
        };

        _mockDataGenerator.Setup(x => x.GenerateData()).Returns(mockWeatherData);

        string capturedContent = null;
        _mockFileWriter
            .Setup(x => x.WriteToFile(It.IsAny<string>(), "TestWeatherData"))
            .Callback<string, string>((content, _) => capturedContent = content);

        // Act
        weatherFactory.Generate();

        // Assert
        _mockFileWriter.Verify(x => x.WriteToFile(It.IsAny<string>(), "TestWeatherData"), Times.Once);

        Assert.NotNull(capturedContent);
        var expectedTimestamp = _mockWeatherConfig.Value.StartTimestampUtc.ToString("yyyy-MM-dd HH:00UTC");
        Assert.Contains(expectedTimestamp, capturedContent);
        Assert.Contains(mockWeatherData.Longitude.ToString(InvariantCulture), capturedContent);
        Assert.Contains(mockWeatherData.Latitude.ToString(InvariantCulture), capturedContent);
        Assert.Contains(mockWeatherData.TemperatureValue.ToString(InvariantCulture), capturedContent);
        Assert.Contains(mockWeatherData.TemperatureUnit, capturedContent);
        Assert.Contains(mockWeatherData.WindSpeedValue.ToString(InvariantCulture), capturedContent);
        Assert.Contains(mockWeatherData.WindSpeedUnit, capturedContent);
        Assert.Contains(mockWeatherData.WindDirection, capturedContent);
        Assert.Contains(mockWeatherData.PrecipitationChance.ToString(), capturedContent);
    }
}