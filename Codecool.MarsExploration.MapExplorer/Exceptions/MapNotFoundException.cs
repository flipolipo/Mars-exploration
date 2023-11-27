namespace Codecool.MarsExploration.MapExplorer.Excepptions;

public class MapNotFoundException : Exception
{
    public MapNotFoundException(string? message) : base(message)
    {
    }
}
