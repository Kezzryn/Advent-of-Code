using AoC_2022_Day_9;

try
{
    const int PART_1_ROPE_SIZE = 2;
    const int PART_2_ROPE_SIZE = 10;

    Rope shortRope = new Rope(PART_1_ROPE_SIZE);
    Rope longRope = new Rope(PART_2_ROPE_SIZE);

    const string PUZZLE_INPUT = "..\\..\\..\\..\\..\\Input Files\\Day 9.txt"; ;
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    shortRope.Move(puzzleInput);
    longRope.Move(puzzleInput);

    Console.WriteLine($"Part1: The tail of the short rope visited {shortRope.NumTailPos()} positions.");
    Console.WriteLine($"Part2: The tail of the long rope visited {longRope.NumTailPos()} positions.");

}
catch (Exception e)
{
    Console.WriteLine(e);
}
