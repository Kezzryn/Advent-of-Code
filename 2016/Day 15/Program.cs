using System.Text.RegularExpressions;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    List<(int start, int numPos)> discs = [];

    for (int i = 0; i < puzzleInput.Length; i++ )
    {
        //Disc #1 has 5 positions; at time=0, it is at position 4.

        MatchCollection match = Regex.Matches(puzzleInput[i], @"\d+");
        discs.Add(
            (int.Parse(match[0].Value) + int.Parse(match[3].Value),   //ID + start
            int.Parse(match[1].Value))); //numpos
    }

    static int FindAlignment(List<(int start, int numPos)> discs)
    {
        for (int time = 0; time < int.MaxValue; time++)
        {
            if (discs.All(x => 0 == (x.start + time) % x.numPos)) 
                return time;
        }
        return -1;
    }

    int part1Answer = FindAlignment(discs);
    
    discs.Add((7, 11));
    int part2Answer = FindAlignment(discs);

    Console.WriteLine($"Part 1: The first opportunity for a prize is waiting for {part1Answer} seconds. ({TimeSpan.FromSeconds(part1Answer):hh\\:mm\\:ss})");
    Console.WriteLine($"Part 2: The second opportunity for a prize is waiting for {part2Answer} seconds.({TimeSpan.FromSeconds(part2Answer):dd\\:hh\\:mm\\:ss})");
}
catch (Exception e)
{
    Console.WriteLine(e);
}