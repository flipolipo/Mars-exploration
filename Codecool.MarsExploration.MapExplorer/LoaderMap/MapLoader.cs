using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.LoaderMap;

public class MapLoader : IMapLoader
{
    public Map Load(string mapFile)
    {
        try
        {
            string[] lines = File.ReadAllLines(mapFile);
            int width = lines[0].Length;
            int height = lines[1].Length;

            string?[,] representation = new string?[height, width];

            for (int x = 0; x < width; x++)
            {
                string line = lines[x];
                for (int y = 0; y < width; y++)
                {
                    representation[x, y] = line[y].ToString();
                }
            }

            return new Map(representation, true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load map from file: {ex.Message}");
            return new Map(null, false);
        }

    }
}
