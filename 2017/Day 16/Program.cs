try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int NUM_DANCERS = 16;
    const long PART_1_DANCES = 1;
    const long PART_2_DANCES = 1000000000;

    string[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(',');

    string Dance(long numDances)
    {
        Dictionary<string, long> danceMoves = [];

        int[] dancers = new int[NUM_DANCERS];
        for (int i = 'a'; i <= 'p'; i++)
        {
            dancers[i % 'a'] = i;
        }
        long currentDance = 1;
        while (currentDance <= numDances)
        {
            foreach(string instruction in puzzleInput)
            {
                switch (instruction[0])
                {
                    case 's':
                        // spin
                        int spinValue = int.Parse(instruction[1..]);
                        dancers = dancers.TakeLast(spinValue).Concat(dancers.Take(dancers.Length - spinValue)).ToArray();

                        break;
                    case 'x':
                        // exchange
                        int[] exchangeIndex = instruction[1..].Split('/').Select(int.Parse).ToArray();
                        (dancers[exchangeIndex[0]], dancers[exchangeIndex[1]]) = (dancers[exchangeIndex[1]], dancers[exchangeIndex[0]]);
                        break;
                    case 'p':
                        // partner
                        int[] partnerIndex = instruction[1..].Split('/').Select(x => (int)x[0]).ToArray();
                        int indexA = Array.IndexOf(dancers, partnerIndex[0]);
                        int indexB = Array.IndexOf(dancers, partnerIndex[1]);

                        (dancers[indexA], dancers[indexB]) = (dancers[indexB], dancers[indexA]);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            //Happens after 31 loops.
            if (!danceMoves.TryAdd(String.Join("", dancers.Select(x => (char)x).ToArray()), currentDance))
            {
                break;
            }
            currentDance++;
        }

        if (danceMoves.Count >= numDances)
        {
            return danceMoves.Where(x => x.Value == numDances).Select(k => k.Key).FirstOrDefault($"{numDances} Not Found");
        }
        else
        {
            long index = numDances % danceMoves.Count;
            return danceMoves.Where(x => x.Value == index).Select(k => k.Key).FirstOrDefault($"{index} Not Found");
        }
    }

    string part1Answer = Dance(PART_1_DANCES);
    string part2Answer = Dance(PART_2_DANCES);

    Console.WriteLine($"Part 1: The final position after the dance is: {part1Answer}");
    Console.WriteLine($"Part 2: After a billion dances, their positions are: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}