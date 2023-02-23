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

int numPresents;
int houseNumber = 0;

while (true)
{
    houseNumber++;
    numPresents = Factor(houseNumber).Sum() * 10;

    if (numPresents >= 33100000) break;
}

Console.WriteLine($"The house number is: {houseNumber}, which got {numPresents}.");
