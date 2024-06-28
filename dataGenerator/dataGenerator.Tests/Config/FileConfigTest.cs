using dataGenerator.Config;
using Xunit;

namespace dataGenerator.Tests.Config;

public class FileConfigTest
{
    [Fact]
    public void TestFileConfigSetter()
    {
        const string expectedFilePath = "test/path/sample.txt";
        var fileConfig = new FileConfig
        {
            FilePath = expectedFilePath
        };
        Assert.Equal(expectedFilePath, fileConfig.FilePath);
    }
}