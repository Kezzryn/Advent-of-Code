static List<string> Rotate90(List<string> source)
{
    List<string> returnValue = new();

    for (int x = 0; x < source[0].Length; x++)
    {
        string newString = "";
        for (int y = source.Count - 1; y >= 0; y--)
        {
            newString += source[y][x];
        }
        returnValue.Add(newString);
    }

    return returnValue;
}

static int FindSplit(List<string> source, int prevMatch = -1)
{
    static bool CheckMatch(string A, string B, bool doSmudgeCheck)
    {
        int matches = Enumerable.Range(0, A.Length).Count(c => A[c] == B[c]);
        if (matches == A.Length) return true;
        if (doSmudgeCheck && matches == A.Length - 1) return true;

        return false;
    } 

    bool doCleaning = prevMatch != -1;

    for (int y = 0; y < source.Count - 1; y++)
    {
        if (CheckMatch(source[y], source[y + 1], doCleaning))
        {
            if (doCleaning && prevMatch == y + 1) continue; // skip prev seen.

            bool isMatch = true;
            int numToEdge = int.Min(y, source.Count - (y + 2));
            for (int j = 1; j <= numToEdge; j++)
            {
                isMatch = CheckMatch(source[y - j], source[y + j + 1], doCleaning);
            }
            if (isMatch) return y + 1;
        }
    }

    return 0;
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string CRLF = "\r\n";
    List<List<string>> puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(CRLF + CRLF).Select(x => x.Split(CRLF).ToList()).ToList();
     
    int part1Answer = 0; 
    int part2Answer = 0;

    foreach (List<string> mirror in puzzleInput)
    {
        List<string> spinMirror = Rotate90(mirror);
        int score = FindSplit(mirror) ;
        int spinScore = FindSplit(spinMirror);

        int cleanScore = FindSplit(mirror, score);
        int spinCleanScore = FindSplit(mirror, spinScore);

        part1Answer += (score * 100) + spinScore;
        part2Answer += (cleanScore  * 100) + spinCleanScore;
    }

    Console.WriteLine($"Part 1: The summarization of the reflection lines is {part1Answer}.");
    Console.WriteLine($"Part 2: After cleaning, the summarization of the reflection lines is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}