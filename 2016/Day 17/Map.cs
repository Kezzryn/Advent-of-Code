namespace AoC_2016_Day_17;
using BKH.Geometry;
using BKH.BaseMap;
using System.Security.Cryptography;
using System.Text;

internal class Map : BaseMap3D
{
    private readonly Dictionary<Point3D, string> _history = [];     // Step tracker.
    private readonly Dictionary<string, List<bool>> _MD5HashHistory = []; // MD5 hash results cache.
    public string History { get; protected set; } = String.Empty;
    private readonly string _secretHash;

    public Map(string puzzleInput)
    {
        for (int x = 0; x < 4; x++)
            for (int y = 0; y < 4; y++)
                _theMap.Add(new Point3D(x,y,0), 1);

        _secretHash = puzzleInput;

        //Using the Z axis in the step counters, so we can visit the "same" room again.
        StartPosition = new(0, 3, 0);   //upper left, to bottom right. 
        EndPosition = new(3, 0, 0);
    }

    protected override int TestStep(Point3D current, Point3D nextPoint)
    {
        // Off the map
        if (!_theMap.TryGetValue(new(nextPoint.As2D()), out int stepLength)) return -1;
        if (!_history.TryGetValue(current, out string? historyValue)) historyValue = String.Empty;

        string dirValue = String.Empty;
        if (IsDoor(historyValue, Direction.RIGHT) && current.X < nextPoint.X) dirValue = "R";
        if (IsDoor(historyValue, Direction.LEFT)  && current.X > nextPoint.X) dirValue = "L";
        if (IsDoor(historyValue, Direction.UP)    && current.Y < nextPoint.Y) dirValue = "U";
        if (IsDoor(historyValue, Direction.DOWN)  && current.Y > nextPoint.Y) dirValue = "D";
        if (dirValue == String.Empty) return -1;

        if (!_history.TryAdd(nextPoint, $"{historyValue}{dirValue}"))
        {
            string temp = $"{historyValue}{dirValue}";
            if (temp.Length > _history[nextPoint].Length) _history[nextPoint] = temp;
        }

        stepCounter.TryAdd(nextPoint, (int.MaxValue, int.MaxValue, null));
        return stepLength;
    }

    protected override int Heuristic(Point3D a, Point3D b)
    {
        return Point3D.TaxiDistance2D(a, b);
    }

    protected override bool IsAtExit(Point3D a)
    {
        if (a.AsPoint2D() == EndPosition.AsPoint2D())
        {
            History = _history[a];
            return true;
        }
        return false; 
    }

    protected override IEnumerable<Point3D> NextSteps(Point3D cursor)
    {
        foreach(Point2D n in cursor.AsPoint2D().GetOrthogonalNeighbors())
        {
            yield return new Point3D(n.X, n.Y, cursor.Z + 1);
        }
    }

    private bool IsDoor(string locationKey, Direction direction)
    {
        string hashKey = $"{_secretHash}{locationKey}";
        if(_MD5HashHistory.TryGetValue(hashKey, out List<bool>? history)) 
        {
            return history[(int)direction];
        }

        byte[] hashdata = MD5.HashData(Encoding.UTF8.GetBytes(hashKey));
        var sBuilder = new StringBuilder(); // Create a new Stringbuilder to collect the bytes and create a string.

        // Loop through each byte of the hashed data and format each one as a hexadecimal string.
        for (int i = 0; i < 4; i++)
        {
            sBuilder.Append(hashdata[i].ToString("x2"));
        }

        List<bool> dirs = sBuilder.ToString().Select("bcdef".Contains).ToList(); // Return the hexadecimal string.
        _MD5HashHistory.TryAdd(locationKey, new(dirs));

        return _MD5HashHistory[locationKey][(int)direction];
    }

    public int A_Slow()
    {
        int maxSteps = -1;
        Dictionary<Direction, char> neighbors = new()
        { 
            { Direction.UP, 'U' },
            { Direction.DOWN, 'D' },
            { Direction.LEFT, 'L' },
            { Direction.RIGHT, 'R' },
        };

        Queue<string> searchQueue = new(); 

        searchQueue.Enqueue(string.Empty);

        while (searchQueue.TryDequeue(out string? cursor))
        {
            if (GetPointFromHistory(cursor) == EndPosition.AsPoint2D())
            {
                if (cursor.Length > maxSteps) maxSteps = cursor.Length;
            }
            else
            {
                foreach ((Direction dir, char neighbor) in neighbors)
                {
                    string nextStep = cursor + neighbor;
                    // Off the map
                    if (_theMap.TryGetValue(GetPointFromHistory(nextStep), out _) && IsDoor(cursor, dir))
                    {
                        searchQueue.Enqueue(nextStep);
                    }
                }
            }
        }
        return maxSteps;
    }

    private Point3D GetPointFromHistory(string history)
    {
        //Coverts a history string into a point.
        //Used in A_Slow to test if we've hit the end.
        int x = StartPosition.X;
        int y = StartPosition.Y;

        foreach(var gb in history.ToCharArray().GroupBy(x => x))
        {
            if (gb.Key == 'U') y += gb.Count();
            if (gb.Key == 'D') y -= gb.Count();
            if (gb.Key == 'R') x += gb.Count();
            if (gb.Key == 'L') x -= gb.Count();
        }

        return new Point3D(x, y, 0);
    }

    private enum Direction
    {
        UP = 0,
        DOWN = 1,
        LEFT = 2,
        RIGHT = 3
    }
}