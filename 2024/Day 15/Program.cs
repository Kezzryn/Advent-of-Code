using AoC_2024_Day_15;
using BKH.Geometry;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(Environment.NewLine + Environment.NewLine);
    
    string[] mapData = puzzleInput[0].Split(Environment.NewLine);
    List<Point2D.Direction> moveData = puzzleInput[1].Split(Environment.NewLine).SelectMany(s => s.Select(x =>
        {
            return x switch
            {
                '^' => Point2D.Direction.Down, //inverted because of y-axis BS. 
                'v' => Point2D.Direction.Up,   //inverted because of y-axis BS. 
                '<' => Point2D.Direction.Left,
                '>' => Point2D.Direction.Right,
                _ => throw new NotImplementedException()
            };
        })).ToList();

    WarehouseOne wh1 = new(mapData, moveData);
    wh1.Run();

    WarehouseTwo wh2 = new(mapData, moveData);
    wh2.Run();

    long part1Answer = wh1.ScoreMap();
    long part2Answer = wh2.ScoreMap();

    Console.WriteLine($"Part 1: The GPS score for the first warehouse is {part1Answer}.");
    Console.WriteLine($"Part 2: The GPS score for the second warehouse is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
