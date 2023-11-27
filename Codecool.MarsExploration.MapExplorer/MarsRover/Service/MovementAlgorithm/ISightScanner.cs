using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.MarsRover.Service.MovementAlgorithm;

public interface ISightScanner
{
    public Dictionary<Coordinate, string> Scan(Coordinate currentPosition, int sight, string?[,] representation);
}