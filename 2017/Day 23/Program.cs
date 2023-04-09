using AoC_2017_Day_23;
static bool isPrime(int number)
{
    // From : https://stackoverflow.com/questions/15743192/check-if-number-is-prime-number
    // Code by Eric Lippert 
    if (number == 1) return false;
    if (number == 2 || number == 3 || number == 5) return true;
    if (number % 2 == 0 || number % 3 == 0 || number % 5 == 0) return false;

    var boundary = (int)Math.Floor(Math.Sqrt(number));

    // You can do less work by observing that at this point, all primes 
    // other than 2 and 3 leave a remainder of either 1 or 5 when divided by 6. 
    // The other possible remainders have been taken care of.
    int i = 6; // start from 6, since others below have been handled.
    while (i <= boundary)
    {
        if (number % (i + 1) == 0 || number % (i + 5) == 0)
            return false;

        i += 6;
    }

    return true;
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Duet part1VM = new(puzzleInput);

    part1VM.Run();
    long part1Answer = part1VM.NumTimesMulInvoked();

    int part2Answer = 0;

    /*
     * VM notes: 
     * Lines 1-2 are initization
     * 5-8 are only executed if a is not zero IE: part 2 of the puzzle. 
     * 9-32 is the main loop 
     * 10-24 and 12-20 are a pair of loops that test for prime-ness by multiplying registers c * d together if they equal b
     * if they do, a flag is set to increment the counter in h.
     * There is no early escape from this, they have to go through the entire permutation of c = 2->b and d = 2->b for each loop.
     * 
     * 25-32 are flow control, bringing the value of 'b' closer to 'c' by the step in line 31 until they match, at which point the program will exit. 
     */

    // extracted the range and increment from the puzzle input. 
    // The VM code finds a count of all NON-prime numbers in a range.
    for (int i = 107900; i <= 124900; i += 17)
    {
        if (!isPrime(i)) part2Answer++;
    }

    Console.WriteLine($"Part 1: The number of times mul is invoked is {part1Answer}.");
    Console.WriteLine($"Part 2: The value of register h is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
