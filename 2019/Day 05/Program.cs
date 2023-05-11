using AoC_2019_IntcodeVM;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    int[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(',').Select(int.Parse).ToArray();

    IntcodeVM vm = new(puzzleInput); //new int[] { 1101,100,-1,4,0 });
    Console.WriteLine("Press 1 for an AC diagnostic, or 5 for thermal radiators.");
    vm.Run();

}
catch (Exception e)
{
    Console.WriteLine(e);
}