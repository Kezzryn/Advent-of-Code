static int FindMatchingBrace(string inputPacket, int startPos)
{
    int matchCount = 0;

    for (int i = startPos; i < inputPacket.Length; i++)
    {
        if (inputPacket[i] == '(') { matchCount++; }
        if (inputPacket[i] == ')') { matchCount--; }
        if (matchCount == 0) return i;
    }

    return -1;
}

static long DoBestMath(string inputLine, bool doPart2 = false)
{
    const long PLUS = -1;
    const long MULT = -2;

    List<long> parseLine = new();

    for(int i = 0; i < inputLine.Length; i++)
    {
        if (inputLine[i] == '(')
        {
            int indexOf = 0;
            indexOf = FindMatchingBrace(inputLine, i);
            parseLine.Add(DoBestMath(inputLine[(i + 1)..indexOf], doPart2));
            i = indexOf;
        }
        else
        {
            parseLine.Add(inputLine[i] switch
                {
                    '+' => PLUS,
                    '*' => MULT,
                    _ => long.Parse(inputLine[i].ToString())
                }
            );
        }
    }

    int cursor = 0;

    do
    {
        cursor = 1; // operator should always be the second position for part 1

        if (doPart2)
        {
            cursor = parseLine.IndexOf(PLUS);
            if (cursor == -1) cursor = parseLine.IndexOf(MULT);
        }

        parseLine[cursor - 1] = parseLine[cursor] switch
        {
            PLUS => parseLine[cursor - 1] + parseLine[cursor + 1],
            MULT => parseLine[cursor - 1] * parseLine[cursor + 1],
            _ => throw new NotImplementedException($"Not implemented {parseLine[cursor]}")
        };

        parseLine.RemoveAt(cursor + 1);
        parseLine.RemoveAt(cursor);
    } while (parseLine.Count > 1);

    return parseLine.First();
}

try
{
    const bool DO_PART_2 = true;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<string> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Select(x => x.Replace(" ", "")).ToList();

    long part1Answer = puzzleInput.Select(x => DoBestMath(x)).Sum();
    long part2Answer = puzzleInput.Select(x => DoBestMath(x, DO_PART_2)).Sum();

    Console.WriteLine($"Part 1: With no operator precident the sum of the answers is {part1Answer}.");
    Console.WriteLine($"Part 2: With addition before multiplicatoin the sum of the answers is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}