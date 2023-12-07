static int HandScore(string hand, bool JokersWild = false)
{
    const int FIVE_OF_A_KIND = 26;
    const int FOUR_OF_A_KIND = 25;
    const int FULL_HOUSE = 24;
    const int THREE_OF_A_KIND = 23;
    const int TWO_PAIR = 22;
    const int ONE_PAIR = 21;
    const int HIGH_CARD = 0;

    string score = JokersWild ? "ZJ23456789TQKA" : "Z23456789TJQKA"; //use the leading Z to make this a base 1 list 
    int returnValue = 1;

    int distinctCount = hand.Distinct().Count();
    int groupByMax = hand.GroupBy(x => x).Select(x => x.Count()).Max();
    int numJokers = JokersWild ? hand.Count(x => x == 'J') : 0;

    returnValue <<= (distinctCount, groupByMax, numJokers) switch
    {
        (1, _, _) => FIVE_OF_A_KIND, // five of a kind 
        (2, 4, 0) => FOUR_OF_A_KIND, // four of a kind, no jokers 
        (2, 4, _) => FIVE_OF_A_KIND, // 4 jokers or 1 joker elevates to five of a kind
        (2, 3, 0) => FULL_HOUSE, // full house,no jokers
        (2, 3, _) => FIVE_OF_A_KIND, // full house, 2 or 3 jokers 
        (3, 3, 0) => THREE_OF_A_KIND, // three of a kind, no jokers
        (3, 3, _) => FOUR_OF_A_KIND,  // three of a kind, 1 or 3 jokers.  2 jokers is a full house. 
        (3, 2, 0) => TWO_PAIR, // two pair
        (3, 2, 1) => FULL_HOUSE, // two pair, 1 joker
        (3, 2, 2) => FOUR_OF_A_KIND, // two pair, 2 jokers, two pair w/3 jokers is full house. 
        (4, _, 0) => ONE_PAIR, // one pair, no jokers
        (4, _, _) => THREE_OF_A_KIND, // one pair, 1 joker, or 1 pair where the pair is the joker. 
        (5, _, 0) => HIGH_CARD,  //high card, doesn't deserve to be shifted. 
        (5, _, 1) => ONE_PAIR, // one pair with 1 joker
        _ => throw new Exception($"{distinctCount}, {groupByMax}, {numJokers}")
    };

    for(int i = 0; i < hand.Length; i++)
    {
        returnValue |= score.IndexOf(hand[i]) << ((4-i) * 4);
    }

    return returnValue;
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const bool JOKERS_WILD = true;

    List<(string hand, int score, int pot)> part1Puzzle = File.ReadAllLines(PUZZLE_INPUT)
        .Select(x => x.Split(' '))
        .Select(x => (x.First(), HandScore(x.First()), int.Parse(x.Last())))
        .OrderBy(x => x.Item2).ToList();

    int part1Answer = 0;
    for (int i = 0; i < part1Puzzle.Count;i++)
    {
        part1Answer += part1Puzzle[i].pot * (i + 1);
    }

    List<(string hand, int score, int pot)> part2Puzzle = File.ReadAllLines(PUZZLE_INPUT)
    .Select(x => x.Split(' '))
    .Select(x => (x.First(), HandScore(x.First(), JOKERS_WILD), int.Parse(x.Last())))
    .OrderBy(x => x.Item2).ToList();

    int part2Answer = 0;
    for (int i = 0; i < part2Puzzle.Count; i++)
    {
        part2Answer += part2Puzzle[i].pot * (i + 1);
    }

    Console.WriteLine($"Part 1: The winnings are: {part1Answer}");
    Console.WriteLine($"Part 2: When the jokers are wild, the winnings are: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}