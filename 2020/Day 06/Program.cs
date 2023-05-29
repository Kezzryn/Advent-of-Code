try
{
    const string CRLF = "\r\n";
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<string> puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(CRLF + CRLF).ToList();

    int part1Answer = 
        puzzleInput.Select(x => x.Replace(CRLF, "")
                                    .Distinct()
                                    .Count()
                            ).Sum();

    int part2Answer = 0;
    puzzleInput.ForEach(group => 
        part2Answer += group.GroupBy(x => x)
                            .Count(x => x.Count() == group.Count(c => c == '\n') + 1));
 
    Console.WriteLine($"Part 1: The sum of the questions where anyone answered yes is  {part1Answer}.");
    Console.WriteLine($"Part 2: The sum of the count of questions where everyone answered yes is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}