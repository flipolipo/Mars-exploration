using Codecool.MarsExploration.MapExplorer.Configuration.Model;
using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.Logger;
using Codecool.MarsExploration.MapExplorer.MarsCommandCenter.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service;
using Codecool.MarsExploration.MapExplorer.Repositories;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service;

public class MapExplorator
{
    private readonly startingConfiguration _startingConfiguration;
    private readonly LoggerBase _logger;
    private readonly IRoverDeployer _roverDeployer;
    private readonly IStepManager _stepManager;
    private readonly IOutcomeAnalyzer _outcomeAnalyzer;
    //private readonly ISimulationRepository _simulationRepository;

    public MapExplorator(startingConfiguration startingConfiguration, IRoverDeployer roverDeployer, IStepManager stepManager, IOutcomeAnalyzer outcomeAnalyzer, LoggerBase logger)
    {
        _startingConfiguration = startingConfiguration;
        _roverDeployer = roverDeployer;
        _stepManager = stepManager;
        _outcomeAnalyzer = outcomeAnalyzer;
        _logger = logger;
    }

    public void ExploreMap()
    {
        var map = _startingConfiguration.Map;
        var landingSpotOfRocket = _startingConfiguration.LandingSpotOfRocket;
        var resourceSymbolsToMonitor = _startingConfiguration.ResourceSymbols.ToArray();
        var stepsToTimeOut = _startingConfiguration.StepsToTimeOut;
        var firstRover = _roverDeployer.Deploy(1, "rover-1", 2, map, landingSpotOfRocket, RoverState.Reserch, 40);
        var rovers = new HashSet<Rover>() { firstRover };
        var commandCentres = new HashSet<CommandCentre>();
        var context = new SimulationContext(0, stepsToTimeOut, rovers, commandCentres, landingSpotOfRocket, map, resourceSymbolsToMonitor, null);
        StartExploring(context);
    }

    private void StartExploring(SimulationContext context)
    {
        var spaceShipLocation = context.SpaceshipLocation;
        var map = context.Map;
        var resourceSymbolsToMonitor = context.ResourceSymbolsToMonitor;
        var firstRover = context.Rovers.ElementAt(0);

        _stepManager.UpdateRoverResources(firstRover, map, resourceSymbolsToMonitor);
        _outcomeAnalyzer.Analize(context);

        while (context.Outcome != ExplorationOutcome.Timeout && context.Outcome != ExplorationOutcome.Error)
        {
            context.NumberOfSteps++;
            var commandCentres = context.CommandCentres;
            var rovers = context.Rovers;
            foreach (var commandCentre in commandCentres)
            {
                commandCentre.CheckForUpdate(context, _logger, 8, 5);
            }
            foreach (var rover in rovers)
            {
                var commandCentreIndex = (int)((rover.Id - 1) / 2);
                var IdIsEven = false;
                var resourceType = "mineral";
                if (rover.Id % 2 == 0)
                {
                    IdIsEven = true;
                    resourceType = "water";
                }

                if (rover.State == RoverState.Reserch && !IdIsEven)
                {
                    _stepManager.ResearchMove(rover, map, context.NumberOfSteps);
                }
                else if (rover.State == RoverState.Build)
                {
                    _stepManager.BuildCommandCentre(5, rover, 6, context);
                }
                else if (rover.State == RoverState.Extraction)
                {
                    _stepManager.ExtractMineral(rover, 3, context.NumberOfSteps, resourceType);
                }
                else if (rover.State == RoverState.MoveToResource)
                {
                    var commandCentre = context.CommandCentres.ElementAt(commandCentreIndex);
                    var resourceCoordinate = commandCentre.GetNearestResourceCoordinate(resourceType);
                    _stepManager.MoveToResource(rover, resourceCoordinate, context.NumberOfSteps, resourceType);
                }
                else if (rover.State == RoverState.ResourceDelivery)
                {
                    var commandCentre = context.CommandCentres.ElementAt(commandCentreIndex);
                    _stepManager.DeliverResource(rover, commandCentre, resourceType, context.NumberOfSteps);
                }
                _stepManager.UpdateRoverResources(firstRover, map, resourceSymbolsToMonitor);
            }
            _outcomeAnalyzer.Analize(context);


            //dodac foreach z commandcentres i sprawdzac czy da sie stworzyc rowera do wody a potem do reserchu
        }

    }


    //_logger.LogOutcome(context.NumberOfSteps, context.Outcome);
    //_simulationRepository.AddInformation(context); ===> Dodać nowe tabelki!
    //_simulationRepository.AddInformationToTableRovers(context);
    //_simulationRepository.AddInformationToTableCommandCentres(context);
    //_simulationRepository.AddInformationToTableConstructions(context);







    //(int water, int mineral) analyzerResult = (0, 0);

    //PlaceSpaceShipOnMap(map, spaceShipLocation);
    //var previousMove = (rover.CurrentPosition, map.Representation[rover.CurrentPosition.X, rover.CurrentPosition.Y]);
    //PlaceRoverOnMap(map, rover.CurrentPosition, null);
    //Console.Clear();
    //Console.WriteLine(context.Map);
    //Thread.Sleep(8000);

    //while (rover.State != RoverState.InSpaceShip)
    //{
    //    Console.Clear();
    //    _stepManager.UpdateRoverResources(rover, map, resourceSymbolsToMonitor);
    //    if (rover.State == RoverState.Reserch)
    //    {
    //        analyzerResult = _outcomeAnalyzer.Analize(context);
    //        if (context.Outcome == null)
    //        {
    //            _stepManager.ResearchMove(rover, map);
    //            context.NumberOfSteps++;
    //            _logger.LogReserchPosition(context.NumberOfSteps, rover);
    //        }
    //        else
    //        {
    //            rover.State = RoverState.Returning;
    //            continue;
    //        }
    //    }
    //    else if (rover.State == RoverState.Returning)
    //    {

    //    }
    //    var actualMove = (rover.CurrentPosition, map.Representation[rover.CurrentPosition.X, rover.CurrentPosition.Y]);
    //    PlaceRoverOnMap(map, rover.CurrentPosition, previousMove);
    //    previousMove = (actualMove.CurrentPosition, actualMove.Item2);
    //    Console.WriteLine(context.Map);
    //    Console.WriteLine($"Water: {analyzerResult.water}/3; Mineral: {analyzerResult.mineral}/3");
    //    Thread.Sleep(600);
    //}
    //_outcomeAnalyzer.Analize(context);
    //_logger.LogOutcome(context.NumberOfSteps, context.Outcome);
    //_simulationRepository.AddInformation(context);
}

//private void PlaceRoverOnMap(Map map, Coordinate currentPosition, (Coordinate previousPosition, string previousSymbol)? previousMove)
//{
//    if (previousMove != null) map.Representation[previousMove.Value.previousPosition.X, previousMove.Value.previousPosition.Y] = previousMove.Value.previousSymbol;
//    map.Representation[currentPosition.X, currentPosition.Y] = "+";
//}

//private void PlaceSpaceShipOnMap(Map map, Coordinate spaceShipLocation)
//{
//    map.Representation[spaceShipLocation.X, spaceShipLocation.Y] = "@";
//}
//}