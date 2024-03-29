﻿using System.Drawing;

namespace AoC_AStar
{
    internal static class AStar
    {
        public static List<Size> Neighbors = new()
        {
            new Size(0,-1),
            new Size(-1,0),
            new Size(1,0),
            new Size(0,1)
        };

        public static int TaxiDistance(Point a, Point b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

        public static bool A_Star(Point start, Point dest, Dictionary<Point, int> theMap, out int numSteps, out List<Point> finalPath, int? maxSteps = null)
        {
            finalPath = new();

            PriorityQueue<Point, int> searchQueue = new(); //we enque based on fScore + h, the distance travelled, plus taxi distance guess to destination.
            HashSet<Point> inSearchQueue = new(); //we add this because we don't have a way to query the queue to see if a specific item is in it.

            //gScore is value of the path from start to here
            //fScore is estimated from here to end. We're using Taxi Distance as our hieruistic 
            Dictionary<Point, (int gScore, int fScore, Point? parent)> stepCounter = new()
            {
                { start, (0, TaxiDistance(start, dest), null) }
            };

            int TestStep(Point cursor, Point nextPoint)
            {
                //Don't move to ourself. 
                if (cursor == nextPoint) return -1;

                //Don't leave the map.
                if (!theMap.TryGetValue(nextPoint, out int riskValue)) return -1;
                
                //we'll exceed our step count (if applicable) 
                if (maxSteps != null && stepCounter[cursor].gScore + 1 > maxSteps) return -1;

                stepCounter.TryAdd(nextPoint, (int.MaxValue, int.MaxValue, null));
                return riskValue;
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

                foreach (Size neighbor in Neighbors)
                {
                    Point nextStep = cursor + neighbor;

                    //bounds and valid move check. 
                    int risk = TestStep(cursor, nextStep);
                    if (risk != -1)
                    {
                        //tentative_gScore := gScore[current] + d(current, neighbor)
                        int t_gScore = stepCounter[cursor].gScore + risk;

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

    }
}
