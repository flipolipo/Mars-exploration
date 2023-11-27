using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.Configuration.Model;

public record startingConfiguration(Map Map, Coordinate LandingSpotOfRocket, IEnumerable<string> ResourceSymbols, int StepsToTimeOut);
