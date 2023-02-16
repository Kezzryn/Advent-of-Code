using Newtonsoft.Json.Linq;

long JDocSum (JToken jToken, bool ignoreRed = false)
{
    // Recursivly iterate through the JSON document tree, counting all the values.
    // For Part 2, we ignore any objects (and their children) with "red" in one of their values.

    long returnValue = 0;
    switch (jToken.Type)
    {
        case JTokenType.Object:
            if (ignoreRed && jToken.Values().Contains("red")) return 0;
            
            foreach (JToken child in jToken.Values())
            {
                returnValue += JDocSum(child, ignoreRed);
            }
            break;
        case JTokenType.Array:
            foreach (JToken child in jToken.Children())
            {
                returnValue += JDocSum(child, ignoreRed);
            }
            break;
        case JTokenType.Integer:
            return jToken.Value<long>();
        case JTokenType.String:
            // do nothing, no numbers to count. 
            break;
        default:
            Console.WriteLine($"JT = {jToken.Type}");
            break;
    }

    return returnValue;
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);
    const bool IGNORE_RED = true;

    long answerPart1 = JDocSum(JObject.Parse(puzzleInput));
    long answerPart2 = JDocSum(JObject.Parse(puzzleInput), IGNORE_RED);

    Console.WriteLine($"Part 1: The sum of all numbers is {answerPart1}");
    Console.WriteLine($"Part 2: The sum of all numbers, excluding red, is {answerPart2}");

}
catch (Exception e)
{
    Console.WriteLine(e);
}