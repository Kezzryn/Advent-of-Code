try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    bool skipNext = false;
    bool inGarbage = false;

    int part1Answer = 0;
    int part2Answer = 0;
    int groupScore = 0;
    
    foreach (char symbol in puzzleInput)
    {
        if (inGarbage)
        {
            if (skipNext)
            {
                skipNext = false;
            } 
            else
            {
                switch (symbol)
                {
                    case '!':
                        skipNext = true;
                        break;
                    case '>':
                        inGarbage = false;
                        break;
                    default:
                        part2Answer++;
                        break;
                }
            }
        }
        else
        {
            switch (symbol)
            {
                case '{':
                    groupScore++;
                    break;
                case '}':
                    part1Answer += groupScore;
                    groupScore--;
                    break;
                case '<':
                    inGarbage = true;
                    break;
                default:
                    //do nothing
                    break;
            } 
        }
    }


    Console.WriteLine($"Part 1: The group score is {part1Answer}.");
    Console.WriteLine($"Part 2: The number of non-cancelled characters in the garbage is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}