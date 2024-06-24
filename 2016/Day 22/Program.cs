try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<(int x, int y), (int Used, int Avail)> mapData = new();
    
    foreach(string line in puzzleInput.Skip(2))
    {
        // skip two header lines 
        // /dev/grid/node-x0-y11    94T   71T    23T   75%
        string[] s = line.Replace("T", "").Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        int[] coords = s[0].Split('-', StringSplitOptions.TrimEntries)[1..].Select(n => int.Parse(n[1..])).ToArray();

        mapData.Add((coords[0], coords[1]), (int.Parse(s[2]), int.Parse(s[3])));
    }

    int part1Answer = (from x in mapData
                        from y in mapData
                        where x.Key != y.Key
                        where x.Value.Used != 0
                        where x.Value.Used <= y.Value.Avail
                        select (x, y)).Count();

    Console.WriteLine($"Part 1: The number of viable node-pairs are: {part1Answer}.");

    // Part 2 was reasoned out in excel.
    // The unmovable data forms a wall from x1 to x25 down y21
    // cursor starts at x17 y22
    // the cursor takes 17 + 22 + 34 moves to get to the data.
    // then one move to shift the data one square. 
    // then 34 moves of 5 shifts each to shift the payload data to the exit. 

    int part2Answer = 17 + 22 + 34 + 1 + (5 * 34);
    Console.WriteLine($"Part 2: The number of moves to shift the data out is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}