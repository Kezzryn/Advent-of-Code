using System.Drawing;

namespace AoC_2016_Day_17
{
    internal class Map
    {
        // this serves as our reference starting position for everything. 
        private readonly Dictionary<Point, int> _theMap = new();
        private readonly Dictionary<Point3D, string> _history = new();

        //as found on the map.
        private readonly Point3D _defaultStart = new(0, 3, 0);
        private readonly Point3D _defaultEnd = new(3, 0, 0);

        public Point MapMin { get; }

        public Point MapMax { get; }

        //the path after an A_Star call. 
        public List<Point3D> FinalPath = new();

        public string History = String.Empty;
        
        private readonly string _secretHash;

        public Map(string puzzleInput)
        {
            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
                    _theMap.Add(new Point(x,y), 1);

            _secretHash = puzzleInput;

            MapMin = new Point(_theMap.Keys.Select(k => k.X).Min(), _theMap.Keys.Select(k => k.Y).Min());
            MapMax = new Point(_theMap.Keys.Select(k => k.X).Max(), _theMap.Keys.Select(k => k.Y).Max());
        }

        public static int TaxiDistance(Point s, Point e)
        {
            return Math.Abs(s.X - e.X) + Math.Abs(s.Y - e.Y);
        }

        public Point3D DefaultStart => _defaultStart;

        public Point3D DefaultEnd => _defaultEnd;

        private bool TestStep(Point3D current, Point3D nextPoint)
        {
            const int UP = 0;
            const int DOWN = 1;
            const int LEFT = 2;
            const int RIGHT = 3;

            // Off the map
            if (!_theMap.TryGetValue(nextPoint.Get2DPoint(), out _ )) return false;

            string histValue = _history.TryGetValue(current, out string? historyValue) ? historyValue : String.Empty;
            string hash = MD5Hasher.GetHash($"{_secretHash}{histValue}");

            // 0U 1D 2L 3R 
            bool[] dirs = new bool[4];

            for(int i = 0; i < 4; i++)
            {
                dirs[i] = "bcdef".Contains(hash[i]);
            }

            string dirValue = String.Empty;

            if (current.X < nextPoint.X) 
            {
                if (!dirs[RIGHT]) return false;
                dirValue = "R";
            }

            if (current.X > nextPoint.X)
            {
                if (!dirs[LEFT]) return false;
                dirValue = "L";
            }

            if (current.Y < nextPoint.Y)
            {
                if (!dirs[UP]) return false;
                dirValue = "U";
            }

            if (current.Y > nextPoint.Y)
            {
                if (!dirs[DOWN]) return false;
                dirValue = "D";
            }

            if (!_history.TryAdd(nextPoint, $"{histValue}{dirValue}"))
            {
                string temp = $"{histValue}{dirValue}";
                if (temp.Length > _history[nextPoint].Length) _history[nextPoint] = temp;
            }
            return true;
        }

        public bool A_Star(Point3D start, Point3D dest, out int numSteps, bool longPath = false)
        {
            FinalPath.Clear();

            int maxSteps = -1;
            _history.Clear();

            List<Point3D> neighbors = new()
            {
                new Point3D(1,0,1), // right
                new Point3D(-1,0,1),// left
                new Point3D(0,1,1), // up
                new Point3D(0,-1,1)// down
                //new Point3D(0,0,1)  // stay
            };

            PriorityQueue<Point3D, int> searchQueue = new(); //we enque based on fScore + h, the distance travelled, plus taxi distance guess to destination.
            HashSet<Point3D> inSearchQueue = new(); //we add this because we don't have a way to query the queue to see if a specific item is in it.

            //gScore is value of the path from start to here
            //fScore is estimated from here to end. We're using Taxi Distance as our hieruistic 
            Dictionary<Point3D, (int gScore, int fScore, Point3D? parent)> stepCounter = new()
            {
                { start, (0, TaxiDistance(start.Get2DPoint(), dest.Get2DPoint()), null) }
            };

            _history.Add(start, "");

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
                    if (numSteps > maxSteps) maxSteps = numSteps;

                    //unroll our history. 
                    FinalPath.Add(cursor);
                    Point3D? p = stepCounter[cursor].parent;

                    while (p != null)
                    {
                        FinalPath.Add(p);
                        p = stepCounter[p].parent;
                    }

                    History = _history[cursor].ToString();

                    if (!longPath) return true;
                }

                foreach (Point3D neighbor in neighbors)
                {
                    Point3D nextStep = cursor + neighbor;

                    //bounds and valid move check. 
                    if (TestStep(cursor, nextStep))
                    {
                        if (!stepCounter.ContainsKey(nextStep)) stepCounter.Add(nextStep, (int.MaxValue, int.MaxValue, null));

                        //tentative_gScore := gScore[current] + d(current, neighbor)
                        int t_gScore = stepCounter[cursor].gScore + 1;

                        //if tentative_gScore < gScore[neighbor]
                        if (t_gScore < stepCounter[nextStep].gScore || longPath)
                        {
                            //cameFrom[neighbor] := current
                            //gScore[neighbor] := tentative_gScore
                            //fScore[neighbor] := tentative_gScore + h(neighbor)
                            stepCounter[nextStep] = (t_gScore, t_gScore + TaxiDistance(nextStep.Get2DPoint(), dest.Get2DPoint()), cursor);

                            //if neighbor not in openSet openSet.add(neighbor) 
                            if (!inSearchQueue.Contains(nextStep) || longPath)
                            {
                                searchQueue.Enqueue(nextStep, stepCounter[nextStep].fScore);
                                inSearchQueue.Add(nextStep);
                            }
                        }
                    }
                }
            }
            if (longPath)
            {
                numSteps = maxSteps;
                return true;
            }
            numSteps = -1;
            return false;
        }

        public int A_Slow(Point3D start, Point3D dest)
        {
            // stripped all the optimizations out of A_Start and returned the max value. 
            int maxSteps = -1;
            _history.Clear();
            List<string> neighbors = new()
            {
                "U","D","L","R"
            };

            Queue<string> searchQueue = new(); 

            searchQueue.Enqueue(string.Empty);

            while (searchQueue.Count > 0)
            {
                string cursor = searchQueue.Dequeue();

                if (GetPointFromHistory(cursor, start) == dest.Get2DPoint())
                {
                    if( cursor.Length > maxSteps) maxSteps = cursor.Length;
                    continue;
                }

                foreach (string neighbor in neighbors)
                {
                    //bounds and valid move check. 
                    if (TestStepA_Slow(cursor, neighbor, start))
                    {
                        string nextStep = cursor + neighbor;
                        searchQueue.Enqueue(nextStep);
                    }
                }
            }
            return maxSteps;
        }

        private static Point GetPointFromHistory(string history, Point3D start)
        {
            //Coverts a history string into a point.  used in A_Slow to test if they've hit the end.
            int x = start.X;
            int y = start.Y;
            foreach (char c in history)
            {
                switch (c)
                {
                    case 'U':
                        y++;
                        break;
                    case 'D':
                        y--;
                        break;
                    case 'L':
                        x--;
                        break;
                    case 'R':
                        x++;
                        break;
                    default:
                        break;
                }
            }

            return new Point(x, y);
        }

        private bool TestStepA_Slow(string current, string nextPoint, Point3D start)
        {

            const int UP = 0;
            const int DOWN = 1;
            const int LEFT = 2;
            const int RIGHT = 3;

            // Off the map
            if (!_theMap.TryGetValue(GetPointFromHistory(current + nextPoint, start), out _)) return false;

            string hash = MD5Hasher.GetHash($"{_secretHash}{current}");

            // 0U 1D 2L 3R 
            bool[] dirs = new bool[4];

            for (int i = 0; i < 4; i++)
            {
                dirs[i] = "bcdef".Contains(hash[i]);
            }

            if (nextPoint == "R" && !dirs[RIGHT]) return false;
            if (nextPoint == "L" && !dirs[LEFT]) return false;
            if (nextPoint == "U" && !dirs[UP]) return false;
            if (nextPoint == "D" && !dirs[DOWN]) return false;

            return true;
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

                    //if (_theMap.TryGetValue(cursor.Get2DPoint(), out MapSymbols value))
                    //{
                    //    if (drawPath.Contains(cursor)) Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    //    if (cursor.Get2DPoint() == _defaultStart) Console.ForegroundColor = ConsoleColor.Green;
                    //    if (cursor.Get2DPoint() == _defaultEnd) Console.ForegroundColor = ConsoleColor.Blue;

                    //    Console.Write((char)(value + 96));

                    //    if (Console.ForegroundColor != ConsoleColor.Gray) Console.ForegroundColor = ConsoleColor.Gray;
                    //}
                    //else
                    //{
                    //    Console.ForegroundColor = ConsoleColor.Red;
                    //    Console.Write("?");
                    //    Console.ForegroundColor = ConsoleColor.Gray;
                    //}
                }
                Console.WriteLine(" ");
            }
        }
    }
}
