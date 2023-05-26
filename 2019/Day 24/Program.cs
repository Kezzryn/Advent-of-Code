using System.Collections;

static int getIntFromBitArray(BitArray bitArray)
{
    if (bitArray.Length > 32) throw new ArgumentException("Argument length shall be at most 32 bits.");
    int[] array = new int[1];
    bitArray.CopyTo(array, 0);
    return array[0];
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string CRLF = "\r\n";
    const int SIDE = 5;
    const int MIDPOINT = 2;

    bool[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Replace(CRLF, "").Select(x => x == '#').ToArray();

    BitArray source = new(puzzleInput);
    BitArray target = new(puzzleInput);

    LinkedList<int> recursiveBugs = new();

    HashSet<int> history = new();

    int part1Answer = 0; 
    int part2Answer = 0;

    bool isDone = false;

    while(!isDone)
    {
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

    LinkedListNode<int> cursor = recursiveBugs.First ?? recursiveBugs.AddFirst(0);
    

    const int upperNorth = 7;
    const int upperSouth = 17;
    const int upperWest = 11;
    const int upperEast = 13;

    const int lowerNorth = 0;
    const int lowerSouth = 1;
    const int lowerWest = 2;
    const int lowerEast = 3;


    int getBit(int x, int pos) => (x & (1 << pos)) != 0 ? 1 : 0;
    int setBit(int x, int pos) => x |= 1 << pos;
    int clearBit(int x, int pos) => x &= ~(1 << pos);
    
    int getLowerLayer(int x, int pos)
    {
        // X is expected to be the lower layer value. 
        int returnValue = 0;
        // n = 20-24, s = 0 - 4
        // w = 4,9, 14, 19, 24
        // e = 0, 5, 10, 15 20 
        switch (pos)
        {
            case lowerNorth:
                returnValue += getBit(x, 20);
                returnValue += getBit(x, 21);
                returnValue += getBit(x, 22);
                returnValue += getBit(x, 23);
                returnValue += getBit(x, 24);
                break;
            case lowerSouth:
                returnValue += getBit(x, 0);
                returnValue += getBit(x, 1);
                returnValue += getBit(x, 2);
                returnValue += getBit(x, 3);
                returnValue += getBit(x, 4);
                break;
            case lowerWest:
                returnValue += getBit(x, 4);
                returnValue += getBit(x, 9);
                returnValue += getBit(x, 14);
                returnValue += getBit(x, 19);
                returnValue += getBit(x, 24);
                break;
            case lowerEast:
                returnValue += getBit(x, 0);
                returnValue += getBit(x, 5);
                returnValue += getBit(x, 10);
                returnValue += getBit(x, 15);
                returnValue += getBit(x, 20);
                break;
        }
        return returnValue;
    }

    int test = 0;


    int testY(int newY, LinkedListNode<int> current)
    {
        int returnValue = 0;
        if (newY < 0)
        {
            // go up a level. 
        }
        else if (newY == 13)
        {
            // go down a level
        }
        else
        {
            // we're on the level.
        }
        return returnValue;
    }

    for (int y = 0; y < SIDE; y++)
    {
        for (int x = 0; x < SIDE; x++)
        {
            int adj = 0;

            adj += testY(-1);

            adj += (newY < 0) ?  ? 1 : 0 : 0; // north



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



    // upper
    //  n = 7, s = 17, w = 11, e = 13 

    // lower 
    // n = 20-24, s = 0 - 4
    // w = 4,9, 14, 19, 24
    // e = 0, 5, 10, 15 20 
    /*
       adj += ((y - 1) >= 0) ? source[((y - 1) * SIDE) + x] ? 1 : 0 : 0; // north
                adj += ((y + 1) <= SIDE - 1) ? source[((y + 1) * SIDE) + x] ? 1 : 0 : 0; // south

                adj += ((x - 1) >= 0) ? source[(y * SIDE) + (x - 1)] ? 1 : 0 : 0; // west
                adj += ((x + 1) <= SIDE - 1) ? source[(y * SIDE) + x + 1] ? 1 : 0 : 0; // east
     */


    //    BitArray test = new BitArray(new int[] { part1Answer });
    Console.WriteLine($"Part 1: {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}