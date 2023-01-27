using System.Drawing;

namespace AoC_2022_Day_14
{
    public static class MapNode
    {
        public const char Rock = '#';
        public const char Sand = 'o';
        public const char Air = ' ';
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


        //is there a floor, or do we go into the abyss? 
        private bool _floor;
        //set this at new(), otherwise the floor will move as we pile sand on it and the map gets bigger.
        private int _floorHeight = 0;

        public Map(string[] mapData, bool floorState = false)
        {
            foreach (string line in mapData)
            //EG: 481,54-> 481,47-> 481,54-> 483,54-> 483,45-> 483,54-> 485,54-> 485,52-> 485,54
            // CAN BE SINGLE POINTS.
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
                        foreach (int x in Enumerable.Range(startX, numX))
                        {
                            foreach (int y in Enumerable.Range(startY, numY))
                            {
                                _theMap.TryAdd(new Point(x, y), MapNode.Rock);
                            }
                        }
                    }
                }
                else
                {
                    //line is a single point
                    var temp = line.Split(",").Select(int.Parse).ToArray();
                    _theMap.TryAdd(new Point(temp[0], temp[1]), MapNode.Rock);
                }

                _floor = floorState;

                //floorHeight is defined as two plus the highest y coordinate
                //If we ever added rocks after new() we'd have to recalc this.
                _floorHeight = GetMapCorners().max.Y + 2;
            }
        }

        public void AddFloor() => _floor = true;
        public void RemoveFloor() => _floor = false;
        public bool IsFloor()
        {
            return _floor;
        }

        public int CountSand()
        {
            return _theMap.Values.Where(x => x == MapNode.Sand).Count();
        }

        public void SweepUp()
        {
            //take all the sand off the map. 
            //no RemoveAll :( 
            List<Point> sandNodes = _theMap.Where(kvp => kvp.Value == MapNode.Sand)
                         .Select(kvp => kvp.Key)
                         .ToList();

            foreach (Point p in sandNodes)
            {
                _theMap.Remove(p);
            }
        }

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

                    if (IsFloor() && nextStep.Y >= _floorHeight)
                    {
                        isFalling = false;
                        break;
                    }
                    if (!IsFloor() && nextStep.Y > _floorHeight - 2)
                    {
                        isFalling = true;
                        intoAbyss = true;
                        break;
                    }

                    if (!_theMap.ContainsKey(nextStep))
                    {
                        sand = nextStep;
                        isFalling = true;
                        break;
                    }
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

        public (Point min, Point max) GetMapCorners()
        {
            //This is an expensive function, avoid it. 
            Point min = new(
                    _theMap.Keys.Select(x => x.X).Min(),
                    _theMap.Keys.Select(x => x.Y).Min()
                );


            Point max = new(
                    _theMap.Keys.Select(x => x.X).Max(),
                    _theMap.Keys.Select(x => x.Y).Max()
                );
            return (min, max);
        }


        public void PrintMap()
        {
            (Point min, Point max) = GetMapCorners();
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
                    char output = (map[x, y] == '\0') ? MapNode.Air : map[x, y];
                    Console.ForegroundColor = output switch
                    {
                        MapNode.Rock => ConsoleColor.Blue,
                        MapNode.Sand => ConsoleColor.Yellow,
                        MapNode.Air => ConsoleColor.Gray,
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

