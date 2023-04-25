using AoC_2018_Day_13;
using System.Numerics;
 
try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    List<MineCart> mineCarts = new();  
    Dictionary<Complex, MapSymbols> theMap = new();
    Dictionary<Complex, int> crashTest = new();

    int idNum = 0;
    foreach ((int x, int y) in from a in Enumerable.Range(0, puzzleInput[0].Length)
                              from b in Enumerable.Range(0, puzzleInput.Length)
                              select (a, b))
    {
        switch (puzzleInput[y][x])
        {
            case '>' or '<' or '^' or 'v':
                theMap.Add(new(x, y), MapSymbols.Straight);
                mineCarts.Add(new MineCart(x, y, puzzleInput[y][x], idNum++));
                crashTest.Add(mineCarts.Last().Position, mineCarts.Last().ID);
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

    bool isDone = false;

    string part1Answer = string.Empty;
    string part2Answer = string.Empty;

    while (!isDone)
    {
        foreach(MineCart cart in mineCarts.OrderBy(x => x.Position.Imaginary).ThenBy(x => x.Position.Real))
        {
            if (cart.IsCrashed) continue;

            crashTest.Remove(cart.Position);
            cart.Step();
    
            if (crashTest.TryAdd(cart.Position, cart.ID))
            {
                switch (theMap[cart.Position])
                {
                    case MapSymbols.Intersection:
                        cart.Intersection();
                        break;
                    case MapSymbols.Curve_A or MapSymbols.Curve_B:
                        cart.FollowCurve(theMap[cart.Position]);
                        break;
                    case MapSymbols.Out_Of_Bounds:
                        throw new Exception($"{cart.Position} is out of bounds");
                    default:
                        break;
                }
            }
            else
            {
                if (part1Answer == string.Empty) part1Answer = $"{cart.Position.Real},{cart.Position.Imaginary}";
                cart.IsCrashed = true;

                int cartID = crashTest[cart.Position];
                mineCarts.Where(w => w.ID == cartID).First().IsCrashed = true;
                crashTest.Remove(cart.Position);
            }
        }

        var workingCarts = mineCarts.Where(w => w.IsCrashed == false);
        if (workingCarts.Count() == 1)
        {
            part2Answer = $"{workingCarts.First().Position.Real},{workingCarts.First().Position.Imaginary}";
            isDone = true;
        }
    }

    
    Console.WriteLine($"Part 1: The first crash occurs at: {part1Answer}");
    Console.WriteLine($"Part 2: The last working cart is at: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}