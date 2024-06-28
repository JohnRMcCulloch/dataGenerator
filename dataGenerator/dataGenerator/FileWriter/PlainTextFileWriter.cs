using dataGenerator.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace dataGenerator.FileWriter;

/// <summary>
/// Write content to a plain text file.
/// </summary>
public class PlainTextFileWriter : IFileWriter
{
    private readonly ILogger<PlainTextFileWriter> _log;
    private readonly FileConfig _weatherConfig;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlainTextFileWriter"/> class.
    /// </summary>
    /// <param name="log">The logger instance for logging information.</param>
    /// <param name="weatherConfig">The file configuration options.</param>
    public PlainTextFileWriter(ILogger<PlainTextFileWriter> log, IOptions<FileConfig> weatherConfig)
    {
        _log = log;
        _weatherConfig = weatherConfig.Value;
    }

    /// <summary>
    /// Writes the specified content to a file with the given file name.
    /// </summary>
    /// <param name="content">The content to be written to the file.</param>
    /// <param name="fileName">The name of the file to which the content will be written.</param>
    public void WriteToFile(string content, string fileName)
    {
        try
        {
            var filePath = Path.Combine(GetDirectoryPath(), $"{fileName}{DateTime.Now:MMddyyyy.hhmmss}.txt");

            File.WriteAllText(filePath, content);
            _log.LogInformation("Data successfully written to file: {FilePath}", filePath);
        }
        catch (Exception ex)
        {
            _log.LogError("An error occurred while writing to file: {Exception}", ex);
        }
    }

    /// <summary>
    /// Gets the directory path from the configuration and ensures it exists.
    /// </summary>
    /// <returns>The directory path for the file.</returns>
    private string GetDirectoryPath()
    {
        var directoryPath = _weatherConfig.FilePath;
        if (Directory.Exists(directoryPath)) return directoryPath;
        Directory.CreateDirectory(directoryPath!);
        _log.LogInformation("Directory created successfully {directory}", directoryPath);
        return directoryPath!;
    }
}