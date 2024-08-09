using BKH.Geometry;
using System.Text.RegularExpressions;

namespace AoC_2018_Day_17;

public static class MapNode
{
    public const char Clay = '#';
    public const char Sand = '.';
    public const char Water = '|';
    public const char Lake = '~';
}

public class Map
{
    //A note on coordinates, "down" is Y+1 in this map.  0,0 is ground level, 0,1 is one unit "down"
    private readonly Dictionary<Point2D, char> _theMap = [];
    private readonly Point2D _spigot = new(500, 0);

    private readonly Point2D DOWN = new(0, 1);
    private readonly Point2D LEFT = new(-1, 0);
    private readonly Point2D RIGHT = new(1, 0);

    private Point2D _mapMin;
    private Point2D _mapMax;

    public Map(string[] mapData)
    {
        foreach (string line in mapData)
        //EG: y=13, x=498..504
        // x or y can be in either order.
        {
            int[] matches = Regex.Matches(line, @"\d+").Select(x => int.Parse(x.Value)).ToArray();

            int startX = 0;
            int numX = 0;

            int startY = 0;
            int numY = 0;

            if (line[0] == 'x')
            {
                startX = matches[0];
                numX = 1;

                startY = matches[1];
                numY = matches[2] - matches[1] + 1;
            }
            else
            {
                startY = matches[0];
                numY = 1;

                startX = matches[1];
                numX = matches[2] - matches[1] + 1;
            }

            foreach ((int x, int y) in from a in Enumerable.Range(startX, numX)
                                       from b in Enumerable.Range(startY, numY)
                                       select (a, b))
            {
                _theMap.TryAdd(new Point2D(x, y), MapNode.Clay);
            }
        }

        _mapMin = new(
                _theMap.Keys.Select(x => x.X).Min(),
                _theMap.Keys.Select(x => x.Y).Min()
            );

        _mapMax = new(
                _theMap.Keys.Select(x => x.X).Max(),
                _theMap.Keys.Select(x => x.Y).Max()
            );
    }

    public int CountAllWater()
    {
        int minY = _theMap.Where(x => x.Value == MapNode.Clay).Select(x => x.Key.Y).Min();
        return _theMap.Count(x => (x.Value == MapNode.Water || x.Value == MapNode.Lake) && x.Key.Y >= minY);
    }

    public int CountStandingWater()
    {
        return _theMap.Values.Count(x => x == MapNode.Lake);
    }

    public void DripWater() => DripWater(null);
    
    public void DripWater(Point2D? spigot)
    {
        // return types for doLeftRight
        const int CONTINUE = -1;
        const int SPIGOT = 0;
        const int WALL = 1;

        List<Point2D> drips = [ spigot ?? _spigot ];

        // find the next basin, or the abyss.
        bool isFlowing = true;
        do
        {
            Point2D nextStep = drips.Last() + DOWN;

            if (nextStep.Y > _mapMax.Y)
            {
                // unroll and return.
                foreach (Point2D p in drips)
                {
                    if (p != _spigot && !_theMap.TryAdd(p, MapNode.Water)) _theMap[p] = MapNode.Water;
                }
                return;
            }
            
            isFlowing = !(_theMap.TryGetValue(nextStep, out char value) && (value == MapNode.Clay || value == MapNode.Lake));

            if (isFlowing) drips.Add(nextStep);
        } while (isFlowing);

        //unroll back up filling in the water as we go. 
        foreach (Point2D p in drips.Select(x => x).Reverse())
        {
            if (!_theMap.TryGetValue(p + DOWN, out char downValue)) throw new Exception($"{p} Nothing under this???");

            bool isRightDone = false;
            bool isLeftDone = false;

            Point2D goLeft = p;
            Point2D goRight = p;
            if (_theMap[p + DOWN] == MapNode.Water)
            {
                if (p != drips.First() && !_theMap.TryAdd(p, MapNode.Water)) _theMap[p] = MapNode.Water;
                continue;
            }

            bool isLake = false;
            int leftResult = CONTINUE;
            int rightResult = CONTINUE;
            bool isDone = false;
            do
            {
                int doLeftRight(ref Point2D dir, Point2D step)
                {
                    // -1 keep checking
                    // 0 spigot - flowing water
                    // 1 blocked - build a lake
                    int returnValue = CONTINUE;

                    Point2D nextStep = dir + step;
                    Point2D nextDown = nextStep + DOWN;

                    char sideTest = _theMap.TryGetValue(nextStep, out char sideValue) ? sideValue : MapNode.Sand;
                    char downTest = _theMap.TryGetValue(nextDown, out char downValue) ? downValue : MapNode.Sand;

                    if (downTest == MapNode.Sand) 
                    {
                        DripWater(nextStep);
                        downTest = _theMap[nextDown];
                    }

                    if (downTest == MapNode.Water)
                    {
                        dir += step;
                        return SPIGOT;
                    }

                    if (sideTest == MapNode.Clay) return WALL;

                    dir += step;
                    return returnValue;
                }

                if (!isLeftDone) leftResult = doLeftRight(ref goLeft, LEFT);
                if (!isRightDone) rightResult = doLeftRight(ref goRight, RIGHT);

                if (leftResult != CONTINUE) isLeftDone = true;
                if (rightResult != CONTINUE) isRightDone = true;

                if (isLeftDone && isRightDone)
                {
                    isDone = true;
                    if (leftResult == WALL && rightResult == WALL) isLake = true;
                }

            } while (!isDone);

            char layerSymbol = isLake ? MapNode.Lake : MapNode.Water;
            for (int x = goLeft.X; x <= goRight.X; x++)
            {
                if (!_theMap.TryAdd(new(x, goLeft.Y), layerSymbol)) _theMap[goRight] = layerSymbol;
            }
        }
    }

    public void RawMap()
    {
        //debug fuction to dump the raw map data. 
        foreach (KeyValuePair<Point2D, char> kvp in _theMap)
        {
            Console.WriteLine($"{kvp.Key} = {kvp.Value}");
        }
    }

    public void DrawMap()
    {
        _mapMin = new(
               _theMap.Keys.Select(x => x.X).Min(),
               _theMap.Keys.Select(x => x.Y).Min()
           );

        _mapMax = new(
                _theMap.Keys.Select(x => x.X).Max(),
                _theMap.Keys.Select(x => x.Y).Max()
            );

        Point2D offset = _mapMax - _mapMin;

        char[,] map = new char[offset.X + 1, offset.Y + 1];

        foreach (KeyValuePair<Point2D, char> kvp in _theMap)
        {
            int newX = offset.X - (_mapMax.X - kvp.Key.X);
            int newY = offset.Y - (_mapMax.Y - kvp.Key.Y);
            map[newX, newY] = kvp.Value;
        }

        for (int y = 0; y < offset.Y + 1; y++)
        {
            for (int x = 0; x < offset.X + 1; x++)
            {
                char output = (map[x, y] == '\0') ? MapNode.Sand : map[x, y];
                Console.ForegroundColor = output switch
                {
                    MapNode.Clay => ConsoleColor.Gray,
                    MapNode.Sand => ConsoleColor.Yellow,
                    MapNode.Water => ConsoleColor.Blue,
                    MapNode.Lake => ConsoleColor.DarkBlue,
                    _ => ConsoleColor.Red,
                };
                Console.Write(output);
            }
            Console.WriteLine();
        }
        Console.ResetColor();
        Console.WriteLine();
    }
}

