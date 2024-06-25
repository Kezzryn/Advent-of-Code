using BKH.Base10Converter;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    string part1Answer = Base10Converter.FromBase10(
                    puzzleInput.Sum(snafu => Base10Converter.ToBase10(snafu, TranslationMap.Maps["SNAFU"])),
                    TranslationMap.Maps["SNAFU"]);

     Console.WriteLine($"The snafu number is {part1Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}