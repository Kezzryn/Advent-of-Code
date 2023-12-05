try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<string> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).ToList();

    List<int> numberOfWins = puzzleInput.Select(s => s[s.IndexOf(':')..s.IndexOf('|')].Split(' ', StringSplitOptions.RemoveEmptyEntries)
    .Intersect(s[(s.IndexOf('|') + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries)).Count()).ToList();

    int part1Answer = numberOfWins.Where(w => w != 0).Sum(s => 1 << s - 1);

    List<int> numberOfTickets = Enumerable.Repeat(1, numberOfWins.Count).ToList();

    for (int i = 0; i < numberOfWins.Count; i++)
    {
        for (int j = 1; j <= numberOfWins[i]; j++)
        {
            numberOfTickets[i + j] += numberOfTickets[i];
        }
    }

    int part2Answer = numberOfTickets.Sum();

    Console.WriteLine($"Part 1: You score {part1Answer} on the scratchcards.");
    Console.WriteLine($"Part 2: You are buried under a pile of {part2Answer} scratchcards.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}