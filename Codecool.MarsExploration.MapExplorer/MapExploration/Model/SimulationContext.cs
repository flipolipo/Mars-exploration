using Codecool.MarsExploration.MapExplorer.MarsCommandCenter.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Model;

public class SimulationContext
{
    public int NumberOfSteps { get; set; }
    public int TimeoutSteps { get; init; }
    public HashSet<Rover> Rovers { get; init; }
    public HashSet<CommandCentre> CommandCentres { get; init; }
    public Coordinate SpaceshipLocation { get; init; }
    public Map Map { get; init; }
    public string[] ResourceSymbolsToMonitor { get; init; }
    public ExplorationOutcome? Outcome { get; set; }
    public HashSet<CommandCentre> AvaibleCommandCenters { get; set; }

    public SimulationContext(int numberOfSteps, int timeoutSteps, HashSet<Rover> rovers, HashSet<CommandCentre> commandCentres, Coordinate spaceshipLocation, Map map, string[] resourceSymbolsToMonitor, ExplorationOutcome? outcome)
    {
        NumberOfSteps = numberOfSteps;
        TimeoutSteps = timeoutSteps;
        Rovers = rovers;
        CommandCentres = commandCentres;
        SpaceshipLocation = spaceshipLocation;
        Map = map;
        ResourceSymbolsToMonitor = resourceSymbolsToMonitor;
        Outcome = outcome;
    }
}
