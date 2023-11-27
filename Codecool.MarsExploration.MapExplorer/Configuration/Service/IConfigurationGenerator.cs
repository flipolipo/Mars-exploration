using Codecool.MarsExploration.MapExplorer.Configuration.Model;

namespace Codecool.MarsExploration.MapExplorer.Configuration.Service;

public interface IConfigurationGenerator
{
    public startingConfiguration? GetStartingConfiguration(string filePathToMap, IEnumerable<string> resourceSymbols, int stepsToTimeOut);
}
