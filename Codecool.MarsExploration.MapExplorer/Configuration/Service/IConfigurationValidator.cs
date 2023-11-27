
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.Configuration.Service;

public interface IConfigurationValidator
{
    public bool ValidateLandingSpot(Coordinate landingCoordinates, string?[,] representation);

    public bool ValidateEmptyAdjacentCoordinates(Coordinate landingCoordinate, string?[,] representation);

    public bool ValidateMapStringPath(string filePath);

    public bool ValidateResourceList(IEnumerable<string> listOfResources);

    public bool ValidateTimeOut(int timeOut);
}
