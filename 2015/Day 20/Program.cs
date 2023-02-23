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
    const int targetPresents = 33100000;
    const int presentsPerElfPart1 = 10;
    const int presentsPerElfPart2 = 11;
    
    int houseNumber = 0;
    int houseNumberPart1 = 0;
    int houseNumberPart2 = 0;

    while (true)
    {
        houseNumber++;
        var f = Factor(houseNumber);

        if (houseNumberPart1 == 0) if (f.Sum() * presentsPerElfPart1 >= targetPresents) houseNumberPart1 = houseNumber;
        if (houseNumberPart2 == 0) if (f.Where(x => x > houseNumber / 50).Sum() * presentsPerElfPart2 >= targetPresents) houseNumberPart2 = houseNumber;

        if (houseNumberPart1 > 0 &&  houseNumberPart2 > 0) break;
    }

    Console.WriteLine($"Part 1: The first house to get more than {targetPresents} is: {houseNumberPart1}.");

    Console.WriteLine($"Part 2: After changing to only delivering to 50 houses, the first house to get more than {targetPresents} is: {houseNumberPart2}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}