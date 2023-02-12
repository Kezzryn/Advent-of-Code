using AoC_2022_Day_25;

string NextPassword(string currentPassword)
{
    const string alphabet = "abcdefghijklmnopqrstuvwxyz";
    Base10Converter converter = new();
    converter.AddDictionary("ElfPassword", new TranslationKey("abcdefghjkmnpqrstuvwxyz", 0));

    // Passwords may not contain the letters i, o, or l, as these letters can be mistaken for other characters and are therefore confusing.
    // Removed i, l, o from the translation key, as per rule 2. 
    // Our puzzle input does not contain any of these characters.
    // If it did, we'd have to rethink this setup.

    string newPW = "";
    long pwTest = converter.ConvertToBase10(currentPassword, "ElfPassword");

    bool isDone = false;
    while (!isDone)
    {
        pwTest++;
        newPW = converter.ConvertFromBase10(pwTest, "ElfPassword");

        //  Passwords must include one increasing straight of at least three letters, like abc, bcd, cde, and so on, up to xyz. 
        bool ruleOne = false;
        for (int i = 0; i < alphabet.Length - 2; i++)
        {
            if (newPW.Contains(alphabet[i..(i + 3)]))
            {
                ruleOne = true;
                break;
            }
        }
        if (!ruleOne) continue;

        //  Passwords must contain at least two different,
        //  non - overlapping pairs of letters, like aa, bb, or zz.
        bool ruleThree = false;
        foreach (char a in alphabet)
        {
            int indexA = newPW.IndexOf($"{a}{a}");
            if (indexA != -1)
            {
                foreach (char b in alphabet.Except(new[] { a }))
                {
                    int indexB = newPW.IndexOf($"{b}{b}", indexA);
                    if (indexB != -1)
                    {
                        ruleThree = true;
                        break;
                    }
                }
                if (ruleThree) break;
            }
        }

        isDone = ruleOne && ruleThree;
    }

    return newPW; 
}


try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    string newPasswordPart1 = NextPassword(puzzleInput);
    string newPasswordPart2 = NextPassword(newPasswordPart1);

    Console.WriteLine($"Part 1: Santa's new password is {newPasswordPart1}.");
    Console.WriteLine($"Part 2: Santa's next password is {newPasswordPart2}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}