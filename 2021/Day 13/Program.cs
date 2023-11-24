try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string CRLF = "\r\n";

    const int DOTS = 0;
    const int INSTRUCTIONS = 1;

    string[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(CRLF + CRLF).ToArray();

    HashSet<(int X, int Y)> thePaper = new();

    foreach (string line in puzzleInput[DOTS].Split(CRLF))
    {
        int X = int.Parse(line.Split(',').First());
        int Y = int.Parse(line.Split(',').Last());

        thePaper.Add((X, Y));  
    }

    void Fold(int fold, char axis)
    {
        const char Y_AXIS = 'y';
        HashSet<(int, int)> toDelete = new();

        int maxOuter = axis == Y_AXIS ? thePaper.Max(v => v.X) : thePaper.Max(v => v.Y);
        int maxInner = axis == Y_AXIS ? thePaper.Max(v => v.Y) : thePaper.Max(v => v.X);

        for (int outer = 0; outer <= maxOuter; outer++)
        {
            for (int inner = maxInner; inner > fold; inner--)
            {
                (int, int) currentPoint = axis == Y_AXIS ? (outer, inner) : (inner, outer);
                if (thePaper.Contains(currentPoint))
                {
                    int newInner = fold - (inner - fold);
                    toDelete.Add(currentPoint);
                    thePaper.Add(axis == Y_AXIS ? (outer, newInner) : (newInner, outer));
                }
            }
        }

        foreach (var key in toDelete)
        {
            thePaper.Remove(key);
        }
    }

    int part1Answer = -1;

    foreach(string line in puzzleInput[INSTRUCTIONS].Split(CRLF))
    {
        char axis = line.Split('=').First().Last();
        int pos = int.Parse(line.Split('=').Last());

        Fold(pos, axis);

        if (part1Answer == -1) part1Answer = thePaper.Count;
    }

    Console.WriteLine($"Part 1: There are {part1Answer} dots visable after a single fold.");
    Console.WriteLine("Part 2:");
    Console.WriteLine();

    int maxX = thePaper.Max(x => x.X);
    int maxY = thePaper.Max(x => x.Y);

    foreach((int x, int y) in from y in Enumerable.Range(0, maxY + 1)
                              from x in Enumerable.Range(0, maxX + 1)
                              select(x, y))
    {
        Console.Write(thePaper.Contains((x, y)) ? 'x' : ' ');
        if (x == maxX) Console.WriteLine();
    }
    
    Console.WriteLine();
}
catch (Exception e)
{
    Console.WriteLine(e);
}