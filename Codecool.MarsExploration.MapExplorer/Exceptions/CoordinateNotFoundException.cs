namespace Codecool.MarsExploration.MapExplorer.Excepptions;

public class CoordinateNotFoundException : Exception
{
    public CoordinateNotFoundException(string? message) : base(message)
    {
    }
}
