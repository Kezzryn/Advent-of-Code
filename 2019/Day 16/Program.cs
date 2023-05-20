using System.Diagnostics;
using System.Text;

static string doFFT(string input, int multiplier = 1)
{
    const int NUM_PHASES = 100;

    int length = input.Length * multiplier;
    int answerOffset = multiplier == 1 ? 0 : int.Parse(input[0..7]);

    int[] fftA = new int[length];
    int[] fftB = new int[length];
    int[] cumulativeSum = new int[length + 1];

    // initial load.
    for (int i = 0; i < length; i++)
    {
        fftA[i] = input[i % input.Length] - '0';
    }

    ref int[] ptrSource = ref fftA;
    ref int[] ptrTarget = ref fftB;

    Stopwatch sw = Stopwatch.StartNew();
    for (int phase = 1; phase <= NUM_PHASES; phase++)
    {
        if (phase % 2 == 0)
        {
            ptrSource = ref fftB;
            ptrTarget = ref fftA;
        }
        else
        {
            ptrSource = ref fftA;
            ptrTarget = ref fftB;
        }

        // Precompute the cumulative sums. This solves all sorts of performance issues. 
        cumulativeSum[0] = 0;
        for (int i = 1; i <= length; i++)
        {
            cumulativeSum[i] = ptrSource[i - 1] + cumulativeSum[i - 1];
        }

        for (int step = 1; step <= length; step++)
        {
            int newValue = 0;
            int sign = 1;

            for (int j = step - 1; j < length; j += step << 1)
            {
                newValue += sign * cumulativeSum[int.Min(j + step, length)] - cumulativeSum[j];

                sign = -sign;
            }

            ptrTarget[step - 1] = Math.Abs(newValue % 10);
        }
    }

    sw.Stop();

    StringBuilder sb = new();
    ptrTarget[answerOffset..(answerOffset + 8)].ToList().ForEach(x => sb.Append(x));

    return sb.ToString();
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int MULTIPLIER = 10000;

    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    string part1Answer = doFFT(puzzleInput);
    string part2Answer = doFFT(puzzleInput, MULTIPLIER);

    Console.WriteLine($"Part 1: After 100 phases, the first eight digits of the output are: {part1Answer}.");
    Console.WriteLine($"Part 2: After 100 phases of the input duped {MULTIPLIER} times returns: {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}