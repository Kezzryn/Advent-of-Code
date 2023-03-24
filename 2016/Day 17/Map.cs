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
        
        string _secretHash; 

        public Map(string puzzleInput)
        {

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    _theMap.Add(new Point(x,y), 1);
                }
            }

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

            // have we moved right? 
            if (current.X < nextPoint.X) 
            {
                if (!dirs[RIGHT]) return false;
                _history.Add(nextPoint, $"{histValue}R");
            }

            if (current.X > nextPoint.X)
            {
                if (!dirs[LEFT]) return false;
                _history.Add(nextPoint, $"{histValue}L");
            }

            if (current.Y < nextPoint.Y)
            {
                if (!dirs[UP]) return false;
                _history.Add(nextPoint, $"{histValue}U");
            }

            if (current.Y > nextPoint.Y)
            {
                if (!dirs[DOWN]) return false;
                _history.Add(nextPoint, $"{histValue}D");
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

                    //unroll our history. 
                    FinalPath.Add(cursor);
                    Point3D? p = stepCounter[cursor].parent;

                    while (p != null)
                    {
                        FinalPath.Add(p);
                        p = stepCounter[p].parent;
                    }

                    History = _history[cursor].ToString();

                    return true;
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
