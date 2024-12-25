try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(Environment.NewLine + Environment.NewLine);

    List<List<int>> doorKeys = [];
    List<List<int>> doorLocks = [];

    foreach(string item in puzzleInput)
    {
        string[] keyOrLock = item.Split(Environment.NewLine);

        bool isLock = keyOrLock[0][0] == '#';

        List<int> itemData = Enumerable.Repeat((isLock ? int.MinValue : int.MaxValue), 5).ToList();

        for (int x = 0; x < keyOrLock[0].Length; x++)
        { 
            for (int y = 0; y < keyOrLock.Length; y++)
            {
                if (keyOrLock[y][x] != '#') continue;
                if((isLock && itemData[x] < y) || (!isLock && itemData[x] > y)) itemData[x] = y;
            }
        }

        if(isLock)
            doorLocks.Add(itemData);
        else
            doorKeys.Add(itemData);
    }

    int part1Answer = doorKeys.Sum(dk => doorLocks.Count(dl => dk.Zip(dl).All(a => a.First > a.Second)));

    Console.WriteLine($"Part 1: There are {part1Answer} potential key lock pairs.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}