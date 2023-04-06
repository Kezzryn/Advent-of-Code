using System.Drawing;
using System.Numerics;
using System.Text;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Point cursor = new (puzzleInput[0].IndexOf('|'), 0);
    Complex direction = new(0, 1);
    StringBuilder part1Answer = new();

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
                // try right turn
                // Assumption: If we can't turn one way we CAN turn the other. 
                // Assumption2: There is padding around the puzzle, removing the requirement for a bounds check.
                Complex testTurn = direction * Complex.ImaginaryOne;
                Point nextStep = cursor + new Size((int)testTurn.Real, (int)testTurn.Imaginary);

                direction *= (puzzleInput[nextStep.Y][nextStep.X] == '|' || puzzleInput[nextStep.Y][nextStep.X] == '-') ? Complex.ImaginaryOne : -Complex.ImaginaryOne; 
                break;
            case ' ':
                // we've stepped off the map.
                isDone = true;
                break;
            default:
                part1Answer.Append(puzzleInput[cursor.Y][cursor.X]);
                break;
        }
        cursor += new Size((int)direction.Real, (int)direction.Imaginary);
        if (!isDone) part2Answer++; // let's avoid off-by-one errors :) 
    }

    Console.WriteLine($"Part 1: Along the path the following letters are encountered. {part1Answer}");
    Console.WriteLine($"Part 2: The number of steps taken is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}