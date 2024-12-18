namespace BKH.BaseMap;
using BKH.Geometry;

public class BaseMap
{
    // this serves as our reference starting position for everything. 
    protected readonly Dictionary<Point2D, int> _theMap = [];

    //the path after an A_Star call. 
    public List<Point2D> FinalPath = [];

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

    protected Dictionary<Point2D, (int gScore, int fScore, Point2D? parent)> stepCounter = [];

    protected readonly Dictionary<(Point2D, Point2D), int> cache = [];

    public BaseMap(string[] puzzleInput)
    {
        LoadMap(puzzleInput);
    }
    public BaseMap() { }

    protected virtual void LoadMap(string[] puzzleInput)
    {
        throw new NotImplementedException();
    }

    protected virtual int TestStep(Point2D cursor, Point2D nextStep)
    {
        throw new NotImplementedException();
    }

    protected virtual IEnumerable<Point2D> NextSteps(Point2D cursor)
    {
        throw new NotImplementedException();
    }

    protected virtual int Heuristic(Point2D a, Point2D b)
    {
        return Point2D.TaxiDistance2D(a, b);
    }

    public bool A_Star() => A_Star(StartPosition, EndPosition);
    
    public bool A_Star(Point2D start, Point2D end)
    {
        FinalPath.Clear();
        stepCounter.Clear();

        if(cache.TryGetValue((start, end), out int cacheValue))
        {
            NumSteps = cacheValue;
            return true;
        }

        PriorityQueue<Point2D, int> searchQueue = new(); //we enque based on fScore + h, the distance travelled, plus taxi distance guess to destination.
        HashSet<Point2D> inSearchQueue = []; //we add this because we don't have a way to query the queue to see if a specific item is in it.

        int gScore = 0; //gScore is value of the path from start to here
        stepCounter.Add(start, (gScore, Heuristic(start, end), null));

        searchQueue.Enqueue(start, stepCounter[start].fScore);
        inSearchQueue.Add(start);

        while (searchQueue.TryDequeue(out Point2D cursor, out int priority))
        {
            inSearchQueue.Remove(cursor);

            //We have arrived!
            if (cursor == end)
            {
                NumSteps = stepCounter[cursor].gScore;

                cache[(start, end)] = NumSteps;
                cache[(end, start)] = NumSteps;

                //unroll our history. 
                FinalPath.Add(cursor);
                Point2D? p = stepCounter[cursor].parent;

                while (p != null)
                {
                    FinalPath.Add((Point2D)p);
                    p = stepCounter[(Point2D)p].parent;
                }
               
                return true;
            }

            foreach (Point2D nextStep in NextSteps(cursor))
            {
                //bounds and valid move check. 
                int dist = TestStep(cursor, nextStep);
                if (dist == -1) continue;

                //tentative_gScore := gScore[current] + d(current, neighbor)
                int t_gScore = stepCounter[cursor].gScore + dist;

                //if tentative_gScore < gScore[neighbor]
                if (t_gScore < stepCounter[nextStep].gScore)
                {
                    //cameFrom[neighbor] := current
                    //gScore[neighbor] := tentative_gScore
                    //fScore[neighbor] := tentative_gScore + h(neighbor)
                    stepCounter[nextStep] = (t_gScore, t_gScore + Heuristic(cursor, end), cursor);

                    //if neighbor not in openSet openSet.add(neighbor) 
                    if (!inSearchQueue.Contains(nextStep))
                    {
                        searchQueue.Enqueue(nextStep, stepCounter[nextStep].fScore);
                        inSearchQueue.Add(nextStep);
                    }
                }
            }
        }
        NumSteps = -1;
        return false;
    }

    public void DisplayMap() => DisplayMap(FinalPath);

    public void DisplayMap(List<Point2D> drawPath)
    {
        HashSet<Point2D> hs = new(drawPath);

        for (int y = MapMin.Y; y <= MapMax.Y; y++)
        {
            for (int x = MapMin.X; x <= MapMax.X; x++)
            {
                Point2D cursor = new(x,y);

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
