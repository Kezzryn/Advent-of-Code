using System.Text;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int PART1_NUM_LOOPS = 40;
    const int MAX_LOOPS = 50;

    string puzzleInputPart1 = File.ReadAllText(PUZZLE_INPUT);
    string puzzleInputPart2 = File.ReadAllText(PUZZLE_INPUT);

    string SpeakAndSay(string input)
    {
        StringBuilder sb = new();

        int i = 0;
        while (i < input.Length)
        {
            char currChar = input[i];
            int nextIndex = input.IndexOfAny("1234567890".Except(new char[] { currChar }).ToArray(), i);

            string chunk = (nextIndex == -1) ? input[i..] : input[i..nextIndex];

            sb.Append(chunk.Length);
            sb.Append(currChar);

            i = (nextIndex == -1) ? input.Length : nextIndex;
        }

        return sb.ToString(); 
    }

    for (int i = 1; i <= MAX_LOOPS; i++ )
    {
        if (i <= PART1_NUM_LOOPS) puzzleInputPart1 = SpeakAndSay(puzzleInputPart1);
        puzzleInputPart2 = SpeakAndSay(puzzleInputPart2);
    }
    
    Console.WriteLine($"Part 1: The length of the result after {PART1_NUM_LOOPS} iterations is {puzzleInputPart1.Length} .");
    Console.WriteLine($"Part 2: The length of the result after {MAX_LOOPS} iterations is {puzzleInputPart2.Length} .");

}
catch (Exception e)
{
    Console.WriteLine(e);
}