try
{
    const int NUM_STEPS_PART1 = 2;
    const int NUM_STEPS_PART2 = 50;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    bool[] imageEnhancement = puzzleInput.First().Select(s => s == '#').ToArray();

    //I don't know if I should be proud or scared.
    HashSet<(int X, int Y)> theMap = puzzleInput.Reverse().SkipLast(2).SelectMany((l, LI) => l.Select((c, CI) => c == '#' ? (CI, LI) : (-1,-1)).Where(w => w != (-1,-1))).ToHashSet();

    int minX = theMap.Select(x => x.X).Min();
    int maxX = theMap.Select(x => x.X).Max();
    int minY = theMap.Select(x => x.Y).Min();
    int maxY = theMap.Select(x => x.Y).Max();

    //void PrintMap()
    //{
    //    for (int y = maxY; y >= minY; y--)
    //    {
    //        for (int x = minX; x <= maxX; x++)
    //        {
    //            Console.Write(theMap.Contains((x, y)) ? "#" : " ");
    //        }
    //        Console.WriteLine();
    //    }
    //    Console.WriteLine();
    //    Console.WriteLine();
    //}

    int part1Answer = 0;
    int part2Answer = 0;

    int numLoops = 1;

    //The void flickers on the full puzzle input. It starts dark (false). 
    bool backGroundFlicker = false;

    HashSet<(int, int)> toAdd = new();
    HashSet<(int, int)> toRemove = new();

    do
    {
        minX--;
        minY--;
        maxX++;
        maxY++;

        toAdd.Clear();
        toRemove.Clear();

        foreach ((int X, int Y) in from y in Enumerable.Range(minY, maxY - minY + 1)
                                   from x in Enumerable.Range(minX, maxX - minX + 1)
                                   select (x, y))
        {
            int translationIndex = 0;
            int shiftDigit = 8;
            foreach ((int nX, int nY) pos in from y in Enumerable.Range(Y - 1, 3).Reverse()
                                             from x in Enumerable.Range(X - 1, 3)
                                             select (x, y))
            {
                if (((pos.nX <= minX || pos.nX >= maxX || pos.nY <= minY || pos.nY >= maxY) && backGroundFlicker) || theMap.Contains(pos))
                { 
                    translationIndex |= 1 << shiftDigit;
                }
                shiftDigit--;
            }

            if (theMap.Contains((X,Y)))
            {
                if (!imageEnhancement[translationIndex]) toRemove.Add((X, Y));
            }
            else
            {
                if (imageEnhancement[translationIndex]) toAdd.Add((X, Y));
            }
        }

        theMap.ExceptWith(toRemove);
        theMap.UnionWith(toAdd);

        backGroundFlicker = imageEnhancement[backGroundFlicker ? 511 : 0];

        if (numLoops == NUM_STEPS_PART1) part1Answer = theMap.Count;
        if (numLoops == NUM_STEPS_PART2) part2Answer = theMap.Count;
    } while (++numLoops <= NUM_STEPS_PART2);

    Console.WriteLine($"Part 1: The number of lights after {NUM_STEPS_PART1} steps is {part1Answer}."); 
    Console.WriteLine($"Part 2: The number of lights after {NUM_STEPS_PART2} steps is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}