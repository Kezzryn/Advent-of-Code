namespace AoC_2024_Day_14;
using BKH.Geometry;

internal class Roomba
{
    private int MAX_WIDTH = -1;
    private int MAX_HEIGHT = -1;
    public Point2D Position { get; private set; }
    public Point2D Velocity {get; private set; }

    public Roomba(string data, int maxWidth, int maxHeight)
    {
        MAX_WIDTH = maxWidth;
        MAX_HEIGHT = maxHeight;
        //p=53,89 v=-54,-83
        List<int> split = data.Split("=, ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(x => int.TryParse(x, out _))
                .Select(int.Parse)
                    .ToList();

        Position = new(split[0], split[1]);
        Velocity = new(split[2], split[3]);
    }

    private static int FindOffset(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }

    public void Step(int numSteps = 1)
    {
        int newX = Position.X;
        int newY = Position.Y;
        int velX = numSteps > 0 ? Velocity.X : Velocity.X * -1;
        int velY = numSteps > 0 ? Velocity.Y : Velocity.Y * -1;

        for (int i = 0; i < Math.Abs(numSteps); i++)
        {
            newX = FindOffset(newX + velX, MAX_WIDTH);
            newY = FindOffset(newY + velY, MAX_HEIGHT);

        }
        Position = new(newX, newY);
    }
}
