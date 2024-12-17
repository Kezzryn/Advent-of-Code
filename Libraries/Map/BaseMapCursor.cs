namespace BKH.BaseMap;
using BKH.Geometry;

public class BaseMapCursor
{
    // this serves as our reference starting position for everything. 
    protected readonly Dictionary<Point2D, int> _theMap = [];

    //the path after an A_Star call. 
    public List<Point2D> FinalPath = [];

    //as found on the map.
    public required Cursor StartPosition { get; set; }
    public Point2D EndPosition { get; set; }

    public Point2D MapMin { get { return new Point2D(_theMap.Keys.Select(k => k.X).Min(), _theMap.Keys.Select(k => k.Y).Min()); } }
    public Point2D MapMax { get { return new Point2D(_theMap.Keys.Select(k => k.X).Max(), _theMap.Keys.Select(k => k.Y).Max()); } }

    /// <summary>
    /// The maximum number of steps to search.
    /// 0 is unlimited steps.
    /// </summary>
    public int MaxSteps { get; set; } = 0;
    public int NumSteps { get; set; } = 0;

    protected Dictionary<Cursor, (int gScore, int fScore, Cursor? parent)> stepCounter = [];

    private readonly Dictionary<(Point2D, Point2D), int> cache = [];

    public BaseMapCursor(string[] puzzleInput)
    {
        LoadMap(puzzleInput);
    }
    public BaseMapCursor() { }

    protected virtual void LoadMap(string[] puzzleInput)
    {
        throw new NotImplementedException();
    }

    protected virtual bool TestStep(Cursor nextStep)
    {
        //dont forget to add to the step counter here.
        throw new NotImplementedException();
    }

    protected virtual List<(Cursor, int)> NextSteps(Cursor cursor)
    {
        throw new NotImplementedException();
    }

    protected virtual int Heuristic(Cursor a, Point2D b)
    {
        return Point2D.TaxiDistance2D(a.XYAsPoint2D, b);
    }

    protected virtual int Heuristic(Point2D a, Point2D b)
    {
        return Point2D.TaxiDistance2D(a, b);
    }

    public bool A_Star() => A_Star(StartPosition, EndPosition);
    
    //used in 2024 Day 16
    protected virtual bool TestGScore(int t_gScore, int gScore)
    {
        return t_gScore < gScore;
    }
    public void ClearCache()  => cache.Clear();
  
    public bool A_Star(Cursor start, Point2D end)
    {
        FinalPath.Clear();
        stepCounter.Clear();

        if(cache.TryGetValue((start.XYAsPoint2D, end), out int cacheValue))
        {
            NumSteps = cacheValue;
            return true;
        }

        PriorityQueue<Cursor, int> searchQueue = new(); //we enque based on fScore + h, the distance travelled, plus taxi distance guess to destination.
        HashSet<Cursor> inSearchQueue = []; //we add this because we don't have a way to query the queue to see if a specific item is in it.

        int gScore = 0; //gScore is value of the path from start to here
        stepCounter.Add(start, (gScore, Heuristic(start.XYAsPoint2D, end), null));

        searchQueue.Enqueue(start, stepCounter[start].fScore);
        inSearchQueue.Add(start);

        while (searchQueue.TryDequeue(out Cursor? cursor, out int priority))
        {
            inSearchQueue.Remove(cursor);

            //We have arrived!
            if (cursor.XYAsPoint2D == end)
            {
                NumSteps = stepCounter[cursor].gScore;

                cache[(start.XYAsPoint2D, end)] = NumSteps;
                cache[(end, start.XYAsPoint2D)] = NumSteps;

                //unroll our history. 
                FinalPath.Add(cursor.XYAsPoint2D);
                Cursor? p = stepCounter[cursor].parent;

                while (p != null)
                {
                    FinalPath.Add(p.XYAsPoint2D);
                    p = stepCounter[p].parent;
                }
               
                return true;
            }

            foreach ((Cursor nextStep, int dist) in NextSteps(cursor))
            {
                //bounds and valid move check. 
                if (!TestStep(nextStep)) continue;
                //int dist = TestStep(cursor, nextStep);
                //if (dist == -1) continue;

                //tentative_gScore := gScore[current] + d(current, neighbor)
                int t_gScore = stepCounter[cursor].gScore + dist;

                //if tentative_gScore < gScore[neighbor]
                if (TestGScore(t_gScore, stepCounter[nextStep].gScore)) 
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

    protected virtual char DisplayMapSymbol(int mapValue)
    {
        throw new NotImplementedException();
    }

    public void DisplayMap(List<Point2D> drawPath)
    {
        for (int y = MapMax.Y; y >= MapMin.Y; y--)
        {
            for (int x = MapMin.X; x <= MapMax.X; x++)
            {
                Point2D cursor = new(x,y);

                if (_theMap.TryGetValue(cursor, out int value))
                {
                    if (cursor == StartPosition.XYAsPoint2D)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write('S');
                    }
                    else if (cursor == EndPosition)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write('E');
                    }
                    else if (drawPath.Contains(cursor))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.Write('X');
                    }
                    else
                    {
                        Console.Write(DisplayMapSymbol(value));
                    }
                    
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("?");
                    Console.ResetColor();
                }
            }
            Console.WriteLine("");
        }
    }
}
