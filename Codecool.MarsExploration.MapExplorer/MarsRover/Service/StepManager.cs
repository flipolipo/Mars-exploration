using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.Logger;
using Codecool.MarsExploration.MapExplorer.MarsCommandCenter.Enums;
using Codecool.MarsExploration.MapExplorer.MarsCommandCenter.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service.MovementAlgorithm;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;
using System.Security.AccessControl;


namespace Codecool.MarsExploration.MapExplorer.MarsRover.Service;

public class StepManager : IStepManager
{
    private readonly ISightScanner _sightScanner;
    private readonly IStepAnalyzer _stepAnalyzer;
    private readonly LoggerBase _logger;

    public StepManager(ISightScanner sightScanner, IStepAnalyzer stepAnalyzer, LoggerBase logger)
    {
        _sightScanner = sightScanner;
        _stepAnalyzer = stepAnalyzer;
        _logger = logger;
    }

    public void UpdateRoverResources(Rover rover, Map map, string[] resourceSymbolsToMonitor)
    {
        var spaceAroundRover = _sightScanner.Scan(rover.CurrentPosition, rover.Sight, map.Representation);
        foreach (var space in spaceAroundRover)
        {
            var coordinate = space.Key;
            var symbol = space.Value;
            if (!resourceSymbolsToMonitor.Contains(symbol)) continue;
            if (!rover.ResourceCoordinates.ContainsKey(coordinate)) rover.ResourceCoordinates.Add(coordinate, symbol);
        }
    }

    public void ResearchMove(Rover rover, Map map, int step)
    {
        var nextMove = _stepAnalyzer.GetMove(rover, map);
        ManageNextMove(rover, nextMove);
        _logger.LogReserchPosition(step, rover);
    }

    private void ManageNextMove(Rover rover, Coordinate nextMove)
    {
        rover.ExploredCoordinates.Add(rover.CurrentPosition);
        rover.PreviousMoves.Enqueue(rover.CurrentPosition);
        rover.CurrentPosition = nextMove;
        if (rover.PreviousMoves.Count() > rover.NuberOfStoredSteps) rover.PreviousMoves.Dequeue();
    }

    public void MoveToLocation(Rover rover, Coordinate location)
    {
        var spaceShipX = location.X;
        var spaceShipY = location.Y;
        var roverX = rover.CurrentPosition.X;
        var roverY = rover.CurrentPosition.Y;
        if (spaceShipX > roverX)
        {
            rover.CurrentPosition = rover.CurrentPosition with { X = roverX + 1 };
        }
        else if (spaceShipX < roverX)
        {
            rover.CurrentPosition = rover.CurrentPosition with { X = roverX - 1 };
        }
        else if (spaceShipY > roverY)
        {
            rover.CurrentPosition = rover.CurrentPosition with { Y = roverY + 1 };
        }
        else if (spaceShipY < roverY)
        {
            rover.CurrentPosition = rover.CurrentPosition with { Y = roverY - 1 };
        }
    }

    public void BuildCommandCentre(int requiredTurns, Rover rover, int commandCentreRadius, SimulationContext simulationContext)
    {
        var id = (uint)(simulationContext.CommandCentres.Count() + 1);
        var commandCentreLoc = rover.CommandCentreConf.BuildLocation;
        var roversRespawnLoc = rover.CommandCentreConf.RoversRespawn;

        if (rover.CurrentPosition.Equals(commandCentreLoc))
        {
            rover.TaskProgress++;
            _logger.LogCommandCentreBuilding(simulationContext.NumberOfSteps, rover, id, requiredTurns);
            if (rover.TaskProgress == requiredTurns)
            {
                rover.TaskProgress = 0;
                rover.State = RoverState.MoveToResource;
                var rovers = new HashSet<Rover>() { rover };
                var spaceAround = _sightScanner.Scan(commandCentreLoc, commandCentreRadius, simulationContext.Map.Representation);
                simulationContext.CommandCentres.Add(new CommandCentre(id, $"commandCentre-{id}", commandCentreLoc, roversRespawnLoc, commandCentreRadius, spaceAround, rovers, CommandCentreState.GatheringRawMaterials, 0, 0, 0, 0, 0));
            }
        }
        else
        {
            MoveToLocation(rover, commandCentreLoc);
            _logger.LogMoveToBuild(simulationContext.NumberOfSteps, rover);
        }
    }

    public void ExtractMineral(Rover rover, int requiredTurns, int step, string resourceType)
    {
        rover.TaskProgress++;
        _logger.LogExtraction(step, rover, resourceType, requiredTurns);
        if (rover.TaskProgress == requiredTurns)
        {
            rover.TaskProgress = 0;
            rover.State = RoverState.ResourceDelivery;
        }
    }

    public void MoveToResource(Rover rover, Coordinate resourceLoc, int step, string resourceType)
    {
        MoveToLocation(rover, resourceLoc);
        if (rover.CurrentPosition.Equals(resourceLoc)) rover.State = RoverState.Extraction;
        _logger.LogMoveToResource(step, rover, resourceType);
    }

    public void DeliverResource(Rover rover, CommandCentre commandCentre, string resourceType, int step)
    {
        MoveToLocation(rover, commandCentre.Position);
        if (rover.CurrentPosition.Equals(commandCentre.Position))
        {
            if (resourceType == "mineral")
            {
                rover.ExtractedMinerals++;
                commandCentre.ActMineralAmount++;
                commandCentre.TotalMineralAmount++;
            }
            else if (resourceType == "water")
            {
                rover.ExtractedWater++;
                commandCentre.ActWaterAmount++;
                commandCentre.TotalWaterAmount++;
            }
            rover.State = RoverState.MoveToResource;
        }
        _logger.LogDelivery(step, rover, resourceType);
    }
}
