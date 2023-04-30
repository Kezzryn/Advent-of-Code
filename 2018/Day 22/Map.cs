using System.Drawing;

namespace AoC_2018_Day_22
{
    enum TerrainType
    {
        Rocky = 0,
        Wet = 1,
        Narrow = 2
    }

    enum ToolTypes
    {
        None,
        Torch,
        Climbing
    }

    internal class TheMap
    {
        private readonly Dictionary<Point, int> _theMap = new();
        private static readonly List<Point3D> _neighbors = new()
        {
            new Point3D( 0, 1, 0),
            new Point3D( 1, 0, 0),
            new Point3D( 0,-1, 0),
            new Point3D(-1, 0, 0)
        };

        private int _maxX = 0;
        private int _maxY = 0;
        private readonly int _depth = 0;

        public TheMap(int sourceX, int sourceY, int targetX, int targetY, int depth)
        {
            _depth = depth;
            _theMap.Add(new(sourceX, sourceY), GetErosionAt(sourceX, sourceY));
            _theMap.Add(new(targetX, targetY), GetErosionAt(sourceX, sourceY)); // target is the same type as source.

            FillCave(targetX, targetY);
        }

        public int CaveRisk() => _theMap.Select(x => x.Value % 3).Sum();

        private void FillCave(int newMaxX, int newMaxY)
        {
            newMaxY = int.Max(newMaxY, _maxY);
            newMaxX = int.Max(newMaxX, _maxX);

            foreach ((int x, int y) in from y in Enumerable.Range(0, newMaxY + 1)
                                       from x in Enumerable.Range(0, newMaxX + 1)
                                       where !_theMap.ContainsKey(new(x, y))
                                       select (x, y))

            {
                _theMap.TryAdd(new Point(x, y), GetErosionAt(x, y));
            }
            _maxY = newMaxY;
            _maxX = newMaxX;
        }

        private TerrainType GetTerrainAt(int x, int y) => GetTerrainAt(new(x, y));

        private TerrainType GetTerrainAt(Point p)
        {
            if (p.X > _maxX || p.Y > _maxY) 
            {
                FillCave(p.X + 25, p.Y + 25);
            }

            if (_theMap.TryGetValue(p, out int value))
            {
                return (TerrainType)(value % 3);
            }

            throw new Exception("Failed to expand map.");
        }

        private int GetErosionAt(int x, int y)
        {
            const int X_MULTI = 16807;
            const int Y_MULTI = 48271;
            const int MODULO = 20183;

            return (x, y) switch
            {
                (0, 0) => _depth % MODULO,
                (_, 0) => ((x * X_MULTI) + _depth) % MODULO,
                (0, _) => ((y * Y_MULTI) + _depth) % MODULO,
                _ => ((_theMap[new(x, y - 1)] * _theMap[new(x - 1, y)]) + _depth) % MODULO,
            };
        }

        public void DrawCave(List<Point>? path = null)
        {
            ConsoleColor ccFGC = Console.ForegroundColor;

            int maxMapX = path != null ? path.Select(s => s.X).Max() : _maxX;
            int maxMapY = path != null ? path.Select(s => s.Y).Max() : _maxY;

            foreach ((int x, int y) in from y in Enumerable.Range(0, maxMapY + 1)
                                       from x in Enumerable.Range(0, maxMapX + 1)
                                       select (x, y))
            {
                (char symbol, ConsoleColor color) = GetTerrainAt(x, y) switch
                {
                    TerrainType.Rocky => ('.', ConsoleColor.Gray),
                    TerrainType.Wet => ('=', ConsoleColor.Blue),
                    TerrainType.Narrow => ('|', ConsoleColor.DarkYellow),
                    _ => throw new NotImplementedException($"({x},{y}) {GetTerrainAt(x, y)}")
                };

                Console.ForegroundColor = path != null && path.Contains(new(x, y)) ? ConsoleColor.Red : color; 
                Console.Write(symbol);

                if (x == maxMapX) Console.WriteLine();
            }
            Console.WriteLine();

            Console.ForegroundColor = ccFGC;
        }

        public static int TaxiDistance(int aX, int aY, int bX, int bY) => Math.Abs(aX - bX) + Math.Abs(aY - bY);
        public static int TaxiDistance(Point a, Point b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

        public bool A_Star(Point3D start, Point3D dest, out int numSteps, out List<Point> finalPath, int? maxSteps = null)
        {
            numSteps = int.MaxValue;
            finalPath = new();

            PriorityQueue<Point3D, int> searchQueue = new(); //we enque based on fScore + h, the distance travelled, plus taxi distance guess to destination.
            HashSet<Point3D> inSearchQueue = new(); //we add this because we don't have a way to query the queue to see if a specific item is in it.

            //gScore is value of the path from start to here
            //fScore is estimated from here to end. We're using Taxi Distance as our hieruistic 
            Dictionary<Point3D, (int gScore, int fScore, Point3D? parent)> stepCounter = new()
            {
                { start, (0, TaxiDistance(start.Get2DPoint(), dest.Get2DPoint()), null) }
            };

            bool TestStep(Point3D cursor, Point3D nextPoint, out Point3D nextStep, out int stepCost)
            {
                static bool ValidTool(ToolTypes tool, TerrainType terrain)
                {
                    if (tool == ToolTypes.None && terrain != TerrainType.Rocky) return true;
                    if (tool == ToolTypes.Torch && terrain != TerrainType.Wet) return true;
                    if (tool == ToolTypes.Climbing && terrain != TerrainType.Narrow) return true;

                    return false;
                }

                static ToolTypes SwapTool(ToolTypes tool, TerrainType terrain)
                {
                    return terrain switch
                    {
                        TerrainType.Rocky => tool == ToolTypes.Torch ? ToolTypes.Climbing : ToolTypes.Torch,
                        TerrainType.Wet => tool == ToolTypes.None ? ToolTypes.Climbing : ToolTypes.None,
                        TerrainType.Narrow => tool == ToolTypes.Torch ? ToolTypes.None : ToolTypes.Torch,
                        _ => throw new NotImplementedException()
                    };
                }

                // default next steps.
                stepCost = -1;
                nextStep = new();

                if (nextPoint.X < 0 || nextPoint.Y < 0) return false;   // no negative X,Y 
                if (nextPoint.X > 50) return false; // sanity bounds check
                if (cursor == nextPoint) return false;  //Don't move to ourself. 

                //we'll exceed our step count (if applicable)
                if (maxSteps != null && stepCounter[cursor].gScore + 1 > maxSteps) return false;

                TerrainType currentTerrain = GetTerrainAt(cursor.X, cursor.Y);
                TerrainType nextTerrain = GetTerrainAt(nextPoint.X, nextPoint.Y);
                ToolTypes currentTool = (ToolTypes)cursor.Z;
                ToolTypes nextTool = currentTool;

                if (currentTerrain == nextTerrain || ValidTool(currentTool, nextTerrain))
                {
                    // no reason to change tools needlessly.
                    nextStep = nextPoint;
                    stepCost = 1;
                }
                else
                {
                    nextStep = new(nextPoint.Get2DPoint(), (int)SwapTool(currentTool, currentTerrain));
                    stepCost = 8;
                }
                return true;
            }

            searchQueue.Enqueue(start, Point3D.TaxiDistance2D(start, dest));
            inSearchQueue.Add(start);

            while (searchQueue.Count > 0)
            {
                Point3D cursor = searchQueue.Dequeue();
                inSearchQueue.Remove(cursor);

                if (stepCounter[cursor].gScore > numSteps) continue; // we have a better path. 

                //We have arrived!
                if (cursor.Get2DPoint() == dest.Get2DPoint())
                {
                    int testSteps = stepCounter[cursor].gScore +
                        ((ToolTypes)cursor.Z == ToolTypes.Torch ? 0 : 7);

                    if (testSteps < numSteps)
                    {
                        finalPath.Clear();
                        numSteps = testSteps;

                        //unroll our history. 
                        finalPath.Add(cursor.Get2DPoint());
                        Point3D? p = stepCounter[cursor].parent;

                        while (p != null)
                        {
                            finalPath.Add(p.Get2DPoint());
                            p = stepCounter[p].parent;
                        }
                    }
                }

                foreach (Point3D neighbor in _neighbors)
                {
                    //bounds and valid move check. 
                    if (TestStep(cursor, cursor + neighbor, out Point3D nextStep, out int stepCost))
                    {
                        stepCounter.TryAdd(nextStep, (int.MaxValue, int.MaxValue, null));

                        //tentative_gScore := gScore[current] + d(current, neighbor)
                        int t_gScore = stepCounter[cursor].gScore + stepCost;

                        //if tentative_gScore < gScore[neighbor]
                        if (t_gScore < stepCounter[nextStep].gScore)
                        {
                            //cameFrom[neighbor] := current
                            //gScore[neighbor] := tentative_gScore
                            //fScore[neighbor] := tentative_gScore + h(neighbor)
                            stepCounter[nextStep] = (t_gScore, t_gScore + Point3D.TaxiDistance2D(nextStep, dest), cursor);

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
            return numSteps < int.MaxValue;
        }
    }
}
