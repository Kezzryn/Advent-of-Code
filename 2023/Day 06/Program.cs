try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    List<int> timeP1 = puzzleInput[0][(puzzleInput[0].IndexOf(':') + 1)..].Split(' ',StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
    int timeP2 = int.Parse(puzzleInput[0][(puzzleInput[0].IndexOf(':') + 1)..].Replace(" ", ""));

    List<int> distanceP1 = puzzleInput[1][(puzzleInput[1].IndexOf(':') + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
    long distanceP2 = long.Parse(puzzleInput[1][(puzzleInput[1].IndexOf(':') + 1)..].Replace(" ", ""));

    int part1Answer = 1;
    for (int race = 0; race < timeP1.Count; race++)
    {
        part1Answer *= Enumerable.Range(0, timeP1[race]).Select(x => (timeP1[race] - x) * x).Count(x => x > distanceP1[race]);
    }

    int part2Answer = Enumerable.Range(0, timeP2).Select(x => (long)(timeP2 - x) * x).Where(w => w > distanceP2).Count();

    Console.WriteLine($"Part 1: {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
