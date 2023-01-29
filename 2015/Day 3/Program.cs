using System.Drawing;

try
{
    Dictionary<char, Size> directions = new()
    {
        { '<', new Size(-1, 0) },
        { '>', new Size(1, 0)  },
        { 'v', new Size(0, -1) },
        { '^', new Size(0, 1)  }
    };

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string moves = File.ReadAllText(PUZZLE_INPUT);

    HashSet<Point> visitedHousesPart1 = new ();
    HashSet<Point> visitedHousesPart2 = new();

    Point soloSanta = new(0,0);

    Point santa = new(0, 0);
    Point roboSanta = new(0, 0);

    bool isRoboSanta = false; 

    visitedHousesPart1.Add(santa);
    visitedHousesPart2.Add(roboSanta);

    foreach (char direction in moves)
    {
        soloSanta += directions[direction];
        visitedHousesPart1.Add(soloSanta);

        if (isRoboSanta)
        {
            roboSanta += directions[direction];
            visitedHousesPart2.Add(roboSanta);
        }
        else
        {
            santa += directions[direction];
            visitedHousesPart2.Add(santa);
        }
        isRoboSanta = !isRoboSanta;
    }
    Console.WriteLine($"Part 1: Santa visited {visitedHousesPart1.Count} houses.");
    Console.WriteLine($"Part 2: Santa and Robo Santa visited {visitedHousesPart2.Count} houses.");

}
catch (Exception e)
{
    Console.WriteLine(e);
}