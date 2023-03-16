using System.Drawing;
using System.Numerics;

namespace AoC_2016_Day_13
{
    internal class Map
    {
        private readonly Point _defaultStart = new(1, 1);
        private readonly Point _defaultEnd = new(31, 39);

        public Point MapMin => new (_finalPath.Select(k => k.X).Min(), _finalPath.Select(k => k.Y).Min());
        public Point MapMax => new (_finalPath.Select(k => k.X).Max(), _finalPath.Select(k => k.Y).Max());

        // the path after an A_Star call. 
        public List<Point> _finalPath = new();

        // puzzle magic number for maze generation.
        private readonly int _designer_number;

        public Map(int designer_number) 
        {
            _designer_number = designer_number;
        }

        public Point DefaultStart() => _defaultStart;

        public Point DefaultEnd() => _defaultEnd;

        private bool GenMap(Point p) => GenMapPoint(p.X, p.Y);

        public bool GenMapPoint(int x, int y)
        {
            uint v = (uint)((x * x) + (3 * x) + (2 * x * y) + y + (y * y) + _designer_number);
            return (BitOperations.PopCount(v) % 2) == 0;
            /*
            // This is bonkers. I do not understand it, but it works.
            // Also this is the same algrothm implemented in the software fallback of the PopCount function.
            // http://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetParallel
            // The best method for counting bits in a 32 - bit integer v is the following:

            v -= ((v >> 1) & 0x55555555);                    // reuse input as temporary
            v = (v & 0x33333333) + ((v >> 2) & 0x33333333);     // temp
            int c = ((v + (v >> 4) & 0xF0F0F0F) * 0x1010101) >> 24; // count
            
            return (c % 2) == 0;
            */
        }

        public static int TaxiDistance(Point a, Point b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

        public bool A_Star(Point start, Point dest, out int numSteps, int? maxSteps = null)
        {
            _finalPath.Clear();

            List<Size> neighbors = new()
            {
                new Size(1,0),
                new Size(-1,0),
                new Size(0,1),
                new Size(0,-1)
            };

            PriorityQueue<Point, int> searchQueue = new(); //we enque based on fScore + h, the distance travelled, plus taxi distance guess to destination.
            HashSet<Point> inSearchQueue = new(); //we add this because we don't have a way to query the queue to see if a specific item is in it.

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
                if (nextPoint.X < 0 || nextPoint.Y < 0) return false;

                //we'll exceed our step count (if applicable) 
                if (maxSteps != null && stepCounter[cursor].gScore + 1 > maxSteps) return false;

                // check our map coordinate for validity.
                if (GenMap(nextPoint))
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
                    _finalPath.Add(cursor);
                    Point? p = stepCounter[cursor].parent;

                    while (p != null)
                    {
                        _finalPath.Add((Point)p);
                        p = stepCounter[(Point)p].parent;
                    }

                    return true;
                }

                foreach (Size neighbor in neighbors)
                {
                    Point nextStep = cursor + neighbor;

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

        public void DisplayMap() => DisplayMap(_finalPath);

        public void DisplayMap(List<Point> drawPath)
        {
            Point cursor = new();

            for (int y = 0; y <= MapMax.Y+1; y++)
            {
                for (int x = 0; x <= MapMax.X + 1; x++)
                {
                    cursor.X = x;
                    cursor.Y = y;
                    char c = GenMapPoint(x, y) ? '.' : '#';

                    if (drawPath.Contains(cursor))
                    {
                        c = 'x';
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    if (cursor == _defaultStart) Console.ForegroundColor = ConsoleColor.Green;
                    if (cursor == _defaultEnd) Console.ForegroundColor = ConsoleColor.Blue;

                    Console.Write(c);

                    if (Console.ForegroundColor != ConsoleColor.Gray) Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.WriteLine(" ");
            }
        }

    }
}
