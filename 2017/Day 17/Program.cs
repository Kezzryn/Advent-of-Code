try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    int puzzleInput = int.Parse(File.ReadAllText(PUZZLE_INPUT));

    int Spinnado(int numLoops)
    {
        List<int> buffer = [0];
        int cursor = 0;

        for (int i = 1; i <= numLoops; i++)
        {
            cursor = ((cursor + puzzleInput) % buffer.Count) + 1;
            buffer.Insert(cursor, i);
        }
        return buffer[(buffer.IndexOf(2017) + 1) % buffer.Count];
    }

    int Skipnado(int numLoops)
    {
        int buffer = 0;
        int cursor = 0;

        for (int i = 1; i <= numLoops; i++)
        {
            cursor = ((cursor + puzzleInput) % i) + 1;
            if (cursor == 1) buffer = i;
        }
        return buffer;
    }

    int part1Answer = Spinnado(2017);
    int part2Answer = Skipnado(50000000);

    Console.WriteLine($"Part 1: You try to short circuit the Spinlock with {part1Answer} ...");
    Console.WriteLine($"Part 2: But it gets angry and you try again at {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}