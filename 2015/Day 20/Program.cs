using BKH.EnumExtentions;

try
{
    const long PPE_Part1 = 10; //Presents Per Elf
    const long PPE_Part2 = 11;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    long puzzleInput = long.Parse(File.ReadAllText(PUZZLE_INPUT));

    long part1Answer = -1;
    long part2Answer = -1;

    for(long houseNumber = 0; part1Answer == -1 || part2Answer == -1; houseNumber++)
    {
        IEnumerable<long> f = houseNumber.Factor();

        if (part1Answer == -1)
            if (f.Sum() * PPE_Part1 >= puzzleInput)
                part1Answer = houseNumber;

        if (part2Answer == -1)
            if (f.Where(x => x > houseNumber / 50).Sum() * PPE_Part2 >= puzzleInput)
                part2Answer = houseNumber;
    }

    Console.WriteLine($"Part 1: The first house to get more than {puzzleInput} is: {part1Answer}.");
    Console.WriteLine($"Part 2: After changing to only delivering to 50 houses, the first house to get more than {puzzleInput} is: {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}