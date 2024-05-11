static int MONAD_INTCODE(int z, int w, int a, int b, int c)
{
    //inp w       //W
    //mul x 0
    int x = 0;

    //add x z
    x += z;

    //mod x 26
    x %= 26;

    //div z INPUT A    //A
    z /= a;

    //add x INPUT B    //B
    x += b;

    //eql x w
    x = x == w ? 1 : 0;

    //eql x 0
    x = x == 0 ? 1 : 0;

    //mul y 0
    int y = 0;

    //add y 25
    y += 25;

    //mul y x
    y *= x;

    //add y 1
    y += 1;

    //mul z y
    z *= y;

    //mul y 0
    y *= 0;

    //add y w
    y += w;

    //add y INPUT C     //C
    y += c;

    //mul y x
    y *= x;

    //add z y
    z += y;

    return z;
}

static int MONAD_NATIVE(int z, int w, int a, int b, int c)
{
    int x = (z % 26) + b;

    z /= a; //1 or 26 
    
    if (x != w) z = (z * 26) + w + c;

    return z;
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    //Extract the puzzle data so it's not hardcoded.
    Console.WriteLine("Parsing input.");
    int[] a = new int[14];
    int[] b = new int[14];
    int[] c = new int[14];

    int i = 0;
    foreach(string[] part in puzzleInput.Chunk(18))
    {
        a[i] = int.Parse(part[4].Split(' ').Last());
        b[i] = int.Parse(part[5].Split(' ').Last());
        c[i] = int.Parse(part[15].Split(' ').Last());

        i++;
    }

    //Work the functions backwards, recording the valid ranges that can be fed into each step. 
    Console.WriteLine();
    const int MAX_Z_VALUE = 7_200_000; //discovered through development.
    const int MAX_Z_IN = 0;
    const int MAX_Z_OUT = 1;

    int[,] ranges = new int[14,2]; // [max zinput, max zoutput]
    int z_maxTarget = 0;
    
    for (int funcID = 13; funcID >= 0; funcID--)
    {
        Console.WriteLine($"Building function ranges for function ID: {funcID}");
        int z_currMax = int.MinValue;

        for (int tryMonad = 1; tryMonad <= 9; tryMonad++)
        {
            for (int z_value = 0; z_value <= MAX_Z_VALUE; z_value++)
            {
                int ret_z_value = MONAD_NATIVE(z_value, tryMonad, a[funcID], b[funcID], c[funcID]);

                if (ret_z_value <= z_maxTarget && z_value >= z_currMax) z_currMax = z_value;
            }
        }
        ranges[funcID, MAX_Z_IN] = z_currMax;
        ranges[funcID, MAX_Z_OUT] = z_maxTarget;

        Console.WriteLine($"--Max input: {ranges[funcID, MAX_Z_IN]} Max output: {ranges[funcID, MAX_Z_OUT]}");
        z_maxTarget = z_currMax;
    }

    Console.WriteLine();
    Console.WriteLine("Building pair list.");

    List<(int, int)> pairs = [];

    int[] MONADLow = Enumerable.Repeat(int.MaxValue, 14).ToArray();
    int[] MONADHigh = Enumerable.Repeat(int.MinValue, 14).ToArray();

    const int TARGET_Z = 0;

    for (int funcID_A = 0; funcID_A <= ranges.GetUpperBound(0); funcID_A++)
    {
        for (int funcID_B = funcID_A + 1; funcID_B <= ranges.GetUpperBound(0); funcID_B++)
        {
            if (ranges[funcID_A,MAX_Z_OUT] == ranges[funcID_B, MAX_Z_IN]
            && ranges[funcID_A, MAX_Z_IN] == ranges[funcID_B, MAX_Z_OUT])
            {
                Console.WriteLine($"Matched ({funcID_A}, {funcID_B})");

                foreach ((int MONAD_A, int MONAD_B) in from x in Enumerable.Range(1, 9)
                                                       from y in Enumerable.Range(1, 9)
                                                       select (x, y))
                {
                    int retValA = MONAD_NATIVE(TARGET_Z, MONAD_A, a[funcID_A], b[funcID_A], c[funcID_A]);
                    int retValB = MONAD_NATIVE(retValA, MONAD_B, a[funcID_B], b[funcID_B], c[funcID_B]);
                    if (retValB == TARGET_Z)
                    {
                        if (MONAD_A < MONADLow[funcID_A]) MONADLow[funcID_A] = MONAD_A;
                        if (MONAD_B < MONADLow[funcID_B]) MONADLow[funcID_B] = MONAD_B;

                        if (MONAD_A > MONADHigh[funcID_A]) MONADHigh[funcID_A] = MONAD_A;
                        if (MONAD_B > MONADHigh[funcID_B]) MONADHigh[funcID_B] = MONAD_B;
                    }
                }
                Console.WriteLine($"--Function ID {funcID_A} is valid from {MONADLow[funcID_A]} to {MONADHigh[funcID_A]}");
                Console.WriteLine($"--Function ID {funcID_B} is valid from {MONADLow[funcID_B]} to {MONADHigh[funcID_B]}");
            }
        }
    }
    
    Console.WriteLine();
    Console.WriteLine("Validating.");
    int z_INTCODE_LOW = 0;
    int z_NATIVE_LOW = 0;

    int z_INTCODE_HIGH = 0;
    int z_NATIVE_HIGH = 0;

    string part1Answer = "";
    string part2Answer = "";
    for (int funcID = 0; funcID <= MONADHigh.GetUpperBound(0); funcID++)
    {
        z_INTCODE_HIGH = MONAD_INTCODE(z_INTCODE_HIGH, MONADHigh[funcID], a[funcID], b[funcID], c[funcID]);
        z_NATIVE_HIGH = MONAD_NATIVE(z_NATIVE_HIGH, MONADHigh[funcID], a[funcID], b[funcID], c[funcID]);

        part1Answer += MONADHigh[funcID].ToString();

        z_INTCODE_LOW = MONAD_INTCODE(z_INTCODE_LOW, MONADLow[funcID], a[funcID], b[funcID], c[funcID]);
        z_NATIVE_LOW = MONAD_NATIVE(z_NATIVE_LOW, MONADLow[funcID], a[funcID], b[funcID], c[funcID]);

        part2Answer += MONADLow[funcID].ToString();
    }

    if (z_INTCODE_HIGH == z_NATIVE_HIGH) Console.WriteLine("Upper bound INTCODE matches C# rewrite.");
    if (z_INTCODE_LOW == z_NATIVE_LOW) Console.WriteLine("Lower bound INTCODE matches C# rewrite.");

    Console.WriteLine();
    Console.WriteLine($"Part 1: The highest valid MONAD number is {part1Answer}.");
    Console.WriteLine($"Part 2: The lowest valid MONAD number is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}