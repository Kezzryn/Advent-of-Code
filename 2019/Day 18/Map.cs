using System.Drawing;

namespace AoC_Map
{
    enum MapSymbols
    {
        Open,
        Wall,
        Door
    }

    internal class Map
    {
        private readonly Dictionary<Point, MapSymbols> _theMap = new();

        private readonly PriorityQueue<Point, int> _searchQueue = new();
        //we enque based on fScore + h, the distance travelled, plus taxi distance guess to destination.
        private readonly HashSet<Point> _inSearchQueue = new();
        //we add this because we don't have a way to query the queue to see if a specific item is in it.

        private readonly Dictionary<Point, int> _theDoors = new();
        private readonly List<Point> _finalPath = new();
        private readonly Dictionary<(Point, Point), (int keysNeeded, int numSteps)> _pathCache = new();

        private Point _nextStep = new();
        private Point _cursor = new();
        private bool _doHistory = true;
        
        public bool TraceHistory { get => _doHistory; }
        public static List<Size> Neighbors = new()
        {
            new Size(0,-1),
            new Size(-1,0),
            new Size(1,0),
            new Size(0,1)
        };
        public static List<Size> DiagonalNeighbors = new()
        {
            new Size(-1,-1),
            new Size(-1, 1),
            new Size( 1,-1),
            new Size( 1, 1)
        };

        public Map() { }

        public bool AddDoor(Point point, int keyID) => _theDoors.TryAdd(point, keyID);
        public bool A_Star(Point start, Point dest, out int numSteps, int? maxSteps = null)
        {
            _searchQueue.Clear();
            _inSearchQueue.Clear();

            //gScore is value of the path from start to here
            //fScore is estimated from here to end. We're using Taxi Distance as our hieruistic 
            Dictionary<Point, (int gScore, int fScore, Point? parent)> stepCounter = new()
            {
                { start, (0, TaxiDistance(start, dest), null) }
            };

            bool TestStep()
            {
                if (_theMap[_nextStep] == MapSymbols.Wall) return false;
                //if (_theMap[_nextStep] == MapSymbols.Door) return (_theDoors[nextPoint] & heldKeys) != 0;

                //we'll exceed our step count (if applicable) 
                if (maxSteps != null && stepCounter[_cursor].gScore + 1 > maxSteps) return false;

                return true;
            }

            _searchQueue.Enqueue(start, TaxiDistance(start, dest));
            _inSearchQueue.Add(start);

            while (_searchQueue.Count > 0)
            {
                _cursor = _searchQueue.Dequeue();
                _inSearchQueue.Remove(_cursor);

                //We have arrived!
                if (_cursor == dest)
                {
                    numSteps = stepCounter[_cursor].gScore;

                    //unroll our history. 
                    if (_doHistory)
                    {
                        _finalPath.Clear();
                        _finalPath.Add(_cursor);
                        Point? p = stepCounter[_cursor].parent;

                        while (p != null)
                        {
                            _finalPath.Add((Point)p);
                            p = stepCounter[(Point)p].parent;
                        }
                    }

                    return true;
                }

                foreach (Size neighbor in Neighbors)
                {
                    _nextStep = _cursor + neighbor;

                    //bounds and valid move check. 
                    if (TestStep())
                    {
                        stepCounter.TryAdd(_nextStep, (int.MaxValue, int.MaxValue, null));

                        //tentative_gScore := gScore[current] + d(current, neighbor)
                        int t_gScore = stepCounter[_cursor].gScore + 1;

                        //if tentative_gScore < gScore[neighbor]
                        if (t_gScore < stepCounter[_nextStep].gScore)
                        {
                            //cameFrom[neighbor] := current
                            //gScore[neighbor] := tentative_gScore
                            //fScore[neighbor] := tentative_gScore + h(neighbor)
                            stepCounter[_nextStep] = (t_gScore, t_gScore + TaxiDistance(_nextStep, dest), _cursor);

                            //if neighbor not in openSet openSet.add(neighbor) 
                            if (!_inSearchQueue.Contains(_nextStep))
                            {
                                _searchQueue.Enqueue(_nextStep, stepCounter[_nextStep].fScore);
                                _inSearchQueue.Add(_nextStep);
                            }
                        }
                    }
                }
            }
            numSteps = -1;
            return false;
        }
        public void ClearCache() => _pathCache.Clear();
        public List<Point> GetHistory() => _finalPath;
        public int GetKeysNeeded()
        {
            int returnValue = 0;
            if (_finalPath.Count == 0) return -1;

            foreach ((Point door, int keyID) in _theDoors.Where(x => _finalPath.Contains(x.Key)))
            {
                returnValue |= keyID;
            }
            return returnValue;
        }
        public void PrintMap()
        {
            Point min = new(0, 0);
            Point max = new(0, 0);

            min.X = _theMap.Min(x => x.Key.X);
            min.Y = _theMap.Min(x => x.Key.Y);
            max.X = _theMap.Max(x => x.Key.X);
            max.Y = _theMap.Max(x => x.Key.Y);

            for(int y =  min.Y; y <= max.Y; y++)
            {
                for (int x = min.X; x <= max.X; x++)
                {
                    Console.Write(_theMap[new(x, y)] switch
                    {
                        MapSymbols.Wall => '#',
                        MapSymbols.Open => ' ',
                        MapSymbols.Door => (char)(Math.Log2(_theDoors[new(x,y)]) + 64),
                        _ => '?'
                    });
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public void SetNode(Point point, MapSymbols symbols)
        {
            if (!_theMap.TryAdd(point, symbols)) _theMap[point] = symbols;
        }
        public static int TaxiDistance(Point a, Point b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);    
        public bool TestPath(Point start, Point end, int haveKeys, out int numSteps)
        {
            int keysNeeded; 

            if(_pathCache.TryGetValue((start, end), out (int keysNeeded, int numSteps) cacheValue))
            {
                numSteps = cacheValue.numSteps;
                keysNeeded = cacheValue.keysNeeded;
            }
            else
            {
                A_Star(start, end, out int aStarNumSteps);
                
                numSteps = aStarNumSteps;
                keysNeeded = GetKeysNeeded();

                _pathCache.Add((start, end), (keysNeeded, aStarNumSteps));
            }
            return numSteps != -1 && keysNeeded == (haveKeys & keysNeeded);
        }
        public void ToggleHistory() => _doHistory = !_doHistory;
    }
}
