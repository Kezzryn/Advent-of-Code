namespace AoC_2016_Day_13;
using BKH.BaseMap;
using BKH.Geometry;

using System.Numerics;

internal class Map : BaseMap
{
    private const int WALL = 1;
    private const int AIR = 0;

    // puzzle magic number for maze generation.
    private readonly int _designer_number = 0;

    private int GenMapPoint(Point2D p) => GenMapPoint(p.X, p.Y);

    private int GenMapPoint(int x, int y) => int.IsEvenInteger(BitOperations.PopCount((uint)((x * x) + (3 * x) + (2 * x * y) + y + (y * y) + _designer_number))) ? AIR : WALL;

    public bool IsValidMapPoint(int x, int y) => IsValidMapPoint(new Point2D(x, y));
    public bool IsValidMapPoint(Point2D point)
    {
        if (!_theMap.TryGetValue(point, out int mapValue))
        {
            mapValue = GenMapPoint(point);
            _theMap[point] = mapValue;
        }
        return mapValue == AIR;
    }

    public Map(int designerNumber, Point2D defaultStart, Point2D defaultEnd)
    {
        _designer_number = designerNumber;
        _theMap.Add(defaultStart, GenMapPoint(defaultStart));
        _theMap.Add(defaultEnd, GenMapPoint(defaultEnd));
        StartPosition = defaultStart;
        EndPosition = defaultEnd;
    }

    protected override int TestStep(Point2D cursor, Point2D nextStep)
    {
        //Don't move to ourself. 
        if (cursor == nextStep) return -1;

        //we're off the map
        if (nextStep.X < 0 || nextStep.Y < 0) return -1;

        //we'll exceed our step count (if applicable) 
        if (MaxSteps != 0 && stepCounter[cursor].gScore + 1 >= MaxSteps) return -1;

        // check our map coordinate for validity.
        if (!IsValidMapPoint(nextStep.X, nextStep.Y)) return -1; 
            
        stepCounter.TryAdd(nextStep, (int.MaxValue, int.MaxValue, null));
        return 1;
    }

    protected override IEnumerable<Point2D> NextSteps(Point2D cursor)
    {
        return cursor.GetOrthogonalNeighbors();
    }
}
