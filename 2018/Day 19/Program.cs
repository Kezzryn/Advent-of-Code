using AoC_2018_Day_19;

static List<int> Factor(int number)
{
    // From StackOverflow. 
    // https://stackoverflow.com/questions/239865/best-way-to-find-all-factors-of-a-given-number

    var factors = new List<int>();
    int max = (int)Math.Sqrt(number);  // Round down

    for (int factor = 1; factor <= max; ++factor) // Test from 1 to the square root, or the int below it, inclusive.
    {
        if (number % factor == 0)
        {
            factors.Add(factor);
            if (factor != number / factor) // Don't add the square root twice!  Thanks Jon
                factors.Add(number / factor);
        }
    }
    return factors;
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    ChronalVM vm = new(puzzleInput);

    vm.Run();

    int part1Answer = vm.GetRegisterValue(0);

    //  vm.ClearRegisters();
    //  vm.SetRegister(0, 1);
    //  vm.Run();

    // the program calculates the sum of the factors of a number. 
    // for part 1, it calcuates the sum of the factors for 909
    // 909 is the R1 value after lines 17 to 24 are run. 

    // for part 2, it calcuates the sum of the factors for 10551309
    // this is the R1 value after lines 27 to 33 are run. 

    //line 0 is a jump to the 17->34 block of initialization code 
    //lines 1 to 16 is a pair of loops that can be expressed as: 
    /*
     *  for (int R3 = 1; R3 < R1; R3++) 
     *  {
     *      for(int R5 = 1; R5 < R1; R5++) 
     *      {
     *          if (R3 * R5 == R1)
     *          {
     *              R0 += R5;
     *          }
     *      }
     *  }
     */

    int part2Answer = Factor(10551309).Sum(); // vm.GetRegisterValue(0);

    Console.WriteLine($"Part 1: The value in register 0 is: {part1Answer}");
    Console.WriteLine($"Part 2: Initalizing register 0 to 1 results in: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}