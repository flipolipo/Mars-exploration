using Codecool.MarsExploration.MapExplorer.MarsCommandCenter.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.MarsRover.Model;

public class Rover
{
    public uint Id { get; init; }
    public string Name { get; init; }
    public Coordinate CurrentPosition { get; set; }
    public int Sight { get; init; }
    public Dictionary<Coordinate, string> ResourceCoordinates { get; init; }
    public RoverState State { get; set; }
    public Queue<Coordinate> PreviousMoves { get; init; }
    public int NuberOfStoredSteps { get; init; }
    public HashSet<Coordinate> ExploredCoordinates { get; init; }
    public CommandCentreBuildConfiguration? CommandCentreConf { get; set; }
    public int TaskProgress { get; set; }
    public int ExtractedMinerals { get; set; }
    public int ExtractedWater { get; set; }

    public Rover(uint id,
                string name,
                Coordinate currentPosition,
                int sight,
                Dictionary<Coordinate, string> resourceCoordinates,
                RoverState state,
                Queue<Coordinate> previousMoves,
                int nuberOfStoredSteps,
                HashSet<Coordinate> exploredCoordinates,
                CommandCentreBuildConfiguration commandCentreConf,
                int taskProgress,
                int extractedMinerals,
                int extractedWater)
    {
        Id = id;
        Name = name;
        CurrentPosition = currentPosition;
        Sight = sight;
        ResourceCoordinates = resourceCoordinates;
        State = state;
        PreviousMoves = previousMoves;
        NuberOfStoredSteps = nuberOfStoredSteps;
        ExploredCoordinates = exploredCoordinates;
        CommandCentreConf = commandCentreConf;
        TaskProgress = taskProgress;
        ExtractedMinerals = extractedMinerals;
        ExtractedWater = extractedWater;
    }
}
