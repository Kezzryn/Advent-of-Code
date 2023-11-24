List<char> openBrace = new() // order must match closeBrace
    {
        '<', '{', '[', '('
    };

List<char> closeBrace = new() // order must match openBrace
    {
        '>', '}', ']', ')'
    };

Dictionary<char, char> expectedMatch = new()
{
    { '<', '>' },
    { '{', '}' },
    { '[', ']' },
    { '(', ')' },

};

char FindMatchingBrace(string inputString, int startPos)
{

    int matchCount = 0;

    for (int i = startPos; i < inputString.Length; i++)
    {
        if (openBrace.Contains(inputString[i])) matchCount++;
        if (closeBrace.Contains(inputString[i])) matchCount--;
        if (matchCount == 0) return inputString[i];
    }

    return '\0';
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<string> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).ToList();
    List<string> incomplete = new();

    int part1Answer = 0;
    foreach (string line in puzzleInput)
    {
        bool isInvalid = false;
        for (int i = 0; i < line.Length; i++)
        {
            if (openBrace.Contains(line[i]))
            {
                char matchBrace = FindMatchingBrace(line, i);

                if (matchBrace != '\0' && matchBrace != expectedMatch[line[i]])
                {
                    isInvalid = true;
                    part1Answer += matchBrace switch
                    {
                        ')' => 3,
                        ']' => 57,
                        '}' => 1197,
                        '>' => 25137,
                        _ => throw new Exception($"{line[i]} => {matchBrace} @ {i} != {expectedMatch}\n{line}")
                    };
                }
                if (isInvalid) break;
            }
        }
        if (!isInvalid) incomplete.Add(line);
    }

    List<long> autocompleteScores = new();

    foreach(string line in incomplete)
    {
        List<char> missingItems = new();
        string temp = String.Empty;
        for (int i = 0; i < line.Length; i++)
        {
            if (openBrace.Contains(line[i]))
            {
                char matchBrace = FindMatchingBrace(line, i);
                if (matchBrace == '\0')
                {
                    missingItems.Insert(0, expectedMatch[line[i]]);
                }
            }
        }

        long score = 0; 
        foreach(char c in missingItems)
        {
            score *= 5;
            score += c switch
            {
                ')' => 1,
                ']' => 2,
                '}' => 3,
                '>' => 4,
                _ => throw new Exception()
            };
        }
        autocompleteScores.Add(score);
    }

    autocompleteScores.Sort();
    Console.WriteLine($"{autocompleteScores.Count / 2}");
    long part2Answer = autocompleteScores[(autocompleteScores.Count) / 2];

    Console.WriteLine($"Part 1: The syntax error score is {part1Answer}.");
    Console.WriteLine($"Part 2: The autocorrect error score is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}