using Codecool.MarsExploration.MapExplorer.Configuration.Service;
using Codecool.MarsExploration.MapExplorer.Exceptions;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

using Codecool.MarsExploration.MapGenerator.Calculators.Service;

namespace Codecool.MarsExploration.MapExplorer.Configuration.Validation;

public class ConfigurationValidator : IConfigurationValidator
{
    private readonly ICoordinateCalculator _coordinateCalculator;

    public ConfigurationValidator(ICoordinateCalculator coordinateCalculator)
    {
        _coordinateCalculator = coordinateCalculator;
    }

    public bool ValidateLandingSpot(Coordinate landingCoordinates, string?[,] representation)
    {
        return representation[landingCoordinates.X, landingCoordinates.Y] == " ";
    }

    public bool ValidateEmptyAdjacentCoordinates(Coordinate landingCoordinate, string?[,] representation)
    {
        var adjacentCoordinates = _coordinateCalculator.GetAdjacentCoordinates(landingCoordinate, representation.GetLength(0));

        foreach (var coordinate in adjacentCoordinates)
        {
            if (representation[coordinate.X, coordinate.Y] == " ") return true;
        }
        return false;
    }

    public bool ValidateMapStringPath(string filePath)
    {
        return !string.IsNullOrEmpty(filePath) && File.Exists(filePath) ? true : throw new InvalidMapFilePathException("Invalid filePath argument");
    }

    public bool ValidateResourceList(IEnumerable<string> listOfResources)
    {
        return (listOfResources.Any() && listOfResources.All(resource => !string.IsNullOrEmpty(resource))) ? true : throw new InvalidResourceListException("Invalid listOfResource argument");
    }

    public bool ValidateTimeOut(int timeOut)
    {
        return timeOut > 0 ? true : throw new InvalidTimeoutException("Invalid timeout argument");
    }
}