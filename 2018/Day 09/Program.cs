try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    int totalElves = puzzleInput.Split(' ').Take(1).Select(int.Parse).First();
    int totalMarbles = puzzleInput.Split(' ').TakeLast(2).Take(1).Select(int.Parse).First();

    static long PlayWithMarbles(int numPlayers, int numMarbles)
    {
        LinkedList<int> marbles = new();
        LinkedListNode<int> cursor = marbles.AddFirst(0);

        long[] playerScore = new long[numPlayers];

        for (int i = 1; i <= numMarbles; i++)
        {
            if ((i % 23) == 0)
            {
                int currentPlayer = i % numPlayers;
                playerScore[currentPlayer] += i;

                for (int z = 0; z < 7; z++)
                {
                    cursor = cursor.Previous ?? marbles.Last!;
                }

                playerScore[currentPlayer] += cursor.Value;
                cursor = cursor.Next ?? marbles.First!;
                marbles.Remove(cursor.Previous ?? marbles.Last!);
            }
            else
            {
                cursor = marbles.AddAfter(cursor.Next ?? marbles.First!, i);
            }
        }

        return playerScore.Max();
    }

    long part1Answer = PlayWithMarbles(totalElves, totalMarbles);
    long part2Answer = PlayWithMarbles(totalElves, totalMarbles * 100);

    Console.WriteLine($"Part 1: The top score is {part1Answer}.");
    Console.WriteLine($"Part 2: Using 100 times more marbles, the top score is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}