using Newtonsoft.Json.Linq;

static long JDocSum (JToken jToken, bool ignoreRed = false)
{
    // Recursivly iterate through the JSON document tree, counting all the values.
    // For Part 2, we ignore any objects (and their children) with "red" in one of their values.

    return (jToken.Type) switch
    {
        JTokenType.Object => (ignoreRed && jToken.Values().Contains("red")) ? 0 : jToken.Values().Sum(x => JDocSum(x, ignoreRed)),
        JTokenType.Array => jToken.Children().Sum(x => JDocSum(x, ignoreRed)),
        JTokenType.Integer => jToken.Value<long>(),
        JTokenType.String => 0,
        _ => 0
    };
}

try
{
    const bool IGNORE_RED = true;
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    long part1Answer = JDocSum(JObject.Parse(puzzleInput));
    long part2Answer = JDocSum(JObject.Parse(puzzleInput), IGNORE_RED);

    Console.WriteLine($"Part 1: The sum of all numbers is {part1Answer}");
    Console.WriteLine($"Part 2: The sum of all numbers, excluding red, is {part2Answer}");

}
catch (Exception e)
{
    Console.WriteLine(e);
}