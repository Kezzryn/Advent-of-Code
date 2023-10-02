using System.Text.RegularExpressions;

static int CountOverlap(List<string> input, bool doPart2 = false)
{
    const int X1 = 0;
    const int Y1 = 1;
    const int X2 = 2;
    const int Y2 = 3;

    Dictionary<(int x, int y), int> grid = new();

    foreach (string line in input)
    {
        int[] coords = Regex.Matches(line, "\\d+").Select(x => int.Parse(x.Value)).ToArray();

        if (coords[X1] == coords[X2]) // vertical line
        {
            if (coords[Y1] > coords[Y2]) (coords[Y1], coords[Y2]) = (coords[Y2], coords[Y1]); //make it so we always go low to high.

            for (int y = coords[Y1]; y <= coords[Y2]; y++)
            {
                if (!grid.TryAdd((coords[X1], y), 1)) grid[(coords[X1], y)]++;
            }
        }
        else if (coords[Y1] == coords[Y2]) // horizontal line
        {
            if (coords[X1] > coords[X2]) (coords[X1], coords[X2]) = (coords[X2], coords[X1]); //make it so we always go low to high.

            for (int x = coords[X1]; x <= coords[X2]; x++)
            {
                if (!grid.TryAdd((x, coords[Y1]), 1)) grid[(x, coords[Y1])]++;
            }
        }
        else if (doPart2) // diagonal line, but only for part 2.
        {
            if (coords[X1] > coords[X2])    // normalize X as low to high so the while loop has a direction.
            {
                (coords[X1], coords[X2]) = (coords[X2], coords[X1]); 
                (coords[Y1], coords[Y2]) = (coords[Y2], coords[Y1]); 
            }

            int x = coords[X1];
            int y = coords[Y1];
            int stepX = coords[X1] < coords[X2] ? 1 : -1;
            int stepY = coords[Y1] < coords[Y2] ? 1 : -1;
            
            while(x <= coords[X2])
            {
                if (!grid.TryAdd((x, y), 1)) grid[(x, y)]++;
                x += stepX;
                y += stepY;
            }
        }
    }

    return grid.Count(x => x.Value >= 2);
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const bool DO_PART2 = true;

    List<string> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).ToList();

    int part1Answer = CountOverlap(puzzleInput);
    int part2Answer = CountOverlap(puzzleInput, DO_PART2);

    Console.WriteLine($"Part 1: There are {part1Answer} intersections with horizontal and vertial lines.");
    Console.WriteLine($"Part 2: Adding in diagonal lines, brings the count to {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}