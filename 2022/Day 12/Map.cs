using System.Drawing;

namespace AoC_2022_Day_12
{
    internal class Map
    {
        //an X,Y and the int represents the height of that location.
        private readonly Dictionary<Point, int> _theMap = new();

        //as found on the map.
        private readonly Point _defaultStart = new(0, 0);
        private readonly Point _defaultEnd = new(0, 0);

        public Point MapMin { get; }
        public Point MapMax { get; }

        //the path after an A_Star call. 
        public List<Point> finalPath = new();

        public Map(string puzzleFilename)
        {
            string[] puzzleInput = File.ReadAllLines(puzzleFilename);

            puzzleInput = puzzleInput.Reverse().ToArray(); //flip it so when we scan it in, we put the 0,0 in the lower left, not the upper left. 

            int x = 0;
            int y = 0;

            foreach (string line in puzzleInput)
            {
                x = 0;
                foreach (char c in line)
                {
                    switch (c)
                    {
                        //UNICODE / ASCII charts have char 'A' = 65 and 'a' = 97, which we can translate into a position in the alphabet with a modulo 32 operation.
                        //eg: 'A' % 32 = 1  (65/32 = 2 R 1 ) 
                        //input data uses letters to represent height from a=1 -> z=26 
                        case 'S':
                            _theMap.Add(new Point(x, y), 'a' % 32);
                            _defaultStart = new Point(x, y);
                            break;
                        case 'E':
                            _theMap.Add(new Point(x, y), 'z' % 32);
                            _defaultEnd = new Point(x, y);
                            break;
                        default:
                            _theMap.Add(new Point(x, y), c % 32);
                            break;
                    }
                    x++;
                }
                y++;
            }

            MapMin = new Point(_theMap.Keys.Select(k => k.X).Min(), _theMap.Keys.Select(k => k.Y).Min());
            MapMax = new Point(_theMap.Keys.Select(k => k.X).Max(), _theMap.Keys.Select(k => k.Y).Max());
        }

        public Point DefaultStart()
        {
            return _defaultStart;
        }

        public Point DefaultEnd()
        {
            return _defaultEnd;
        }

        public int HeightAt(Point point)
        {
            //heights should be 1 to 26 
            return (_theMap.TryGetValue(point, out int value)) ? value : -1;
        }
        public static int TaxiDistance(Point a, Point b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

        public bool A_Star(Point start, Point dest, out int numSteps, int? maxSteps = null)
        {
            finalPath.Clear();

            List<Point> neighbors = new()
            {
                new Point(1,0),
                new Point(-1,0),
                new Point(0,1),
                new Point(0,-1)
            };

            PriorityQueue<Point, int> searchQueue = new(); //we enque based on fScore + h, the distance travelled, plus taxi distance guess to destination.
            HashSet<Point> inSearchQueue = new(); //we add this because we don't have a away to query the queue to see if a specific item is in it.

            //gScore is value of the path from start to here
            //fScore is estimated from here to end. We're using Taxi Distance as our hieruistic 
            Dictionary<Point, (int gScore, int fScore, Point? parent)> stepCounter = new()
            {
                { start, (0, TaxiDistance(start, dest), null) }
            };

            bool TestStep(Point cursor, Point nextPoint)
            {
                //Don't move to ourself. 
                if (cursor == nextPoint) return false;

                //we're off the map
                if (!_theMap.ContainsKey(nextPoint)) return false;

                //we'll exceed our step count (if applicable) 
                if (maxSteps != null && stepCounter[cursor].gScore + 1 > maxSteps) return false;

                //height check can go down any number, but only up one level. 
                if (_theMap[nextPoint] <= _theMap[cursor] || _theMap[nextPoint] == _theMap[cursor] + 1)
                {
                    if (!stepCounter.ContainsKey(nextPoint))
                    {
                        stepCounter.Add(nextPoint, (int.MaxValue, int.MaxValue, null));
                    }
                    return true;
                }

                return false; //default to nope. 
            }

            searchQueue.Enqueue(start, TaxiDistance(start, dest));
            inSearchQueue.Add(start);

            while (searchQueue.Count > 0)
            {
                Point cursor = searchQueue.Dequeue();
                inSearchQueue.Remove(cursor);

                //We have arrived!
                if (cursor == dest)
                {
                    numSteps = stepCounter[cursor].gScore;

                    //unroll our history. 
                    finalPath.Add(cursor);
                    Point? p = stepCounter[cursor].parent;

                    while (p != null)
                    {
                        finalPath.Add((Point)p);
                        p = stepCounter[(Point)p].parent;
                    }

                    return true;
                }

                foreach (Point neighbor in neighbors)
                {
                    Point nextStep = cursor + (Size)neighbor;

                    //bounds and valid move check. 
                    if (TestStep(cursor, nextStep))
                    {
                        //tentative_gScore := gScore[current] + d(current, neighbor)
                        int t_gScore = stepCounter[cursor].gScore + 1;

                        //if tentative_gScore < gScore[neighbor]
                        if (t_gScore < stepCounter[nextStep].gScore)
                        {
                            //cameFrom[neighbor] := current
                            //gScore[neighbor] := tentative_gScore
                            //fScore[neighbor] := tentative_gScore + h(neighbor)
                            stepCounter[nextStep] = (t_gScore, t_gScore + TaxiDistance(nextStep, dest), cursor);

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

        public void DisplayMap() => DisplayMap(finalPath);
        public void DisplayMap(List<Point> drawPath)
        {
            Point cursor = new();

            for (int y = MapMax.Y; y >= MapMin.Y; y--)
            {
                for (int x = MapMin.X; x <= MapMax.X; x++)
                {
                    cursor.X = x;
                    cursor.Y = y;

                    if (_theMap.TryGetValue(cursor, out int value))
                    {
                        if (drawPath.Contains(cursor)) Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        if (cursor == _defaultStart) Console.ForegroundColor = ConsoleColor.Green;
                        if (cursor == _defaultEnd) Console.ForegroundColor = ConsoleColor.Blue;

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
