using AoC_2023_Day_16;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int maxX = puzzleInput[0].Length - 1;
    int maxY = puzzleInput.Length - 1;
    
    HashSet<(int, int)> SolveMaze(Cursor cursor, HashSet<((int, int), (int, int))>? collisions = null)
    {
        if (collisions == null) collisions = new();
        HashSet<(int, int)> visited = new();
        if(!collisions.Add((cursor.XY, cursor.GetDir))) return visited; // loop check

        while (true) 
        {
            (int nX, int nY) = cursor.NextStep();
            if (nX < 0 || maxX < nX || nY < 0 || maxY < nY) return visited; // end state check. 

            cursor.Step();
            if (!visited.Add(cursor.XY))
                if (!collisions.Add((cursor.XY, cursor.GetDir)))
                    return visited; // more loop detection.

            char symbol = puzzleInput[nY][nX];
            switch (symbol)
            {
                case '/':
                    if (cursor.IsHorizontal) cursor.TurnLeft(); else cursor.TurnRight();
                    break;
                case '\\':
                    if (cursor.IsHorizontal) cursor.TurnRight(); else cursor.TurnLeft();
                    break;
                case '|':
                case '-':
                    if ((symbol == '|' && cursor.IsHorizontal) || (symbol == '-' && cursor.IsVertical))
                    {
                        Cursor clone = new(cursor);
                        clone.TurnLeft();
                        visited.UnionWith(SolveMaze(clone, collisions));
                        cursor.TurnRight();
                    }
                    break;
                default:
                    break;  // handles '.' 
            };
        }
    }

    int part1Answer = SolveMaze(new(-1,0,1,0)).Count;

    List<int> part2Answer = new();
    for(int i = 0; i < maxX; i++)
    {
        part2Answer.Add(SolveMaze(new(-1, i, 1, 0)).Count);    // from left
        part2Answer.Add(SolveMaze(new(maxX + 1, i, -1, 0)).Count);    // from right
        part2Answer.Add(SolveMaze(new(i, -1, 0, 1)).Count);    // from top
        part2Answer.Add(SolveMaze(new(i, maxY + 1, 0,-1)).Count);    // from bottom
    }

    Console.WriteLine($"Part 1: There are {part1Answer} tiles energized with the starting configuration.");
    Console.WriteLine($"Part 2: The best start energizes {part2Answer.Max()} tiles.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
