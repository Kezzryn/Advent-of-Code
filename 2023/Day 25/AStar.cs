namespace AoC_AStar
{
    internal static class AStar
    {
       // public static int TaxiDistance(Point a, Point b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

        public static bool A_Star(string start, string dest, Dictionary<string, List<string>> theMap, out int numSteps, out List<string> finalPath, int? maxSteps = null)
        {
            finalPath = new();

            PriorityQueue<string, int> searchQueue = new(); //we enque based on fScore + h, the distance travelled, plus taxi distance guess to destination.
            HashSet<string> inSearchQueue = []; //we add this because we don't have a way to query the queue to see if a specific item is in it.

            //gScore is value of the path from start to here
            //fScore is estimated from here to end. We're using Taxi Distance as our hieruistic 
            Dictionary<string, (int gScore, int fScore, string? parent)> stepCounter = new()
            {
                { start, (0, theMap.Count, null) }
            };

            int TestStep(string cursor, string nextPoint)
            {
                //Don't move to ourself. 
                if (cursor == nextPoint) return -1;

                //Don't leave the map.
                //if (!theMap.TryGetValue(nextPoint, out int riskValue)) return -1;
                
                //we'll exceed our step count (if applicable) 
                if (maxSteps != null && stepCounter[cursor].gScore + 1 > maxSteps) return -1;

                stepCounter.TryAdd(nextPoint, (int.MaxValue, int.MaxValue, null));
                return 0;// riskValue;
            }

            searchQueue.Enqueue(start, theMap.Count);
            inSearchQueue.Add(start);

            while (searchQueue.Count > 0)
            {
                string cursor = searchQueue.Dequeue();
                inSearchQueue.Remove(cursor);

                //We have arrived!
                if (cursor == dest)
                {
                    numSteps = stepCounter[cursor].gScore;

                    //unroll our history. 
                    finalPath.Add(cursor);
                    string? p = stepCounter[cursor].parent;

                    while (p != null)
                    {
                        finalPath.Add(p);
                        p = stepCounter[p].parent;
                    }

                    return true;
                }

                foreach (string nextStep in theMap[cursor])
                {
                    //?Point nextStep = cursor + neighbor;

                    //bounds and valid move check. 

                    stepCounter.TryAdd(nextStep, (int.MaxValue, int.MaxValue, null));
                    
                    //tentative_gScore := gScore[current] + d(current, neighbor)
                    int t_gScore = stepCounter[cursor].gScore + 1;

                    //if tentative_gScore < gScore[neighbor]
                    if (t_gScore < stepCounter[nextStep].gScore)
                    {
                        //cameFrom[neighbor] := current
                        //gScore[neighbor] := tentative_gScore
                        //fScore[neighbor] := tentative_gScore + h(neighbor)
                        stepCounter[nextStep] = (t_gScore, t_gScore + theMap.Count - t_gScore, cursor);

                        //if neighbor not in openSet openSet.add(neighbor) 
                        if (!inSearchQueue.Contains(nextStep))
                        {
                            searchQueue.Enqueue(nextStep, stepCounter[nextStep].fScore);
                            inSearchQueue.Add(nextStep);
                        }
                    }
                }
            }
            numSteps = -1;
            return false;
        }

    }
}
