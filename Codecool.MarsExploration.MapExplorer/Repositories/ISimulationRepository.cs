using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.MarsCommandCenter.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;

namespace Codecool.MarsExploration.MapExplorer.Repositories;

public interface ISimulationRepository
{
    //public void AddInformation(SimulationContext simulationContext);
    public void AddInformationToTableRovers(SimulationContext simulationContext);
    public void AddInformationToTableCommandCentres(SimulationContext simulationContext);
    public void AddInformationToTableConstructions(SimulationContext simulationContext);
}
