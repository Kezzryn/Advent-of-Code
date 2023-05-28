using AoC_2019_IntcodeVM;
using System.Text;

try
{
    const string PROMPT = "> ";
    const string TITLE = "Santa Intcode 9000";

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    long[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(',').Select(long.Parse).ToArray();

    IntcodeVM santaBot = new(puzzleInput);

    bool isDone = false;
    bool isRunning = false;

    string ReadInput()
    {
        ConsoleKeyInfo cki;
        StringBuilder sb = new();
        bool doneInput = false;
        Console.Write(PROMPT);
        do
        {
            cki = Console.ReadKey();
            switch (cki.Key)
            {
                case ConsoleKey.UpArrow:
                    sb.Append("north");
                    doneInput = true;
                    break;
                case ConsoleKey.DownArrow:
                    sb.Append("south");
                    doneInput = true;
                    break;
                case ConsoleKey.LeftArrow:
                    sb.Append("west");
                    doneInput = true;
                    break;
                case ConsoleKey.RightArrow:
                    sb.Append("east");
                    doneInput = true;
                    break;
                case ConsoleKey.Tab:
                    sb.Append("inv");
                    doneInput = true;
                    break;
                case ConsoleKey.Backspace:
                    if (sb.Length > 0) sb = sb.Remove(sb.Length - 1, 1);
                    // backspace moves the cursor back, so whitespace, then \b to back the cursor up again.
                    Console.Write(" ");
                    if (Console.CursorLeft > PROMPT.Length) Console.Write("\b");
                    break;
                case ConsoleKey.Enter:
                    doneInput = true;
                    break;
                default:
                    if ((cki.Modifiers & ConsoleModifiers.Control) != 0 && cki.Key == ConsoleKey.C)
                    {
                        isDone = true;
                        break;
                    }
                    sb.Append(cki.KeyChar);
                    break;
            }
        } while (!doneInput);
        Console.WriteLine();
        return sb.ToString();
    }

    Console.Title = TITLE;
    Console.Clear();
    
    while (!isDone)
    {
        if (santaBot.CurrentState != State.Halted)
        {
            isRunning = santaBot.Run() != State.Halted;

            while (santaBot.GetOutput(out char output))
            {
                Console.Write(output);
            }

            string input = ReadInput();

            if (input != string.Empty) santaBot.SetInput(input);
        }
    }

    Console.WriteLine("Bye!");
}
catch (Exception e)
{
    Console.WriteLine(e);
}