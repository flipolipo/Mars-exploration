using Codecool.MarsExploration.MapExplorer.Exploration.ExplorationSimulationSteps;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorerTest;

public class SightScannerTest
{
    [Test]
    public void Scan_ReturnsExpectedSpaceAroundRover()
    {
        // Arrange
        var rover = new Rover("Rover1", new Coordinate(1, 2), 1, new Dictionary<Coordinate, string>(), RoverState.Reserch, new Queue<Coordinate>(), 1, new HashSet<Coordinate>());
        var rapresentation = new string[5, 5]
         {
                { "O", "O", "O", "O", "O" },
                { "O", "O", "O", "O", "O" },
                { "O", "O", "R", "O", "O" },
                { "O", "O", "O", "O", "O" },
                { "O", "O", "O", "O", "O" },
         };

        var sightScanner = new SightScanner();

        // Act
        var result = sightScanner.Scan(rover, rapresentation);

        // Assert
        Assert.AreEqual(8, result.Count);
    }
}
