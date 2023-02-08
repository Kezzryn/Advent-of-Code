using AoC_2022_Day_22;

void WriteCursorPos(Tuple<int, int, int> cp)
{
    Console.WriteLine($"Cursor is at: ({cp.Item1}, {cp.Item2}) facing {cp.Item3 switch
    {
        Directions.Right => "Right",
        Directions.Down => "Down",
        Directions.Left => "Left",
        Directions.Up => "Up",
        _ => throw new NotImplementedException($"No idea where we're going {cp.Item3}")
    }}");
}

try
{
    const string CRLF = "\r\n"; // if we ever run this on another platform, fix this. 

    const string PUZZLE_INPUT = "PuzzleInputTest.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    string[] cubeData = puzzleInput.Split(CRLF + CRLF)[0].Split(CRLF);
    string movesData = puzzleInput.Split(CRLF + CRLF)[1];

    Cube part1 = new(cubeData, CubeMapTypes.CubeMap);
    //WriteCursorPos(part1.CursorPosition());
    //Cube part2 = new(cubeData, CubeMapTypes.CubeMap);

    List<string> moves = Cube.TranslateMoveset(movesData);
    Tuple<int, int, int> cp = part1.CursorPosition();

    foreach (string move in moves)
    {
        //Console.WriteLine(move);

        part1.Move(move);
//        WriteCursorPos(part1.CursorPosition());

        //part2.Move(move);
      //  Console.ReadKey();
    }

    Tuple<int, int, int > cp1 = part1.CursorPosition();
    Console.WriteLine($"Part 1: The final password is: {(cp1.Item2 * 1000) + (cp1.Item1 * 4) + cp1.Item3}");

    //Tuple<int, int, int> cp2 = part2.CursorPosition();
    //Console.WriteLine($"Part 1: The final password is: {(cp2.Item1 * 1000) + (cp2.Item2 * 4) + cp2.Item3}");

}
catch (Exception e)
{
    Console.WriteLine(e);
}

