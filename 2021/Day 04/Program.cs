static int getBit(int x, int pos) => (x & (1 << pos)) != 0 ? 1 : 0;
static int setBit(int x, int pos) => x |= 1 << pos;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string CRLF = "\r\n";
    string[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(CRLF + CRLF);

    List<int> bingoNumbers = puzzleInput[0].Split(',').Select(int.Parse).ToList();
    Dictionary<int, List<int>> bingoCards = new();
    Dictionary<int, int> cardMarks = new();

    List<int> winningBitMasks = new()
    {
        32505856,   //1111100000000000000000000
        1015808,    //0000011111000000000000000
        31744,      //0000000000111110000000000
        992,        //0000000000000001111100000
        31,         //0000000000000000000011111
        17318416,   //1000010000100001000010000
        8659208,    //0100001000010000100001000
        4329604,    //0010000100001000010000100
        2164802,    //0001000010000100001000010
        1082401     //0000100001000010000100001
    };

    for (int cardNum = 1; cardNum < puzzleInput.Length; cardNum++)
    {
        string line = puzzleInput[cardNum].Replace(CRLF, " ");
        bingoCards[cardNum] = new(line.Split(' ',StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToList());
        cardMarks[cardNum] = 0;
    }

    int part1Answer = -1;
    int part2Answer = 0;

    static int scoreBingo(int lastNumDrawn, List<int>bingoCard, int cardMark)
    {
        int returnValue = 0;

        for (int i = 0; i < bingoCard.Count; i++)
        {
            returnValue += getBit(cardMark, i) == 0 ? bingoCard[i] : 0;
        }

        returnValue *= lastNumDrawn;

        return returnValue;
    }

    List<int> winningCards = new();  

    for (int bingoNum = 0; bingoNum < bingoNumbers.Count; bingoNum++)
    {
        foreach ((int cardNum, List<int> bingoCard) in bingoCards)
        {
            if (winningCards.Contains(cardNum)) continue;
            int index = bingoCard.IndexOf(bingoNumbers[bingoNum]);
            if (index != -1)
            {
                cardMarks[cardNum] = setBit(cardMarks[cardNum], index);
                if(winningBitMasks.Any(x => (cardMarks[cardNum] & x) == x))
                {
                    if (part1Answer == -1)
                    {
                        part1Answer = scoreBingo(bingoNumbers[bingoNum], bingoCard, cardMarks[cardNum]);
                    }
                    if (bingoCards.Count - winningCards.Count == 1)
                    {
                        part2Answer = scoreBingo(bingoNumbers[bingoNum], bingoCard, cardMarks[cardNum]);
                    }
                    winningCards.Add(cardNum);
                }
            }
        }
    }
    
    Console.WriteLine($"Part 1: The score of the first winning bingo card is {part1Answer}.");
    Console.WriteLine($"Part 2: The score of the last winning bingo card is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}