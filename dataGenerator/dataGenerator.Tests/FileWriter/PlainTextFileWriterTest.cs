using System;
using System.IO;
using dataGenerator.Config;
using dataGenerator.FileWriter;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace dataGenerator.Tests.FileWriter;

public class PlainTextFileWriterTests
{
    private readonly Mock<ILogger<PlainTextFileWriter>> _mockLogger;
    private readonly IOptions<FileConfig> _mockFileConfig;
    private readonly PlainTextFileWriter _plainTextFileWriter;
    private readonly string _testDirectoryPath;

    public PlainTextFileWriterTests()
    {
        _mockLogger = new Mock<ILogger<PlainTextFileWriter>>();
        _testDirectoryPath = Path.Combine(Path.GetTempPath(), "TestFileWriter");

        _mockFileConfig = Options.Create(new FileConfig
        {
            FilePath = _testDirectoryPath
        });

        _plainTextFileWriter = new PlainTextFileWriter(_mockLogger.Object, _mockFileConfig);
    }

    [Fact]
    public void WriteToFile_Success()
    {
        // Arrange
        const string content = "Test content";
        const string fileName = "TestFile";
        var expectedFilePath = Path.Combine(_testDirectoryPath, $"{fileName}{DateTime.Now:MMddyyyy.hhmmss}.txt");

        // Act
        _plainTextFileWriter.WriteToFile(content, fileName);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Data successfully written to file")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);

        Assert.True(File.Exists(expectedFilePath));
        Assert.Equal(content, File.ReadAllText(expectedFilePath));
    }

    [Fact]
    public void WriteToFile_ExceptionThrown()
    {
        // Arrange
        const string content = "Test content";
        const string fileName = "TestFile";

        // Simulate exception by setting an invalid file path
        const string invalidFilePath = "\0InvalidPath";
        _mockFileConfig.Value.FilePath = invalidFilePath;

        // Act
        _plainTextFileWriter.WriteToFile(content, fileName);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("An error occurred while writing to file")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}