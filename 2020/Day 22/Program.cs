try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string CRLF = "\r\n";

    List<string> puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(CRLF + CRLF).ToList();
    List<int> puzzleDeckA = puzzleInput.First().Split(CRLF).Skip(1).Select(int.Parse).ToList();
    List<int> puzzleDeckB = puzzleInput.Last().Split(CRLF).Skip(1).Select(int.Parse).ToList();

    static int GetDeckScore(List<int> deck)
    {
        int returnValue = 0;

        for (int i = 0; i < deck.Count; i++)
        {
            returnValue += deck[i] * (deck.Count - i);
        }

        return returnValue;
    }

    static int PlayCombat(List<int> deck1, List<int> deck2)
    {
        do
        {
            if (deck1[0] > deck2[0])
            {
                deck1.Add(deck1[0]);
                deck1.Add(deck2[0]);
            }
            else
            {
                deck2.Add(deck2[0]);
                deck2.Add(deck1[0]);
            }

            deck1.RemoveAt(0);
            deck2.RemoveAt(0);

        } while (deck1.Count > 0 && deck2.Count > 0);

        return int.Max(GetDeckScore(deck1), GetDeckScore(deck2));
    }

    static int PlayRecursiveCombat(List<int> deck1, List<int> deck2, int depth = 1)
    {
        int returnValue = 0;

        HashSet<(int P1, int P2)> history = new();

        int roundWinner = -1;
        do
        {
            if (!history.Add((GetDeckScore(deck1), GetDeckScore(deck2)))) return 1; // player 1 wins, infinite loop check.

            if (deck1[0] <= (deck1.Count - 1) && deck2[0] <= (deck2.Count - 1))
            {
                roundWinner = PlayRecursiveCombat(deck1.Skip(1).Take(deck1[0]).ToList(), deck2.Skip(1).Take(deck2[0]).ToList(), depth + 1);
            }
            else
            {
                roundWinner = deck1[0] > deck2[0] ? 1 : 2;
            }

            if (roundWinner == 1)
            {
                deck1.Add(deck1[0]);
                deck1.Add(deck2[0]);
            }
            else
            {
                deck2.Add(deck2[0]);
                deck2.Add(deck1[0]);
            }

            deck1.RemoveAt(0);
            deck2.RemoveAt(0);

        } while (deck1.Count > 0 && deck2.Count > 0);

        if (depth == 1)
        {
            returnValue = int.Max(GetDeckScore(deck1), GetDeckScore(deck2));
        }
        else
        {
            returnValue = deck1.Count == 0 ? 2 : 1; // return if deck 1 or 2 won.
        }

        return returnValue;
    }

    // use new() to pass a clone of the list.
    int part1Answer = PlayCombat(new(puzzleDeckA), new(puzzleDeckB));
    Console.WriteLine($"Part 1: After a game of Combat the winning score is {part1Answer}.");

    int part2Answer = PlayRecursiveCombat(new(puzzleDeckA), new(puzzleDeckB));
    Console.WriteLine($"Part 2: After a game of Recursive Combat the winning score is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}