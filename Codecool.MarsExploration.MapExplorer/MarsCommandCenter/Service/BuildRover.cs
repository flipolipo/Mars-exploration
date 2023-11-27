using Codecool.MarsExploration.MapExplorer.Exceptions;
using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.MarsCommandCenter.Enums;
using Codecool.MarsExploration.MapExplorer.MarsCommandCenter.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service;

namespace Codecool.MarsExploration.MapExplorer.MarsCommandCenter.Service;

public class BuildRover
{
    //private readonly CommandCentre _commandCenter;
    //private readonly IRoverDeployer _roverDeployer;
    //private readonly int AmountNeededToBuild;
    //private readonly SimulationContext _simulationContext;

    //public BuildRover(CommandCentre commandCenter, IRoverDeployer roverDeployer, int amountNeededToBuild, SimulationContext simulationContext)
    //{
    //    _commandCenter = commandCenter;
    //    _roverDeployer = roverDeployer;
    //    AmountNeededToBuild = amountNeededToBuild;
    //    _simulationContext = simulationContext;
    //}

    //public void Build()
    //{
    //    if (_commandCenter != null)
    //    {
    //        _commandCenter.CurrentState = State.ConstructingRover;
    //        // W tym miejscu event log na temat zmiany stanu
    //        for (var i = 0; i < AmountNeededToBuild; i++)
    //        {
    //            _commandCenter.MineralAmount = _commandCenter.MineralAmount--;
    //            _commandCenter.WaterAmount = _commandCenter.WaterAmount--;
    //            //Tutaj log ktory bedzie mial na koniec cos takiego STEP {i} of {AmountNeededToBuild}
    //        }
    //        _roverDeployer.Deploy(_commandCenter.Id, _commandCenter.Map, _commandCenter.Position, $"rover-{_commandCenter.Id}", 2, RoverState.Reserch, 0, null);
    //        // W tym miejscu log event o tym że powstał nowy rover
    //        _commandCenter.CurrentState = State.Active;
    //    }
    //    else throw new CommandCenterNotFoundException("Invalid CommandCenter!");
    //}
}
