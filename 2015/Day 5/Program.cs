
using System.Text.RegularExpressions;

bool NiceStringPart1(string str)
{
    string[] badStrings = 
    {
        "ab","cd","pq","xy"
    };

    //  It does not contain the strings ab, cd, pq, or xy, even if they are part of one of the other requirements.
    if (badStrings.Any(str.Contains)) return false;

    //  It contains at least three vowels(aeiou only), like aei, xazegov, or aeiouaeiouaeiou.
    int vowelCount = str.Where(c => "aeiou".ToCharArray().Contains(c)).ToArray().Count();
    
    //  It contains at least one letter that appears twice in a row, like xx, abcdde(dd), or aabbccdd(aa, bb, cc, or dd).
    int doubleLettersCount = Regex.Matches(str, @"(.)\1").Count;

    return (vowelCount >= 3 && doubleLettersCount >= 1);
}

bool NiceStringPart2(string str)
{
    //It contains a pair of any two letters that appears at least twice in the string without overlapping, like xyxy (xy) or aabcdefgaa (aa), but not like aaa (aa, but it overlaps).
    //It contains at least one letter which repeats with exactly one letter between them, like xyx, abcdefeghi(efe), or even aaa.


    return false;
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";

    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    int nicePart1 = puzzleInput
        .Select(NiceStringPart1)
        .ToArray()
        .Sum(x => x ? 1 : 0);

    int nicePart2 = puzzleInput
        .Select(NiceStringPart2)
        .ToArray()
        .Sum(x => x ? 1 : 0);

    Console.WriteLine($"Part 1: The number of entries on the nice list are {nicePart1}.");
    Console.WriteLine($"Part 2: The number of entries on the nice list are {nicePart2}.");

}
catch (Exception e)
{
    Console.WriteLine(e);
}