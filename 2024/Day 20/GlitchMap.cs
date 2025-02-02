namespace AoC_2024_Day_20;
using BKH.Geometry;

internal class GlitchMap
{
    private const int OPEN = 0;
    private const int WALL = 1;

    // this serves as our reference starting position for everything. 
    private readonly Dictionary<Point2D, int> _theMap = [];
    private bool _doGlitch = false;

    //the path after an A_Star call. 
    public Dictionary<Point2D, int> CleanPath = [];
    public Dictionary<(Point2D start, Point2D end), int> GlitchPaths = [];

    //as found on the map.
    public Point2D StartPosition { get; set; }
    public Point2D EndPosition { get; set; }

    public Point2D MapMin { get { return new Point2D(_theMap.Keys.Select(k => k.X).Min(), _theMap.Keys.Select(k => k.Y).Min()); } }
    public Point2D MapMax { get { return new Point2D(_theMap.Keys.Select(k => k.X).Max(), _theMap.Keys.Select(k => k.Y).Max()); } }

    /// <summary>
    /// The maximum number of steps to search.
    /// 0 is unlimited steps.
    /// </summary>
    public int MaxSteps { get; set; } = 0;
    public int NumSteps { get; set; } = 0;

    private readonly Dictionary<Point2D, (int gScore, int fScore, Point2D? parent)> _stepCounter = [];

    public GlitchMap(string[] puzzleInput)
    {
        LoadMap(puzzleInput);
    }

    public GlitchMap() { }

    private void LoadMap(string[] puzzleInput)
    {
        foreach (Point2D p in from Y in Enumerable.Range(0, puzzleInput.Length)
                              from X in Enumerable.Range(0, puzzleInput[0].Length)
                              select new Point2D(X, Y))
        {
            char mapSymbol = puzzleInput[p.Y][p.X];
            _theMap[p] = mapSymbol == '#' ? WALL : OPEN;
            if (mapSymbol == 'S') StartPosition = p;
            if (mapSymbol == 'E') EndPosition = p;
        }
    }

    private bool TestStep(Point2D cursor, Point2D nextStep, out int numSteps)
    {
        numSteps = Point2D.TaxiDistance2D(cursor, nextStep);
        if (_theMap.TryGetValue(nextStep, out var mapStep) && mapStep == WALL) return false;
        return true;
    }

    static private IEnumerable<Point2D> NextSteps(Point2D cursor)
    {
        return cursor.GetOrthogonalNeighbors();
    }

    static private int Heuristic(Point2D a, Point2D b) => Point2D.TaxiDistance2D(a, b);
    
    public void CheckGlitch(Point2D source, int maxGlitchDist)
    {
        const int TARGET_REDUCTION = 100;
        _doGlitch = true;
        int baseSteps = CleanPath[source] - 1; // minus one to fix the offset.

        for (int y = maxGlitchDist; y >= -maxGlitchDist; y--)
        {
            int xOffset = maxGlitchDist - Math.Abs(y);
            for (int x = -xOffset; x <= xOffset; x++)
            {
                Point2D dest = new(x + source.X, y + source.Y);
                if (_theMap.TryGetValue(dest, out int mapValue) && 
                    mapValue == OPEN && 
                    CleanPath.TryGetValue(dest, out int cleanSteps))
                {
                    int numGlitchSteps = baseSteps + Point2D.TaxiDistance2D(source, dest) + (CleanPath.Count - cleanSteps);

                    if ((NumSteps - numGlitchSteps) >= TARGET_REDUCTION)
                    {
                        GlitchPaths.TryAdd((source, dest), numGlitchSteps);
                    }
                }
                else
                {
                    //This never happened, so this code path was never developed.
                    Console.WriteLine($"Glitch off the core path. {dest}");
                }
            }
        }

        _doGlitch = false;
    }

    public bool A_Star() => A_Star(StartPosition, EndPosition);

    public bool A_Star(Point2D start, Point2D end)
    {
        _stepCounter.Clear();

        PriorityQueue<Point2D, int> searchQueue = new(); //we enque based on fScore + h, the distance travelled, plus taxi distance guess to destination.
        HashSet<Point2D> inSearchQueue = []; //we add this because we don't have a way to query the queue to see if a specific item is in it.

        int gScore = 0; //gScore is value of the path from start to here
        _stepCounter.Add(start, (gScore, Heuristic(start, end), null));

        searchQueue.Enqueue(start, _stepCounter[start].fScore);
        inSearchQueue.Add(start);
        while (searchQueue.TryDequeue(out Point2D cursor, out _))
        {
            inSearchQueue.Remove(cursor);
            
            if(_doGlitch)
            {
                //This never was developed. Never found a glitch that went off the happy path.
            }
            else
            {
                //We have arrived!
                if (cursor == end)
                {
                    NumSteps = _stepCounter[cursor].gScore;

                    //unroll our history. 
                    CleanPath.Add(cursor, NumSteps);
                    Point2D? p = _stepCounter[cursor].parent;

                    while (p != null)
                    {
                        CleanPath.Add((Point2D)p, NumSteps - CleanPath.Count);
                        p = _stepCounter[(Point2D)p].parent;
                    }
                    return true;
                }
            }

            foreach (Point2D nextStep in NextSteps(cursor))
            {
                //bounds and valid move check. 
                if(!TestStep(cursor, nextStep, out int dist)) continue;
                _stepCounter.TryAdd(nextStep, (int.MaxValue, int.MaxValue, null));

                //tentative_gScore := gScore[current] + d(current, neighbor)
                int t_gScore = _stepCounter[cursor].gScore + dist;

                //if tentative_gScore < gScore[neighbor]
                if (t_gScore < _stepCounter[nextStep].gScore)
                {
                    //cameFrom[neighbor] := current
                    //gScore[neighbor] := tentative_gScore
                    //fScore[neighbor] := tentative_gScore + h(neighbor)
                    _stepCounter[nextStep] = (t_gScore, t_gScore + Heuristic(cursor, end), cursor);

                    //if neighbor not in openSet openSet.add(neighbor) 
                    if (!inSearchQueue.Contains(nextStep))
                    {
                        searchQueue.Enqueue(nextStep, _stepCounter[nextStep].fScore);
                        inSearchQueue.Add(nextStep);
                    }
                }
            }
        }
        NumSteps = -1;
        return false;
    }

    public void DisplayMap() => DisplayMap(CleanPath.Keys.ToList());

    public void DisplayMap(List<Point2D> drawPath)
    {
        HashSet<Point2D> hs = new(drawPath);

        for (int y = MapMin.Y; y <= MapMax.Y; y++)
        {
            for (int x = MapMin.X; x <= MapMax.X; x++)
            {
                Point2D cursor = new(x, y);

                if (_theMap.TryGetValue(cursor, out int value))
                {
                    if (hs.Contains(cursor)) Console.ForegroundColor = ConsoleColor.Red;

                    if (cursor == StartPosition) Console.ForegroundColor = ConsoleColor.Green;
                    if (cursor == EndPosition) Console.ForegroundColor = ConsoleColor.Blue;

                    if (value == 1) Console.Write('#');
                    if (value == 0) Console.Write('.');
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("?");

                }
                Console.ResetColor();
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}
