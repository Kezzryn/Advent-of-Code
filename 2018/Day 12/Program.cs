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
    return row.Select((x, i) => x ? i + baseIndex : 0).Sum();
}

static bool RowCompare(List<bool> row1, List<bool> row2)
{
    int indexFlowerRow1 = row1.IndexOf(true); //First index of values might be down the array.
    int indexFlowerRow2 = row2.IndexOf(true);
    int lengthRow1 = row1.Count - indexFlowerRow1;
    int lengthRow2 = row2.Count - indexFlowerRow2;

    if (lengthRow1 != lengthRow2) return false;
    
    return Enumerable.Range(0, lengthRow1).All(x => row1[indexFlowerRow1 + x] == row2[indexFlowerRow2 + x]);
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";

    const int MAX_GENERATIONS = 100; // Pattern repeats on the 99th iteration.
    const int PART_1_GENERATION = 20;
    const long PART_2_GENERATION = 50_000_000_000;

    const int FLOWER = '#';
    const string CRLF = "\r\n";

    string[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(CRLF + CRLF);

    List<bool>[] flowers = [[], []];
    flowers[0] = [.. puzzleInput[0].Split(' ').Last().Select(c => c == FLOWER)];

    Dictionary<int, bool> flowerGrowth = 
        puzzleInput[1].Split(CRLF).Select(x => x.Split(" => "))
            .ToDictionary(
                    x => BoolAryToInt(x.First().Select(c => c == FLOWER).ToArray()),
                    x => x.Last().First() == FLOWER);

    bool[] keyArray = new bool[5];

    int part1Answer = 0;
    long part2Answer = 0;

    int baseIndex = 0;

    int indexNewFlowers = 0;
    int indexCurrentFlowers = 1; 

    for (int generationNum = 1; generationNum <= MAX_GENERATIONS; generationNum++)
    {
        (indexNewFlowers, indexCurrentFlowers) = (indexCurrentFlowers, indexNewFlowers);

        int cursorCurrentFlowers = 0;

        flowers[indexNewFlowers].Clear();

        while (cursorCurrentFlowers < flowers[indexCurrentFlowers].Count)
        {
            if (cursorCurrentFlowers == 0)
            {
                // check one spot before the list. 
                Array.Clear(keyArray); // reset to false 
                flowers[indexCurrentFlowers].CopyTo(0, keyArray, 3, 2);
                if (flowerGrowth[BoolAryToInt(keyArray)])
                {
                    // examing the puzzle rules shows that only a single rule creates a new node before the start.
                    flowers[indexNewFlowers].Add(true);
                    baseIndex--;
                }
            }

            int windowMin = int.Max(cursorCurrentFlowers - 2, 0);
            int windowMax = int.Min(cursorCurrentFlowers + 2, flowers[indexCurrentFlowers].Count - 1);
            int windowSize = windowMax - windowMin + 1;
            int startIndex = int.Max(2 - (cursorCurrentFlowers - windowMin), 0);

            Array.Clear(keyArray); // reset to false 
            flowers[indexCurrentFlowers].CopyTo(windowMin, keyArray, startIndex, windowSize);
            flowers[indexNewFlowers].Add(flowerGrowth[BoolAryToInt(keyArray)]);

            if (cursorCurrentFlowers == flowers[indexCurrentFlowers].Count - 1)
            {
                // examing the puzzle rules shows that a new node is only added immediatly to the end.
                Array.Clear(keyArray); // reset to false 
                flowers[indexCurrentFlowers].CopyTo(cursorCurrentFlowers - 1, keyArray, 0, 2);
                if (flowerGrowth[BoolAryToInt(keyArray)])
                {
                    flowers[indexNewFlowers].Add(true);
                }
            }
            cursorCurrentFlowers++;
        }

        if (generationNum == PART_1_GENERATION) part1Answer = ScoreLine(flowers[indexNewFlowers], baseIndex);

        if (RowCompare(flowers[indexNewFlowers], flowers[indexCurrentFlowers]))
        {
            int prevAnswer = ScoreLine(flowers[indexCurrentFlowers], baseIndex);
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