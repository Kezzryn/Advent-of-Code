using System.Collections;

static int BoolAryToInt(bool[] boolAry)
{
    BitArray bitField = new(boolAry);
    int[] intConverter = new int[1];
    bitField.CopyTo(intConverter, 0);
    return intConverter[0];
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int NUM_GENERATIONS = 115;
    const int FLOWER = '#';
    const string CRLF = "\r\n";
    string[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(CRLF + CRLF);
    string initialState = puzzleInput[0].Split(' ').Last();
    string[][] puzzleKeys = puzzleInput[1].Split(CRLF).Select(x => x.Split(" => ").ToArray()).ToArray();

    Dictionary<int, bool> flowerGrowth = new();
    bool[] keyArray = new bool[5];

    foreach (string[] s in puzzleKeys)
    {
        keyArray = s[0].Select(c => c == FLOWER).ToArray();

        flowerGrowth.Add(BoolAryToInt(keyArray), s[1][0] == FLOWER);
    }

    List<bool>[] flowers = new List<bool>[2];
    flowers[0] = new List<bool>();
    flowers[1] = new List<bool>();

    foreach (char c in initialState)
    {
        flowers[0].Add(c == FLOWER);
    }

    int cursorPrevFlowers;
    int baseIndex = 0;

    //Console.WriteLine($"{0,-2}: #..#.#..##......###...###");
    for (int i = 1; i <= NUM_GENERATIONS; i++)
    {
        int indexNewFlowers = i % 2;
        int indexPrevFlowers = (i-1) % 2;
        flowers[indexNewFlowers].Clear();
        cursorPrevFlowers = 0;

        while (cursorPrevFlowers < flowers[indexPrevFlowers].Count)
        {
            Array.Clear(keyArray); // reset to false 
            if (cursorPrevFlowers == 0)
            {
                // check one spot before the list. 
                flowers[indexPrevFlowers].CopyTo(0, keyArray, 3, 2);
                if (flowerGrowth[BoolAryToInt(keyArray)])
                {
                    // examing the puzzle rules shows that only a single rule creates a new node before the start.
                    // if ...##  then prepend, and decrease the base index  
                    flowers[indexNewFlowers].Add(true);
                    baseIndex--;
                }
                Array.Clear(keyArray); // reset to false 
            }

            int windowMin = int.Max(cursorPrevFlowers - 2, 0);
            int windowMax = int.Min(cursorPrevFlowers + 2, flowers[indexPrevFlowers].Count - 1);
            int windowSize = windowMax - windowMin + 1;
            int startIndex = int.Max(2 - (cursorPrevFlowers - windowMin), 0);
           // Console.Write($"{cursorPrevFlowers} {windowMin} {windowMax} {windowSize} {startIndex} {flowers[indexPrevFlowers].Count-1}  ");

            flowers[indexPrevFlowers].CopyTo(windowMin, keyArray, startIndex, windowSize);
            int keyID = BoolAryToInt(keyArray);

           // Console.Write($"{keyID} = {flowerGrowth[keyID]}   ");

            flowers[indexNewFlowers].Add(flowerGrowth[BoolAryToInt(keyArray)]);
           // Console.WriteLine(flowers[indexNewFlowers].Last());

            if (cursorPrevFlowers == flowers[indexPrevFlowers].Count - 1)
            {
                // check the off the end positions. 
                Array.Clear(keyArray); // reset to false 
                flowers[indexPrevFlowers].CopyTo(cursorPrevFlowers - 1, keyArray, 0, 2);
                if (flowerGrowth[BoolAryToInt(keyArray)])
                {
                    flowers[indexNewFlowers].Add(true);
                }

                Array.Clear(keyArray); // reset to false 
                flowers[indexPrevFlowers].CopyTo(cursorPrevFlowers, keyArray, 0, 1);
                if (flowerGrowth[BoolAryToInt(keyArray)])
                {
                    flowers[indexNewFlowers].Add(true);
                }
            }
            cursorPrevFlowers++;
        }

//        StringBuilder sb = new();
        //Console.Write($"{i,-2}: ");
        //int rowAnswer = 0;

        
        //for (int z = 0; z < flowers[indexNewFlowers].Count; z++)
        //{
         //   Console.Write($"{(flowers[indexNewFlowers][z] ? '#' : '.')}");
         //   rowAnswer += flowers[indexNewFlowers][z] ? baseIndex + z : 0;
            //sb.Append(flower ? '1' : '0');
        //}
       // Console.WriteLine($"  {rowAnswer}");

        //Console.WriteLine($"{Convert.ToUInt32(sb.ToString()[^96..^64], 2)} {Convert.ToUInt32(sb.ToString()[^64..^32], 2)}  {Convert.ToUInt32(sb.ToString()[^32..], 2)}");
    }

    Console.WriteLine();
    int part1Answer = 0;
    for (int i = 0; i < flowers[NUM_GENERATIONS % 2].Count; i++)
    {
        part1Answer += flowers[NUM_GENERATIONS % 2][i] ? baseIndex + i : 0;
    }

    //50000000000
    //process stabalizes at about generation 100, then increments by 25 per generation. 
    long part2Answer = ((50000000000 - NUM_GENERATIONS) * 25) + part1Answer;

    Console.WriteLine($"Part 1: {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}