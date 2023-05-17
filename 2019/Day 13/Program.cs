using AoC_2019_IntcodeVM;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const bool DO_PART_TWO = true;

    const long GO_LEFT = 1;
    const long GO_RIGHT = -1;
    
    const long TILE_EMPTY = 0;
    const long TILE_WALL = 1;
    const long TILE_BLOCK = 2;
    const long TILE_PADDLE = 3;
    const long TILE_BALL = 4;

    long[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(',').Select(long.Parse).ToArray();
    IntcodeVM arcade = new(puzzleInput);
   
    if (DO_PART_TWO) arcade.SetMemory(0, 2);

    State currentState = State.Paused_For_Input;
    int part1Answer = 0;
   
    long ballX = 0;
    long paddleX = 0;

    static void PrintBlock(long x, long y, long value)
    {
        if (x == -1)
        {
            Console.SetCursorPosition(0,0);
            Console.Write($"Score : {value,-6} ");
        }
        else
        {
            // y + 2 to make room for the scoreboard.
            Console.SetCursorPosition((int)x, (int)y + 2);
            Console.Write(value switch
            {
                TILE_EMPTY => ' ',
                TILE_WALL => '█',
                TILE_BLOCK => '▒',
                TILE_PADDLE => '▄',
                TILE_BALL => 'Θ',
                _ => throw new NotImplementedException()
            });
        }
    }

    Console.Clear();
    Console.CursorVisible = false;

    while (currentState != State.Halted)
    {
        currentState = arcade.Run();

        while (arcade.IsOutput())
        {
            arcade.GetOutput(out long x);
            arcade.GetOutput(out long y);
            arcade.GetOutput(out long block);

            switch (block)
            {
                case TILE_BALL:
                    ballX = x;
                    break;
                case TILE_PADDLE:
                    paddleX = x;
                    break;
                case TILE_BLOCK:
                    part1Answer++;
                    break;
                default:
                    break;
            }

            PrintBlock(x, y, block);
        }

        if (currentState == State.Paused_For_Input)
        {
            long readKey = 0;
            if (ballX < paddleX) readKey = GO_RIGHT;
            if (ballX > paddleX) readKey = GO_LEFT;
            arcade.SetInput(readKey);
        }
        Thread.Sleep(25);
    }

    Console.WriteLine();
    Console.WriteLine($"Part 1: The number of block sqaures is: {(DO_PART_TWO ? "PART TWO ACTIVE." : part1Answer)}");


}
catch (Exception e)
{
    Console.WriteLine(e);
}