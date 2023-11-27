using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.MarsRover.Service.MovementAlgorithm;

public class StepAnalyzer : IStepAnalyzer
{
    private static readonly Random _random = new();

    public Coordinate GetMove(Rover rover, Map map)
    {
        var leftResources = new Dictionary<Coordinate, (string, Coordinate)>();
        if (rover.CurrentPosition.X - rover.Sight > 0) leftResources.Add(rover.CurrentPosition, ("", rover.CurrentPosition with { X = rover.CurrentPosition.X - 1 }));
        var rightResources = new Dictionary<Coordinate, (string, Coordinate)>();
        if (rover.CurrentPosition.X + rover.Sight < map.Representation.GetLength(0) - 1) rightResources.Add(rover.CurrentPosition, ("", rover.CurrentPosition with { X = rover.CurrentPosition.X + 1 }));
        var upperResources = new Dictionary<Coordinate, (string, Coordinate)>();
        if (rover.CurrentPosition.Y - rover.Sight > 0) upperResources.Add(rover.CurrentPosition, ("", rover.CurrentPosition with { Y = rover.CurrentPosition.Y - 1 }));
        var lowerResources = new Dictionary<Coordinate, (string, Coordinate)>();
        if (rover.CurrentPosition.Y + rover.Sight < map.Representation.GetLength(1) - 1) lowerResources.Add(rover.CurrentPosition, ("", rover.CurrentPosition with { Y = rover.CurrentPosition.Y + 1 }));
        foreach (var space in rover.ResourceCoordinates)
        {
            var coordinate = space.Key;
            var symbol = space.Value;
            if (coordinate.Y >= rover.CurrentPosition.Y - rover.Sight
                && coordinate.Y <= rover.CurrentPosition.Y + rover.Sight
                && coordinate.X >= rover.CurrentPosition.X - rover.Sight
                && coordinate.X <= rover.CurrentPosition.X + rover.Sight)
            {
                if (coordinate.X >= rover.CurrentPosition.X - rover.Sight
                    && coordinate.X < rover.CurrentPosition.X)
                {
                    leftResources.Add(coordinate, (symbol, rover.CurrentPosition with { X = rover.CurrentPosition.X - 1 }));
                }
                else if (coordinate.X <= rover.CurrentPosition.X + rover.Sight
                    && coordinate.X > rover.CurrentPosition.X)
                {
                    rightResources.Add(coordinate, (symbol, rover.CurrentPosition with { X = rover.CurrentPosition.X + 1 }));
                }
                if (coordinate.Y >= rover.CurrentPosition.Y - rover.Sight
                    && coordinate.Y < rover.CurrentPosition.Y)
                {
                    upperResources.Add(coordinate, (symbol, rover.CurrentPosition with { Y = rover.CurrentPosition.Y - 1 }));
                }
                else if (coordinate.Y <= rover.CurrentPosition.Y + rover.Sight
                    && coordinate.Y > rover.CurrentPosition.Y)
                {
                    lowerResources.Add(coordinate, (symbol, rover.CurrentPosition with { Y = rover.CurrentPosition.Y + 1 }));
                }
            }
        }

        var maxDictionaries = GetDictionariesWithMaxElementsAndDoNotIncludePrevoiusMove(rover.PreviousMoves, leftResources, rightResources, upperResources, lowerResources).ToList();
        var randomDictionary = maxDictionaries.Any() ? maxDictionaries.ElementAt(_random.Next(maxDictionaries.Count())) : new Dictionary<Coordinate, (string, Coordinate)>() { { rover.CurrentPosition, ("", rover.PreviousMoves.Last()) } };
        var nextMove = randomDictionary.First().Value.Item2;
        return nextMove;
    }

    private IEnumerable<Dictionary<Coordinate, (string, Coordinate)>> GetDictionariesWithMaxElementsAndDoNotIncludePrevoiusMove(Queue<Coordinate> previousMoves, params Dictionary<Coordinate, (string, Coordinate)>[] dictionaries)
    {
        var maxCount = 1;

        foreach (var dictionary in dictionaries)
        {
            if (dictionary.Count == 0) continue;
            var plannedMove = dictionary.First().Value.Item2;
            if (dictionary.Count() > maxCount && !previousMoves.Contains(plannedMove)) maxCount = dictionary.Count;
        }

        foreach (var dictionary in dictionaries)
        {
            if (dictionary.Count == 0) continue;
            var plannedMove = dictionary.First().Value.Item2;
            if (maxCount == dictionary.Count() && !previousMoves.Contains(plannedMove)) yield return dictionary;
        }
    }
}
