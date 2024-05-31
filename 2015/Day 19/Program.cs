try
{
    const string CRLF = "\r\n";
    const string INSTRUCTION_SPLIT = " => ";

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    List<(string source, string target)> puzzleTransformations =
        puzzleInput.Split(CRLF + CRLF)
                        .First()
                        .Split(CRLF)
                            .Select(x => x.Split(INSTRUCTION_SPLIT))
                                .Select(x => (x.First(), x.Last()))
                            .ToList();

    string startingMolecule = puzzleInput.Split(CRLF + CRLF).Last();

    HashSet<string> part1Answers = [];

    foreach((string source, string target) in puzzleTransformations)
    {
        int cutIndex = startingMolecule.IndexOf(source);
        while (cutIndex != -1) {
            int startIndex = cutIndex;

            part1Answers.Add(startingMolecule
                .Remove(startIndex, source.Length)
                .Insert(startIndex, target)
            );

            cutIndex = startingMolecule.IndexOf(source, startIndex + 1);
        }
    }

    Console.WriteLine($"Part 1: There are {part1Answers.Count} combos.");

    // Part 2. 
    // We need to reverse the process, so let's try for a right back search,
    // looking for the biggest matches first and reducing our item back to what it was. 
    
    int part2Answer = 0;
    bool isDone = false; //failsafe in case we get to a place where we have no matches, but don't have a way forward.
    while (startingMolecule.Length > 1 || !isDone)
    {
        isDone = true;
        foreach ((string source, string target) in puzzleTransformations.OrderByDescending(x => x.target.Length))
        {
            int cutIndex = startingMolecule.LastIndexOf(target);
            while (cutIndex != -1)
            {
                startingMolecule = startingMolecule
                    .Remove(cutIndex, target.Length)
                    .Insert(cutIndex, source);

                isDone = false;
                part2Answer++;
                cutIndex = startingMolecule.LastIndexOf(target, int.Max(cutIndex - 1,0));
            }
        }
    }

    Console.WriteLine($"Part 2: We need to make {part2Answer} replacements to get to the medicine molecule.");

    // ... or it turns out after looking at Reddit, that we can count certain symbols and do math and THAT works because this is
    // organic chemestry or something that I could not find the blog post on.
    // Count the number of symbols (Aa or A is one symbol) and subtract the number of Ar, Rn and twice the number of Y, symbol counts
    // and one more for the starting letter. 
}
catch (Exception e)
{
    Console.WriteLine(e);
}
