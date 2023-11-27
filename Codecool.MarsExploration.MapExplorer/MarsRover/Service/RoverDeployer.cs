using Codecool.MarsExploration.MapExplorer.Excepptions;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Service;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.MarsRover.Service;

public class RoverDeployer : IRoverDeployer
{
    private static readonly Random _random = new();

    private ICoordinateCalculator _coordinateCalculator;
    public RoverDeployer(ICoordinateCalculator coordinateCalculator)
    {
        _coordinateCalculator = coordinateCalculator;
    }

    public Rover Deploy(uint id, string name, int sight, Map map, Coordinate baseLocation, RoverState state, int storedSteps)
    {
        if (map == null) throw new MapNotFoundException("Map not found");
        if (baseLocation == null) throw new CoordinateNotFoundException("Landing spot of rocket not found");

        var adjacentCoordinates = _coordinateCalculator.GetAdjacentCoordinates(baseLocation, map.Representation.GetLength(0)).ToList();
        var emptySpot = GetEmptyCoordinate(map, adjacentCoordinates);

        return new Rover(id, name, emptySpot, sight, new(), state, new(), storedSteps, new(), null, 0, 0, 0);
    }

    private Coordinate? GetEmptyCoordinate(Map map, List<Coordinate> adjacentCoordinates)
    {
        while (adjacentCoordinates.Any())
        {
            var randomCoordinate = adjacentCoordinates.ElementAt(_random.Next(adjacentCoordinates.Count()));
            if (map.Representation[randomCoordinate.X, randomCoordinate.Y] == " ") return randomCoordinate;
            adjacentCoordinates.Remove(randomCoordinate);
        }
        return null;
    }
}
