using System.Text.RegularExpressions;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<string> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).ToList();

    Dictionary<string, int> MAX_ITEMS = new()
    {
        { "red", 12},
        { "green", 13},
        { "blue", 14 }
    };

    int part1Answer = 0;
    int part2Answer = 0;

    foreach (string line in puzzleInput)
    {
        int gameID = int.Parse(line[line.IndexOf(' ')..line.IndexOf(':')]);
         
        //Do you lke making assumptions about how data looks? 
        List<(int cubeCount, string cubeColor)> splits = 
            Regex.Matches(line[(line.IndexOf(':')+1)..], @"(\d+.(?:red|green|blue))")
                .Select(x => x.Value.Split(' '))
                .Select(x => (int.Parse(x[0]), x[1]))
                .ToList(); 

        int maxRed = splits.Where(w => w.cubeColor == "red").Max(x => x.cubeCount);
        int maxGreen = splits.Where(w => w.cubeColor == "green").Max(x => x.cubeCount);
        int maxBlue = splits.Where(w => w.cubeColor == "blue").Max(x => x.cubeCount);
       
        if (MAX_ITEMS["red"] >= maxRed && MAX_ITEMS["green"] >= maxGreen && MAX_ITEMS["blue"] >= maxBlue)
        {
            part1Answer += gameID;  
        }

        part2Answer += (maxRed * maxGreen * maxBlue);
    }

    Console.WriteLine($"Part 1: The sum of the IDs of possible games is {part1Answer}.");
    Console.WriteLine($"Part 2: The sum of the power of the needed cubes is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}