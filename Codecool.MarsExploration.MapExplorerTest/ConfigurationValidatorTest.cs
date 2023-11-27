using Codecool.MarsExploration.MapExplorer.Configuration.Service;
using Codecool.MarsExploration.MapExplorer.Configuration.Validation;
using Codecool.MarsExploration.MapExplorer.Exceptions;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Service;
using Moq;

namespace Codecool.MarsExploration.MapExplorerTest;

public class ConfigurationValidatorTest
{
    private Mock<ICoordinateCalculator> coordinateCalculatorMock;
    private IConfigurationValidator configurationValidator;
    private Coordinate landingCoordinates;
    private string?[,] representation;
    private string filePath;
    private IEnumerable<string> listOfResources;
    int timeOut;

    [SetUp]
    public void SetUp()
    {
        coordinateCalculatorMock = new Mock<ICoordinateCalculator>();
        configurationValidator = new ConfigurationValidator(coordinateCalculatorMock.Object);

        landingCoordinates = new Coordinate(2, 2);
        representation = new string?[5, 5];
        filePath = "map.txt";
        listOfResources = new List<string> { "Resource1", "Resource2" };
        timeOut = 10;

        File.WriteAllText(filePath, "hhv");
    }

    [TearDown]
    public void TearDown()
    {
        File.Delete(filePath);
    }

    [Test]
    public void Validate_InvalidLandingSpot_ReturnsFalse()
    {
        // Arrange
        representation[1, 1] = "A";
        landingCoordinates = new Coordinate(1, 1);

        // Act
        bool result = configurationValidator.ValidateLandingSpot(landingCoordinates, representation);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Validate_InvalidAdjacentCoordinates_ReturnsFalse()
    {
        // Arrange
        coordinateCalculatorMock.Setup(mock => mock.GetAdjacentCoordinates(It.IsAny<Coordinate>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new List<Coordinate>());

        // Act
        bool result = configurationValidator.ValidateEmptyAdjacentCoordinates(landingCoordinates, representation);

        // Assert
        Assert.False(result);
    }

    [Test]
    public void Validate_InvalidMapFilePath_ReturnsFalse()
    {
        // Arrange
        filePath = "invalid_path.txt";

        // Act & Assert
        Assert.Throws<InvalidMapFilePathException>(() => configurationValidator.ValidateMapStringPath(filePath));
    }

    [Test]
    public void Validate_EmptyResourceList_ReturnsFalse()
    {
        // Arrange
        listOfResources = new List<string>();

        // Act & Assert
        Assert.Throws<InvalidResourceListException>(() => configurationValidator.ValidateResourceList(listOfResources));
    }

    [Test]
    public void Validate_NegativeTimeout_ReturnsFalse()
    {
        // Arrange
        timeOut = -5;

        //Act & Assert
        Assert.Throws<InvalidTimeoutException>(() => configurationValidator.ValidateTimeOut(timeOut));
    }

    [Test]
    public void Validate_Timeout_ReturnsTrue()
    {
        // Arrange
        timeOut = 5;

        //Act
        bool result = configurationValidator.ValidateTimeOut(timeOut);

        //Assert
        Assert.That(result, Is.True);
    }
}
