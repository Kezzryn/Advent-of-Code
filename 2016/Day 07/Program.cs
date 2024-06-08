static bool HasABBA(string s)
{
    for (int i = 0; i <= s.Length - 4; i++)
    {
        if (s[i] == s[i + 3] && s[i + 1] == s[i + 2] && s[i] != s[i + 1]) return true;
    }
    return false;
}

static List<string> GetBAB(string s)
{
    List<string> list = [];
    for (int i = 0; i <= s.Length - 3; i++)
    {
        //if ABA, add BAB 
        if (s[i] == s[i + 2] && s[i] != s[i + 1])
            list.Add($"{s[i + 1]}{s[i]}{s[i + 1]}");
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
        string[] split = line.Split("[]".ToCharArray());

        List<string> supernet = split.Where((_, i) => int.IsEvenInteger(i)).ToList();
        List<string> hypernet = split.Where((_, i) => int.IsOddInteger(i)).ToList();

        if (supernet.Any(HasABBA) && !hypernet.Any(HasABBA)) part1Answer++;
        if (hypernet.Any(x => supernet.SelectMany(GetBAB).Any(x.Contains))) part2Answer++;
    }

    Console.WriteLine($"Part 1: {part1Answer} of the IPs support TLS.");
    Console.WriteLine($"Part 2: {part2Answer} of the IPs support SSL.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}