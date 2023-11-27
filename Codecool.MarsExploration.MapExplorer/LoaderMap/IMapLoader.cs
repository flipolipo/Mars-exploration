using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.LoaderMap;

public interface IMapLoader
{
    Map Load(string mapFile);
}
