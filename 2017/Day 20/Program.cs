using AoC_2017_Day_20;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    List<Particle> part1Particles = [];
    List<Particle> part2Particles = [];

    for (int i = 0; i < puzzleInput.Length; i++)
    {
        part1Particles.Add(new(puzzleInput[i], i));
        part2Particles.Add(new(puzzleInput[i], i));
    }

    for(int i = 0; i < 1000; i++)
    {
        part1Particles.ForEach(x => x.Step());
    }

    Console.WriteLine($"Part 1: The particle that stays the closest is {part1Particles.OrderBy(x => x.Dist).First().ID}.");

    bool isDone = false;
    int collCounter = 0;
    List<int> idToWipe = [];

    while (!isDone)
    {
        idToWipe.Clear();
        var groups = part2Particles.GroupBy(x => x.Pos).Where(x => x.Count() > 1).ToList();
        
        if (groups.Any())
        {
            collCounter = 0;
            groups.ForEach(x => idToWipe.AddRange(x.Select(s => s.ID)));
        }

        idToWipe.ForEach(x => part2Particles.RemoveAll(y => y.ID == x));

        part2Particles.ForEach(x => x.Step());

        collCounter++;
        // 11 was settled on by some experimentation after finding the answer. 
        // anything less will finish too early to detect all collisions.
        if (collCounter >= 11) isDone = true; 
    }

    Console.WriteLine($"Part 2: The number of surviving particles is {part2Particles.Count}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
