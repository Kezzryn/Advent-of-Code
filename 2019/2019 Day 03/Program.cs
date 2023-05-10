using System.Drawing;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[][] puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(x => x.Split(',')).ToArray();

    Dictionary<Point, int>[] wires = new Dictionary<Point, int>[puzzleInput.GetLength(0)];

    HashSet<Point> intersections = new();

    Dictionary<char, Size> direction = new()
    {
        {  'U', new(0,1) },
        {  'D', new(0,-1) },
        {  'L', new(-1,0) },
        {  'R', new(1,0) },
    };

    for(int line = 0; line < puzzleInput.GetLength(0); line++)
    {
        Point cursor = new(0, 0);
        wires[line] = new();
        int steps = 0;
        foreach (string instruction in puzzleInput[line])
        {
            char dir = instruction[0];
            int dist = int.Parse(instruction[1..]);

            for (int i = 0; i < dist; i++)
            {
                cursor += direction[dir];
                steps++;
                wires[line].TryAdd(cursor, steps);
            }
        }
    }
    intersections = wires[0].Keys.Intersect(wires[1].Keys).ToHashSet();

    int part1Answer = intersections.Min(a => Math.Abs(a.X) + Math.Abs(a.Y));
    int part2Answer = intersections.Min(x => wires[0][x] + wires[1][x]);

    Console.WriteLine($"Part 1: The distance to the closest intersection of the two wires is {part1Answer}.");
    Console.WriteLine($"Part 2: The fewest combined steps to reach an intersection is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
