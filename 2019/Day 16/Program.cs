using System.Diagnostics;
using System.Text;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int NUM_PHASES = 100;
    const int MULTIPLIER = 10000;

    string puzzleInput = "03036732577212944063491565474664"; //"File.ReadAllText(PUZZLE_INPUT);

    int length = puzzleInput.Length * MULTIPLIER;
    int answerOffset = int.Parse(puzzleInput[0..7]);

    int[] fftA = new int[length];
    int[] fftB = new int[length];
    int[] cumulativeSum = new int[length + 1];

    // initial load.
    for (int m = 1; m <= MULTIPLIER; m++)
    {
        for (int i = 0; i < puzzleInput.Length; i++)
        {
            fftA[i * m] = puzzleInput[i] - '0';
        }
    }

    int newValue = 0;
    int sign = 0;
    int j = 0;
    int sum = 0;
    int end = 0; 

    ref int[] ptrSource = ref fftA;
    ref int[] ptrTarget = ref fftB;

    Stopwatch sw = Stopwatch.StartNew();
    for (int phase = 1; phase <= NUM_PHASES; phase++)
    {
        //Console.WriteLine($"phase {phase} {sw.ElapsedMilliseconds}  ");
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

        cumulativeSum[0] = 0;
        for (int i = 1; i <= length; i++)
        {
            cumulativeSum[i] = ptrSource[i - 1] + cumulativeSum[i-1];
        }

        for(int step = 1; step <= length; step++)
        {
            //if ((step % 1000) == 0) Console.WriteLine($"-- {step} {sw.ElapsedMilliseconds}");

            newValue = 0;
            sign = 1;
            for(j = step - 1; j < length; j += step << 1)
            {
                end = int.Min(j + step, length);
                sum = cumulativeSum[end] - cumulativeSum[j];
                
               //Console.Write($"{sum * sign} ");

                newValue += sign * sum;
                
                sign = -sign;
            }
            
            ptrTarget[step-1] = Math.Abs(newValue % 10);
            //Console.WriteLine(ptrTarget[step - 1]);
        }

        //foreach (int value in ptrTarget)
        //{
        //    Console.Write(value);
        //}
        //Console.WriteLine();
    }

    sw.Stop();

    StringBuilder sb = new();
    foreach(int i in ptrTarget[answerOffset..(answerOffset+7)])
    {
        sb.Append(ptrTarget[i]);
    }

    string part1Answer = sb.ToString();
    int part2Answer = 0;


    Console.WriteLine($"Time: {sw.ElapsedMilliseconds}");
    Console.WriteLine($"Part 1: {part1Answer}");
    Console.WriteLine($"Part 2: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}