namespace BKH.BaseMap;
using BKH.Geometry;

public class BaseMap3D
{
    // this serves as our reference starting position for everything. 
    protected readonly Dictionary<Point3D, int> _theMap = [];

    //the path after an A_Star call. 
    public List<Point3D> FinalPath = [];

    //as found on the map.
    public Point3D StartPosition { get; set; }
    public Point3D EndPosition { get; set; }

    public Point3D MapMin { get { return 
                new Point3D(_theMap.Keys.Select(k => k.X).Min(), 
                            _theMap.Keys.Select(k => k.Y).Min(), 
                            _theMap.Keys.Select(k => k.Z).Min()); } }
    public Point3D MapMax { get { return 
                new Point3D(_theMap.Keys.Select(k => k.X).Max(),
                            _theMap.Keys.Select(k => k.Y).Max(),
                            _theMap.Keys.Select(k => k.Z).Max()); } }

    public int MaxSteps { get; set; } = 0;
    public int NumSteps { get; set; } = 0;

    protected Dictionary<Point3D, (int gScore, int fScore, Point3D? parent)> stepCounter = [];

    private readonly Dictionary<(Point3D, Point3D), int> cache = [];

    public BaseMap3D(string[] puzzleInput)
    {
        LoadMap(puzzleInput);
    }
    public BaseMap3D() { }

    protected virtual void LoadMap(string[] puzzleInput)
    {
        throw new NotImplementedException();
    }

    protected virtual int TestStep(Point3D cursor, Point3D nextStep)
    {
        throw new NotImplementedException();
    }

    protected virtual IEnumerable<Point3D> NextSteps(Point3D cursor)
    {
        throw new NotImplementedException();
    }

    protected virtual int Heuristic(Point3D a, Point3D b)
    {
        return Point3D.TaxiDistance3D(a, b);
    }

    protected virtual bool IsAtExit(Point3D a)
    {
        return a == EndPosition;
    }

    public bool A_Star() => A_Star(StartPosition, EndPosition);

    public bool A_Star(Point3D start, Point3D end)
    {
        FinalPath.Clear();
        stepCounter.Clear();

        if (cache.TryGetValue((start, end), out int cacheValue))
        {
            NumSteps = cacheValue;
            return true;
        }

        PriorityQueue<Point3D, int> searchQueue = new(); //we enque based on fScore + h, the distance travelled, plus taxi distance guess to destination.
        HashSet<Point3D> inSearchQueue = []; //we add this because we don't have a way to query the queue to see if a specific item is in it.

        int gScore = 0; //gScore is value of the path from start to here
        stepCounter.Add(start, (gScore, Heuristic(start, end), null));

        searchQueue.Enqueue(start, stepCounter[start].fScore);
        inSearchQueue.Add(start);

        while (searchQueue.TryDequeue(out Point3D cursor, out int priority))
        {
            inSearchQueue.Remove(cursor);

            //We have arrived!
            if (IsAtExit(cursor))
            {
                NumSteps = stepCounter[cursor].gScore;

                cache[(start, end)] = NumSteps;
                cache[(end, start)] = NumSteps;

                //unroll our history. 
                FinalPath.Add(cursor);
                Point3D? p = stepCounter[cursor].parent;

                while (p != null)
                {
                    FinalPath.Add((Point3D)p);
                    p = stepCounter[(Point3D)p].parent;
                }

                return true;
            }

            foreach (Point3D nextStep in NextSteps(cursor))
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

    //public void DisplayMap() => DisplayMap(FinalPath);

    //public void DisplayMap(List<Point3D> drawPath)
    //{
    //    Point3D cursor = new();

    //    for (int y = MapMax.Y; y >= MapMin.Y; y--)
    //    {
    //        for (int x = MapMin.X; x <= MapMax.X; x++)
    //        {
    //            cursor.X = x;
    //            cursor.Y = y;

    //            if (_theMap.TryGetValue(cursor, out int value))
    //            {
    //                if (drawPath.Contains(cursor)) Console.ForegroundColor = ConsoleColor.DarkMagenta;
    //                if (cursor == StartPosition) Console.ForegroundColor = ConsoleColor.Green;
    //                if (cursor == EndPosition) Console.ForegroundColor = ConsoleColor.Blue;

    //                Console.Write((char)(value + 96));

    //                if (Console.ForegroundColor != ConsoleColor.Gray) Console.ForegroundColor = ConsoleColor.Gray;
    //            }
    //            else
    //            {
    //                Console.ForegroundColor = ConsoleColor.Red;
    //                Console.Write("?");
    //                Console.ForegroundColor = ConsoleColor.Gray;
    //            }
    //        }
    //        Console.WriteLine(" ");
    //    }
    //}
}
