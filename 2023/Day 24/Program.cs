using AoC_2023_Day_24;

static bool RaysIntersection2D((VecDecimal start, VecDecimal vec) a, (VecDecimal start, VecDecimal vec) b, out VecDecimal result)
{
    //from here: https://stackoverflow.com/questions/2931573/determining-if-two-rays-intersect
    result = new(0, 0, 0);
    if (a.start == b.start)
    {
        result = a.start;
        return true;
    }

    //find the delta between the starting points.
    decimal dx = b.start.X - a.start.X;
    decimal dy = b.start.Y - a.start.Y;

    // det = determinate https://en.wikipedia.org/wiki/Determinant
    decimal det = b.vec.X * a.vec.Y - b.vec.Y * a.vec.X;

    if (det == 0) return false; // det == 0  parallel lines that never intersect. 

    decimal u = (dy * b.vec.X - dx * b.vec.Y) / det;  //future/past check for b 
    decimal v = (dy * a.vec.X - dx * a.vec.Y) / det;  //future/past check for a
    if (u >= 0 && v >= 0)
    {
        // return a translated start point to find our point
        result = a.start + a.vec * u;
        return true;
    }

    return false;
}

static (VecDecimal, VecDecimal) HailstoneToXY((VecDecimal pos, VecDecimal velocity) hailstone) => (new(hailstone.pos.X, hailstone.pos.Y), new(hailstone.velocity.X, hailstone.velocity.Y));

static (VecDecimal, VecDecimal) HailstoneToXZ((VecDecimal pos, VecDecimal velocity) hailstone) => (new(hailstone.pos.X, hailstone.pos.Z), new(hailstone.velocity.X, hailstone.velocity.Z));

try
{
    const long MIN_INTERCEPT = 200000000000000;
    const long MAX_INTERCEPT = 400000000000000;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    List<(VecDecimal pos, VecDecimal velocity)> hailstones = [];

    foreach (string line in puzzleInput)
    {
        string[] temp = line.Split('@');
        var coord = temp[0].Split(',').Select(long.Parse).ToArray();
        var vec = temp[1].Split(',').Select(long.Parse).ToArray();

        hailstones.Add((new(coord[0], coord[1], coord[2]), new(vec[0], vec[1], vec[2])));
    }

    int part1Answer = 0;

    for (int i = 0; i < hailstones.Count - 1; i++)
    {
        for (int j = i + 1; j < hailstones.Count; j++)
        {
            if (RaysIntersection2D(HailstoneToXY(hailstones[i]), HailstoneToXY(hailstones[j]), out VecDecimal result)
                && MIN_INTERCEPT <= result.X && result.X <= MAX_INTERCEPT
                && MIN_INTERCEPT <= result.Y && result.Y <= MAX_INTERCEPT)
            {
                part1Answer++;
            }
        }
    }

    // Basically copied this 'cause I wasn't getting Part 2 of this puzzle at all. :( https://aoc.csokavar.hu/?day=24
    static VecDecimal Solve2d(List<(VecDecimal pos, VecDecimal vel)> hailstones)
    {
        static (VecDecimal pos, VecDecimal vel) translateV((VecDecimal pos, VecDecimal vel) p, VecDecimal vel) =>
            (p.pos, new VecDecimal(p.vel.X - vel.X, p.vel.Y - vel.Y));

        static bool Hits((VecDecimal pos, VecDecimal vel) p, VecDecimal pos)
        {
            decimal d = (pos.X - p.pos.X) * p.vel.Y - (pos.Y - p.pos.Y) * p.vel.X;
            return Math.Abs(d) < 0.0001m;
        }

        var s = 500; //arbitrary limits for the brute force.
        for (var v1 = -s; v1 < s; v1++)
        {
            for (var v2 = -s; v2 < s; v2++)
            {
                var vel = new VecDecimal(v1, v2);

                // p0 and p1 are linearly independent => stone != null
                if (RaysIntersection2D(
                    translateV(hailstones[0], vel),
                    translateV(hailstones[1], vel),
                    out VecDecimal stone))
                {
                    if (hailstones.All(p => Hits(translateV(p, vel), stone)))
                    {
                        return stone;
                    }
                }
            }
        }

        return new(-10, -10);
    }

    List<(VecDecimal pos, VecDecimal vel)> hailstonesXY = [];
    List<(VecDecimal pos, VecDecimal vel)> hailstonesXZ = [];

    foreach(var h in hailstones)
    {
        hailstonesXY.Add(HailstoneToXY(h));
        hailstonesXZ.Add(HailstoneToXZ(h));
    }

    VecDecimal xy = Solve2d(hailstonesXY);
    VecDecimal xz = Solve2d(hailstonesXZ);

    long part2Answer = (long)(xy.X + xy.Y + xz.Y);

    Console.WriteLine($"Part 1: There are {part1Answer} intersections in the target area.");
    Console.WriteLine($"Part 2: The product of the thrown rocks starting coordinates is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
