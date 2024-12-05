try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string CRLF = "\r\n";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);
    
    string[] temp = puzzleInput.Split(CRLF + CRLF);
    Dictionary<int, HashSet<int>> rules = [];

    foreach (string line in temp[0].Split(CRLF))
    {
        int[] ruleNums = line.Split('|').Select(int.Parse).ToArray();

        if (!rules.TryAdd(ruleNums[0], [ruleNums[0]]))
        {
            rules[ruleNums[0]].Add(ruleNums[1]);
        };
    }

    List<List<int>> safteyManual = temp[1].Split(CRLF).Select(x => x.Split(',').Select(int.Parse).ToList()).ToList();

    bool CheckRule(int A, int B) => !(rules.TryGetValue(B, out HashSet<int>? res) && res.Contains(A));

    int part1Answer = 0;
    int part2Answer = 0;

    foreach (List<int> pages in safteyManual)
    {
        bool isValid = Enumerable.Range(1, pages.Count - 1).All(x => CheckRule(pages[x - 1], pages[x]));

        int midPoint = (pages.Count - 1) / 2;
        if (isValid)
        { 
            part1Answer += pages[midPoint];
        }
        else
        {
            // so apparently you can define the sort rules in Sort()
            pages.Sort((a, b) => rules.TryGetValue(a, out HashSet<int>? results) && results.Contains(b) ? -1 : 1);
            part2Answer += pages[midPoint];
        }
    }

    Console.WriteLine($"Part 1: The sum of the middle pages of the corretly ordered updates is {part1Answer}.");
    Console.WriteLine($"Part 2: After sorting the incorrectly ordered pages, the middle page sum is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}