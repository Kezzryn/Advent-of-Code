static int SimulateDiracDice(int player1Pos, int player2Pos)
{
    int player1Score = 0;
    int player2Score = 0;
    int diceIndex = 0;

    int[] dice = new int[100];
    for (int i = 1; i <= dice.Length; i++) dice[i % 100] = i;
    int Roll() => dice[++diceIndex % 100] + dice[++diceIndex % 100] + dice[++diceIndex % 100];

    while (player1Score < 1000 && player2Score < 1000)
    {
        player1Pos = (player1Pos + Roll()) % 10;
        player1Score += player1Pos + 1; //+1 for modulo math fix.
        if (player1Score >= 1000) continue;
        player2Pos = (player2Pos + Roll()) % 10;
        player2Score += player2Pos + 1; //+1 for modulo math fix.
    }

    return int.Min(player1Score, player2Score) * diceIndex;
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string CRLF = "\r\n";
    const int P1Start = 0;
    const int P2Start = 1;

    // the start back one to make the board 0-9 for modulo math. 
    int[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(CRLF).Select(x => int.Parse(x.Split(' ').Last())-1).ToArray();

    int part1Answer = SimulateDiracDice(puzzleInput[P1Start], puzzleInput[P2Start]);

    List<(int roll, int times)> rollProb = new()
    {
        (3,1),(4,3),(5,6),(6,7),(7,6),(8,3),(9,1)
    };

    long p1Wins = 0;
    long p2Wins = 0;

    Queue<(int, int, int, int, long)> queue = new();
    queue.Enqueue((puzzleInput[P1Start], puzzleInput[P2Start], 0, 0, 1));

    while (queue.TryDequeue(out (int p1Pos, int p2Pos, int p1Score, int p2Score, long multiverse) queueData))
    {
        foreach ((int p1Roll, int p1Mult) in rollProb)
        {
            int newP1Pos = (queueData.p1Pos + p1Roll) % 10;
            int newP1Score = queueData.p1Score + newP1Pos + 1; //+1 for modulo math fix.
            if (newP1Score >= 21)
            {
                p1Wins += p1Mult * queueData.multiverse;
            }
            else
            {
                foreach ((int p2Roll, int p2Mult) in rollProb)
                {
                    int newP2Pos = (queueData.p2Pos + p2Roll) % 10;
                    int newP2Score = queueData.p2Score + newP2Pos + 1; //+1 for modulo math fix.
                    if (newP2Score >= 21)
                    {
                        p2Wins += p2Mult * queueData.multiverse;
                    }
                    else
                    {
                        queue.Enqueue((newP1Pos, newP2Pos, newP1Score, newP2Score, queueData.multiverse * p1Mult * p2Mult));
                    }
                }
            }
        }
    }

    long part2Answer = long.Max(p1Wins, p2Wins);

    Console.WriteLine($"Part 1: {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}