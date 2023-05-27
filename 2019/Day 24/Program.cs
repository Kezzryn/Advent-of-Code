static int getBit(int x, int pos) => (x & (1 << pos)) != 0 ? 1 : 0;
static int setBit(int x, int pos) => x |= 1 << pos;
//static int clearBit(int x, int pos) => x &= ~(1 << pos);

try
{
    const int SIDE = 5;
    const int CENTER_SQUARE = 12; // the center square of the board, as counted from 0. 
  
    const int MAX_GENERATIONS = 200;
    const bool DO_PART2 = true;
    
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT).Replace("\r\n", "");

    int FindAdjY(int currentX, int currentY, LinkedListNode<int> current, bool doRecursion = false)
    {
        // Bit position for upper levels
        const int UPPER_NORTH = 7; 
        const int UPPER_SOUTH = 17;
        
        // Bitmask for lower values
        const int LOWER_NORTH = 31; // north edge, 0 - 4
        const int LOWER_SOUTH = 32505856; //  south edge 20-14
        
        int returnValue = 0;

        for (int ystep = -1; ystep <= 1; ystep += 2)
        {
            int newY = currentY + ystep;
            int newPOS = (newY * SIDE) + currentX;
            
            if (newY < 0 || newY >= SIDE) // go up a level
            { 
                if (doRecursion) returnValue += current.Previous == null ? 0 : getBit(current.Previous.Value, (newY < 0) ? UPPER_NORTH : UPPER_SOUTH);
            }
            else if (doRecursion && newPOS == CENTER_SQUARE) // go down a level
            {
                returnValue += current.Next == null ? 0 : int.PopCount(current.Next.Value & ((newY > currentY) ? LOWER_NORTH : LOWER_SOUTH));
            }
            else
            {
                returnValue += getBit(current.Value, newPOS); // we're on the same level.
            }
        }
        return returnValue;
    }

    int FindAdjX(int currentX, int currentY, LinkedListNode<int> current, bool doRecursion = false)
    {
        // Bit position for upper levels
        const int UPPER_WEST = 11;
        const int UPPER_EAST = 13;

        // Bitmask for lower value
        const int LOWER_WEST = 1082401; // west edge, 0,5,10,15,20
        const int LOWER_EAST = 17318416; //east edge, 4,9,14,19,24
        int returnValue = 0;

        for (int xstep = -1; xstep <= 1; xstep += 2)
        {
            int newX = currentX + xstep;
            int newPOS = (currentX + xstep) + (currentY * SIDE);

            if (newX < 0 || newX >= SIDE)   // go up a level
            {
                if (doRecursion) returnValue += current.Previous == null ? 0 : getBit(current.Previous.Value, (newX < 0) ? UPPER_WEST : UPPER_EAST);
            }
            else if (doRecursion && newPOS == CENTER_SQUARE) // go down a level
            {
                returnValue += current.Next == null ? 0 : int.PopCount(current.Next.Value & ((newX > currentX) ? LOWER_WEST : LOWER_EAST));
            }
            else
            {
                returnValue += getBit(current.Value, newPOS); // same level.
            }
        }
        return returnValue;
    }

    int CalcBugs(string startState, bool doPart2 = false)
    {
        LinkedList<int> bugsA = new();
        LinkedList<int> bugsB = new();

        ref LinkedList<int> source = ref bugsA;
        ref LinkedList<int> target = ref bugsB;

        int startBoard = 0;
        
        for (int i = 0; i < startState.Length; i++)
        {
            if (startState[i] == '#')
                startBoard = setBit(startBoard, i);
        }

        HashSet<int> history = new()
        {
            startBoard
        };

        bugsA.AddFirst(startBoard);

        for (int generation = 0; generation <= MAX_GENERATIONS; generation++)
        {
            if (int.IsEvenInteger(generation))
            {
                source = ref bugsA;
                target = ref bugsB;
            }
            else
            {
                source = ref bugsB;
                target = ref bugsA;
            }

            target.Clear();

            if (doPart2) // expand the boards.
            {
                if ((source.First == null ? 0 : source.First.Value) != 0) source.AddFirst(0);
                if ((source.Last == null ? 0 : source.Last.Value) != 0) source.AddLast(0);
            }

            for (LinkedListNode<int>? cursor = source.First; cursor != null; cursor = cursor.Next)
            {
                int newBoard = 0;
                foreach ((int x, int y) in from y in Enumerable.Range(0, SIDE)
                                           from x in Enumerable.Range(0, SIDE)
                                           select (x, y))
                {
                    int currPos = (y * SIDE) + x;
                    if (doPart2 && currPos == CENTER_SQUARE) continue;

                    int adj = FindAdjX(x, y, cursor, doPart2) + FindAdjY(x, y, cursor, doPart2);
                    int currValue = getBit(cursor.Value, currPos);
                    
                    if (adj == 1 || (currValue == 0 && adj == 2))
                    {
                        newBoard = setBit(newBoard, currPos);
                    }
                }
                target.AddLast(newBoard);
                if (!doPart2 && !history.Add(newBoard)) return newBoard;
            } 
        }
        return source.Sum(int.PopCount);
    }

    int part1Answer = CalcBugs(puzzleInput);
    int part2Answer = CalcBugs(puzzleInput, DO_PART2);

    Console.WriteLine($"Part 1: The first repeated biodiversity index is {part1Answer}.");
    Console.WriteLine($"Part 2: The number of bugs after {MAX_GENERATIONS} is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}