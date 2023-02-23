try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string CRLF = "\r\n";
    const string INSTRUCTION_SPLIT = " => ";

    const int SOURCE = 0;
    const int TARGET = 1;

    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    string startingMolecule = puzzleInput.Split(CRLF + CRLF)[1];
    List<string[]> puzzleTransformations = puzzleInput.Split(CRLF + CRLF)[0].Split(CRLF)
        .Select(x => x.Split(INSTRUCTION_SPLIT)).ToList();

    List<string> answersPart1 = new();

    foreach(string[] transform in puzzleTransformations)
    {
        int cutIndex = startingMolecule.IndexOf(transform[SOURCE]);
        while (cutIndex != -1) {
            int startIndex = cutIndex;

            answersPart1.Add(startingMolecule
                .Remove(startIndex, transform[SOURCE].Length)
                .Insert(startIndex, transform[TARGET])
            );

            cutIndex = startingMolecule.IndexOf(transform[SOURCE], startIndex + 1);
        }
    }

    Console.WriteLine($"Part 1: There are {answersPart1.Distinct().Count()} combos.");

    // Part 2. 
    // We need to reverse the process, so let's try for a right back search,
    // looking for the biggest matches first and reducing our item back to what it was. 
    
    string reduction = startingMolecule;
    int cutCounter = 0;
    bool isDone = false; //failsafe in case we get to a place where we have no matches, but don't have a way forward.
    while (reduction.Length > 1 || !isDone)
    {
        isDone = true;
        foreach (string[] transform in puzzleTransformations.OrderByDescending(x => x[TARGET].Length))
        {
            int cutIndex = reduction.LastIndexOf(transform[TARGET]);
            while (cutIndex != -1)
            {
                int startIndex = cutIndex;

                reduction = reduction
                    .Remove(startIndex, transform[TARGET].Length)
                    .Insert(startIndex, transform[SOURCE]);

                isDone = false;
                cutCounter++;
                cutIndex = reduction.LastIndexOf(transform[TARGET], int.Max(startIndex - 1,0));
            }
        }
    }

    Console.WriteLine($"Part 2: We need to make {cutCounter} replacements to get to the medicine molecule.");

    // ... or it turns out after looking at Reddit, that we can count certain symbols and do math and THAT works because this is
    // organic chemestry or something that I could not find the blog post on.
    // Count the number of symbols (Aa or A is one symbol) and subtract the number of Ar, Rn and twice the number of Y, symbol counts
    // and one more for the starting letter. 
}
catch (Exception e)
{
    Console.WriteLine(e);
}
