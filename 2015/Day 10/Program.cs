using System.Text;

static string SpeakAndSay(string input)
{
    StringBuilder sb = new();

    int i = 0;
    while (i < input.Length)
    {
        char currChar = input[i];
        int nextIndex = input.IndexOfAny("1234567890".Except([currChar]).ToArray(), i);

        string chunk = (nextIndex == -1) ? input[i..] : input[i..nextIndex];

        sb.Append(chunk.Length);
        sb.Append(currChar);

        i = (nextIndex == -1) ? input.Length : nextIndex;
    }

    return sb.ToString();
}

try
{
    const int PART1_NUM_LOOPS = 40;
    const int MAX_LOOPS = 50;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    int part1Answer = 0;
    
    for (int i = 1; i <= MAX_LOOPS; i++ )
    {
        puzzleInput = SpeakAndSay(puzzleInput);
        if (i == PART1_NUM_LOOPS) part1Answer = puzzleInput.Length;
    }

    int part2Answer = puzzleInput.Length;

    Console.WriteLine($"Part 1: The length of the result after {PART1_NUM_LOOPS} iterations is {part1Answer}.");
    Console.WriteLine($"Part 2: The length of the result after {MAX_LOOPS} iterations is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}