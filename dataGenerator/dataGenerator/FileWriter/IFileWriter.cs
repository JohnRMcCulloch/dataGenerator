namespace dataGenerator.FileWriter;

public interface IFileWriter
{
    void WriteToFile(string content, string fileName);
}