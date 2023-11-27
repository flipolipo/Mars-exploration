using Codecool.MarsExploration.MapExplorer.Configuration.Model;
using Codecool.MarsExploration.MapExplorer.Exceptions;
using Codecool.MarsExploration.MapExplorer.LoaderMap;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.Configuration.Service;

public class ConfigurationGenerator : IConfigurationGenerator
{
    private static readonly Random _random = new();
    private readonly IMapLoader _mapLoader;
    private readonly IConfigurationValidator _configurationValidator;

    public ConfigurationGenerator(IMapLoader mapLoader, IConfigurationValidator configurationValidator)
    {
        _mapLoader = mapLoader;
        _configurationValidator = configurationValidator;
    }

    public startingConfiguration? GetStartingConfiguration(string filePathToMap, IEnumerable<string> resourceSymbols, int stepsToTimeOut)
    {
        try
        {
            _configurationValidator.ValidateMapStringPath(filePathToMap);
            var map = _mapLoader.Load(filePathToMap);
            var landingCoordinate = GetLandingCoordinate(map.Representation);

            _configurationValidator.ValidateResourceList(resourceSymbols);
            _configurationValidator.ValidateTimeOut(stepsToTimeOut);
            return new(map, landingCoordinate, resourceSymbols, stepsToTimeOut);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    private Coordinate GetLandingCoordinate(string?[,] representation)
    {
        var allCordinates = GetMapCoordinates(representation).ToList();

        while (allCordinates.Any())
        {

            var randomCoordinate = allCordinates.ElementAt(_random.Next(allCordinates.Count()));
            if (_configurationValidator.ValidateLandingSpot(randomCoordinate, representation))
            {
                if (_configurationValidator.ValidateEmptyAdjacentCoordinates(randomCoordinate, representation)) return randomCoordinate;
                else allCordinates.Remove(randomCoordinate);
            }
            else allCordinates.Remove(randomCoordinate);
        }
        throw new LandingSpotNotFoundException("There is no free spot on the map having free space around it");
    }

    private IEnumerable<Coordinate> GetMapCoordinates(string?[,] representation)
    {
        var width = representation.GetLength(0);
        var height = representation.GetLength(1);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                yield return new Coordinate(i, j);
            }
        }
    }
}
