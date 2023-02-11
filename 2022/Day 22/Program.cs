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

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    string[] cubeData = puzzleInput.Split(CRLF + CRLF)[0].Split(CRLF);
    string movesData = puzzleInput.Split(CRLF + CRLF)[1];

    Cube part1 = new(cubeData, CubeMapTypes.FlatMap);
    Cube part2 = new(cubeData, CubeMapTypes.CubeMap);
    //part2.ToggleVerbose();

    List<string> moves = Cube.TranslateMoveset(movesData);

    foreach (string move in moves)
    {
        part1.Move(move);
        part2.Move(move);
    }

    Tuple<int, int, int > cp1 = part1.CursorPosition();
    Console.WriteLine($"Part 1: The final password is: {(cp1.Item2 * 1000) + (cp1.Item1 * 4) + cp1.Item3}");

    Tuple<int, int, int> cp2 = part2.CursorPosition();
    //WriteCursorPos(cp2);
    Console.WriteLine($"Part 2: The final password is: {(cp2.Item2 * 1000) + (cp2.Item1 * 4) + cp2.Item3}");

}
catch (Exception e)
{
    Console.WriteLine(e);
}

