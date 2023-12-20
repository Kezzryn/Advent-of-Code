global using XMAS = int[];
global using RULE = (int xmasIndex, char opCode, int compareValue, string result);

using System.Text.RegularExpressions;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string CRLF = "\r\n";
    string[] puzzleInput = [.. File.ReadAllText(PUZZLE_INPUT).Split(CRLF + CRLF)];

    Dictionary<char, int> XMASMap = new()
    {
        {'x', 0}, {'m', 1}, {'a', 2}, {'s', 3}
    };

    Dictionary<string, List<RULE>> rules = [];
    foreach (string line in puzzleInput[0].Split(CRLF))
    {
        //px{a<2006:qkq,m>2090:A,rfg}
        string label = line[..line.IndexOf('{')];

        rules.Add(label, []);
        foreach (string rule in line[(line.IndexOf('{')+1)..^1].Split(','))
        {
            RULE tempRule = (-1, 'E', -1, rule); //default as end node.
            if (rule.Contains(':'))
            {
                tempRule.xmasIndex = XMASMap[rule[0]];
                tempRule.opCode = rule[1];
                tempRule.compareValue = int.Parse(rule[2..rule.IndexOf(':')]);
                tempRule.result = rule[(rule.IndexOf(':') + 1)..];
            }
            rules[label].Add(tempRule);
        }
    }

    List<XMAS> xmas = puzzleInput[1].Split(CRLF)
        .Select(s => Regex.Matches(s, @"\d+")
            .Select(s => int.Parse(s.Value))
            .ToArray())
        .ToList();

    int CheckParts(XMAS xmas, string ruleName)
    {
        foreach (RULE rule in rules[ruleName])
        {
            if (rule.opCode == 'E' ||
                (rule.opCode == '>' && xmas[rule.xmasIndex] > rule.compareValue) ||
                (rule.opCode == '<' && xmas[rule.xmasIndex] < rule.compareValue))
            {
                if (rule.result == "A") return xmas.Sum();
                if (rule.result == "R") return 0;
                return CheckParts(xmas, rule.result);
            }
        }
        return 0;
    }
    
    long CheckCombos(XMAS startMin, XMAS startMax, string ruleName)
    {
        XMAS min = new int[startMin.Length];
        Array.Copy(startMin, min, startMin.Length);

        XMAS max = new int[startMax.Length];
        Array.Copy(startMax, max, startMax.Length);

        long DoResult(string result)
        {
            if (result == "R") return 0;
            if (result == "A")
            {
                long returnValue = 1;
                for (int i = max.GetLowerBound(0); i <= max.GetUpperBound(0); i++)
                {
                    returnValue *= ((max[i] - min[i]) + 1);
                }
                return returnValue;
            }
            return CheckCombos(min, max, result);
        }

        long returnValue = 0;
        foreach (RULE rule in rules[ruleName])
        {
            switch (rule.opCode)
            {
                case 'E':
                    returnValue += DoResult(rule.result);
                    break;
                case '<':
                    int tempMax = max[rule.xmasIndex];
                    if (min[rule.xmasIndex] < rule.compareValue)
                    {
                        max[rule.xmasIndex] = int.Min(max[rule.xmasIndex], rule.compareValue - 1);
                        returnValue += DoResult(rule.result);
                    }
                    // reset and adjust to do the "else" branch on the next loop
                    max[rule.xmasIndex] = tempMax;
                    min[rule.xmasIndex] = int.Max(startMin[rule.xmasIndex], rule.compareValue);
                    break;
                case '>':
                    int tempMin = min[rule.xmasIndex];
                    if (max[rule.xmasIndex] > rule.compareValue)
                    {
                        min[rule.xmasIndex] = int.Max(min[rule.xmasIndex], rule.compareValue + 1);
                        returnValue += DoResult(rule.result);
                    }
                    // reset and adjust to do the "else" branch on the next loop
                    min[rule.xmasIndex] = tempMin;
                    max[rule.xmasIndex] = int.Min(startMax[rule.xmasIndex], rule.compareValue);
                    break;
                default:
                    throw new NotImplementedException($"unknown rule.opCode: {rule.opCode}");
            };
        }

        return returnValue;
    }

    int part1Answer = xmas.Sum(s => CheckParts(s, "in"));
    long part2Answer = CheckCombos([1, 1, 1, 1], [4000, 4000, 4000, 4000], "in");

    Console.WriteLine($"Part 1: The sum of the rating numbers of the accepted parts {part1Answer}.");
    Console.WriteLine($"Part 2: There are {part2Answer} effective combinations.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}