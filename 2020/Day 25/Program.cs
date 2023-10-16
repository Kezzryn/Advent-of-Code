try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string CRLF = "\r\n";
    const long DIVISOR = 20201227;

    List<long> puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(CRLF).Select(long.Parse).ToList();
    (long cardPublicKey, long doorPublicKey) = (puzzleInput.First(), puzzleInput.Last());

    static long transformSubjectNumber(long subjectNumber, long loopSize)
    {
        long value = 1;
        
        for(long i = 0; i < loopSize; i++)
        {
            value = (value * subjectNumber) % DIVISOR;
        }
        return value;
    }

    static long findLoopSize(long targetKey)
    {
        const long subjectNumber = 7;
        long value = 1;
        long loopSize = 0;

        do
        {
            loopSize++;
            value = (value * subjectNumber) % DIVISOR;
        } while (value != targetKey);

        return (value == targetKey) ? loopSize : -1;
    }

    long cardPrivateKey = transformSubjectNumber(cardPublicKey, findLoopSize(doorPublicKey));
    long doorPrivateKey = transformSubjectNumber(doorPublicKey, findLoopSize(cardPublicKey));

    if (cardPrivateKey != doorPrivateKey) throw new Exception("Key mismatch.");

    Console.WriteLine($"Part 1: The private key is {cardPrivateKey}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
