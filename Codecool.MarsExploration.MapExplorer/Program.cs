using Codecool.MarsExploration.MapExplorer.Configuration.Service;
using Codecool.MarsExploration.MapExplorer.Configuration.Validation;

using Codecool.MarsExploration.MapExplorer.Exploration.Service;
using Codecool.MarsExploration.MapExplorer.LoaderMap;
using Codecool.MarsExploration.MapExplorer.Logger;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service.MovementAlgorithm;
using Codecool.MarsExploration.MapExplorer.Repositories;
using Codecool.MarsExploration.MapGenerator.Calculators.Service;

namespace Codecool.MarsExploration.MapExplorer;

class Program
{
    private static readonly string WorkDir = AppDomain.CurrentDomain.BaseDirectory;

    public static void Main(string[] args)
    {
        var mapFile = $@"{WorkDir}\Resources\exploration-2.map";
        var dbSimulationFile = $"{WorkDir}\\Resources\\Db\\Simulation.db";

        var resourceSymbolsToMonitor = new string[] { "%", "*", " " };

        LoggerBase logger = new ConsoleLogger();
        //ISimulationRepository simulationRepository = new SimulationRepository(dbSimulationFile);
        IMapLoader mapLoader = new MapLoader();
        ICoordinateCalculator coordinateCalculator = new CoordinateCalculator();
        IConfigurationValidator configurationValidator = new ConfigurationValidator(coordinateCalculator);
        IConfigurationGenerator configuratorGenerator = new ConfigurationGenerator(mapLoader, configurationValidator);
        var startingConfiguration = configuratorGenerator.GetStartingConfiguration(mapFile, resourceSymbolsToMonitor, 1000);
        IRoverDeployer roverDeployer = new RoverDeployer(coordinateCalculator);

        ISightScanner sightScanner = new SightScanner();
        IStepAnalyzer stepAnalyzer = new StepAnalyzer();
        IStepManager stepManager = new StepManager(sightScanner, stepAnalyzer, logger);

        IOutcomeAnalyzer outcomeAnalyzer = new OutcomeAnalyzer(coordinateCalculator, 10);

        var explorator = new MapExplorator(startingConfiguration, roverDeployer, stepManager, outcomeAnalyzer, logger);
        explorator.ExploreMap();
    }
}
