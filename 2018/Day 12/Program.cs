using System.Collections;

static int BoolAryToInt(bool[] boolAry)
{
    BitArray bitField = new(boolAry);
    int[] intConverter = new int[1];
    bitField.CopyTo(intConverter, 0);
    return intConverter[0];
}

static int ScoreLine(List<bool> row, int baseIndex)
{
    int returnValue = 0;
    for (int i = 0; i < row.Count; i++)
    {
        returnValue += row[i] ? i + baseIndex : 0;
    }
    return returnValue;
}

static bool RowCompare(List<bool> row1, List<bool> row2)
{
    const bool FLOWER = true;

    int indexFlowerRow1 = row1.IndexOf(FLOWER);
    int indexFlowerRow2 = row2.IndexOf(FLOWER);
    int length = row1.Count - indexFlowerRow1;

    if (length != row2.Count - indexFlowerRow2) return false;
    
    for (int i = 0;i < length;i++)
    {
        if (row1[indexFlowerRow1 + i] != row2[indexFlowerRow2 + i]) return false;
    }
    
    return true;
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";

    const int MAX_GENERATIONS = 100;
    const int PART_1_GENERATION = 20;
    const long PART_2_GENERATION = 50_000_000_000;

    const int FLOWER = '#';
    const string CRLF = "\r\n";

    string[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(CRLF + CRLF);
    string initialState = puzzleInput[0].Split(' ').Last();
    string[][] puzzleKeys = puzzleInput[1].Split(CRLF).Select(x => x.Split(" => ").ToArray()).ToArray();

    Dictionary<int, bool> flowerGrowth = new();
    bool[] keyArray = new bool[5];

    int part1Answer = 0;
    long part2Answer = 0;

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
    
    int baseIndex = 0;

    for (int generationNum = 1; generationNum <= MAX_GENERATIONS; generationNum++)
    {
        int indexNewFlowers = generationNum % 2;
        int indexPrevFlowers = (generationNum - 1) % 2;

        int cursorPrevFlowers = 0;

        flowers[indexNewFlowers].Clear();

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
                    flowers[indexNewFlowers].Add(true);
                    baseIndex--;
                }
                Array.Clear(keyArray); // reset to false 
            }

            int windowMin = int.Max(cursorPrevFlowers - 2, 0);
            int windowMax = int.Min(cursorPrevFlowers + 2, flowers[indexPrevFlowers].Count - 1);
            int windowSize = windowMax - windowMin + 1;
            int startIndex = int.Max(2 - (cursorPrevFlowers - windowMin), 0);

            flowers[indexPrevFlowers].CopyTo(windowMin, keyArray, startIndex, windowSize);
            flowers[indexNewFlowers].Add(flowerGrowth[BoolAryToInt(keyArray)]);

            if (cursorPrevFlowers == flowers[indexPrevFlowers].Count - 1)
            {
                // examing the puzzle rules shows that a new node is only added immediatly to the end.
                Array.Clear(keyArray); // reset to false 
                flowers[indexPrevFlowers].CopyTo(cursorPrevFlowers - 1, keyArray, 0, 2);
                if (flowerGrowth[BoolAryToInt(keyArray)])
                {
                    flowers[indexNewFlowers].Add(true);
                }
            }
            cursorPrevFlowers++;
        }

        if (generationNum == PART_1_GENERATION) part1Answer = ScoreLine(flowers[indexNewFlowers], baseIndex);

        if (RowCompare(flowers[indexNewFlowers], flowers[indexPrevFlowers]))
        {
            int prevAnswer = ScoreLine(flowers[indexPrevFlowers], baseIndex);
            int rowAnswer = ScoreLine(flowers[indexNewFlowers], baseIndex);

            part2Answer = ((PART_2_GENERATION - generationNum) * (rowAnswer - prevAnswer)) + rowAnswer;
            break;
        }
    }

    Console.WriteLine($"Part 1: The flower score after {PART_1_GENERATION} is {part1Answer}.");
    Console.WriteLine($"Part 2: The flower score after {PART_2_GENERATION} is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}