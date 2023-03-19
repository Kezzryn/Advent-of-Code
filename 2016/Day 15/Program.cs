using System.Text.RegularExpressions;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    List<(int id, int start, int numpos)> discs = new();

    for (int i = 0; i < puzzleInput.Length; i++ )
    {
        //Disc #1 has 5 positions; at time=0, it is at position 4.

        var match = Regex.Matches(puzzleInput[i], @"\d+");
        discs.Add(
            (int.Parse(match[0].Value),  //ID
            int.Parse(match[3].Value),   //start
            int.Parse(match[1].Value))); //numpos
    }

    bool isValid = false;
    int part1Answer = -1;
    int part2Answer = -1;
    int time = 0;
    while (!isValid)
    {
        time++;
        isValid = true;
        foreach (var (id, start, numpos) in discs)
        {
            isValid = isValid && 0 == (start + id + time) % numpos;
            if (!isValid) break;
        }

        if (isValid)
        {
            if (part1Answer == -1) // reset for part 2
            {
                part1Answer = time;
                discs.Add((7, 0, 11));
                time = 0;
                isValid = false;
            } 
            else if (part2Answer == -1) 
            {
                part2Answer = time;
            }
        }
    }

    Console.WriteLine($"Part 1: The first opportunity for a prize is waiting for {part1Answer} seconds. ({TimeSpan.FromSeconds(part1Answer):hh\\:mm\\:ss})");
    Console.WriteLine($"Part 2: The second opportunity for a prize is waiting for {part2Answer} seconds.({TimeSpan.FromSeconds(part2Answer):dd\\:hh\\:mm\\:ss})");

}
catch (Exception e)
{
    Console.WriteLine(e);
}