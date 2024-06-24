using BKH.EnumExtentions;
using System.Collections;
using System.Text;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int DISK_SIZE_PART_1 = 272;
    const int DISK_SIZE_PART_2 = 35651584;

    BitArray puzzleInput = new(File.ReadAllText(PUZZLE_INPUT).Select(x => x == '1' ? 1 : 0).ToArray());

    static string CalcCheckSum(int diskSize, BitArray bitPuzzle)
    {
        BitArray ZERO_INSERT = new([false]);
  
        // create the dragon fractal thingy.
        while (bitPuzzle.Length < diskSize)
        {
            BitArray mirrorBit = new(bitPuzzle);
            bitPuzzle = bitPuzzle.Append(mirrorBit.Not().Reverse().Prepend(ZERO_INSERT));
        }

        bitPuzzle.Length = diskSize;
        BitArray checkSum = new(bitPuzzle);

        // calculate the checksum
        while (int.IsEvenInteger(checkSum.Length))
        {
            bool[] newSum = new bool[checkSum.Length / 2];

            for (int i = 0; i < checkSum.Length; i += 2)
            {
                newSum[i / 2] = checkSum[i] == checkSum[i + 1];
            }
            checkSum = new(newSum);
        }

        //Stuff it back out as a string. 
        StringBuilder stringSum = new();
        foreach (bool b in checkSum)
        {
            stringSum.Append(b ? "1" : "0");
        }

        return stringSum.ToString();
    }

    string part1Answer = CalcCheckSum(DISK_SIZE_PART_1, puzzleInput);
    string part2Answer = CalcCheckSum(DISK_SIZE_PART_2, puzzleInput);

    Console.WriteLine($"Part 1: The checksum is: {part1Answer}");
    Console.WriteLine($"Part 2: The checksum is: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}