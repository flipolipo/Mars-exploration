using Codecool.MarsExploration.MapExplorer.Excepptions;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Service;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;
using Moq;

namespace Codecool.MarsExploration.MapExplorerTest;

public class RoverDeployerTest
{
    private Mock<ICoordinateCalculator> mockCoordinateCalculator;
    private RoverDeployer roverDeployer;
    private Map map;
    private Coordinate landingSpotOfRocket;

    [SetUp]
    public void SetUp()
    {
        mockCoordinateCalculator = new Mock<ICoordinateCalculator>();
        roverDeployer = new RoverDeployer(mockCoordinateCalculator.Object);
        map = new Map(new string?[5, 5], true);
        landingSpotOfRocket = new Coordinate(2, 2);
    }

    [Test]
    public void Deploy_ValidMapAndLandingSpot_CreatesRover()
    {
        // Arrange
        List<Coordinate> adjacentCoordinates = new List<Coordinate>
        {
            new Coordinate(1, 2),
            new Coordinate(3, 2),
            new Coordinate(2, 1),
            new Coordinate(2, 3),
        };
        mockCoordinateCalculator.Setup(mock => mock.GetAdjacentCoordinates(landingSpotOfRocket, 5, 1)).Returns(adjacentCoordinates);

        // Act
        Rover rover = roverDeployer.Deploy(map, landingSpotOfRocket);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(rover.Id, Is.EqualTo("rover-1"));
            Assert.IsEmpty(rover.ResourceCoordinates);
            Assert.That(rover.Sight, Is.GreaterThan(0));
        });
    }

    [Test]
    public void Deploy_Null_Map_ThrowsException()
    {
        Assert.Throws<MapNotFoundException>(() => roverDeployer.Deploy(null, landingSpotOfRocket));
    }

    [Test]
    public void Deploy_Null_LandingSpotOfRocket_ThrowsException()
    {
        Assert.Throws<CoordinateNotFoundException>(() => roverDeployer.Deploy(map, null));
    }
}
