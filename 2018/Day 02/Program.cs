try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int numDouble = 0;
    int numTriple = 0;

    foreach (string line in puzzleInput)
    {
        var grouped = line.ToCharArray().GroupBy(g => g);
        numDouble += grouped.Any(x => x.Count() == 2) ? 1 : 0;
        numTriple += grouped.Any(x => x.Count() == 3) ? 1 : 0;
    }

    int part1Answer = numDouble * numTriple;
    Console.WriteLine($"Part 1: The checksum for the box IDs is {part1Answer}.");

    //// from megasolution thread.
    //// very nice way to do this.
    //for (int i = 0; i < puzzleInput[0].Length; i++)
    //{
    //    var pair = puzzleInput.Select(id => id.Remove(i, 1)).GroupBy(id => id).FirstOrDefault(g => g.Count() > 1);
    //    if (pair != null)
    //    {
    //       var common = pair.First();
    //       return common;
    //    }
    //}

    string part2Answer = "";
    bool foundMatch = false;
    for (int boxAIndex = 0; boxAIndex < puzzleInput.Length - 1; boxAIndex++)
    {
        char[] boxACharAry = puzzleInput[boxAIndex].ToCharArray();
        for (int boxBIndex = boxAIndex + 1; boxBIndex < puzzleInput.Length; boxBIndex++)
        {
            var boxBCharAry = puzzleInput[boxBIndex].ToCharArray();

            int numDiffs = 0;
            int diffIndex = 0;
            for(int i  = 0; i < boxBCharAry.Length; i++)
            {
                if (boxBCharAry[i] != boxACharAry[i])
                {
                    numDiffs++;
                    diffIndex = i;
                }
            }

            if (numDiffs == 1)
            {
                foundMatch = true;
                part2Answer = puzzleInput[boxAIndex][..diffIndex] + puzzleInput[boxAIndex][(diffIndex + 1)..];
                break;
            }
        }
            if (foundMatch) break;
    }

    Console.WriteLine($"Part 2: The common letters of the box IDs are {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}