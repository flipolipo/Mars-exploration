using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.Logger;
using Codecool.MarsExploration.MapExplorer.MarsCommandCenter.Enums;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using System.Xml.Linq;

namespace Codecool.MarsExploration.MapExplorer.MarsCommandCenter.Model;

public class CommandCentre
{
    public uint Id { get; init; }
    public string Name { get; init; }
    public Coordinate Position { get; init; }
    public Coordinate RoverRespawnLoc { get; init; }
    public int Radius { get; init; }
    public IDictionary<Coordinate, string> SpaceAround { get; init; }
    public CommandCentreState CurrentState { get; set; }
    public HashSet<Rover> Rovers { get; set; }
    public int TaskProgress { get; set; }
    public int ActWaterAmount { get; set; }
    public int ActMineralAmount { get; set; }
    public int TotalMineralAmount { get; set; }
    public int TotalWaterAmount { get; set; }

    public CommandCentre(uint id, string name, Coordinate position, Coordinate roverRespawnLoc, int radius, IDictionary<Coordinate, string> spaceAround, HashSet<Rover> rovers, CommandCentreState currentState, int taskProgress, int actWaterAmount, int actMineralAmount, int totalWaterAmount, int totalMineralAmount)
    {
        Id = id;
        Name = name;
        Position = position;
        RoverRespawnLoc = roverRespawnLoc;
        Radius = radius;
        SpaceAround = spaceAround;
        CurrentState = currentState;
        Rovers = rovers;
        TaskProgress = taskProgress;
        ActWaterAmount = actWaterAmount;
        ActMineralAmount = actMineralAmount;
        TotalWaterAmount = totalWaterAmount;
        TotalMineralAmount = totalMineralAmount;
    }

    public void CheckForUpdate(SimulationContext simulationContext, LoggerBase logger, int requiredTurns, int requiredResourceAmount)
    {
        var roverId = (uint)Rovers.Count() + 1;
        var roverName = $"rover-{roverId}";
        if (Rovers.Count() == 1 && ActMineralAmount >= requiredResourceAmount)
        {
            CurrentState = CommandCentreState.ConstructingWaterRover;
            TaskProgress++;
            logger.LogConstruction(simulationContext.NumberOfSteps, roverName, TaskProgress, requiredTurns, "water");
            if (TaskProgress == requiredTurns)
            {
                var rover = new Rover(roverId, roverName, RoverRespawnLoc, 2, new(), RoverState.MoveToResource, new(), 40, new(), null, 0, 0, 0);
                Rovers.Add(rover);
                simulationContext.Rovers.Add(rover);
                ActMineralAmount = ActMineralAmount - requiredResourceAmount;
                TaskProgress = 0;
                CurrentState = CommandCentreState.GatheringRawMaterials;
            }
        }
        else if (Rovers.Count() == 2 && CurrentState != CommandCentreState.ConstructingWaterRover && ActWaterAmount >= requiredResourceAmount)
        {
            CurrentState = CommandCentreState.ConstructingReserchRover;
            TaskProgress++;
            logger.LogConstruction(simulationContext.NumberOfSteps, roverName, TaskProgress, requiredTurns, "reserch");
            if (TaskProgress == requiredTurns)
            {
                var rover = new Rover(roverId, roverName, RoverRespawnLoc, 2, new(), RoverState.Reserch, new(), 40, new(), null, 0, 0, 0);
                Rovers.Add(rover);
                simulationContext.Rovers.Add(rover);
                ActWaterAmount = ActWaterAmount - requiredResourceAmount;
                TaskProgress = 0;
                CurrentState = CommandCentreState.GatheringRawMaterials;
            }
        }
    }

    public Coordinate GetNearestResourceCoordinate(string type)
    {
        string symbol = "";
        if (type == "mineral") symbol = "%";
        else if (type == "water") symbol = "*";
        var resourceCoordinates = SpaceAround.Where(resource => resource.Value == symbol).Select(resource => resource.Key).ToList(); ;
        return resourceCoordinates.Aggregate((nearest, current) => DistanceFromCommandCentre(nearest) < DistanceFromCommandCentre(current) ? nearest : current);
    }

    private double DistanceFromCommandCentre(Coordinate other)
    {
        int deltaX = other.X - Position.X;
        int deltaY = other.Y - Position.Y;
        return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }
}
