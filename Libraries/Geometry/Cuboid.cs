namespace BKH.Geometry;
public class Cuboid(int[] low, int[] high)
{
    //Lower, forward and left. Should always be same or less than the 2nd point.
    public int X1 { get; private set; } = low[0];
    public int Y1 { get; private set; } = low[1];
    public int Z1 { get; private set; } = low[2];
    public int X2 { get; private set; } = high[0];
    public int Y2 { get; private set; } = high[1];
    public int Z2 { get; private set; } = high[2];

    public long Volume { get { return (long)(X2 - X1 + 1) * (long)(Y2 - Y1 + 1) * (long)(Z2 - Z1 + 1); } }

    public Cuboid(Cuboid o) : this([o.X1, o.Y1, o.Z1], [o.X2, o.Y2, o.Z2]) { }

    public bool Contains(Cuboid other) => (X1 <= other.X1 && X2 >= other.X2) &&
                                  (Y1 <= other.Y1 && Y2 >= other.Y2) &&
                                  (Z1 <= other.Z1 && Z2 >= other.Z2);

    public bool ContainedBy(Cuboid other) => other.Contains(this);

    public List<Cuboid> Exclude(Cuboid other)
    {
        // I had a chopping system that worked, but cut into 27 bits, which still had issues. The solution here guided me, however, I still had to debug an edge case where if the two blocks started or ended on the same coordinate the logic would invent a new incorrect edge.
        // https://www.reddit.com/r/adventofcode/comments/rlxhmg/comment/hpv4sjl/

        if (ContainedBy(other)) return [];

        List<Cuboid> returnValue = [];
        Cuboid current = new(this);

        // ccc 
        //ooo
        // Cut right side off, if one exists. 
        if (current.X1 <= other.X2 && other.X2 < current.X2)
        {
            returnValue.Add(new([other.X2 + 1, current.Y1, current.Z1], [current.X2, current.Y2, current.Z2]));
            current = new([current.X1, current.Y1, current.Z1], [other.X2, current.Y2, current.Z2]);
        }

        //ccc 
        // ooo
        // Cut left side off, if one exists. 
        if (current.X1 < other.X1 && other.X1 <= current.X2)
        {
            returnValue.Add(new([current.X1, current.Y1, current.Z1], [other.X1 - 1, current.Y2, current.Z2]));
            current = new([other.X1, current.Y1, current.Z1], [current.X2, current.Y2, current.Z2]);
        }

        // Do it again, but for Y.
        if (current.Y1 <= other.Y2 && other.Y2 < current.Y2)
        {
            returnValue.Add(new([current.X1, other.Y2 + 1, current.Z1], [current.X2, current.Y2, current.Z2]));
            current = new([current.X1, current.Y1, current.Z1], [current.X2, other.Y2, current.Z2]);
        }

        if (current.Y1 < other.Y1 && other.Y1 <= current.Y2)
        {
            returnValue.Add(new([current.X1, current.Y1, current.Z1], [current.X2, other.Y1 - 1, current.Z2]));
            current = new([current.X1, other.Y1, current.Z1], [current.X2, current.Y2, current.Z2]);
        }

        // Do it again, again, but for Z.
        if (current.Z1 <= other.Z2 && other.Z2 < current.Z2)
        {
            returnValue.Add(new([current.X1, current.Y1, other.Z2 + 1], [current.X2, current.Y2, current.Z2]));
            current = new([current.X1, current.Y1, current.Z1], [current.X2, current.Y2, other.Z2]);
        }

        if (current.Z1 < other.Z1 && other.Z1 <= current.Z2)
        {
            returnValue.Add(new([current.X1, current.Y1, current.Z1], [current.X2, current.Y2, other.Z1 - 1]));
            // never referenced
            // current = new([current.X1, current.Y1, other.Z1], [current.X2, current.Y2, current.Z2]);
        }

        return returnValue;
    }

    private static bool EdgeOverlap(int aStart, int aEnd, int bStart, int bEnd) =>
        (aStart >= bStart || aEnd >= bStart) && (aStart <= bEnd || aEnd <= bEnd);

    public bool Overlap(Cuboid other) =>
        EdgeOverlap(X1, X2, other.X1, other.X2) &&
        EdgeOverlap(Y1, Y2, other.Y1, other.Y2) &&
        EdgeOverlap(Z1, Z2, other.Z1, other.Z2);

    public override string ToString()
    {
        return $"({X1}, {Y1}, {Z1}), ({X2}, {Y2}, {Z2})";
    }
}