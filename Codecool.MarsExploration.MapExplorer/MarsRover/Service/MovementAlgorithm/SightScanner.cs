using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.MarsRover.Service.MovementAlgorithm;

public class SightScanner : ISightScanner
{
    public Dictionary<Coordinate, string> Scan(Coordinate currentPosition, int sight, string?[,] representation)
    {
        var spaceAround = new Dictionary<Coordinate, string>();
        var leftLimit = currentPosition.X - sight > 0 ? currentPosition.X - sight : 0;
        var rightLimit = currentPosition.X + sight < representation.GetLength(0) ? currentPosition.X + sight : representation.GetLength(0) - 1;
        var upperLimit = currentPosition.Y - sight > 0 ? currentPosition.Y - sight : 0;
        var lowerLimit = currentPosition.Y + sight < representation.GetLength(1) ? currentPosition.Y + sight : representation.GetLength(1) - 1;
        for (int x = leftLimit; x <= rightLimit; x++)
        {
            for (int y = upperLimit; y <= lowerLimit; y++)
            {
                var coordinate = new Coordinate(x, y);
                var symbol = representation[x, y];
                if (currentPosition.Equals(coordinate))
                {
                    continue;
                }
                spaceAround.Add(coordinate, symbol);
            }
        }
        return spaceAround;
    }
}
