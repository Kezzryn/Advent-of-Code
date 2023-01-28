using AoC_2022_Day_25;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Base10Converter base10Converter = new();

    Console.WriteLine($"The snafu number is {base10Converter.ConvertFromBase10(
                puzzleInput.Sum(
                    snafu => base10Converter.ConvertToBase10(snafu, "Snafu"))
                , "Snafu")}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}