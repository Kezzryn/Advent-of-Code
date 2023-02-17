try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<string, int> theRealSue = new()
    {
        { "children",   3 },
        { "cats",       7 },
        { "samoyeds",   2 },
        { "pomeranians",3 },
        { "akitas",     0 },
        { "vizslas",    0 },
        { "goldfish",   5 },
        { "trees",      3 },
        { "cars",       2 },
        { "perfumes",   1 }
    };


    foreach (string maybeSue in puzzleInput)
    {
        // Sue 9: vizslas: 8, cats: 2, trees: 1
        // 0   1  2        3  4     5  6      7

        string[] splitSue = maybeSue.Replace(':', ' ').Replace(',',' ').Replace("  ", " ").Split(' ');


        if (
                theRealSue[splitSue[2]] == int.Parse(splitSue[3]) && 
                theRealSue[splitSue[4]] == int.Parse(splitSue[5]) && 
                theRealSue[splitSue[6]] == int.Parse(splitSue[7])
            )
        {
            Console.WriteLine($"Part 1: Sue {splitSue[1]} is probably our aunt.");
        }

        //        In particular, the cats and trees readings indicates that there are greater than that many(due to the unpredictable nuclear decay of cat dander and tree pollen),
        //        while the pomeranians and goldfish readings indicate that there are fewer than that many(due to the modial interaction of magnetoreluctance).

        bool part2Test = true;

        for (int i = 2; i <= 6; i +=2 )
        {
            part2Test = part2Test && splitSue[i] switch
            {
                "cats" or "trees" => theRealSue[splitSue[i]] < int.Parse(splitSue[i+1]),
                "pomeranians" or "goldfish" => theRealSue[splitSue[i]] > int.Parse(splitSue[i + 1]),
                _ => theRealSue[splitSue[i]] == int.Parse(splitSue[i + 1])
            };
        }

        if (part2Test) Console.WriteLine($"Part 2: Sue {splitSue[1]} is actually our aunt.");
    }

}
catch (Exception e)
{
    Console.WriteLine(e);
}
