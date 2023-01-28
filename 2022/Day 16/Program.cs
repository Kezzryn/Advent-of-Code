using AoC_2022_Day_16;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";

    CaveComplex volcano = new(File.ReadAllLines(PUZZLE_INPUT));

    //Console.WriteLine("LOADED");
    //Console.WriteLine("*****");

    //volcano.PrintLayout();

    volcano.Simplify();

    volcano.PrintLayout();

   // int pressure = volcano.DontBlowUp(30);
}
catch (Exception e)
{
    Console.WriteLine(e);
}
