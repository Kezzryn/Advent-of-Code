using BKH.Base10Converter;
using System.Collections.Concurrent;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    List<(long, List<long>)> calibration = [];
    foreach (string line in puzzleInput)
    {
        string[] parts = line.Split(": ");
        calibration.Add((long.Parse(parts[0]), parts[1].Split(' ').Select(long.Parse).ToList()));
    }

    static long MergeOp(long A, long B)
    {
        //https://stackoverflow.com/questions/4483886/how-can-i-get-a-count-of-the-total-number-of-digits-in-a-number
        if (B < 10) return (A * 10) + B;
        if (B < 100) return (A * 100) + B;
        if (B < 1000) return (A * 1000) + B;

        throw new Exception();
    }

    static long CheckList(long answer, List<long> calibrations, TranslationMap tm, ConcurrentDictionary<int, string> preCompute)
    {
        int numLoops = (int)Math.Pow(tm.Base, calibrations.Count - 1) - 1;
        int padSize = Base10Converter.FromBase10(numLoops, tm).Length;

        for(int mathBits = 0; mathBits <= numLoops; mathBits++)
        {
            long resultValue = calibrations[0];
            if(!preCompute.TryGetValue(mathBits, out string? opcodes))
            {
                opcodes = Base10Converter.FromBase10(mathBits, tm).PadLeft(padSize,'0');
            }

            for (int opIndex = 0; opIndex < calibrations.Count - 1; opIndex++)
            {
                resultValue = opcodes[opIndex] switch
                {
                    '0' => resultValue + calibrations[opIndex + 1],
                    '1' => resultValue * calibrations[opIndex + 1],
                    '2' => MergeOp(resultValue, calibrations[opIndex + 1]),
                    _ => throw new NotImplementedException()
                };
                if (resultValue > answer) break;
            }
         
            if (resultValue == answer) return answer;
        }

        return 0; 
    }

    ConcurrentDictionary<int, string> preCompute = new();
    long part1Answer = calibration.AsParallel().Sum(x => CheckList(x.Item1, x.Item2, TranslationMap.Maps["BIN"], preCompute));

    TranslationMap trinary = new("012");
    preCompute.Clear();
    long part2Answer = calibration.AsParallel().Sum(x => CheckList(x.Item1, x.Item2, trinary, preCompute));

    Console.WriteLine($"Part 1: {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}