try
{
    const string PUZZLE_INPUT = @"PuzzleInput.txt";
    const string CRLF = "\r\n"; //platform depependent. 

    //Puzzle data is integer values (calories) seperated by a space. 
    //break the input by double CRLF to break into each elf, then convert to int and sum the resultant lists to get the total for that elf.

    List<int> elves = File.ReadAllText(PUZZLE_INPUT)
                    .Split(CRLF + CRLF)
                    .Select(elf =>
                        elf.Split(CRLF)
                        .Select(int.Parse)
                        .Sum())
                    .ToList();

    Console.WriteLine($"Part 1: The elf carrying the most calories is {elves.Max()}");

    Console.WriteLine($"Part 2: The top three elves are carrying {elves.OrderByDescending(elf => elf).Take(3).Sum()} calories.");
}
catch (Exception e)
{
     Console.WriteLine(e);
}