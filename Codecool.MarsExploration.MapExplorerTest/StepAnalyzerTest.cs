using Codecool.MarsExploration.MapExplorer.Exploration.ExplorationSimulationSteps;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorerTest;

public class StepAnalyzerTest
{
    [Test]
    public void GetMove_ReturnsValidCoordinate()
    {
        // Arrange
        var rover = new Rover("Rover1", new Coordinate(2, 2), 1, new Dictionary<Coordinate, string>(), RoverState.Returning, new Queue<Coordinate>(), 1, new HashSet<Coordinate>());
        var map = new Map(new string[,]
        {
                { "O", "O", "O", "O", "O" },
                { "O", "O", "O", "O", "O" },
                { "O", "O", "R", "O", "O" },
                { "O", "O", "O", "O", "O" },
                { "O", "O", "O", "O", "O" },
                { "O", "O", "O", "O", "O" },
                { "O", "O", "O", "O", "O" },
                { "O", "O", "O", "O", "O" }
        });

        var stepAnalyzer = new StepAnalyzer();

        // Act
        var nextMove = stepAnalyzer.GetMove(rover, map);

        // Assert
        Assert.IsTrue(nextMove.X >= 1 && nextMove.X <= 3 && nextMove.Y >= 1 && nextMove.Y <= 3);
    }
}
