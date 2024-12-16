using BKH.Geometry;

namespace AoC_2024_Day_15;

internal class WarehouseOne
{
    private const int OPEN = 0;
    private const int WALL = 1;
    private const int BOX = 2;

    private Point2D robot = new();
    private readonly Dictionary<Point2D, int> theMap = [];
    private readonly List<Point2D.Direction> moves = [];

    public WarehouseOne(string[] mapData, List<Point2D.Direction> moveData)
    {
        foreach (Point2D p in from y in Enumerable.Range(0, mapData.Length)
                                   from x in Enumerable.Range(0, mapData[0].Length)
                                   select new Point2D(x, y))
        {
            char mapObject = mapData[p.Y][p.X];
            if (mapObject == '@') robot = p;
            theMap[p] = mapObject switch
            {
                '#' => WALL,
                'O' => BOX,
                _ => OPEN
            };
        }

        moves = moveData;
    }
    
    private Point2D FindOpenSpace(Point2D pos, Point2D.Direction dir)
    {
        Point2D p = pos.OrthogonalNeighbor(dir);
        while (theMap.TryGetValue(p, out int tileValue))
        {
            if (tileValue == OPEN) return p;
            if (tileValue == WALL) return new(-1, -1);
            p = p.OrthogonalNeighbor(dir);
        }
        return new(-1, -1);
    }

    private bool PushBoxes(Point2D.Direction moveDir)
    {
        Point2D openPos = FindOpenSpace(robot, moveDir);
        if (openPos.X == -1) return false;

        Point2D boxPos = robot.OrthogonalNeighbor(moveDir);

        (theMap[boxPos], theMap[openPos]) = (theMap[openPos], theMap[boxPos]);

        return true;
    }

    public void Run()
    {
        foreach (Point2D.Direction moveDir in moves)
        {
            Point2D nextStep = robot.OrthogonalNeighbor(moveDir);
            int nextTile = theMap[nextStep];
            if (nextTile == OPEN || (nextTile == BOX && PushBoxes(moveDir))) robot = nextStep;
        }
    }

    public long ScoreMap() => theMap.Where(x => x.Value == BOX).Sum(x => x.Key.X + (100 * x.Key.Y));

    public void PrintMap()
    {
        int maxX = theMap.Keys.Max(x => x.X);
        int maxY = theMap.Keys.Max(x => x.Y);

        for (int y = 0; y <= maxY; y++)
        {
            Console.Write($"{y + 1,-4}");
            for (int x = 0; x <= maxX; x++)
            {
                Point2D p = new(x, y);
                if (p == robot)
                    Console.Write('@');
                else
                    Console.Write((theMap[p] == OPEN ? '.' : theMap[p] == WALL ? '#' : 'O'));
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}
