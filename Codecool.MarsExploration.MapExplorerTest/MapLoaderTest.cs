using Codecool.MarsExploration.MapExplorer.LoaderMap;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorerTest;

public class MapLoaderTest
{

    private const string ValidMapFilePath = "valid_map.txt";
    private const string InvalidMapFilePath = "invalid_map.txt";

    [SetUp]
    public void SetUp()
    {
        // Create a valid map file for testing
        File.WriteAllText(ValidMapFilePath, "ABC\nDEF\nGHI");

        // Create an invalid map file for testing
        File.WriteAllText(InvalidMapFilePath, "");
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up the test files
        File.Delete(ValidMapFilePath);
        File.Delete(InvalidMapFilePath);
    }

    [Test]
    public void Load_ValidMapFile_ReturnsValidMap()
    {
        // Arrange
        MapLoader mapLoader = new MapLoader();

        // Act
        Map map = mapLoader.Load(ValidMapFilePath);

        // Assert
        Assert.NotNull(map.Representation);
    }

    [Test]
    public void Load_InvalidMapFile_ReturnsInvalidMap()
    {
        // Arrange
        MapLoader mapLoader = new MapLoader();

        // Act
        Map map = mapLoader.Load(InvalidMapFilePath);

        // Assert
        Assert.Null(map.Representation);
    }
}
