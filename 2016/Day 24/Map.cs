namespace AoC_2016_Day_24;
using BKH.BaseMap;
using BKH.Geometry;

public class Map(string[] puzzleInput) : BaseMap(puzzleInput)
{
    private const int WALL = 1;
    private const int AIR = 0;
    
    public readonly Dictionary<char, Point2D> WayPoints = [];

    protected override void LoadMap(string[] puzzleInput)
    {
        for (int y = 0; y < puzzleInput.Length; y++)
        {
            for (int x = 0; x < puzzleInput[y].Length; x++)
            {
                char c = puzzleInput[y][x];
                if (c == '#')
                {
                    _theMap.Add(new Point2D(x, y), WALL);
                }
                else
                {
                    _theMap.Add(new(x, y), AIR);
                    if (int.TryParse(c.ToString(), out int _))
                    {
                        WayPoints.Add(c, new Point2D(x, y));
                    }
                }
            }
        }
    }

    protected override int TestStep(Point2D cursor, Point2D nextPoint)
    {
        //Don't move to ourself. 
        if (cursor == nextPoint) return -1;

        //Don't leave the map.
        if (!_theMap.TryGetValue(nextPoint, out int _)) return -1;

        //we'll exceed our step count (if applicable) 
        if (MaxSteps != 0 && stepCounter[cursor].gScore + 1 > MaxSteps) return -1;

        if (_theMap[nextPoint] == WALL) return -1;

        stepCounter.TryAdd(nextPoint, (int.MaxValue, int.MaxValue, null));

        return 1; // number of steps.
    }

    protected override IEnumerable<Point2D> NextSteps(Point2D cursor)
    {
        return cursor.GetOrthogonalNeighbors();
    }
}
