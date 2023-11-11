using System.Text;
static int CommonChars(string left, string right)
{
    // Return number of characters common to two strings.
    // Thanks stackoverflow! 
    // https://stackoverflow.com/questions/21749258/find-number-of-characters-mutual-between-two-strings-in-c-sharp
    return left.GroupBy(c => c)
        .Join(
            right.GroupBy(c => c),
            g => g.Key,
            g => g.Key,
            (lg, rg) => lg.Zip(rg, (l, r) => l).Count())
        .Sum();
}

static void Assign1478(IEnumerable<string> sourceList, string[] theNumbers)
{
    //Easy matches with unique lengths.
    foreach (string line in sourceList)
    {
        if (line.Length == 2) theNumbers[1] = line;
        if (line.Length == 4) theNumbers[4] = line;
        if (line.Length == 3) theNumbers[7] = line;
        if (line.Length == 7) theNumbers[8] = line;
    }
}

static void Assign690(IEnumerable<string> sourceList, string[] theNumbers)
{
    // Can be determined once we know what 4 and 1 is.
    foreach (string line in sourceList)
    {
        int numMatchFour = CommonChars(line, theNumbers[4]);
        int numMatchOne = CommonChars(line, theNumbers[1]);

        if (numMatchFour == 4) theNumbers[9] = line;   // The only one of this group to shares 4 lines with 4
        if (numMatchFour == 3 && numMatchOne == 1) theNumbers[6] = line;   //  shares 3 lines with 4 and 1 line with 1 
        if (numMatchFour == 3 && numMatchOne == 2) theNumbers[0] = line;   //  shares 3 lines with 4 and 2 lines with 1 
    }
}

static void Assign235(IEnumerable<string> sourceList, string[] theNumbers)
{
    // 3 contains both of 1
    // 2 and 5 both contains only one of 1 
    // 5 is the only one that contains the left upper segment, 'b'  
    // 'b' can be determined by:
    // 4 - 1 - (8 - 0) = 'b'

    char segmentB = theNumbers[4].ToCharArray()
            .Except(theNumbers[1].ToCharArray())
            .Except(theNumbers[8].ToCharArray()
                .Except(theNumbers[0].ToCharArray())
                    .First()
                    .ToString()
                )
            .First();

    foreach (string line in sourceList)
    {
        int numMatchOne = CommonChars(line, theNumbers[1]);
        if (numMatchOne == 2) theNumbers[3] = line; // shares 2 lines with 1 
        if (numMatchOne == 1 && line.Contains(segmentB)) theNumbers[5] = line; // shares 1 line with 1, and the segment to match 5
        if (numMatchOne == 1 && !line.Contains(segmentB)) theNumbers[2] = line; // And finally... the 2.
    }
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);


    int part1Answer = 0;
    int part2Answer = 0;

    foreach (string line in puzzleInput)
    {
        string[] lineValues = line.Split(" | ");
        string[] numbers = new string[10];

        // do some pre sorting so each signal set is in alphebetical order.
        List<string> signalPatterns = lineValues.First().Split(' ').Select(x => new string(x.OrderBy(c => c).ToArray())).ToList();
        List<string> displayCodes = lineValues.Last().Split(' ').Select(x => new string(x.OrderBy(c => c).ToArray())).ToList();

        Assign1478(signalPatterns.Where(x => x.Length != 5 && x.Length != 6), numbers);
        Assign690(signalPatterns.Where(x => x.Length == 6), numbers);
        Assign235(signalPatterns.Where(x => x.Length == 5), numbers);

        StringBuilder sb = new();

        foreach (string displayDigit in displayCodes)
        {
            if (displayDigit == numbers[1] || 
                displayDigit == numbers[4] ||
                displayDigit == numbers[7] ||
                displayDigit == numbers[8]) 
                    part1Answer++;

            sb.Append(Array.IndexOf(numbers, displayDigit));
        }
        part2Answer += int.Parse(sb.ToString());
    }

    Console.WriteLine($"Part 1: {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}