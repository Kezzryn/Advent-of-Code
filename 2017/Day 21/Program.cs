using System.Collections;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";

    const int TWOS_MARKER = 3584; //1110 0000 0000 These are the bits above 511, the largest key we otherwise make. 
    const int PART_1_ITERATIONS = 5;
    const int PART_2_ITERATIONS = 18;
    List<string[]> puzzleInput = File.ReadAllLines(PUZZLE_INPUT)
        .Select(x => x.Replace("/", "")
                      .Replace('.', '0')
                      .Replace('#', '1')
                        .Split(" => ").ToArray()
                ).ToList();

    Dictionary<int, bool[]> ruleBook = [];
    int[] bitToIntConverter = new int[1];

    int part1Answer = 0;
    int part2Answer = 0;

    // Reminder, the positions change as swaps happen, which makes for interesting times.
    static char[] Rotate(char[] aryChar)
    {
        if (aryChar.Length == 4)
        {
            (aryChar[0], aryChar[1]) = (aryChar[1], aryChar[0]);
            (aryChar[1], aryChar[3]) = (aryChar[3], aryChar[1]);
            (aryChar[2], aryChar[3]) = (aryChar[3], aryChar[2]);
        }
        else
        {
            (aryChar[0], aryChar[2]) = (aryChar[2], aryChar[0]);
            (aryChar[1], aryChar[5]) = (aryChar[5], aryChar[1]);
            (aryChar[2], aryChar[8]) = (aryChar[8], aryChar[2]);
            (aryChar[3], aryChar[5]) = (aryChar[5], aryChar[3]);
            (aryChar[5], aryChar[7]) = (aryChar[7], aryChar[5]);
            (aryChar[6], aryChar[8]) = (aryChar[8], aryChar[6]);
        }
        return aryChar;
    }

    static char[] Mirror(char[] aryChar)
    {
        if (aryChar.Length == 4)
        {
            (aryChar[0], aryChar[1]) = (aryChar[1], aryChar[0]);
            (aryChar[2], aryChar[3]) = (aryChar[3], aryChar[2]);
        }
        else
        {
            (aryChar[0], aryChar[2]) = (aryChar[2], aryChar[0]);
            (aryChar[3], aryChar[5]) = (aryChar[5], aryChar[3]);
            (aryChar[6], aryChar[8]) = (aryChar[8], aryChar[6]);
        }
        return aryChar;
    }

    foreach (string[] splitLine in puzzleInput)
    {
        List<string> keys = [];
        char[] keyChars = splitLine[0].ToCharArray();

        for (int i = 0; i < 2; i++)
        {
            keys.Add(new(keyChars));
            Array.Reverse(keyChars);
            keys.Add(new(keyChars));

            keyChars = Mirror(keyChars);

            keys.Add(new(keyChars));
            Array.Reverse(keyChars);
            keys.Add(new(keyChars));

            //Skip the Rotation function on the second pass since we're leaving the loop.
            if (i == 0) keyChars = Rotate(keyChars);
        }

        keys = keys.Distinct().ToList();

        bool[] value = new bool[splitLine[1].Length];
        for (int i = 0; i < splitLine[1].Length; i++)
        {
            value[i] = splitLine[1][i] == '1';
        }

        keys.ForEach(key => ruleBook.Add(Convert.ToInt32(key, 2) | (splitLine[0].Length == 4 ? TWOS_MARKER : 0), value));
    }

    //Art is stored in a 1D array, effectivly flattening the sub grids of the puzzle.
    //This could be further optimized by storing sub grids, and patterns of sub grids rather than the girds.
    
    //Starting state:
    bool[] boolArt =
    [
        false, true, false, // .#.
        false, false, true, // ..#
        true, true, true    // ###
    ];

    for (int i = 1; i <= PART_2_ITERATIONS; i++)
    {
        int currentSideLength = (int)Math.Sqrt(boolArt.Length);

        int chunkSize = (boolArt.Length % 2) == 0 ? 2 : 3;
        int chunkSizeSquared = chunkSize * chunkSize;
        int chunkPerSide = currentSideLength / chunkSize;

        int nextChunkSize = chunkSize + 1;
        int nextChunkSizeSquared = nextChunkSize * nextChunkSize;
        int nextSideLength = nextChunkSize * chunkPerSide;

        bool[] boolKey = new bool[chunkSizeSquared];
        bool[] nextBoolArt = new bool[nextChunkSizeSquared * (boolArt.Length / chunkSizeSquared)];

        IEnumerable<(int, int)> sourceChunkPos =
            from row in Enumerable.Range(0, chunkPerSide).Select(r => r * chunkPerSide * chunkSizeSquared)
            from col in Enumerable.Range(0, chunkPerSide).Select(c => c * chunkSize)
            select (col, row);

        foreach ((int col, int row) in sourceChunkPos)
        {
            //pull the current subkey out of the chunk.
            foreach (int keyIndex in Enumerable.Range(0, chunkSize))
            {
                int sourceIndex = row + col + (keyIndex * currentSideLength);
                int destIndex = keyIndex * chunkSize;

                Array.Copy(boolArt, sourceIndex, boolKey, destIndex, chunkSize);
            }

            BitArray bitArray = new(boolKey);
            bitArray.CopyTo(bitToIntConverter, 0); // Bitarray to integer magic. 
            bool[] newArtChunk = ruleBook[bitToIntConverter[0] | (chunkSize == 2 ? TWOS_MARKER : 0)];

            int newRow = row / chunkSizeSquared * nextChunkSizeSquared;
            int newCol = col / chunkSize * nextChunkSize;
            
            // ... and put the new chunk into the next artwork. 
            foreach (int keyIndex in Enumerable.Range(0, nextChunkSize))
            {
                int nextSourceIndex = keyIndex * nextChunkSize;
                int nextDestIndex = newRow + newCol + (keyIndex * nextSideLength);

                Array.Copy(newArtChunk, nextSourceIndex, nextBoolArt, nextDestIndex, nextChunkSize);
            }
        }
        boolArt = nextBoolArt;

        if (i == PART_1_ITERATIONS) part1Answer = boolArt.Count(c => c);
    }

    part2Answer = boolArt.Count(c => c);

    Console.WriteLine($"Part 1: There are {part1Answer} pixels on after {PART_1_ITERATIONS} iterations.");
    Console.WriteLine($"Part 2: There are {part2Answer} pixels on after {PART_2_ITERATIONS} iterations.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
