using BKH.Geometry;
namespace AoC_2024_Day_15;

internal class WarehouseTwo
{
    private const int OPEN = 0;
    private const int WALL = 1;
    private const int BOX = 2;

    private Point2D robot = new();
    private readonly Dictionary<Point2D, int> theMap;
    private readonly List<Point2D.Direction> moves;

    public WarehouseTwo(string[] mapData, List<Point2D.Direction> moveData)
    {

        theMap = [];
        moves = moveData;

        foreach ((int X, int Y) in from y in Enumerable.Range(0, mapData.Length)
                                   from x in Enumerable.Range(0, mapData[0].Length)
                                   select (x, y))
        {
            char mapObject = mapData[Y][X];
            Point2D p = new(X * 2, Y);
            if (mapObject == '@') robot = p;
            theMap[p] = mapObject switch
            {
                '#' => WALL,
                'O' => BOX,
                '.' => OPEN,
                '@' => OPEN,
                _ => throw new NotImplementedException()
            };
            theMap[p.OrthogonalNeighbor(Point2D.Direction.Right)] = mapObject == '#' ? WALL : OPEN;
        }
    }

    private bool CanPushBox(Point2D box, Point2D.Direction moveDir, HashSet<Point2D> movingBoxes)
    {
        movingBoxes.Add(box);
        Point2D nextStep = box.OrthogonalNeighbor(moveDir);
        Point2D nextStepLeft = nextStep.OrthogonalNeighbor(Point2D.Direction.Left);
        Point2D nextStepRight = nextStep.OrthogonalNeighbor(Point2D.Direction.Right);
        if (theMap.TryGetValue(nextStep, out int nextTile) &&
          theMap.TryGetValue(nextStepLeft, out int nextTileLeft) &&
          theMap.TryGetValue(nextStepRight, out int nextTileRight))
        {
            if (moveDir == Point2D.Direction.Up || moveDir == Point2D.Direction.Down)
            {
                return (nextTileLeft, nextTile, nextTileRight) switch
                {
                    (_, WALL, _) or (_, _, WALL) => false,
                    (_, OPEN, OPEN) and not (BOX, _, _) => true,
                    (BOX, _, BOX) => CanPushBox(nextStepLeft, moveDir, movingBoxes) &&
                                     CanPushBox(nextStepRight, moveDir, movingBoxes),
                    (BOX, _, _) => CanPushBox(nextStepLeft, moveDir, movingBoxes),
                    (_, BOX, _) => CanPushBox(nextStep, moveDir, movingBoxes),
                    (_, _, BOX) => CanPushBox(nextStepRight, moveDir, movingBoxes),
                    _ => throw new NotImplementedException(),
                };
            }
            else
            {
                if (moveDir == Point2D.Direction.Right)
                {
                    return nextTileRight switch
                    {
                        WALL => false,
                        OPEN => true,
                        BOX => CanPushBox(nextStepRight, moveDir, movingBoxes),
                        _ => throw new NotImplementedException()
                    };
                }
                if (moveDir == Point2D.Direction.Left) {
                    return (nextTileLeft, nextTile) switch
                    {
                        (BOX, _) => CanPushBox(nextStepLeft, moveDir, movingBoxes),
                        (_, WALL) => false,
                        (_, OPEN) => true,
                        _ => throw new NotImplementedException($"{moveDir}, {nextTileLeft}, {nextTile}, {nextTileRight}")
                    };
                } 
            }
        }
        return false;
    }

    private bool PushBoxes(Point2D firstBox, Point2D.Direction moveDir)
    {
        HashSet<Point2D> boxes = [];
        if (!CanPushBox(firstBox, moveDir, boxes)) return false;
        
        IEnumerable<Point2D> boxOrder = moveDir switch 
        {
            Point2D.Direction.Up => boxes.OrderByDescending(x => x.Y),
            Point2D.Direction.Down => boxes.OrderBy(x => x.Y),
            _ => boxes
        };

        foreach (Point2D box in boxOrder)
        {
            Point2D swap = box.OrthogonalNeighbor(moveDir);
            (theMap[box], theMap[swap]) = (theMap[swap], theMap[box]);
        }
        return true;
    }

    public long ScoreMap() => theMap.Where(x => x.Value == BOX).Sum(x => x.Key.X + (100 * x.Key.Y));

    public void Run()
    {
        foreach (Point2D.Direction moveDir in moves)
        {
            Point2D nextStep = robot.OrthogonalNeighbor(moveDir);
            Point2D nextStepLeft = nextStep.OrthogonalNeighbor(Point2D.Direction.Left);

            if (theMap.TryGetValue(nextStep, out int nextTile) && 
                theMap.TryGetValue(nextStepLeft, out int nextTileLeft))
            {
                switch (nextTileLeft, nextTile)
                {
                    case (_, WALL):
                        //do nothing.
                        break;
                    case (BOX, _):
                        if (PushBoxes(nextStepLeft, moveDir)) robot = nextStep;
                        break;
                    case (_, BOX):
                        if(PushBoxes(nextStep, moveDir)) robot = nextStep;
                        break;
                    case (_, OPEN):
                        robot = nextStep;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }

    public void PrintMap(Point2D.Direction? dir = null)
    {
        char movechar = dir switch
        {
            Point2D.Direction.Up => 'v',
            Point2D.Direction.Down => '^',
            Point2D.Direction.Left => '<',
            Point2D.Direction.Right => '>',
            _ => '@'
        };

        int maxX = theMap.Keys.Max(x => x.X);
        int maxY = theMap.Keys.Max(x => x.Y);

        for (int y = 0; y <= maxY; y++)
        {
            Console.Write($"{y + 1,-4}");
            for (int x = 0; x <= maxX; x++)
            {
                Point2D p = new(x, y);
                if (p == robot)
                    Console.Write(movechar);
                else
                {
                    if (theMap[p] == OPEN) Console.Write('.');
                    if (theMap[p] == WALL) Console.Write('#');
                    if (theMap[p] == BOX)
                    {
                        Console.Write("[]");
                        x++;
                    }
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}
