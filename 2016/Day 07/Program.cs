bool HasABBA(string s)
{
    for (int i = 0;i <= s.Length-4;i++)
    {
        if (s[i] == s[i + 3] && s[i + 1] == s[i + 2] && s[i] != s[i + 1]) return true;
    }
    return false;
}

List<string> GetBAB(string s)
{
    List<string> list = new ();
    for (int i = 0; i <= s.Length - 3; i++)
    {
        if (s[i] == s[i + 2] && s[i] != s[i + 1]) list.Add($"{s[i + 1]}{s[i]}{s[i + 1]}");
    }
    return list;
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int part1Answer = 0;
    int part2Answer = 0;

    foreach (string line in puzzleInput)
    {
        int zeroOrOne = 1;
        string[] split = line.Split("[]".ToCharArray());
        List<string> supernet = new();
        List<string> hypernet = new();

        foreach (string word in split)
        {
            zeroOrOne = (zeroOrOne + 1) % 2; // flip it
            if (zeroOrOne == 0) supernet.Add(word); else hypernet.Add(word);
        }

        part1Answer += 
            supernet.Select(HasABBA).Where(x => x == true).Any() && 
            !hypernet.Select(HasABBA).Where(x => x == true).Any() 
            ? 1 : 0;

        part2Answer += hypernet.Select(x => supernet.SelectMany(GetBAB).ToList().Any(x.Contains)).Any(y => y == true) ? 1 : 0;
    }

    Console.WriteLine($"Part 1: {part1Answer} of IPs that support TLS.");
    Console.WriteLine($"Part 2: {part2Answer} of IPs that support SSL.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}