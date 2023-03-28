try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int part1Answer = 0;
    int part2Answer = 0;

    foreach (string line in puzzleInput)
    {
        var data = line.Split('\t', StringSplitOptions.TrimEntries & StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).OrderBy(x => x);

        part1Answer += data.TakeLast(1).Union(data.Take(1)).Aggregate((x,y) => x - y);

        foreach (var a  in data)
        {
            foreach (var b in data)
            {
                if (a == b) continue;
                if (a > b && (a % b) == 0 )
                {
                    part2Answer += a / b; 
                    break;
                }
            }
        }
    }


    Console.WriteLine($"Part 1: The spreadsheet's checksum is {part1Answer}.");
    Console.WriteLine($"Part 2: The even number's checksum is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}