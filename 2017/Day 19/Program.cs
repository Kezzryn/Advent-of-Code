using BKH.Geometry;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Cursor cursor = new(puzzleInput[0].IndexOf('|'), 0, 0, 1);
    string part1Answer = String.Empty;

    int part2Answer = 0;
    bool isDone = false;
    while (!isDone)
    {
        switch (puzzleInput[cursor.Y][cursor.X])
        {
            case '|' or '-':
                //everything is normal, continue. 
                break;
            case '+':
                // try a turn
                // Assumption 1: If we can't turn one way we CAN turn the other. 
                // Assumption 2: There is padding around the puzzle, removing the requirement for a bounds check.
                cursor.TurnLeft();
                (int nextX, int nextY) = cursor.NextStep();
                if(puzzleInput[nextY][nextX] == ' ') cursor.TurnAround();

                break;
            case ' ':
                // we've stepped off the map, and therefor should be finished.
                isDone = true;
                break;
            default:
                part1Answer += puzzleInput[cursor.Y][cursor.X];
                break;
        }
        cursor.Step();
        if (!isDone) part2Answer++; // let's avoid off-by-one errors :) 
    }

    Console.WriteLine($"Part 1: Along the path the following letters are encountered. {part1Answer}");
    Console.WriteLine($"Part 2: The number of steps taken is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}