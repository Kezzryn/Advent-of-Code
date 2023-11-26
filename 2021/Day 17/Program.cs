using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);
    
    int[] matches = Regex.Matches(puzzleInput, @"[-]?\d+").Select(x => int.Parse(x.Value)).ToArray();

    (int x, int y) min = (int.Min(matches[0], matches[1]), int.Min(matches[2], matches[3]));
    (int x, int y) max = (int.Max(matches[0], matches[1]), int.Max(matches[2], matches[3]));

    bool onTarget(int x, int y) => x >= min.x && x <= max.x && y >= min.y && y <= max.y;

    bool hasMissed(int x, int y, int xV)
    {
        if ((x < min.x || x > max.x) && xV == 0) return true; //falling straight down and will miss
        if (y < min.y) return true; //below 
        if (x > max.x) return true; //overshot 

        return false; 
    }

    int part1Answer = 0;
    int part2Answer = 0;

    //scattershot the hell out of this.
    //x is between 1 and x, anything larger will overshoot. Anything smaller will likewise miss.
    //for part 2, we need to aim at the target, so the initial velosity starts with min.y, as anything larger will overshoot. The range goes up to effectivly Abs(min.y) in an effort to get height.
    foreach((int X, int Y) initVelosity in from x in Enumerable.Range(1, max.x)
                                           from y in Enumerable.Range(min.y, Math.Abs(min.y * 2))
                                           select(x,y))
    {
        (int x, int y) currPos = (0, 0);
        (int vX, int vY) = initVelosity;

        int bestY = 0;
        while (!hasMissed(currPos.x, currPos.y, vX))
        {
            if (currPos.y > bestY) bestY = currPos.y;
            if(onTarget(currPos.x, currPos.y))
            {
                part2Answer++;
                if (bestY > part1Answer) part1Answer = bestY;
                break;
            }
            currPos.x += vX;
            currPos.y += vY;
            vX--;
            if (vX < 0) vX = 0;
            vY--;
        }
    }

    Console.WriteLine($"Part 1: The highest shot we can make is {part1Answer}.");
    Console.WriteLine($"Part 2: There are {part2Answer} initial velocities that will hit the target.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
