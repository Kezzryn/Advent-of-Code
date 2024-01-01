using System.Drawing;

//It started its life as AStar, but has none of the optimizations.
namespace AoC_MaxStar
{
    internal static class MaxStar
    {
        public static List<Size> Neighbors = [
            new Size(0, -1),
            new Size(-1, 0),
            new Size(1, 0),
            new Size(0, 1)
        ];

        public static Dictionary<Point, Dictionary<Point, int>> MakeMap(string[] theMap, Point start, Point end, bool doPart1 = true)
        {
            const char WALL = '#';
            int maxX = theMap[0].Length - 1;
            int maxY = theMap.Length - 1;

            Dictionary<Point, Dictionary<Point, int>> returnValue = [];
            returnValue[start] = [];
            returnValue[end] = [];

            // find all the intersections. 
            foreach(Point p in from x in Enumerable.Range(1, maxX -1)
                               from y in Enumerable.Range(1, maxY - 1)
                               where (theMap[y][x] != WALL)
                               where (Neighbors.Count(n => theMap[n.Height + y][n.Width + x] == WALL) < 2)
                               select new Point(x,y))
            {
                returnValue[p] = [];
            }

            // Go nesting go!
            foreach (Point startPoint in returnValue.Keys)
            {
                foreach (Point firstStep in Neighbors.Select(s => startPoint + s))
                {
                    if (firstStep.X < 0 || firstStep.X > maxX || firstStep.Y < 0 || firstStep.Y > maxY) continue;
                    if (theMap[firstStep.Y][firstStep.X] == WALL) continue;

                    if (doPart1)
                    {
                        bool validDirection = theMap[firstStep.Y][firstStep.X] switch
                        {
                            '^' => firstStep.Y == startPoint.Y - 1,
                            'v' => firstStep.Y == startPoint.Y + 1,
                            '<' => firstStep.X == startPoint.X - 1,
                            '>' => firstStep.X == startPoint.X + 1,
                            _ => true // not one of those? We don't care.
                        };
                        if (!validDirection) continue;
                    }

                    HashSet<Point> visited = [startPoint, firstStep];
                    Point cursor = firstStep;

                    do
                    {
                        foreach(Point nextStep in Neighbors.Select(s => cursor + s).Where(w => theMap[w.Y][w.X] != WALL))
                        {
                            if (visited.Add(nextStep))
                            {
                                cursor = nextStep;
                                break;
                            }
                        }
                    } while (!returnValue.ContainsKey(cursor));

                    returnValue[startPoint][cursor] = visited.Count - 1;
                    if (!doPart1)
                    {
                        returnValue[cursor][startPoint] = visited.Count - 1;
                    }
                }
            }

            //Graphvis debug text.
            //"X=1,Y=0" -> "X=11,Y=9" [label = " 91"]
            //foreach (Point source in returnValue.Keys.OrderBy(o => o.X).ThenBy(tb => tb.Y))
            //{
            //    foreach ((Point target, int dist) in returnValue[source].OrderBy(o => o.Key.X).ThenBy(tb => tb.Key.Y))
            //    {
            //        Console.WriteLine($"\"{source.X}, {source.Y}\" -> \"{target.X}, {target.Y}\" [label = \" {dist}\"]");
            //    }
            //}

            return returnValue;
        }

        public static int Max_Star(Point start, Point dest, Dictionary<Point, Dictionary<Point, int>> theMap)
        {
            int numSteps = int.MinValue;
            Dictionary<Point, long> nodeIDMask = theMap.Keys.Select((s, i) => (s, 1L << i)).ToDictionary();

            Dictionary<(Point, long), int> stepCounter = [ ];

            PriorityQueue<(Point,long), int> searchQueue = new();
            searchQueue.Enqueue((start, nodeIDMask[start]), 0);
            stepCounter.Add((start, nodeIDMask[start]), 0);

            while (searchQueue.TryDequeue(out (Point cursor, long history) sq, out int currentStepCount))
            {
                //We have arrived!
                if (sq.cursor == dest && currentStepCount > numSteps)
                {
                    numSteps = currentStepCount;
                    continue;
                }
               
                //we got here on exactly the same nodes but with a better route.
                if (stepCounter[sq] > currentStepCount) continue;

                foreach ((Point nextStep, int stepCost) in theMap[sq.cursor])
                {
                    // resolves to 0 if the mask and history do not share common bits.
                    if ((sq.history & nodeIDMask[nextStep]) == 0)
                    {
                        long newHistory = sq.history | nodeIDMask[nextStep];

                        stepCounter[(nextStep, newHistory)] = currentStepCount + stepCost;
                        searchQueue.Enqueue((nextStep, newHistory), currentStepCount + stepCost);
                    } 
                }
            }
            return numSteps;
        }
    }
}
