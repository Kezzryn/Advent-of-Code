using System.Text;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<int> puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split('\t').Select(int.Parse).ToList();

    static string hash(List<int> data)
    {
        StringBuilder sb = new();
        for(int i = 0; i < data.Count; i++)
        {
            sb.Append($"{data[i]:00}");
        }
        return sb.ToString();
    }
    string hashText = hash(puzzleInput);

    HashSet<string> answerSet = new()
    {
        hashText
    };

    List<string> answerIndex = new()
    {
        hashText
    };

    do
    {
        int maxValue = puzzleInput.Max();
        int maxIndex = puzzleInput.FindIndex(x => x == maxValue);

        puzzleInput[maxIndex] = 0;

        for(int i = 1; i <= maxValue; i++)
        {
            int index = (i + maxIndex) % puzzleInput.Count;
            puzzleInput[index]++;
        }

        hashText = hash(puzzleInput);
        answerIndex.Add(hashText);
    } while (answerSet.Add(hashText));

    // add one 'cause indexOf is zero bound
    int part2Answer = answerIndex.Count - (answerIndex.IndexOf(hashText) + 1);

    Console.WriteLine($"Part 1: {answerSet.Count}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}