using Codecool.MarsExploration.MapExplorer.Configuration.Service;
using Codecool.MarsExploration.MapExplorer.LoaderMap;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;
using Moq;

namespace Codecool.MarsExploration.MapExplorerTest;

public class ConfigurationGeneratorTest
{
    private ConfigurationGenerator _configurationGenerator;
    private Mock<IMapLoader> _mapLoaderMock;
    private Mock<IConfigurationValidator> _configurationValidatorMock;

    [SetUp]
    public void Setup()
    {
        _mapLoaderMock = new Mock<IMapLoader>();
        _configurationValidatorMock = new Mock<IConfigurationValidator>();
        _configurationGenerator = new ConfigurationGenerator(_mapLoaderMock.Object, _configurationValidatorMock.Object);
    }

    [Test]
    public void GetStartingConfiguration_ValidInputs_ReturnsStartingConfiguration()
    {
        // Arrange
        string?[,] representation = new string?[3, 3]
        {
            { " ", " ", " " },
            { " ", " ", " " },
            { " ", " ", " " }
        };

        string filePath = "map.txt";
        IEnumerable<string> listOfResources = new List<string> { "Resource1", "Resource2" };
        int timeOut = 10;
        var landingCoordinates = new Coordinate(1, 1);



        _mapLoaderMock.Setup(x => x.Load(It.IsAny<string>())).Returns(new Map(representation, true));
        _configurationValidatorMock.Setup(x => x.ValidateMapStringPath(It.IsAny<string>())).Returns(true);
        _configurationValidatorMock.Setup(x => x.ValidateResourceList(It.IsAny<IEnumerable<string>>())).Returns(true);
        _configurationValidatorMock.Setup(x => x.ValidateTimeOut(It.IsAny<int>())).Returns(true);
        _configurationValidatorMock.Setup(x => x.ValidateLandingSpot(It.IsAny<Coordinate>(), It.IsAny<string?[,]>())).Returns(true);
        _configurationValidatorMock.Setup(x => x.ValidateEmptyAdjacentCoordinates(It.IsAny<Coordinate>(), It.IsAny<string?[,]>())).Returns(true);

        // Act
        var startingConfiguration = _configurationGenerator.GetStartingConfiguration(filePath, listOfResources, timeOut);

        // Assert
        Assert.IsNotNull(startingConfiguration);
        Assert.IsNotNull(startingConfiguration.Map);
        Assert.AreEqual(listOfResources, startingConfiguration.ResourceSymbols);
        Assert.AreEqual(timeOut, startingConfiguration.StepsToTimeOut);
    }

    [Test]
    public void GetStartingConfiguration_InvalidInputs_ReturnsStartingConfiguration()
    {
        // Arrange
        string?[,] representation = new string?[3, 3]
        {
            { " ", " ", " " },
            { " ", " ", " " },
            { " ", " ", " " }
        };

        string filePath = "invalid_map.txt";
        IEnumerable<string> listOfResources = new List<string> { "Resource1", "Resource2" };
        int timeOut = 10;
        var landingCoordinates = new Coordinate(1, 1);



        _mapLoaderMock.Setup(x => x.Load(It.IsAny<string>())).Returns(new Map(representation, true));
        _configurationValidatorMock.Setup(x => x.ValidateMapStringPath(It.IsAny<string>())).Returns(false);
        _configurationValidatorMock.Setup(x => x.ValidateResourceList(It.IsAny<IEnumerable<string>>())).Returns(false);
        _configurationValidatorMock.Setup(x => x.ValidateTimeOut(It.IsAny<int>())).Returns(false);
        _configurationValidatorMock.Setup(x => x.ValidateLandingSpot(It.IsAny<Coordinate>(), It.IsAny<string?[,]>())).Returns(false);
        _configurationValidatorMock.Setup(x => x.ValidateEmptyAdjacentCoordinates(It.IsAny<Coordinate>(), It.IsAny<string?[,]>())).Returns(false);

        // Act
        var startingConfiguration = _configurationGenerator.GetStartingConfiguration(filePath, listOfResources, timeOut);

        // Assert
        Assert.IsNull(startingConfiguration);
    }
}
