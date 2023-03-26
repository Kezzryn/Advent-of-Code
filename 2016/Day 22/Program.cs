bool isViable(KeyValuePair<(int x, int y), (int size, int used, int avail)> a, KeyValuePair<(int x, int y), (int size, int used, int avail)> b)
{
    if (a.Key == b.Key) return false;  //Nodes A and B are not the same node.
    if (a.Value.used == 0) return false; // viable if Node A is not empty(its Used is not zero).

    if (a.Value.used <= b.Value.avail) return true; // The data on node A(its Used) would fit on node B(its Avail).

    return false;
}


try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<(int x, int y), (int size, int used, int avail)> mapData = new();

    // skip two header lines 
    for (int i = 2; i < puzzleInput.Length; i++)
    {
        // /dev/grid/node-x0-y11    94T   71T    23T   75%

        string[] s = puzzleInput[i].Replace("T", "").Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        int[] coords = s[0].Split('-', StringSplitOptions.TrimEntries)[1..].Select(n => int.Parse(n[1..])).ToArray();

        mapData.Add((coords[0], coords[1]), (int.Parse(s[1]), int.Parse(s[2]), int.Parse(s[3])));
    }

    int part1Answer = 0;
    foreach (KeyValuePair<(int x, int y), (int size, int used, int avail)> kvp in mapData)
    {
        foreach (KeyValuePair<(int x, int y), (int size, int used, int avail)> other_kvp in mapData)
        {
            part1Answer += isViable(kvp, other_kvp) ? 1 : 0;
        }
    }

    Console.WriteLine($"Part 1: The number of viable node-pairs are: {part1Answer}.");

    // Part 2 was reasoned out in excel.
    // The unmovable data forms a wall from x1 to x25 down y21
    // cursor starts at x17 y 22
    // the cursor takes 17 + 22 + 34 moves to get to the data.
    // then one move to shift the data one square. 
    // then 34 moves of 5 shifts each to shift the payload data to the exit. 

    int part2Answer = 17 + 22 + 35 + (5 * 34);
    Console.WriteLine($"Part 2: The number of moves to shift the data out is {part2Answer}.");

}
catch (Exception e)
{
    Console.WriteLine(e);
}

