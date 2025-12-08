using BKH.Segments;

try
{
    const string CRLF = "\r\n";
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);


    Segments<int> test = new();

    test.AddRange(1, 5);
    test.AddRange(2, 6);
    test.AddRange(9, 11);


    List<(long low, long high)> puzzleRanges = [.. puzzleInput.Split(CRLF + CRLF).First().Split(CRLF).Select(x => x.Split('-')).Select(x => (long.Parse(x.First()), long.Parse(x.Last())))];

    List<long> puzzleIngredient = [.. puzzleInput.Split(CRLF + CRLF).Last().Split(CRLF).Select(x => long.Parse(x))];

    int part1Answer = puzzleIngredient.Count(x => puzzleRanges.Any(y => (y.low<= x && x <= y.high)));

    
    Segments<long> seg = new();

    foreach(var (low, high) in puzzleRanges)
    {
        seg.AddRange(low, high);
    }

    long part2Answer = seg.GetSlices().Sum(x => x.Length) + seg.GetSlices().Count;

    Console.WriteLine($"Part 1: {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer} low: 344306344403095");
}
catch (Exception e)
{
    Console.WriteLine(e);
}