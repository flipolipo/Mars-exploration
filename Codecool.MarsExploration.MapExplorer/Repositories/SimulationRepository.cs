//using Codecool.MarsExploration.MapExplorer.Exploration.Model;
//using Codecool.MarsExploration.MapExplorer.MarsCommandCenter.Model;
//using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
//using Microsoft.Data.Sqlite;

//namespace Codecool.MarsExploration.MapExplorer.Repositories;

//public class SimulationRepository : ISimulationRepository
//{
//    private readonly string _dbFilePath;

//    public SimulationRepository(string dbFilePath)
//    {
//        _dbFilePath = dbFilePath;
//    }


//    //public void AddInformation(SimulationContext simulationContext)
//    //{
//    //    string outcome = simulationContext.Outcome.ToString();
//    //    int foundResourceNumber = GetResourceAmount(simulationContext.Rover.ResourceCoordinates);
//    //    int numberOfSteps = simulationContext.NumberOfSteps;
//    //    int timeOutSteps = simulationContext.TimeoutSteps;
//    //    string landingSpot = GetLandingSpotStringRepresentation(simulationContext.SpaceshipLocation);
//    //    int date = GetUnixFormat();
//    //    string SimulationQuery = $"INSERT INTO simulation (outcome, unix_time, number_of_resources) VALUES('{outcome}','{date}','{foundResourceNumber}')";
//    //    string SimulationConfigQuery = $"INSERT INTO simulation_config (landing_spot_of_rocket, timeout_steps) VALUES('{landingSpot}','{timeOutSteps}')";
//    //    ExecuteNonQuery(SimulationConfigQuery, _dbFilePath);
//    //    ExecuteNonQuery(SimulationQuery, _dbFilePath);

//    //}

//    public void AddInformationToTableRovers(SimulationContext simulationContext)
//    {
//        foreach (var rover in simulationContext.Rovers)
//        {
//            string roverName = rover.RoverName;
//            int extractedWater = rover.ResourceCoordinates.Count(rc => rc.Value == "water");
//            int extractedMinerals = rover.ResourceCoordinates.Count(rc => rc.Value == "minerals");

//            string query = $"INSERT INTO rovers (rover_name, extracted_water, extracted_minerals) VALUES('{roverName}', '{extractedWater}', '{extractedMinerals}')";
//            ExecuteNonQuery(query, _dbFilePath);
//        }

//    }
//    public void AddInformationToTableCommandCentres(SimulationContext simulationContext)
//    {
//        foreach (var commandCenter in simulationContext.CommandCentres)
//        {
//            string commandCentreName = $"CommandCentre-{commandCenter.Id}";
//            int resourceDeliveredWater = commandCenter.WaterAmount;
//            int resourceDeliveredMinerals = commandCenter.MineralAmount;
//            int resourceInStockWater = commandCenter.AvaibleResourcesInRadius.Count(rc => rc.Value == "water");
//            int resourceInStockMinerals = commandCenter.AvaibleResourcesInRadius.Count(rc => rc.Value == "minerals");

//            string query = $@"
//        INSERT INTO command_centers 
//        (command_center_name, resource_delivered_water, resource_delivered_minerals, resource_in_stock_water, resource_in_stock_minerals) 
//        VALUES ('{commandCentreName}', {resourceDeliveredWater}, {resourceDeliveredMinerals}, {resourceInStockWater}, {resourceInStockMinerals})";

//            ExecuteNonQuery(query, _dbFilePath);
//        }
//    }

//    public void AddInformationToTableConstructions(SimulationContext simulationContext)
//    {
//        List<(string constructionName, int resourceUsed, int stepSimulation)> constructions = new List<(string, int, int)>();

//        foreach (var commandCenter in simulationContext.CommandCentres)
//        {
//            if (commandCenter.CurrentState == CommandCenter.Enums.State.Constructing)
//            {
//                constructions.Add(($"CommandCentre-{commandCenter.Id}", commandCenter.UsedWater + commandCenter.UsedMinerals, simulationContext.NumberOfSteps));
//            }
//        }
//        foreach (var rover in simulationContext.Rovers)
//        {
//            if (rover.State == RoverState.Build)
//            {
//                constructions.Add(($"BuildRover-{rover.Id}", rover.ExtractedWater + rover.ExtractedMinerals, simulationContext.NumberOfSteps));
//            }
//        }
//        foreach (var construction in constructions)
//        {
//            string query = $@"
//            INSERT INTO constructions 
//            (construction_name, resource_used, command_centre_id, rover_id, step_of_simulation) 
//            VALUES ('{construction.constructionName}', {construction.resourceUsed}, {construction.stepSimulation})";

//            ExecuteNonQuery(query, _dbFilePath);
//        }
//    }
//    private void ExecuteNonQuery(string query, string path)
//    {
//        using var connection = GetphisicalDbConnection(path);
//        using var command = GetCommand(query, connection);
//        command.ExecuteNonQuery();
//    }

//    private static SqliteCommand GetCommand(string query, SqliteConnection connection)
//    {
//        return new SqliteCommand
//        {
//            CommandText = query,
//            Connection = connection,
//        };
//    }

//    private SqliteConnection GetphisicalDbConnection(string path)
//    {
//        var dbConnection = new SqliteConnection($"Data source={path};Mode=ReadWrite");
//        dbConnection.Open();
//        return dbConnection;
//    }
//}
