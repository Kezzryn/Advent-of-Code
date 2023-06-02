try
{
    const int BUFFER = -1;
    const int FLOOR = 0;
    const int SEAT_OCCUPIED = 1;
    const int SEAT_EMPTY = 2;

    const bool PART_TWO = true;
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int Simulate(string[] puzzleInput, bool doPart2 = false)
    {
        const int OFFSET = 1;

        // add in a buffer to make neighbors easy.
        int SIDE_X = puzzleInput[0].Length + (OFFSET * 2);
        int SIDE_Y = puzzleInput.Length + (OFFSET * 2);

        int[] seatsA = new int[SIDE_X * SIDE_Y];
        int[] seatsB = new int[SIDE_X * SIDE_Y];

        Array.Fill(seatsA, BUFFER);
        Array.Fill(seatsB, BUFFER);

        ref int[] source = ref seatsA;
        ref int[] target = ref seatsB;

        int numNeighbors = doPart2 ? 5 : 4;

        foreach ((int x, int y) in from row in Enumerable.Range(0, puzzleInput.Length)
                                   from col in Enumerable.Range(0, puzzleInput[0].Length)
                                   select (col, row))
        {
            int pos = ((y + OFFSET) * (SIDE_X)) + (x + OFFSET);

            source[pos] = puzzleInput[y][x] switch
            {
                '.' => FLOOR,
                'L' => SEAT_EMPTY,
                '#' => SEAT_OCCUPIED,
                _ => throw new NotImplementedException($"Unknown input {puzzleInput[y][x]}.")
            };
        }

        int CountNeighbors(int pos, ref int[] source)
        {
            int returnValue = 0;

            for(int direction = 0; direction < 8; direction++)
            {
                int increment = direction switch
                {
                    0 => -SIDE_X - 1,   // upper left
                    1 => -SIDE_X,       // up
                    2 => -SIDE_X + 1,   // upper right
                    3 => -1,            // left
                    4 => 1,             // right
                    5 => SIDE_X - 1,    // lower left
                    6 => SIDE_X,        // down
                    7 => SIDE_X + 1,    // lower right
                    _ => throw new NotImplementedException()
                };

                int testPos = pos + increment;
                while (testPos >= source.GetLowerBound(0) && testPos <= source.GetUpperBound(0))
                {
                    if (source[testPos] != FLOOR)
                    {
                        returnValue += source[testPos] == SEAT_OCCUPIED ? 1 : 0;
                        break;
                    }

                    if (!doPart2) break; // only do one step for part 1
                    testPos += increment;
                } 
            }

            return returnValue;
        }

        bool isDone = false;
        int generation = 0;
        while (!isDone)
        {
            isDone = true;

            source = ref int.IsEvenInteger(generation) ? ref seatsA : ref seatsB;
            target = ref int.IsEvenInteger(generation) ? ref seatsB : ref seatsA;

            for (int pos = 0; pos < source.Length; pos++)
            {
                target[pos] = source[pos] switch
                {
                    SEAT_EMPTY => CountNeighbors(pos, ref source) == 0 ? SEAT_OCCUPIED : source[pos],
                    SEAT_OCCUPIED => CountNeighbors(pos, ref source) >= numNeighbors ? SEAT_EMPTY : source[pos],
                    _ => source[pos]
                };

                if (source[pos] != target[pos]) isDone = false; 
            }

            generation++;
        }

        return source.Count(x => x == SEAT_OCCUPIED);
    }

    int part1Answer = Simulate(puzzleInput);
    int part2Answer = Simulate(puzzleInput, PART_TWO);

    Console.WriteLine($"Part 1: There will be {part1Answer} seats occupied when the pattern stabilizes.");
    Console.WriteLine($"Part 2: With the revised sightline rules, there will be {part2Answer} seats occupied when the pattern stabilizes.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
