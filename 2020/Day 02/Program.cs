try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);
 
    int part1Answer = 0;
    int part2Answer = 0;

    foreach (string line in puzzleInput)
    {
        string[] inst = line.Split(' ', '-').ToArray();

        int lowerBound = int.Parse(inst[0]);
        int upperBound = int.Parse(inst[1]);
        char c = inst[2][0];
        string pw = inst[3];

        int charCount = pw.Count(x => x == c);

        if (charCount <= upperBound && charCount >= lowerBound) part1Answer++;

        if (pw[lowerBound - 1] == c ^ pw[upperBound - 1] == c) part2Answer++;
    }

    Console.WriteLine($"Part 1: The number of valid password is {part1Answer}.");
    Console.WriteLine($"Part 2: With the revised policy, the number of valid passwords is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}