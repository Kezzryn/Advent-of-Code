using System.Drawing;

namespace AoC_2022_Day_24
{
    internal enum MapSymbols
    {
        Wall = '#',
        Air = '.',
        StormWest = '<',
        StormEast = '>',
        StormNorth = '^',
        StormSouth = 'v'
    };

    internal class Map
    {
        // this serves as our reference starting position for everything. 
        private readonly Dictionary<Point, MapSymbols> _theMap = new();

        //as found on the map.
        private readonly Point _defaultStart = new(0, 0);
        private readonly Point _defaultEnd = new(0, 0);

        public Point MapMin { get; }
        public Point MapMax { get; }
        private readonly Point _stormMax;

        //the path after an A_Star call. 
        public List<Point3D> FinalPath = new();
        public Map(string[] puzzleInput)
        {
            int x = 0;
            int y = 0;

            foreach (string line in puzzleInput.Reverse())
            {
                x = 0;
                foreach (MapSymbols c in line.Select(v => (MapSymbols)v))
                {
                    _theMap.Add(new Point(x, y), c);
                    if (y == 0 && c == MapSymbols.Air) _defaultEnd = new Point(x, y);
                    if (y == puzzleInput.GetUpperBound(0) && c == MapSymbols.Air) _defaultStart = new Point(x, y);
                    x++;
                }
                y++;
            }
            MapMin = new Point(_theMap.Keys.Select(k => k.X).Min(), _theMap.Keys.Select(k => k.Y).Min());
            MapMax = new Point(_theMap.Keys.Select(k => k.X).Max(), _theMap.Keys.Select(k => k.Y).Max());
            _stormMax = MapMax + new Size(-1, -1);
        }
        public static int TaxiDistance(Point s, Point e)
        {
            return Math.Abs(s.X - e.X) + Math.Abs(s.Y - e.Y);
        }

        public Point DefaultStart => _defaultStart;
        public Point DefaultEnd => _defaultEnd;

        private static int FindStormOffset(int testNum, int maxWidthOrHeight, int time, bool isReverse = false)
        {
            int offset = isReverse
                ? testNum - (maxWidthOrHeight - (time % maxWidthOrHeight))   // Reversed (South/West)
                : testNum - (time % maxWidthOrHeight);              // Normal (North/East)

            return (offset <= 0) ? maxWidthOrHeight + offset : offset;
        }

        private bool TestStep(Point3D nextPoint)
        {
            // Off the map
            if (!_theMap.TryGetValue(nextPoint.Get2DPoint(), out MapSymbols value)) return false;

            // Thunk
            if (value == MapSymbols.Wall) return false;

            foreach (MapSymbols d in Enum.GetValues(typeof(MapSymbols)))
            {
                //These are the potential locations of storms that will intersect our position at t. 
                int stormX, stormY; 
                switch (d)
                {
                    case MapSymbols.StormNorth or MapSymbols.StormSouth:
                        stormX = nextPoint.X;
                        stormY = FindStormOffset(nextPoint.Y, _stormMax.Y, nextPoint.Z, d == MapSymbols.StormSouth);
                        break;
                    case MapSymbols.StormEast or MapSymbols.StormWest:
                        stormX = FindStormOffset(nextPoint.X, _stormMax.X, nextPoint.Z, d == MapSymbols.StormWest);
                        stormY = nextPoint.Y;
                        break;
                    default:
                        // skip the non N/S/E/W symbols. 
                        continue; 
                };

                if (_theMap[new(stormX, stormY)] == d) return false;
            }

            return true;
        }

        public bool A_Star(Point3D start, Point3D dest, out int numSteps)
        {
            FinalPath.Clear();

            List<Point3D> neighbors = new()
            {
                new Point3D(1,0,1), // right
                new Point3D(-1,0,1),// left
                new Point3D(0,1,1), // up
                new Point3D(0,-1,1),// down
                new Point3D(0,0,1)  // stay
            };

            PriorityQueue<Point3D, int> searchQueue = new(); //we enque based on fScore + h, the distance travelled, plus taxi distance guess to destination.
            HashSet<Point3D> inSearchQueue = new(); //we add this because we don't have a way to query the queue to see if a specific item is in it.

            //gScore is value of the path from start to here
            //fScore is estimated from here to end. We're using Taxi Distance as our hieruistic 
            Dictionary<Point3D, (int gScore, int fScore, Point3D? parent)> stepCounter = new()
            {
                { start, (0, TaxiDistance(start.Get2DPoint(), dest.Get2DPoint()), null) }
            };

            searchQueue.Enqueue(start, TaxiDistance(start.Get2DPoint(), dest.Get2DPoint()));
            inSearchQueue.Add(start);

            while (searchQueue.Count > 0)
            {
                Point3D cursor = searchQueue.Dequeue();
                inSearchQueue.Remove(cursor);

                //We have arrived!
                if (cursor.Get2DPoint() == dest.Get2DPoint())
                {
                    numSteps = stepCounter[cursor].gScore;

                    //unroll our history. 
                    FinalPath.Add(cursor);
                    Point3D? p = stepCounter[cursor].parent;

                    while (p != null)
                    {
                        FinalPath.Add((Point3D)p);
                        p = stepCounter[(Point3D)p].parent;
                    }

                    return true;
                }

                foreach (Point3D neighbor in neighbors)
                {
                    Point3D nextStep = cursor + neighbor;

                    //bounds and valid move check. 
                    if (TestStep(nextStep))
                    {
                        if (!stepCounter.ContainsKey(nextStep)) stepCounter.Add(nextStep, (int.MaxValue, int.MaxValue, null));

                        //tentative_gScore := gScore[current] + d(current, neighbor)
                        int t_gScore = stepCounter[cursor].gScore + 1;

                        //if tentative_gScore < gScore[neighbor]
                        if (t_gScore < stepCounter[nextStep].gScore)
                        {
                            //cameFrom[neighbor] := current
                            //gScore[neighbor] := tentative_gScore
                            //fScore[neighbor] := tentative_gScore + h(neighbor)
                            stepCounter[nextStep] = (t_gScore, t_gScore + TaxiDistance(nextStep.Get2DPoint(), dest.Get2DPoint()), cursor);

                            //if neighbor not in openSet openSet.add(neighbor) 
                            if (!inSearchQueue.Contains(nextStep))
                            {
                                searchQueue.Enqueue(nextStep, stepCounter[nextStep].fScore);
                                inSearchQueue.Add(nextStep);
                            }
                        }
                    }
                }
            }
            numSteps = -1;
            return false;
        }

        public void DisplayMap() => DisplayMap(FinalPath);
        public void DisplayMap(List<Point3D> drawPath)
        {
            Point3D cursor = new();

            for (int y = MapMax.Y; y >= MapMin.Y; y--)
            {
                for (int x = MapMin.X; x <= MapMax.X; x++)
                {
                    cursor.X = x;
                    cursor.Y = y;

                    if (_theMap.TryGetValue(cursor.Get2DPoint(), out MapSymbols value))
                    {
                        if (drawPath.Contains(cursor)) Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        if (cursor.Get2DPoint() == _defaultStart) Console.ForegroundColor = ConsoleColor.Green;
                        if (cursor.Get2DPoint() == _defaultEnd) Console.ForegroundColor = ConsoleColor.Blue;

                        Console.Write((char)(value + 96));

                        if (Console.ForegroundColor != ConsoleColor.Gray) Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("?");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                Console.WriteLine(" ");
            }
        }
    }
}
