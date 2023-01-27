using AoC_2022_Day_25;

try
{
    const string PUZZLE_INPUT = @"..\..\..\..\..\Input Files\Day 25.txt";
    string[] inputFile = File.ReadAllLines(PUZZLE_INPUT);

    Base10Converter base10Converter = new();

    Console.WriteLine($"The snafu number is {base10Converter.ConvertFromBase10(
                inputFile.Sum(
                    snafu => base10Converter.ConvertToBase10(snafu, "Snafu"))
                , "Snafu")}");

}
catch (Exception e)
{
    Console.WriteLine(e);
}