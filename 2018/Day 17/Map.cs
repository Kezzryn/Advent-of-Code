using System.Drawing;

namespace AoC_2018_Day_17
{
    public static class MapNode
    {
        public const char Clay  = '#';
        public const char Sand  = '.';
        public const char Water = '|';
        public const char Lake  = '~';
    }

    public class Map
    {
        //A note on coordinates, "down" is Y+1 in this map.  0,0 is ground level, 0,1 is one unit "down"

        private readonly Dictionary<Point, char> _theMap = new();
        private Point spigot = new(500, 0);

        private readonly List<Size> _directions = new()
        {
            new Size(0, 1), //down
            new Size(-1, 1), //down-left
            new Size(1, 1) //down-right
        };
    


        public Map(string[] mapData)
        {
            foreach (string line in mapData)
            //EG: y=13, x=498..504
            // x or y can be in either order.
            {
                if (line.Contains("->"))
                {
                    Point[] points = line
                        .Split("->")
                        .Select(s => s.Split(','))
                        .Select(a =>
                        new Point(x: int.Parse(a[0]), y: int.Parse(a[1]))).ToArray();

                    var adjacents = points
                            .Zip(points.Skip(1), (a, b) => new { a, b })
                            .ToArray();

                    foreach (var pair in adjacents)
                    {
                        int startX = int.Min(pair.a.X, pair.b.X);
                        int numX = Math.Abs(pair.a.X - pair.b.X) + 1;

                        int startY = int.Min(pair.a.Y, pair.b.Y);
                        int numY = Math.Abs(pair.a.Y - pair.b.Y) + 1;
                        foreach ((int x, int y) in from a in Enumerable.Range(startX, numX)
                                                   from b in Enumerable.Range(startY, numY)
                                                   select (a,b))
                        {
                            _theMap.TryAdd(new Point(x, y), MapNode.Clay);
                        }
                    }
                }
                else
                {
                    //line is a single point
                    var temp = line.Split(",").Select(int.Parse).ToArray();
                    _theMap.TryAdd(new Point(temp[0], temp[1]), MapNode.Clay);
                }

            }
        }

        public int CountWater()
        {
            return _theMap.Values.Where(x => x == MapNode.Water || x == MapNode.Lake).Count();
        }

        //public void SweepUp()
        //{
        //    //take all the sand off the map. 
        //    //no RemoveAll :( 
        //    List<Point> sandNodes = _theMap.Where(kvp => kvp.Value == MapNode.Sand)
        //                 .Select(kvp => kvp.Key)
        //                 .ToList();

        //    foreach (Point p in sandNodes)
        //    {
        //        _theMap.Remove(p);
        //    }
        //}

        public void DropSand(int numGrains)
        {
            for (int i = 1; i <= numGrains; i++)
            {
                if (!DropSand()) break;
            }
        }

        public bool DropSand()
        {
            //return false if the map is full/ended. 

            Point sand = spigot;
            bool intoAbyss = false;
            bool isFalling = false;

            do
            {
                foreach (Size d in _directions)
                {
                    isFalling = false;
                    Point nextStep = sand + d;

                    //if (IsFloor() && nextStep.Y >= _floorHeight)
                    //{
                    //    isFalling = false;
                    //    break;
                    //}
                    //if (!IsFloor() && nextStep.Y > _floorHeight - 2)
                    //{
                    //    isFalling = true;
                    //    intoAbyss = true;
                    //    break;
                    //}

                    //if (!_theMap.ContainsKey(nextStep))
                    //{
                    //    sand = nextStep;
                    //    isFalling = true;
                    //    break;
                    //}
                }
            } while (isFalling && !intoAbyss);

            if (!intoAbyss) _theMap.Add(sand, MapNode.Sand);

            return !(_theMap.ContainsKey(spigot) || intoAbyss);
        }

        public void RawMap()
        {
            //debug fuction to dump the raw map data. 
            foreach (KeyValuePair<Point, char> kvp in _theMap)
            {
                Console.WriteLine($"{kvp.Key} = {kvp.Value}");
            }
        }


        public void PrintMap()
        {
            Point offset = max - (Size)min;

            char[,] map = new char[offset.X + 1, offset.Y + 1];

            foreach (KeyValuePair<Point, char> kvp in _theMap)
            {
                int newX = offset.X - (max.X - kvp.Key.X);
                int newY = offset.Y - (max.Y - kvp.Key.Y);
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
}

