using AoC_Point3D;
using System.Text.RegularExpressions;


// LCM/GCD code from : 
// https://stackoverflow.com/questions/147515/least-common-multiple-for-3-or-more-numbers/29717490#29717490


//least common multiple
static long LCM(long a, long b) => Math.Abs(a * b) / GCD(a, b);

//greatest common divisor
static long GCD(long a, long b) => b == 0 ? a : GCD(b, a % b);

static long GetEnergy(Point3D a) => Math.Abs(a.X) + Math.Abs(a.Y)+ Math.Abs(a.Z);

try
{
    const int NUM_STEPS_PART_ONE = 1000;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    List<(Point3D moon, Point3D vel)> moons = new();
    Point3D theInterval = new(-1, -1, -1);

    foreach (string line in puzzleInput)
    {
        var regexMatches = Regex.Match(line, @"<x=([-]?\d+), y=([-]?\d+), z=([-]?\d+)>");

        Point3D moon = new(regexMatches.Groups.Values.Skip(1).Select(x => long.Parse(x.Value)).ToArray());
        moons.Add((moon, new()));
    }

    long time = 0;
    bool isDone = false;
    long part1Answer = 0;

    while (!isDone)
    {
        time++;
        // apply gravity
        foreach ((int a, int b) in from a in Enumerable.Range(0, moons.Count)
                                   from b in Enumerable.Range(0, moons.Count)
                                   where a != b
                                   select (a, b))
        {
            moons[a].vel.X += Math.Sign(moons[b].moon.X - moons[a].moon.X);
            moons[a].vel.Y += Math.Sign(moons[b].moon.Y - moons[a].moon.Y);
            moons[a].vel.Z += Math.Sign(moons[b].moon.Z - moons[a].moon.Z);
        }

        // apply velocity
        for (int m = 0; m < moons.Count; m++)
        {
            moons[m] = (moons[m].moon + moons[m].vel, moons[m].vel);
        }

        // check for return
        for (int m = 0; m < moons.Count; m++)
        {
            if (theInterval.X == -1 && moons.All(m => m.vel.X == 0)) theInterval.X = time;
            if (theInterval.Y == -1 && moons.All(m => m.vel.Y == 0)) theInterval.Y = time;
            if (theInterval.Z == -1 && moons.All(m => m.vel.Z == 0)) theInterval.Z = time;
        }

        if (time == NUM_STEPS_PART_ONE) part1Answer = moons.Sum(x => GetEnergy(x.moon) * GetEnergy(x.vel));

        if (theInterval.X >= 0 && theInterval.Y >= 0 && theInterval.Z >= 0 && time >= NUM_STEPS_PART_ONE) isDone = true;
    }

    // multiply by two since we find the interval of the balance point before the system reverses itself
    long part2Answer = LCM(LCM(theInterval.X, theInterval.Y), theInterval.Z) * 2;

    Console.WriteLine($"Part 1: The energy of the system after {NUM_STEPS_PART_ONE} steps is: {part1Answer}");
    Console.WriteLine($"Part 2: The system will return to its starting position after {part2Answer} time ticks.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}