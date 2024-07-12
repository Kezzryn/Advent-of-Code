using AoC_2017_Day_15;

const int GEN_A_FACTOR = 16807;
const int GEN_B_FACTOR = 48271;

const int PART_1_NUM_PAIRS = 40_000_000;
const int PART_2_NUM_PAIRS = 5_000_000;

const int PART_2_FACTOR_A = 4;
const int PART_2_FACTOR_B = 8;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Generator GenA = new(GEN_A_FACTOR, int.Parse(puzzleInput[0][^3..]));
    Generator GenB = new(GEN_B_FACTOR, int.Parse(puzzleInput[1][^3..]));

    int part1Answer = Enumerable.Range(0, PART_1_NUM_PAIRS).Count(x => GenA.NextNum() == GenB.NextNum());

    GenA.ResetForPart2(PART_2_FACTOR_A);
    GenB.ResetForPart2(PART_2_FACTOR_B);

    int part2Answer = Enumerable.Range(0, PART_2_NUM_PAIRS).Count(x => GenA.NextNum() == GenB.NextNum());

    Console.WriteLine($"Part 1: There are {part1Answer} matching pairs.");
    Console.WriteLine($"Part 2: There are {part2Answer} matching pairs when they try to syncronize.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}