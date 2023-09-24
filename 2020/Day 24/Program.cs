using System.Text;
static (int x, int y) GridStep(int x, int y, string direction)
{
    switch (direction)
    {
        case "e":
            x++;
            break;
        case "w":
            x--;
            break;
        case "nw":
            if ((Math.Abs(y) % 2) == 1) x--;
            y++;
            break;
        case "ne":
            if ((Math.Abs(y) % 2) == 0) x++;
            y++;
            break;
        case "sw":
            if ((Math.Abs(y) % 2) == 1) x--;
            y--;
            break;
        case "se":
            if ((Math.Abs(y) % 2) == 0) x++;
            y--;
            break;
    }

    return (x, y);
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string CRLF = "\r\n";
    const bool WHITE = false;
    const bool BLACK = true;
    const int MAX_GENERATIONS = 100;

    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    //find all the 'n' and 's' characters then insert a comma after the character after the n or s.
    StringBuilder sb = new();

    for (int i = 0; i < puzzleInput.Length; i++)
    {
        switch (puzzleInput[i])
        {
            case 's':
            case 'n':
                sb.Append(puzzleInput[i]);
                sb.Append(puzzleInput[i + 1]);
                sb.Append(',');
                i++;
                break;
            case '\r':
            case '\n':
                sb.Append(puzzleInput[i]);
                break;
            default:
                sb.Append(puzzleInput[i]);
                sb.Append(',');
                break;
        }
    }
    string[] puzzleInputTranslated = sb.ToString().Split(CRLF);

    // Load the inital map
    Dictionary<(int x, int y), bool> map = new();
    foreach (string line in puzzleInputTranslated)
    {
        string[] directions = line.Split(',', StringSplitOptions.RemoveEmptyEntries);

        int x = 0;
        int y = 0;

        foreach (string step in directions)
        {
            (x, y) = GridStep(x, y, step);
        }

        if (!map.TryAdd((x, y), BLACK)) map[(x, y)] = !map[(x, y)];
    }

    int part1Answer = map.Count(x => x.Value == BLACK);
    Console.WriteLine($"Part 1: The initial load ends with {part1Answer} black hexes.");
    
    string[] neighborSteps = new[] { "ne", "se", "sw", "w", "nw", "ne" };

    for (int gen = 1; gen <= MAX_GENERATIONS; gen++)
    {
        int xMax = map.Keys.Max(m => m.x) + 1;
        int xMin = map.Keys.Min(m => m.x) - 1;
        int xRange = (xMax - xMin) + 1; // extra for 0 

        int yMax = map.Keys.Max(m => m.y) + 1;
        int yMin = map.Keys.Min(m => m.y) - 1;
        int yRange = (yMax - yMin) + 1; // extra for 0 

        //make a copy of any currently known black tiles. Everything else is assumed to be white.
        HashSet<(int, int)> currentBlackTiles = map.Where(x => x.Value == BLACK).Select(x => x.Key).ToHashSet();

        foreach ((int x, int y) coord in from x in Enumerable.Range(xMin, xRange)
                                         from y in Enumerable.Range(yMin, yRange)
                                         select (x, y))
        {
            bool currentTileColor = currentBlackTiles.Contains(coord) ? BLACK : WHITE;

            int neighborX = coord.x;
            int neighborY = coord.y;

            int neighbourCountBlack = 0;
            foreach (string step in neighborSteps)
            {
                (neighborX, neighborY) = GridStep(neighborX, neighborY, step);
                if (currentBlackTiles.Contains((neighborX, neighborY))) neighbourCountBlack++;
            }

            if (currentTileColor == WHITE && neighbourCountBlack == 2)
            {
                //Any white tile with exactly 2 black tiles immediately adjacent to it is flipped to black.
                map[coord] = BLACK;
            }
            else if (currentTileColor == BLACK && (neighbourCountBlack == 0 || neighbourCountBlack > 2))
            {
                //Any black tile with zero or more than 2 black tiles immediately adjacent to it is flipped to white.
                map[coord] = WHITE;
            }
        }
    }

    int part2Answer = map.Count(x => x.Value == BLACK);
    Console.WriteLine($"Part 2: After 100 days of the cell automata, there are {part2Answer} black tiles shown.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}