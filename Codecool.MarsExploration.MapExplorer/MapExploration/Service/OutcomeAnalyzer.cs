using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.MarsCommandCenter.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Service;

using Codecool.MarsExploration.MapGenerator.MapElements.Model;
using System.Collections.Generic;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service;

public class OutcomeAnalyzer : IOutcomeAnalyzer
{
    private static readonly Random _random = new();
    private readonly int DistanceOfMineralFromWater;
    private readonly ICoordinateCalculator _coordinateCalculator;

    public OutcomeAnalyzer(ICoordinateCalculator coordinateCalculator, int distanceOfMineralFromWater)
    {
        _coordinateCalculator = coordinateCalculator;
        DistanceOfMineralFromWater = distanceOfMineralFromWater;
    }

    public void Analize(SimulationContext simulationContext)
    {
        CheckIfMapColonizable(simulationContext);

        if (simulationContext.Rovers.Any(rover => rover.CommandCentreConf != null))
        {
            simulationContext.Outcome = ExplorationOutcome.Colonizable;
        }
        else if (CheckError(simulationContext))
        {
            simulationContext.Outcome = ExplorationOutcome.Error;
        }
        else if (simulationContext.NumberOfSteps >= simulationContext.TimeoutSteps)
        {
            simulationContext.Outcome = ExplorationOutcome.Timeout;
        }
    }

    private bool CheckError(SimulationContext simulationContext)
    {
        var mapSize = simulationContext.Map.Representation.GetLength(0);
        var roverSight = simulationContext.Rovers.ElementAt(0).Sight;
        var currentExploredFields = simulationContext.Rovers.ElementAt(0).ExploredCoordinates.Count();
        var numberOfFieldsToExplore = Math.Pow(mapSize - (2 * roverSight), 2);
        if (numberOfFieldsToExplore <= currentExploredFields)
        {
            return true;
        }
        return false;
    }

    private void CheckIfMapColonizable(SimulationContext simulationContext)
    {
        var rovers = simulationContext.Rovers;
        foreach (var rover in rovers)
        {
            if (rover.CommandCentreConf == null)
            {
                var foundResources = rover.ResourceCoordinates;
                var checkedResources = CheckIfResourcesNotOccupiedByAnotherCommandCentre(simulationContext, foundResources);
                LookForCoordinatesThatMeetColonizationConditions(rover, simulationContext, checkedResources);
            }
        }
    }

    private void LookForCoordinatesThatMeetColonizationConditions(Rover rover, SimulationContext simulationContext, Dictionary<Coordinate, string> checkedResources)
    {
        var maxMapX = simulationContext.Map.Representation.GetLength(0);
        var maxMapY = simulationContext.Map.Representation.GetLength(1);

        var CoordinatePairsMeetColonizationCondition = GetCoordinatePairsMeetColonizationCondition(checkedResources);
        CheckIfThereIsPossibilityToBuildCommandCentre(rover, checkedResources, maxMapX, maxMapY, CoordinatePairsMeetColonizationCondition);
    }

    private void CheckIfThereIsPossibilityToBuildCommandCentre(Rover rover, Dictionary<Coordinate, string> checkedResources, int maxMapX, int maxMapY, HashSet<HashSet<Coordinate>> coordinatePairsMeetColonizationCondition)
    {
        while (coordinatePairsMeetColonizationCondition.Any())
        {
            var randomPossibility = coordinatePairsMeetColonizationCondition.ElementAt(_random.Next(coordinatePairsMeetColonizationCondition.Count()));
            var mineralCoordinate = randomPossibility.ElementAt(0);
            var waterCoordinate = randomPossibility.ElementAt(1);
            var averageCoordinateX = (mineralCoordinate.X + waterCoordinate.X) / 2;
            var averageCoordinateY = (mineralCoordinate.Y + waterCoordinate.Y) / 2;

            var maxRadius = DistanceOfMineralFromWater / 2;
            for (int radius = 1; radius <= maxRadius; radius++)
            {
                var leftLimit = averageCoordinateX - radius > 0 ? averageCoordinateX - radius : 0;
                var rightLimit = averageCoordinateX + radius < maxMapX ? averageCoordinateX + radius : maxMapX - 1;
                var upperLimit = averageCoordinateY - radius > 0 ? averageCoordinateY - radius : 0;
                var lowerLimit = averageCoordinateY + radius < maxMapY ? averageCoordinateY + radius : maxMapY - 1;

                var freeResourcesFromArea = GetFreeResourcesFromArea(checkedResources, leftLimit, rightLimit, upperLimit, lowerLimit);

                while (freeResourcesFromArea.Any())
                {
                    var randomFreeResourceCoordinate = freeResourcesFromArea.ElementAt(_random.Next(freeResourcesFromArea.Count()));
                    var adjacentFreeCoordinates = _coordinateCalculator.GetAdjacentCoordinates(randomFreeResourceCoordinate, maxMapX).Where(coordinate => freeResourcesFromArea.Contains(coordinate));
                    if (adjacentFreeCoordinates.Any())
                    {
                        var randomAdjacentFreeCoordinate = adjacentFreeCoordinates.ElementAt(_random.Next(adjacentFreeCoordinates.Count()));
                        var commandCentreConf = new CommandCentreBuildConfiguration(randomFreeResourceCoordinate, randomAdjacentFreeCoordinate);
                        rover.CommandCentreConf = commandCentreConf;
                        if (rover.Id % 2 != 0) rover.State = RoverState.Build;
                        break;
                    }
                    freeResourcesFromArea.Remove(randomFreeResourceCoordinate);
                }
                if (rover.CommandCentreConf != null) break;
            }
            if (rover.CommandCentreConf != null) break;
            coordinatePairsMeetColonizationCondition.Remove(randomPossibility);
        }
    }

    private HashSet<HashSet<Coordinate>> GetCoordinatePairsMeetColonizationCondition(Dictionary<Coordinate, string> checkedResources)
    {
        var possibleCoordinates = new HashSet<HashSet<Coordinate>>();

        foreach (var mineralResource in checkedResources)
        {
            if (mineralResource.Value == "%")
            {
                foreach (var waterResource in checkedResources)
                {
                    if (waterResource.Value == "*")
                    {
                        var mineralCoordinate = mineralResource.Key;
                        var waterCoordinate = waterResource.Key;

                        if (mineralCoordinate.X - DistanceOfMineralFromWater <= waterCoordinate.X &&
                            mineralCoordinate.X + DistanceOfMineralFromWater >= waterCoordinate.X &&
                            mineralCoordinate.Y - DistanceOfMineralFromWater <= waterCoordinate.Y &&
                            mineralCoordinate.Y + DistanceOfMineralFromWater >= waterCoordinate.Y)
                        {
                            var possibility = new HashSet<Coordinate>() { mineralCoordinate, waterCoordinate };
                            possibleCoordinates.Add(possibility);
                        }
                    }
                }
            }
        }

        return possibleCoordinates;
    }

    private List<Coordinate> GetFreeResourcesFromArea(Dictionary<Coordinate, string> checkedResources, int leftLimit, int rightLimit, int upperLimit, int lowerLimit)
    {
        var freeResourcesFromArea = new List<Coordinate>();
        List<Coordinate> freeResourcesCoordinates = checkedResources
                                                                    .Where(resource => resource.Value == " ")
                                                                    .Select(resource => resource.Key)
                                                                    .ToList();

        foreach (var coordinate in freeResourcesCoordinates)
        {
            if (leftLimit <= coordinate.X &&
                rightLimit >= coordinate.X &&
                upperLimit <= coordinate.Y &&
                lowerLimit >= coordinate.Y)
            {
                freeResourcesFromArea.Add(coordinate);
            }
        }
        return freeResourcesFromArea;
    }

    private Dictionary<Coordinate, string> CheckIfResourcesNotOccupiedByAnotherCommandCentre(SimulationContext simulationContext, Dictionary<Coordinate, string> foundResources)
    {
        var validResources = new Dictionary<Coordinate, string>();
        var coomandCentres = simulationContext.CommandCentres;
        var mapXLimit = simulationContext.Map.Representation.GetLength(0);
        var mapYLimit = simulationContext.Map.Representation.GetLength(1);

        foreach (var resource in foundResources)
        {
            var resourceCoordinate = resource.Key;
            if (!checkIfCoordinateBelongsToAnotherCommandCenter(coomandCentres, mapXLimit, mapYLimit, resourceCoordinate))
            {
                validResources.Add(resource.Key, resource.Value);
            }
        }
        return validResources;
    }

    private static bool checkIfCoordinateBelongsToAnotherCommandCenter(HashSet<CommandCentre> coomandCentres, int mapXLimit, int mapYLimit, Coordinate resourceCoordinate)
    {
        bool occupied = false;
        foreach (var commandCentre in coomandCentres)
        {
            var leftLimit = commandCentre.Position.X - commandCentre.Radius > 0 ? commandCentre.Position.X - commandCentre.Radius : 0;
            var rightLimit = commandCentre.Position.X + commandCentre.Radius < mapXLimit ? commandCentre.Position.X + commandCentre.Radius : mapXLimit - 1;
            var upperLimit = commandCentre.Position.Y - commandCentre.Radius > 0 ? commandCentre.Position.Y - commandCentre.Radius : 0;
            var lowerLimit = commandCentre.Position.Y + commandCentre.Radius < mapYLimit ? commandCentre.Position.Y + commandCentre.Radius : mapYLimit - 1;

            if (resourceCoordinate.X >= leftLimit &&
                resourceCoordinate.X <= rightLimit &&
                resourceCoordinate.Y >= upperLimit &&
                resourceCoordinate.Y <= lowerLimit)
            {
                occupied = true;
                break;
            }
        }
        return occupied;
    }







    //private (bool succes, int water, int mineral) DetermineSuccessCondition(SimulationContext simulationContext)
    //{
    //    var foundResources = simulationContext.Rover.ResourceCoordinates;
    //    string waterSymbol = "*";
    //    string mineralSymbol = "%";
    //    int water = CountResources(foundResources, waterSymbol);
    //    int minerals = CountResources(foundResources, mineralSymbol);
    //    bool succes = false;
    //    if (minerals >= 3 && water >= 3)
    //    {
    //        succes = true;
    //    }
    //    return (succes, water, minerals);
    //}

    //private int CountResources(Dictionary<Coordinate, string> listOfResources, string resource)
    //{
    //    int count = 0;
    //    foreach (var res in listOfResources)
    //    {
    //        if (res.Value == resource)
    //        {
    //            count++;
    //        }
    //    }
    //    return count;
    //}
}
