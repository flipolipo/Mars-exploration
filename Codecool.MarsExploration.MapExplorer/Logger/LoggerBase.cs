using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using System.Security.AccessControl;

namespace Codecool.MarsExploration.MapExplorer.Logger;

public abstract class LoggerBase
{
    public void LogOutcome(int step, ExplorationOutcome? outcome)
    {
        var entry = $"STEP: {step}; EVENT: outcome; OUTCOME: {outcome}";
        LogInfo(entry);
    }

    public void LogReserchPosition(int step, Rover rover)
    {
        var entry = $"STEP: {step}; EVENT: reserch; UNIT {rover.Name}; POSITION: [{rover.CurrentPosition.X},{rover.CurrentPosition.Y}]";
        LogInfo(entry);
    }

    public void LogExtraction(int step, Rover rover, string resourceType, int requirdeTurns)
    {
        var entry = $"STEP: {step}; EVENT: extraction; UNIT: {rover.Name}; RESOURCE: {resourceType}; PROGRESS: {rover.TaskProgress} of {requirdeTurns}";
        LogInfo(entry);
    }

    public void LogDelivery(int step, Rover rover, string resourceType)
    {
        var entry = $"STEP: {step}; EVENT: delivery; UNIT: {rover.Name}; RESOURCE: {resourceType}; POSITION: [{rover.CurrentPosition.X},{rover.CurrentPosition.Y}]";
        LogInfo(entry);
    }

    public void LogMoveToResource(int step, Rover rover, string resourceType)
    {
        var entry = $"STEP: {step}; EVENT: move to resource; UNIT: {rover.Name}; RESOURCE: {resourceType}; POSITION: [{rover.CurrentPosition.X},{rover.CurrentPosition.Y}]";
        LogInfo(entry);
    }

    public void LogMoveToBuild(int step, Rover rover)
    {
        var entry = $"STEP: {step}; EVENT: move to build; UNIT: {rover.Name}; POSITION: [{rover.CurrentPosition.X},{rover.CurrentPosition.Y}]";
        LogInfo(entry);
    }

    public void LogCommandCentreBuilding(int step, Rover rover, uint commandCentreId, int requirdeTurns)
    {
        var entry = $"STEP: {step}; EVENT: build commandCentre{commandCentreId}; UNIT: {rover.Name}; PROGRESS: {rover.TaskProgress} of {requirdeTurns}";
        LogInfo(entry);
    }

    public void LogReturnPosition(Rover rover)
    {
        var entry = $"EVENT: return; UNIT: {rover.Id}; POSITION: [{rover.CurrentPosition.X},{rover.CurrentPosition.Y}]";
        LogInfo(entry);
    }

    public void LogConstruction(int step, string roverName, int progress, int requirdeTurns, string type)
    {
        var entry = $"STEP: {step}; EVENT: {type} rover construction; UNIT: {roverName}; PROGRESS: {progress} of {requirdeTurns}";
        LogInfo(entry);
    }
    protected abstract void LogInfo(string entry);
}
