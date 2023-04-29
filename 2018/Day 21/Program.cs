using AoC_2018_Day_21;
using System.Diagnostics;

try
{
    static List<int>? generateList(int target)
    {
        List<int> returnValue = new();

        int i = 0;
        int R3 = 0;
        int R4 = 0;
        do
        {
            R3 = R4 | 65536;    //	6	bori 4 65536 3
            R4 = 4332021;           //	7	seti 4332021 4 4

            do
            {
                R4 += R3 % 256;

                R4 &= 16777215;         //  10	bani 4 16777215 4
                R4 *= 65899;            //	11	muli 4 65899 4
                R4 &= 16777215;         //	12	bani 4 16777215 4

                if (256 > R3) break;
                R3 /= 256;
            } while (true);

            i++;
            if (returnValue.Contains(R4)) return returnValue;
            returnValue.Add(R4);

        } while (R4 != target);
        return returnValue;
    }

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Stopwatch sw = Stopwatch.StartNew();
    List<int> reg0Values = generateList(0) ?? new();
    sw.Stop();
    Console.WriteLine($"{reg0Values.First()} -> {reg0Values.Last()} in {sw.ElapsedMilliseconds}");
    
    sw.Restart();
    ChronalVM vm = new(puzzleInput);
    List<int> vmReg4Values = vm.Run() ?? new();
    sw.Stop();
    Console.WriteLine($"{vmReg4Values.First()} -> {vmReg4Values.Last()} in {sw.ElapsedMilliseconds}");

    Console.WriteLine();
    Console.WriteLine($"Part 1: Setting R0 = {reg0Values.First()} produces the lowest loop value.");
    Console.WriteLine($"Part 2: Setting R0 = {reg0Values.Last()} produces the highest loop value.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}