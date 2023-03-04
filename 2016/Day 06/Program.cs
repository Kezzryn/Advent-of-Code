using System.Text;

try
{
    const string PUZZLE_INPUT = "PuzzleInputTest.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    //Setup our Stringbuilders, and initalize them. 
    List<StringBuilder> sb = new();
    for (int i = 0; i < puzzleInput[0].Length; i++)
    {
        sb.Add(new StringBuilder());
    }

    //Turn the columns ot rows.  
    foreach (string line in puzzleInput)
    {
        for (int i = 0; i < line.Length; i++)
        {
            sb[i].Append(line[i]);
        }
    }

    StringBuilder part1Answer = new();
    StringBuilder part2Answer = new();
    // Now that we have our new strings. Time to count letters. 
    for (int i = 0; i < sb.Count; i++)
    {
        part1Answer.Append(String.Join("",
            sb[i].ToString()
                .GroupBy(x => x)
                .OrderByDescending(g => g.Count())
                .ThenBy(x => x.Key)
                .SelectMany(x => x)
                .Distinct()
                .Take(1)));

        part2Answer.Append(String.Join("",
             sb[i].ToString()
                .GroupBy(x => x)
                .OrderBy(g => g.Count())
                .ThenBy(x => x.Key)
                .SelectMany(x => x)
                .Distinct()
                .Take(1)));
    }

    Console.WriteLine($"Part 1: The message from the most characters is: {part1Answer}");
    Console.WriteLine($"Part 2: The message from the least characters is: {part2Answer}");

}
catch (Exception e)
{
    Console.WriteLine(e);
}