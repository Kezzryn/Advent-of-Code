using System.Drawing;

namespace AoC_2018_Day_15
{
    enum MapSymbols
    {
        Open,
        Wall
    }

    internal static class AStar
    {
        // in "reading order" -y, -x, +x, +y
        public static List<Size> Neighbors = new()
        {
            new Size(0,-1),
            new Size(-1,0),
            new Size(1,0),
            new Size(0,1)
        };

        public static int TaxiDistance(Point a, Point b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

        public static bool NextStep(Point source, Point target, Dictionary<Point, MapSymbols> theMap, HashSet<Point> collisionMap, out Point nextStep, out int dist)
        {
            nextStep = new(-1, -1);
            dist = -1;
            Dictionary<(Point origon, Point destination), int> results = new();

            foreach(var coordPair in from origon in Neighbors.Select(x => source + x).Where(z => theMap[z] == MapSymbols.Open && !collisionMap.Contains(z)).ToList()
                                     from destination in Neighbors.Select(x => target + x).Where(z => theMap[z] == MapSymbols.Open && !collisionMap.Contains(z)).ToList()
                                     select (origon, destination))
            {
                if (coordPair.origon == target)
                {
                    // adjacency test
                    nextStep = source;
                    dist = 0;
                    return true;
                }
                dist = A_Star(coordPair.origon, coordPair.destination, theMap, collisionMap, out _);
                if (dist != -1)  results.Add(coordPair, dist);
            }

            if (results.Count == 0) return false;
            nextStep = results.OrderBy(x => x.Value).ThenBy(x => x.Key.origon.Y).ThenBy(x => x.Key.origon.X).First().Key.origon;
            dist = results.Values.Min();
            return true;
        }

        public static int A_Star(Point start, Point dest, Dictionary<Point, MapSymbols> theMap, HashSet<Point> collisionMap, out List<Point> finalPath, int? maxSteps = null)
        {
            int numSteps = 0;
            finalPath = new();

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
                if (!theMap.ContainsKey(nextPoint)) return false;

                //we'll exceed our step count (if applicable) 
                if (maxSteps != null && stepCounter[cursor].gScore + 1 > maxSteps) return false;

                
                if (theMap[nextPoint] != MapSymbols.Wall && !collisionMap.Contains(nextPoint))
                {
                    stepCounter.TryAdd(nextPoint, (int.MaxValue, int.MaxValue, null));
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

                    return numSteps;
                }

                foreach (Size neighbor in Neighbors)
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
            return -1;
        }
    }
}
