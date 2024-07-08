using dataGenerator.Config;
using Xunit;

namespace dataGenerator.Tests.Config;

public class FileConfigTest
{
    private const string ExpectedFilePath = "test/path/sample.txt";

    [Fact]
    public void TestFileConfigConstructorSetter()
    {
        var fileConfig = new FileConfig
        {
            FilePath = ExpectedFilePath
        };
        Assert.Equal(ExpectedFilePath, fileConfig.FilePath);
    }

    [Fact]
    public void TestFileConfigVariableSetter()
    {
        var fileConfig = new FileConfig();
        fileConfig.FilePath = ExpectedFilePath;
        Assert.Equal(ExpectedFilePath, fileConfig.FilePath);
    }
}