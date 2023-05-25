//using System.Drawing;
using AoC_Point3D;
using System.Runtime.CompilerServices;

namespace AoC_Map
{
    enum MapSymbols
    {
        Open,
        Wall,
        Portal,
        Blank
    }

    internal class Map
    {
        private readonly Dictionary<(int x, int y), MapSymbols> _theMap = new();

        private readonly PriorityQueue<Point3D, int> _searchQueue = new();
        //we enque based on fScore + h, the distance travelled, plus taxi distance guess to destination.
        private readonly HashSet<Point3D> _inSearchQueue = new();
        //we add this because we don't have a way to query the queue to see if a specific item is in it.

        private readonly Dictionary<(int x, int y), (int x, int y)> _portals = new();
        private readonly List<Point3D> _finalPath = new();
        private readonly Dictionary<(Point3D, Point3D), int> _pathCache = new();

        private Point3D _nextStep = new();
        private Point3D _cursor = new();
        private bool _doHistory = true;
        
        private int _maxX = 0;
        private int _maxY = 0;
        private int _minX = 0;
        private int _minY = 0;

        public static List<(int x, int y)> Neighbors2D = new()
        {
            (0,-1),
            (-1,0),
            (1,0),
            (0,1)
        };

        public Point3D EndPoint { get; set; } = new();
        public Point3D StartPoint { get; set; } = new();
        public bool TraceHistory { get => _doHistory; }

        public Map(string[] puzzleInput)
        {
            Dictionary<string, (int x, int y)> tempPortal = new();

            foreach ((int X, int Y) p in from b in Enumerable.Range(0, puzzleInput.Length)
                                         from a in Enumerable.Range(0, puzzleInput[0].Length)
                                         select (a, b))
            {
                char mapChar = puzzleInput[p.Y][p.X];
                switch (mapChar)
                {
                    case ' ':
                        SetNode(p, MapSymbols.Blank);
                        break;
                    case '#':
                        SetNode(p, MapSymbols.Wall);
                        break;
                    case '.':
                        SetNode(p, MapSymbols.Open);
                        break;
                    case var n when n >= 'A' && n <= 'Z':
                        SetNode(p, MapSymbols.Blank);
                        foreach ((int X, int Y) s in Neighbors2D)
                        {
                            (int X, int Y) testPoint = (p.X + s.X, p.Y + s.Y);

                            if (testPoint.X < 0 || testPoint.Y < 0 || testPoint.X == puzzleInput[0].Length || testPoint.Y == puzzleInput.Length) continue;

                            char testChar = puzzleInput[testPoint.Y][testPoint.X];
                            if (testChar == '.')
                            {
                                string gateID = string.Empty;
                                if (p.Y == testPoint.Y)
                                {
                                    gateID = (testPoint.X == p.X - 1) ? $"{mapChar}{puzzleInput[p.Y][p.X + 1]}" : $"{puzzleInput[p.Y][p.X - 1]}{mapChar}";
                                }
                                if (p.X == testPoint.X)
                                {
                                    gateID = (testPoint.Y == p.Y - 1) ? $"{mapChar}{puzzleInput[p.Y + 1][p.X]}" : $"{puzzleInput[p.Y - 1][p.X]}{mapChar}";
                                }
                                switch (gateID)
                                {
                                    case "AA":
                                        StartPoint = new(testPoint.X, testPoint.Y, 0);
                                        //SetNode(p, MapSymbols.Wall, true);
                                        break;
                                    case "ZZ":
                                        EndPoint = new(testPoint.X, testPoint.Y, 0);
                                        //SetNode(p, MapSymbols.Wall, true);
                                        break;
                                    case "":
                                        break;
                                    default:
                                        if (!tempPortal.TryAdd(gateID, testPoint))
                                        {
                                            AddPortal(testPoint, tempPortal[gateID]);
                                        }
                                        SetNode(testPoint, MapSymbols.Portal, true);
                                        break;
                                }
                                break;
                            }
                        }
                        break;
                    default:
                        throw new NotImplementedException($"Unknown map symbol: {mapChar}");
                };
            }
            _minX = _theMap.Where(w => w.Value == MapSymbols.Wall).Min(m => m.Key.x);
            _minY = _theMap.Where(w => w.Value == MapSymbols.Wall).Min(m => m.Key.y);
            _maxX = _theMap.Where(w => w.Value == MapSymbols.Wall).Max(m => m.Key.x);
            _maxY = _theMap.Where(w => w.Value == MapSymbols.Wall).Max(m => m.Key.y);
        }

        public bool AddPortal((int, int) a, (int, int) b) => _portals.TryAdd(a, b) && _portals.TryAdd(b, a);

        public bool A_Star(Point3D start, Point3D dest, out int numSteps, bool doRecursive, int? maxSteps = null)
        {
            _searchQueue.Clear();
            _inSearchQueue.Clear();

            //gScore is value of the path from start to here
            //fScore is estimated from here to end. We're using Taxi Distance as our hieruistic 
            Dictionary<Point3D, (int gScore, int fScore, Point3D? parent)> stepCounter = new()
            {
                { start, (0, Point3D.TaxiDistance3D(start, dest), null) }
            };

            bool TestStep()
            {
                if (_cursor.Z != 0 && (_nextStep == StartPoint || _nextStep == EndPoint)) return false;

                switch (_theMap[_nextStep.As2D()])
                {
                    case MapSymbols.Wall:
                        return false;
                    case MapSymbols.Portal:
                    case MapSymbols.Open:
                        // should be good.
                        break;
                    case MapSymbols.Blank:
                        if (_theMap[_cursor.As2D()] != MapSymbols.Portal) return false;
                        int newZ = _cursor.Z;

                        if (doRecursive)
                        {
                            if (_theMap[_cursor.As2D()] == MapSymbols.Portal)
                            {
                                // is this an inner or outer portal?
                                newZ += (_cursor.X == _minX || _cursor.X == _maxX || _cursor.Y == _minY || _cursor.Y == _maxY) ? -1 : 1;
                            }
                            if (newZ < 0) return false;
                        }
                       // Console.Write($"PORTAL: {_nextStep} -> ");
                        _nextStep = new(_portals[_cursor.As2D()], newZ);
                       // Console.WriteLine($" {_nextStep}");
                        break;
                }

                if (maxSteps != null && stepCounter[_cursor].gScore + 1 > maxSteps) return false;
                return true;
            }

            _searchQueue.Enqueue(start, Point3D.TaxiDistance3D(start, dest));
            _inSearchQueue.Add(start);

            while (_searchQueue.TryDequeue(out _cursor, out int currentPriority))
            {
                //_cursor = _searchQueue.Dequeue();
                _inSearchQueue.Remove(_cursor);
              //  Console.WriteLine($"De: {_cursor} {currentPriority}");

                //We have arrived!
                if (_cursor == dest)
                {
                    numSteps = stepCounter[_cursor].gScore;

                    //unroll our history. 
                    if (_doHistory)
                    {
                        _finalPath.Clear();
                        _finalPath.Add(_cursor);
                        Point3D? p = stepCounter[_cursor].parent;

                        while (p != null)
                        {
                            _finalPath.Add((Point3D)p);
                            p = stepCounter[(Point3D)p].parent;
                        }
                    }

                    return true;
                }

                foreach ((int x, int y) neighbor in Neighbors2D)
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
                            stepCounter[_nextStep] = (t_gScore, t_gScore + Point3D.TaxiDistance3D(_nextStep, dest), _cursor);

                            //if neighbor not in openSet openSet.add(neighbor) 
                            if (!_inSearchQueue.Contains(_nextStep))
                            {
                               // Console.WriteLine($"En: {_nextStep}, {stepCounter[_nextStep].fScore}");
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
     
        public List<Point3D> GetHistory() => _finalPath;
     
        public void PrintMap()
        {
           bool colorHistory = _finalPath.Any();

           for(int y = _minY; y <= _maxY; y++)
           {
                for (int x = _minX; x <= _maxX; x++)
                {
                    Console.ForegroundColor = colorHistory && _finalPath.Contains(new(x, y, 0)) ? ConsoleColor.Green : ConsoleColor.Gray;
                    Console.Write(_theMap[(x, y)] switch
                    {
                        MapSymbols.Wall => '#',
                        MapSymbols.Open => '.',
                        MapSymbols.Portal => '@',
                        MapSymbols.Blank => ' ',
                        _ => '?'
                    });
                }
                    Console.WriteLine();
            }
            Console.WriteLine();
        }
     
        public void SetNode((int x, int y) p, MapSymbols symbols, bool overwrite = false)
        {
            if (!_theMap.TryAdd(p, symbols) && overwrite) _theMap[p] = symbols;
        }

        public void ToggleHistory() => _doHistory = !_doHistory;
    }
}
