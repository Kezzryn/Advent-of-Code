try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int STORAGE = -1;
    

    List<(int fileID, int value)> puzzleInput = File.ReadAllText(PUZZLE_INPUT)
        .Select(x => x - '0')
        .Select((x, i) => (int.IsEvenInteger(i) ? i / 2 : STORAGE, x))
            .ToList();
    
    static long CompactFreeSpace(List<(int, int)> puzzleData)
    {
        List<int> compacted = [];
        int headPtr = 0;
        int tailPtr = puzzleData.Count - 1;

        (int head_FileID, int head_length) = puzzleData[headPtr];
        (int tail_FileID, int tail_length) = puzzleData[tailPtr];

        void AdvanceHead() => (head_FileID, head_length) = puzzleData[++headPtr];

        void RetreatTail() => (tail_FileID, tail_length) = puzzleData[--tailPtr];

        while (headPtr < tailPtr)
        {
            switch ((head_FileID == STORAGE, tail_FileID == STORAGE))
            {
                case (true, false): //head is storage, tail is data
                    if (tail_length <= head_length)
                    {
                        compacted.AddRange(Enumerable.Repeat(tail_FileID, tail_length));
                        head_length -= tail_length;
                        if (head_length == 0) AdvanceHead();
                        RetreatTail();
                    }
                    else //tail is too large.
                    {
                        compacted.AddRange(Enumerable.Repeat(tail_FileID, head_length));
                        tail_length -= head_length;
                        AdvanceHead();
                    }
                    break;
                case (_, true): //tail is storage
                    RetreatTail();
                    break;
                case (false, _): //head is data
                    compacted.AddRange(Enumerable.Repeat(head_FileID, head_length));
                    AdvanceHead();
                    break;
            }
        }

        //capture trailing tail data, if any
        if (tail_length > 0 && tail_FileID != STORAGE)
        {
            compacted.AddRange(Enumerable.Repeat(tail_FileID, tail_length));
        }

        return compacted.Select((x, i) => (long)(x * i)).Sum();
    }

    static long CompactFreeSpacePart2(List<(int, int)> puzzleData)
    {
        LinkedList<(int, int)> workingCopy = new(puzzleData);
        LinkedListNode<(int fileID, int length)> headPtr = workingCopy.First!;
        LinkedListNode<(int fileID, int length)> tailPtr = workingCopy.Last!;

        while (tailPtr.Previous != null && tailPtr != null)
        {
            headPtr = workingCopy.First!;
            while (headPtr != tailPtr)
            {
                if (headPtr.Value.fileID == STORAGE && headPtr.Value.length >= tailPtr.Value.length)
                {
                    workingCopy.AddBefore(headPtr, tailPtr.Value);
                    if (headPtr.Value.length > tailPtr.Value.length)
                    {
                        workingCopy.AddBefore(headPtr, (STORAGE, headPtr.Value.length - tailPtr.Value.length));
                    }
                    workingCopy.Remove(headPtr);

                    workingCopy.AddAfter(tailPtr, (STORAGE, tailPtr.Value.length));
                    tailPtr = tailPtr.Previous!;
                    workingCopy.Remove(tailPtr.Next!);
                    break;
                }

                headPtr = headPtr.Next!;
            }

            do
            {
                tailPtr = tailPtr.Previous!;
            } while (tailPtr.Value.fileID == STORAGE);
        }

        List<int> score = [];
        headPtr = workingCopy.First!;
        while(headPtr != null)
        {
            (int f, int v) = headPtr.Value;
            score.AddRange(Enumerable.Repeat(f == STORAGE ? 0 : f, v));
            headPtr = headPtr.Next!;
        }

        return score.Select((x, i) => (long)(x * i)).Sum();
    }

    long part1Answer = CompactFreeSpace(puzzleInput);
    long part2Answer = CompactFreeSpacePart2(puzzleInput);

    Console.WriteLine($"Part 1: {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}