try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    int puzzleInput = int.Parse(File.ReadAllText(PUZZLE_INPUT));

    // Part one is called the Josephus Problem
    // https://www.youtube.com/watch?v=uCsD3ZGzMgE
    //answer = 2l + 1; where l is the remainder after subtracting the closest power of two from the initial number.
    int rem = puzzleInput - (1 << (int)Math.Log2(puzzleInput));
    int part1Answer = (rem * 2) + 1;

    Console.WriteLine($"Part 1: The elf that ends with all the presents is: {part1Answer}");

    /* 
     *  Part Two: 
        The only elves that we need visiblity to are at the midpoint and start/end.
         This can be simulated with a pair of queues and two rules. 
        1) If the queues are of equal length, or off by one, (even or odd) 
            then discard the head of the highQueue and dequeue out of lowQueue into highQueue 
        2) if the queues are out by more than two, dequeue out of highQueue into lowQueue
        Finish when only one item remains in both queues.
        Example 12 elves. 
        123456  <= low queue (head of queue is to the left)
        BA0987  <= high queue (head of queue is to the right) 

        1 takes from 7, then moves over. 
        23456
        1BA098

        Then 2 takes from 8 and moves over. 
        3456
        21BA09

        Lists are unbalanced, so 9 skips over before 3 goes 
        34569
        21BA0
        
        repeat
*/

    // initialize with preset capacity. 
    Queue<int> lowQueue = new(puzzleInput / 2);
    Queue<int> highQueue = new(puzzleInput / 2);

    // lowQueue gets the lower numbers. 
    for (int i = 1; i < puzzleInput / 2; i++)
    {
        lowQueue.Enqueue(i);
    }

    // highQueue gets the rest. 
    for (int i = puzzleInput / 2; i <= puzzleInput; i++)
    {
        highQueue.Enqueue(i);
    }

    do
    {
        switch (highQueue.Count - lowQueue.Count)
        {
            case 0 or 1:
                //queues are balanced or off by one, which is fine. 
                _ = highQueue.Dequeue();
                highQueue.Enqueue(lowQueue.Dequeue());
                break;
            default:
                //queues are overly unbalanced, fix them up.
                lowQueue.Enqueue(highQueue.Dequeue());
                break;
        }
    } while (highQueue.Count + lowQueue.Count != 1);

    int part2Answer = highQueue.Peek();

    Console.WriteLine($"Part 2: Going cross circle, the elf that ends with all the presents is: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}