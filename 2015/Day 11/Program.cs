using BKH.Base10Converter;

static string NextPassword(string currentPassword)
{
    TranslationMap elfDictionary = new("abcdefghjkmnpqrstuvwxyz");
    // Removed i, l, o from the translation map, as per Rule 2: Passwords may not contain the letters i, o, or l, as these letters can be mistaken for other characters and are therefore confusing.

    string newPW = "";
    long pwTest = Base10Converter.ToBase10(currentPassword, elfDictionary);

    bool isDone = false;
    while (!isDone)
    {
        pwTest++;
        newPW = Base10Converter.FromBase10(pwTest, elfDictionary);

        //  Passwords must include one increasing straight of at least three letters, like abc, bcd, cde, and so on, up to xyz. 
        bool ruleOne = false;
        for (int i = 0; i < newPW.Length - 2; i++)
        {
            if (newPW[i] + 1 == newPW[i + 1] && newPW[i] + 2 == newPW[i + 2]) ruleOne = true;
        }
        if (!ruleOne) continue;

        //  Passwords must contain at least two different,
        //  non - overlapping pairs of letters, like aa, bb, or zz.
        bool ruleThree = (from a in newPW.Distinct()
                          from b in newPW.Distinct().Except([a])
                          select (a, b)).Any(x => newPW.Contains($"{x.a}{x.a}") && newPW.Contains($"{x.b}{x.b}"));

        isDone = ruleOne && ruleThree;
    }

    return newPW; 
}


try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    string part1Answer = NextPassword(puzzleInput);
    string part2Answer = NextPassword(part1Answer);

    Console.WriteLine($"Part 1: Santa's new password is {part1Answer}.");
    Console.WriteLine($"Part 2: Santa's next password is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}