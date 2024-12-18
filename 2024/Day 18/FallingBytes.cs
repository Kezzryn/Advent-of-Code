namespace AoC_2024_Day_18;
using BKH.BaseMap;
using BKH.Geometry;

internal class FallingBytes: BaseMap
{
    const int WALL = 1;
    const int OPEN = 0;

    public FallingBytes(string[] puzzleInput, int numPoints, int maxRange)
    {
        var l = puzzleInput.Take(numPoints).Select(x => new Point2D(x.Split(',').Select(int.Parse).ToArray())).ToHashSet();

        foreach ((Point2D p, int v) in from y in Enumerable.Range(0, maxRange)
                                        from x in Enumerable.Range(0, maxRange)
                                        select (new Point2D(x,y), l.Contains(new(x,y)) ? WALL : OPEN))
        {
            _theMap[p] = v;
        }
        MaxSteps = maxRange * maxRange;
        StartPosition = MapMin;
        EndPosition = MapMax;
    }

    public void AddMapBlock(string s)
    {
        Point2D p = new(s.Split(',').Select(int.Parse).ToArray());
        _theMap[p] = WALL;
        cache.Clear();
    }

    protected override int TestStep(Point2D cursor, Point2D nextStep)
    {
        stepCounter.TryAdd(nextStep, (int.MaxValue, int.MaxValue, null));
        
        if (_theMap.TryGetValue(nextStep, out var v) && v == OPEN) return 1;

        return -1;
    }

    protected override IEnumerable<Point2D> NextSteps(Point2D cursor)
    {
        return cursor.GetOrthogonalNeighbors();
    }
}
