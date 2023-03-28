using System.Drawing;

namespace AoC_2016_Day_24
{
    internal class Map
    {
        private const int WALL = 1;
        private const int AIR = 0;

        // this serves as our reference starting position for everything. 
        private readonly Dictionary<Point, int> _theMap = new();

        //as found on the map.
        private readonly Point _defaultStart = new(0, 0);
        private readonly Point _defaultEnd = new(0, 0);

        public Point MapMin { get; }

        public Point MapMax { get; }

        //the path after an A_Star call. 
        public List<Point> FinalPath = new();

        public Dictionary<char, Point> WayPoints = new();

        public Map(string[] puzzleInput)
        {
            for(int y = 0; y < puzzleInput.Length; y++)
            {
                for(int x = 0; x < puzzleInput[y].Length; x++)
                {
                    char c = puzzleInput[y][x];
                    
                    if (c == '#')
                    {
                        _theMap.Add(new(x, y), WALL);
                    }
                    else
                    {
                        _theMap.Add(new(x, y), AIR);
                        if (int.TryParse(c.ToString(), out int num))
                        {
                            WayPoints.Add(c, new Point(x, y));
                        }
                    }
                }
            }



            MapMin = new Point(_theMap.Keys.Select(k => k.X).Min(), _theMap.Keys.Select(k => k.Y).Min());
            MapMax = new Point(_theMap.Keys.Select(k => k.X).Max(), _theMap.Keys.Select(k => k.Y).Max());
        }

        public static int TaxiDistance(Point s, Point e)
        {
            return Math.Abs(s.X - e.X) + Math.Abs(s.Y - e.Y);
        }

        public Point DefaultStart => _defaultStart;

        public Point DefaultEnd => _defaultEnd;

        public bool A_Star(Point start, Point dest, out int numSteps)
        {
            FinalPath.Clear();

            int maxSteps = -1;

            List<Size> neighbors = new()
            {
                new Size(1,0), // right
                new Size(-1,0),// left
                new Size(0,1), // up
                new Size(0,-1)// down
            };

            PriorityQueue<Point, int> searchQueue = new(); //we enque based on fScore + h, the distance travelled, plus taxi distance guess to destination.
            HashSet<Point> inSearchQueue = new(); //we add this because we don't have a way to query the queue to see if a specific item is in it.

            //gScore is value of the path from start to here
            //fScore is estimated from here to end. We're using Taxi Distance as our hieruistic 
            Dictionary<Point, (int gScore, int fScore, Point? parent)> stepCounter = new()
            {
                { start, (0, TaxiDistance(start, dest), null) }
            };

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
                    if (numSteps > maxSteps) maxSteps = numSteps;

                    //unroll our history. 
                    FinalPath.Add(cursor);
                    Point? p = stepCounter[cursor].parent;

                    while (p != null)
                    {
                        FinalPath.Add((Point)p);
                        p = stepCounter[(Point)p].parent;
                    }

                    return true;
                }

                foreach (Size neighbor in neighbors)
                {
                    Point nextStep = cursor + neighbor;

                    //bounds and valid move check. 
                    if (_theMap[nextStep] == AIR)
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

        public void DisplayMap() => DisplayMap(FinalPath);
  
        public void DisplayMap(List<Point> drawPath)
        {
            Point cursor = new();

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
