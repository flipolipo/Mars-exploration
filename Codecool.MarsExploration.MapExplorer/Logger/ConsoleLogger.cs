namespace Codecool.MarsExploration.MapExplorer.Logger;

public class ConsoleLogger : LoggerBase
{
    protected override void LogInfo(string entry)
    {
        Console.WriteLine(entry);
    }
}
