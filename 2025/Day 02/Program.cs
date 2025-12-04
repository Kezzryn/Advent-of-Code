try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<List<long>> puzzleInput = [.. File.ReadAllText(PUZZLE_INPUT)
                                    .Split(',')
                                        .Select(x => x.Split('-')
                                                        .Select(y => long.Parse(y)).ToList()) ];

    long part1Answer = 0;
    long part2Answer = 0;

    static int numPlaces(long n)
    {
        if (n < 0) return numPlaces((n == long.MinValue) ? long.MaxValue : -n);
        if (n < 10) return 1;
        return 1 + numPlaces(n / 10);
    }

    //Return true if this is a VALID ID. 
    static bool IsInvalidID_Part1(long num, int len)
    {
        if (int.IsOddInteger(len)) return false;
        long pow = (long)Math.Pow(10, len/2);
        return num / pow == num % pow;
    }

    static bool CheckArray(char[] A, char[] B)
    {
        if(A.Length != B.Length) return false;
        for (int i = 0; i < A.Length; i++)
        {
            if (A[i] != B[i]) return false;
        }
        return true;
    }

    static bool IsInvalidID_Part2(long num, int len)
    {
        string numAsString = num.ToString();

        for (int i = (len / 2); i > 0; i--)
        {
            if (numAsString.Length % i == 0)
            {
                IEnumerable<char[]> chunk = numAsString.Chunk(i);
                if (chunk.All(x => CheckArray(chunk.First(), x))) return true;
            }
        }

        return false; 
    }

    foreach(List<long> range in puzzleInput)
    {
        for (long i = range[0]; i <= range[1]; i++)
        {
            int places = numPlaces(i);
            if (IsInvalidID_Part1(i, places))
            {
                part1Answer += i;
                part2Answer += i;
            }
            else if (IsInvalidID_Part2(i, places))
            {
                part2Answer += i;
            }
        }
    }

    Console.WriteLine($"Part 1: The sum of IDs with a set of digits repeated twice is {part1Answer}."); 
    Console.WriteLine($"Part 2: The sum of IDs with a set of digits repeated more than twice is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}