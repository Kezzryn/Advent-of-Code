static int ReactPolymer(LinkedList<int> polymer)
{
    const int DIFF = 32; // A = 65  a = 97 

    if (polymer.First == null) throw new NullReferenceException();
    LinkedListNode<int> cursor = polymer.First;

    while (cursor.Next != null)
    {
        if (Math.Abs(cursor.Value - cursor.Next.Value) == DIFF)
        {
            polymer.Remove(cursor.Next);

            if (cursor.Previous == null)
            {
                polymer.Remove(cursor);
                cursor = polymer.First;
            }
            else
            {
                cursor = cursor.Previous;
                polymer.Remove(cursor.Next!);   // forgive the null as the cursor came from there.
            }
        }
        else
        {
            cursor = cursor.Next;
        }
    }

    return polymer.Count;
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    LinkedList<int> puzzleInput = new(File.ReadAllText(PUZZLE_INPUT).Select(x => (int)x));

    int part1Answer = ReactPolymer(puzzleInput);

    int part2Answer = int.MaxValue;
    char droppedLetter = '\0';
    
    foreach (int c in Enumerable.Range('A', 26))
    {
        LinkedList<int> temp = new(puzzleInput.Where(x => (x != c) && (x != c+32)));

        int length = ReactPolymer(temp);
        if (length < part2Answer)
        {
            droppedLetter = (char)c;
            part2Answer = length;
        }
    }

    Console.WriteLine($"Part 1: The length of the polymer is {part1Answer} after all reactions have taken place.");
    Console.WriteLine($"Part 2: By dropping {droppedLetter} the reaction can be reduced down to {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
