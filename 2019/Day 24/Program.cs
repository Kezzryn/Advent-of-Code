using System.Collections;

static int getIntFromBitArray(BitArray bitArray)
{

    if (bitArray.Length > 32)
        throw new ArgumentException("Argument length shall be at most 32 bits.");

    int[] array = new int[1];
    bitArray.CopyTo(array, 0);
    return array[0];
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string CRLF = "\r\n";
    const int SIDE = 5;
    const int LENGHT = SIDE * SIDE; 

    bool[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Replace(CRLF, "").Select(x => x == '#').ToArray();

    BitArray source = new(puzzleInput);
    BitArray target = new(puzzleInput);

    HashSet<int> history = new();

    int part1Answer = 0; 
    int part2Answer = 0;

    bool isDone = false;
    int ctr = 0;
    while(!isDone)
    {
        ctr++;
        for (int y = 0; y < SIDE; y++)
        { 
            for (int x = 0; x < SIDE; x++)
            {
                int adj = 0;
                adj += ((y - 1) >= 0) ? source[((y - 1) * SIDE) + x] ? 1 : 0 : 0; // north
                adj += ((y + 1) <= SIDE - 1) ? source[((y + 1) * SIDE) + x] ? 1 : 0 : 0; // south

                adj += ((x - 1) >= 0) ? source[(y * SIDE) + (x - 1)] ? 1 : 0 : 0; // west
                adj += ((x + 1) <= SIDE - 1) ? source[(y * SIDE) + x + 1] ? 1 : 0 : 0; // east

                if (source[(y * SIDE) + x])
                {
                    target[(y * SIDE) + x] = adj == 1;
                }
                else
                {
                    target[(y * SIDE) + x] = adj == 1 || adj == 2;
                }
            }
        }

        source = new BitArray(target);
        if (!history.Add(getIntFromBitArray(source)))
        {
            isDone = true;
            part1Answer = getIntFromBitArray(source);
        }
    }


    Console.WriteLine(ctr);

    Console.WriteLine($"Part 1: {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}