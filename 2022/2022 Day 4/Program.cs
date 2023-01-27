using System.Text.RegularExpressions;

try
{
    const string PUZZLE_INPUT = "..\\..\\..\\..\\..\\Input Files\\Day 4.txt";

    const int LEFT_START = 0;
    const int LEFT_END = 1;
    const int RIGHT_START = 2;
    const int RIGHT_END = 3;

    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int scorePart1 = 0;
    int scorePart2 = 0;

    for (int i = 0; i < puzzleInput.Length; i++)
    {
        int[] numbers = SplitNumbers().Split(puzzleInput[i]).Select(int.Parse).ToArray();

        //1-3,6-8 not overlapped
        //2-6,4-8 partialy overlapped
        //2-8,3-7 fully contained

        //part 1 
        //for each line find if a pair of numbers is fully contained in the other pair. 
        if (
            (numbers[LEFT_START] >= numbers[RIGHT_START] && numbers[LEFT_END] <= numbers[RIGHT_END]) ||
            (numbers[RIGHT_START] >= numbers[LEFT_START] && numbers[RIGHT_END] <= numbers[LEFT_END]))
        {
            scorePart1++;
        }

        //part 2
        //for each line find if a pair of numbers overlaps at all.
        if (
            (numbers[LEFT_START] >= numbers[RIGHT_START] || numbers[LEFT_END] >= numbers[RIGHT_START]) &&
            (numbers[LEFT_START] <= numbers[RIGHT_END] || numbers[LEFT_END] <= numbers[RIGHT_END]))
        {
            scorePart2++;
        }
    }

    Console.WriteLine($"Part one: {scorePart1}");
    Console.WriteLine($"Part two: {scorePart2}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}

partial class Program
{
    [GeneratedRegex("\\D+")]
    private static partial Regex SplitNumbers();
}
