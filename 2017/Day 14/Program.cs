using AoC_2017_Day_14;
using System.Drawing;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string secretHash = File.ReadAllText(PUZZLE_INPUT);

    bool[,] diskGrid = new bool[128,128];
    int part1Answer = 0;

    for (int row = 0; row < 128; row++)
    {
        string hash = KnotHash.HashStringToBinary($"{secretHash}-{row}");
        part1Answer += hash.Count(c => c == '1');
        for (int col = 0; col < 128; col++)
        {
            if (hash[col] == 1) diskGrid[col, row] = true;
        }
    }

    Console.WriteLine($"Part 1: There are {part1Answer} used squares.");

    Queue<Point> points = new();

    for (int row = 0; row < 128; row++)
    {
        for (int col = 0; col < 128; col++)
        {

        }
    }

    Console.WriteLine($"Part 2: ");
}
catch (Exception e)
{
    Console.WriteLine(e);
}