namespace Codecool.MarsExploration.MapExplorer.Logger;

public class FileLogger : LoggerBase
{
    private readonly string _logFile;

    public FileLogger(string logFile)
    {
        _logFile = logFile;
    }

    protected override void LogInfo(string entry)
    {
        try
        {
            using var streamWriter = File.AppendText(_logFile);
            streamWriter.WriteLine(entry);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
