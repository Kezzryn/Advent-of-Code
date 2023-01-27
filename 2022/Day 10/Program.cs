try
{
    const string PUZZLE_INPUT = "..\\..\\..\\..\\..\\Input Files\\Day 10.txt"; ;

    StreamReader sr = new(PUZZLE_INPUT);
    string op = ""; //our operation. 

    int timer = 0;  //global timer for Part 1. 

    int xRegister = 1; //Registers. X is our adder, V is the value we're adding to X. 
    int vRegister = 0;

    const int screenWidth = 39;  //screen is 40 wide, 0 to 39 
    int screenCursor = 0;

    int Part1Answer = 0;
    string Part2Answer = "";

    while (!(sr.EndOfStream && op == ""))
    {
        timer++;
        if (op == "") op = sr.ReadLine() ?? ""; //we might go past end of file if the last operation is an addx. 

        //Part 1 
        //calc on the 20th tick and every 40 after that. 
        if (timer == 20 || (timer - 20) % 40 == 0) Part1Answer += timer * xRegister;

        //Part 2 output.
        Part2Answer += $"{((screenCursor <= xRegister + 1 && screenCursor >= xRegister - 1) ? 'X' : '.')}";

        if (++screenCursor > screenWidth)
        {
            Part2Answer += '\n';
            screenCursor = 0;
        }

        switch (op[0..4])
        {
            case "noop":
                //do nothing. Wipe the command so we know we've done nothing. 
                op = "";
                break;
            case "addx":
                //load the "value" register for next loop,and tag NEXT so we don't load another op. 
                if (!int.TryParse(op[4..], out vRegister)) throw new Exception("Unable to parse addx value");
                op = "NEXT";
                break;
            case "NEXT":
                xRegister += vRegister;
                op = "";
                break;
            default:
                break;
        }
    }

    Console.WriteLine($"Part 1: Signal strength sum is: {Part1Answer}");
    Console.WriteLine();
    Console.WriteLine(Part2Answer);
}
catch (Exception e)
{
    Console.WriteLine(e);
}
