using System.Drawing;

const char NULL_CHAR = '\0';
const char START_CHAR = '5';

string KeypadSolver(List<string> input, char[,] keypad)
{
    Dictionary<Directions, Size> step = new()
    {
        { Directions.Up,    new ( 0,  1) } , 
        { Directions.Down,  new ( 0, -1) } ,
        { Directions.Left,  new (-1, 0) } , 
        { Directions.Right, new ( 1, 0) } , 
    };

    Point cursor = new(0, 0);

    // find our start point, '5'
    bool breakCheck = true;
    for(int x = 0; x < keypad.GetUpperBound(0) && breakCheck; x++)
    {
        for (int y= 0; y < keypad.GetUpperBound(1) && breakCheck; y++)
        {
            if (keypad[x,y] == START_CHAR)
            {
                cursor = new(x,y);
                breakCheck = false;
            }
        }
    }

    string answer = "";
    foreach (string instruction in input)
    {
        foreach (char direction in instruction)
        {
            switch (direction)
            {
                case 'U':
                    if (cursor.Y == keypad.GetUpperBound(1) || (keypad[cursor.X, cursor.Y + step[Directions.Up].Height] == NULL_CHAR)) continue;
                    cursor += step[Directions.Up];
                    break;
                case 'D':
                    if (cursor.Y == keypad.GetLowerBound(1) || (keypad[cursor.X, cursor.Y + step[Directions.Down].Height] == NULL_CHAR)) continue;
                    cursor += step[Directions.Down];
                    break;
                case 'R':
                    if (cursor.X == keypad.GetUpperBound(0) || (keypad[cursor.X + step[Directions.Right].Width, cursor.Y] == NULL_CHAR)) continue;
                    cursor += step[Directions.Right];
                    break;
                case 'L':
                    if (cursor.X == keypad.GetLowerBound(0) || (keypad[cursor.X + step[Directions.Left].Width, cursor.Y] == NULL_CHAR)) continue;
                    cursor += step[Directions.Left];
                    break;
                default:
                    throw new NotImplementedException();
            };
        }
        answer += keypad[cursor.X, cursor.Y];
    }
    return answer; 
}


try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<string> puzzleInput = File.ReadAllLines(PUZZLE_INPUT).ToList();
    //List<string> puzzleInput = new() { "ULL", "RRDDD", "LURDL", "UUUUD" };

    char[,] keypadPart1 =
    {
        { '7', '4', '1' },
        { '8', '5', '2' },
        { '9', '6', '3' } 
    };

    char[,] keypadPart2 = {
        { NULL_CHAR, NULL_CHAR, '5', NULL_CHAR, NULL_CHAR},
        { NULL_CHAR, 'A',       '6', '2',       NULL_CHAR },
        { 'D',       'B',       '7', '3',       '1' },
        { NULL_CHAR, 'C',       '8', '4',       NULL_CHAR },
        { NULL_CHAR, NULL_CHAR, '9', NULL_CHAR, NULL_CHAR }
    };

    string part1Answer = KeypadSolver(puzzleInput, keypadPart1);
    string part2Answer = KeypadSolver(puzzleInput, keypadPart2);

    Console.WriteLine($"Part 1: The bathroom door code is: {part1Answer}");
    Console.WriteLine($"Part 1: The insane bathroom door code is: {part2Answer}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}

enum Directions  {  Up, Down, Right, Left }
