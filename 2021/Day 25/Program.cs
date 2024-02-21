try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<string> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Reverse().ToList();

    const int BLOCKER = -1;
    const int EMPTY = 0;
    const int FACE_EAST = 1;
    const int FACE_SOUTH = 2;

    int maxX = puzzleInput[0].Length;
    int maxY = puzzleInput.Count;

    int[,] theMap = new int[maxX, maxY];

    for(int y = 0; y < maxY; y++)
    {
        for (int x = 0; x < maxX; x++)
        {
            theMap[x, y] = puzzleInput[y][x] switch
            {
                '.' => EMPTY,
                '>' => FACE_EAST,
                'v' => FACE_SOUTH,
                _ => throw new NotImplementedException()
            };
        }
    }

    bool isMoved = false;
    int part1Answer = 0;

    //void PrintMap()
    //{
    //    Console.WriteLine($"\nAfter {part1Answer} step:");
    //    for (int y = maxY -1; y >= 0; y--)
    //    {
    //        //Console.Write($"{y,-2}");
    //        for (int x = 0; x < maxX; x++)
    //        {
    //            char chr = theMap[x, y] switch
    //            {
    //                EMPTY => '.',
    //                FACE_EAST => '>',
    //                FACE_SOUTH => 'v',
    //                _ => 'x'
    //            };
    //            Console.Write(chr);
    //        }
    //        Console.WriteLine();
    //    }
    //}

    do
    {
       // PrintMap();
        part1Answer++;
        isMoved = false;

        //east herd
        for (int y = 0; y < maxY; y++)
        {
            for (int x = 0; x < maxX; x++)
            {
                int nextX = x + 1 == maxX ? 0 : x + 1;

                if (theMap[x,y] == FACE_EAST &&
                    theMap[nextX, y] == EMPTY)
                {
                    (theMap[x, y], theMap[nextX, y]) = (theMap[nextX, y], theMap[x, y]);
                    if (x == 0) theMap[0, y] = BLOCKER;

                    isMoved = true;

                    x++; // step past the change we just made. 
                }
            }
            if (theMap[0, y] == BLOCKER) theMap[0, y] = EMPTY;
        }

        //south herd
        for (int x = 0; x < maxX; x++)
        {
            for (int y = maxY - 1; y >= 0; y--)
            {
                int nextY = y - 1 < 0 ? maxY - 1 : y - 1;

                if (theMap[x, y] == FACE_SOUTH && 
                    theMap[x, nextY] == EMPTY)
                {
                    (theMap[x, y], theMap[x, nextY]) = (theMap[x, nextY], theMap[x, y]);
                    if (y == maxY - 1) theMap[x, maxY - 1] = BLOCKER;
                    isMoved = true;
                    y--;
                }
            }
            if (theMap[x, maxY - 1] == BLOCKER) theMap[x, maxY - 1] = EMPTY;
        }
    } while (isMoved);

    Console.WriteLine($"Part 1: The sea cucumbers stop moving after {part1Answer} steps.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}