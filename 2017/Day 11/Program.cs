try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(',');

    int part2Answer = 0;

    static int HexTaxiDistance(int taxiX,  int taxiY)
    {
        int rv = 0;

        taxiY = taxiY < 0 ? Math.Abs(taxiY + 1) : Math.Abs(taxiY);
        taxiX = Math.Abs(taxiX);

        rv = taxiY < (taxiX / 2) ? taxiX : taxiX + taxiY - (taxiX / 2);

        return rv;
    }

    int x = 0; 
    int y = 0;
    foreach (string step in puzzleInput)
    {
        switch (step)
        {
            case "n":
                y++;
                break;
            case "s":
                y--;
                break;
            case "nw":
                if ((Math.Abs(x) % 2) == 1) y++;
                x--;
                break;
            case "ne":
                if ((Math.Abs(x) % 2) == 1) y++;
                x++;
                break;
            case "sw":
                if ((Math.Abs(x) % 2) == 0) y--;
                x--;
                break;
            case "se":
                if ((Math.Abs(x) % 2) == 0) y--;
                x++;
                break;
            default:
                Console.WriteLine($"missing {step}");
                break;
        }
        part2Answer = int.Max(HexTaxiDistance(x, y), part2Answer);
    }

    int part1Answer = HexTaxiDistance(x, y); 

    Console.WriteLine($"Part 1: The child process is {part1Answer} hexes away.");
    Console.WriteLine($"Part 2: The farthest the child process ever got away was {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}