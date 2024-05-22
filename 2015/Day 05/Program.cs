using System.Text.RegularExpressions;

int NiceStringPart1(string inputString)
{
    string[] badPairs = ["ab", "cd", "pq", "xy"];
    char[] vowels = ['a','e','i','o','u'];
    // It does not contain the strings ab, cd, pq, or xy,
    //  even if they are part of one of the other requirements.
    if (badPairs.Any(inputString.Contains)) return 0;

    // It contains at least three vowels.
    if (inputString.Where(c => vowels.Contains(c)).Count() < 3) return 0;

    // It contains at least one letter that appears twice in a row.
    if(Regex.Matches(inputString, @"(.)\1").Count < 1) return 0;

    return 1;
}

int NiceStringPart2(string inputString)
{
    // It contains a pair of any two letters that appears at least twice in the string without overlapping
    // EG: xyxy (xy) or aabcdefgaa (aa), but not like aaa (aa, but it overlaps).
    if (Regex.Matches(inputString, @"(..).*\1").Count < 1) return 0;

    // It contains at least one letter which repeats with exactly one letter between them, like xyx, abcdefeghi(efe), or even aaa.
    if (Regex.Matches(inputString, @"(.).\1").Count < 1) return 0;
  
    return 1;
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int part1Answer = puzzleInput.Sum(NiceStringPart1);
    int part2Answer = puzzleInput.Sum(NiceStringPart2);

    Console.WriteLine($"Part 1: The number of entries on the nice list are {part1Answer}.");
    Console.WriteLine($"Part 2: The number of entries on the nice list are {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}