using AoC_2018_Day_13;
using BKH.Geometry;
 
try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<string> puzzleInput = [.. File.ReadAllLines(PUZZLE_INPUT)];

    List<MineCart> mineCarts = [];  
    Dictionary<Point2D, MapSymbols> theMap = [];

    foreach ((int x, int y) in from a in Enumerable.Range(0, puzzleInput[0].Length)
                               from b in Enumerable.Range(0, puzzleInput.Count)
                               select (a, b))
    {
        switch (puzzleInput[y][x])
        {
            case '>' or '<' or '^' or 'v':
                theMap.Add(new(x, y), MapSymbols.Straight);
                mineCarts.Add(new MineCart(x, y, puzzleInput[y][x]));
                break;
            default:
                theMap.Add(new(x, y), puzzleInput[y][x] switch
                                        {
                                            '|' or '-' => MapSymbols.Straight,
                                            '+' =>        MapSymbols.Intersection,
                                            '/' =>        MapSymbols.Curve_A,
                                            '\\' =>       MapSymbols.Curve_B,
                                            _ =>          MapSymbols.Out_Of_Bounds
                                        });
                break;
        }
    }

    string part1Answer = string.Empty;
    string part2Answer = string.Empty;

    do
    {
        foreach (MineCart cart in mineCarts.OrderBy(x => x.Position.Y).ThenBy(x => x.Position.X).Where(x => !x.IsCrashed))
        {
            Point2D nextStep = cart.TryStep(theMap[cart.Position]);
            MineCart? crashDummy = mineCarts.Find(x => x.Position == nextStep);

            if(crashDummy is not null && !crashDummy.IsCrashed)
            {
                cart.IsCrashed = true;
                crashDummy.IsCrashed = true;
                if (part1Answer == string.Empty)
                    part1Answer = $"{crashDummy.Position.X},{crashDummy.Position.Y}";
            }
            else
            {
                cart.Step(theMap[cart.Position]);
            }
        }

        if (mineCarts.Count(w => !w.IsCrashed) <= 1)
        {
            MineCart theLastCart = mineCarts.Find(x => !x.IsCrashed)!; // forgive the null We know what we're doing... honest.

            part2Answer = $"{theLastCart.Position.X},{theLastCart.Position.Y}";
        }
    } while (mineCarts.Count(w => !w.IsCrashed) >= 2);


    Console.WriteLine($"Part 1: The first crash occurs at: {part1Answer}");
    Console.WriteLine($"Part 2: The last working cart is at: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}