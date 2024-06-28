namespace dataGenerator.FileWriter;

/// <summary>
/// Defines an interface for writing content to a file.
/// </summary>
public interface IFileWriter
{
    void WriteToFile(string content, string fileName);
}