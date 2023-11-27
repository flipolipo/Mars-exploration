using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.MarsCommandCenter.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.MarsRover.Service;

public interface IStepManager
{
    public void ResearchMove(Rover rover, Map map, int step);
    public void MoveToLocation(Rover rover, Coordinate spaceShipCoordinates);
    public void UpdateRoverResources(Rover rover, Map map, string[] resourceSymbolsToMonitor);
    public void BuildCommandCentre(int requiredTurns, Rover rover, int commandCentreRadius, SimulationContext simulationContext);
    public void ExtractMineral(Rover rover, int requiredTurns, int step, string resourceType);
    public void MoveToResource(Rover rover, Coordinate resourceLoc, int step, string resourceType);
    public void DeliverResource(Rover rover, CommandCentre commandCentre, string type, int step);
}
