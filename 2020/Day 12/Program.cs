using System.Numerics;

static int TaxiDistance(Complex a, Complex b) => (int)(Math.Abs(a.Real - b.Real) + Math.Abs(a.Imaginary - b.Imaginary));

try
{
    const bool DO_PART2 = true;
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    static Complex NSEW(char direction, int value)
    {
        return direction switch
        {
            'N' => new Complex(0, value),
            'S' => new Complex(0, -value),
            'E' => new Complex(value, 0),
            'W' => new Complex(-value, 0),
            _ => throw new NotImplementedException()
        };
    }

    static Complex Rotate(char direction, int value, bool doReverse = false)
    {
        if (doReverse) 
            direction = direction == 'L' ? 'R' : 'L';

        Complex returnValue = value switch
        {
            90 => direction == 'L' ? -Complex.ImaginaryOne : Complex.ImaginaryOne,
            180 => -1,
            270 => direction == 'L' ? Complex.ImaginaryOne : -Complex.ImaginaryOne,
            _ => throw new NotImplementedException()
        };
        
        return returnValue;
    }

    int DriveBoat(bool doPart2 = false)
    {
        Complex cursor = new(0, 0);
        Complex nav = doPart2 ? new(10,1) : new(1, 0);

        foreach (string line in puzzleInput)
        {
            char inst = line[0];
            int value = int.Parse(line[1..]);

            switch (inst)
            {
                case 'L' or 'R':
                    nav *= Rotate(inst, value, doPart2);
                    break;
                case 'F':
                    cursor += nav * value;
                    break;
                default:
                    if (doPart2)
                        nav += NSEW(inst, value);
                    else
                        cursor += NSEW(inst, value);
                    break;
            }
        }
        return TaxiDistance(cursor, Complex.Zero);
    }

    int part1Answer = DriveBoat();
    int part2Answer = DriveBoat(DO_PART2);

    Console.WriteLine($"Part 1: With the initial instructions the ship will end {part1Answer} units away from the start.");
    Console.WriteLine($"Part 2: Using waypoint navigation, the ship ends {part2Answer} units from the start.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}