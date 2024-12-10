try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<int> puzzleInput = File.ReadAllText(PUZZLE_INPUT).Select(x => x - '0').ToList();

    List<(int fileID, int length)> uncompressed = [];

    bool isFile = true;
    int fileID = 0;
    foreach (int value in puzzleInput)
    {
        if (isFile)
        {
            uncompressed.Add((fileID, value));
            fileID++;
            isFile = false;
        }
        else
        {
            if (value != 0) uncompressed.Add((-1, value));
            isFile = true;
        }
    }

    static bool IsStorage(int a) => a == -1; //Is this Clean Code? Ick.

    static long CompactFreeSpace(List<(int, int)> uncompressed)
    {
        List<int> returnValue = [];
        int headPtr = 0;
        int tailPtr = uncompressed.Count - 1;

        (int head_FileID, int head_length) = uncompressed[headPtr];
        (int tail_FileID, int tail_length) = uncompressed[tailPtr];

        void AdvanceHead()
        {
            headPtr++;
            (head_FileID, head_length) = uncompressed[headPtr];
        }

        void RetreatTail()
        {
            tailPtr--;
            (tail_FileID, tail_length) = uncompressed[tailPtr];
        }

        while (headPtr < tailPtr)
        {
            if (IsStorage(tail_FileID))
            {
                RetreatTail();
                continue;
            }
            
            if(!IsStorage(head_FileID))
            {
                returnValue.AddRange(Enumerable.Repeat(head_FileID, head_length));
                AdvanceHead();
                continue;
            }

            if (IsStorage(head_FileID))
            {
                //head can store the tail.
                if (tail_length <= head_length)
                {
                    returnValue.AddRange(Enumerable.Repeat(tail_FileID, tail_length));
                    head_length -= tail_length;
                    if (head_length == 0) AdvanceHead();
                    RetreatTail();
                }
                else //tail is too large.
                {
                    returnValue.AddRange(Enumerable.Repeat(tail_FileID, head_length));
                    tail_length -= head_length;
                    AdvanceHead();
                }
            }
        }
        if (tail_length > 0 && !IsStorage(tail_FileID))
        {
            returnValue.AddRange(Enumerable.Repeat(tail_FileID, tail_length));
        }

        return returnValue.Select((x, i) => (long)(x * i)).Sum();
    }

    static bool HasSpace((int f, int v) dest, int value) => (IsStorage(dest.f) && dest.v >= value);

    static long CompactFreeSpacePart2(List<(int, int)> uncompressed)
    {
        LinkedList<(int, int)> workingCopy = new(uncompressed);
        LinkedListNode<(int head_fileID, int head_length)> headPtr = workingCopy.First!;
        LinkedListNode<(int tail_fileID, int tail_length)> tailPtr = workingCopy.Last!;

        while (tailPtr.Previous != null && tailPtr != null)
        {
            if (IsStorage(tailPtr.Value.tail_fileID))
            {
                tailPtr = tailPtr.Previous;
                continue;
            }

            bool noSpaceFound = true;

            headPtr = workingCopy.First!;
            while (headPtr != tailPtr)
            {
                if (headPtr == tailPtr) break;
                if (HasSpace(headPtr.Value, tailPtr.Value.tail_length))
                {
                    workingCopy.AddBefore(headPtr, tailPtr.Value);
                    if (headPtr.Value.head_length > tailPtr.Value.tail_length)
                    {
                        workingCopy.AddBefore(headPtr, (-1, headPtr.Value.head_length - tailPtr.Value.tail_length));
                    }
                    workingCopy.Remove(headPtr);
                    workingCopy.AddAfter(tailPtr, (-1, tailPtr.Value.tail_length));
                    tailPtr = tailPtr.Previous!;
                    workingCopy.Remove(tailPtr.Next!);
                    noSpaceFound = false;
                    break;
                }
                else
                {
                    headPtr = headPtr.Next!;
                }
            }

            if (noSpaceFound)
            {
                tailPtr = tailPtr.Previous!;
            }
        }

        List<int> score = [];
        headPtr = workingCopy.First!;
        while(headPtr != null)
        {
            (int f, int v) = headPtr.Value;
            score.AddRange(Enumerable.Repeat(f == -1 ? 0 : f, v));
            headPtr = headPtr.Next!;
        }

        return score.Select((x, i) => (long)(x * i)).Sum();
    }

    long part1Answer = CompactFreeSpace(uncompressed);
    long part2Answer = CompactFreeSpacePart2(uncompressed);

    Console.WriteLine($"Part 1: {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}