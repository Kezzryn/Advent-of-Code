using Synacor_Challenge;
using System.Text;

static void WriteError(string message)
{
    ConsoleColor ccFC = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Write("*** ERROR : ");
    Console.ForegroundColor = ccFC;
    Console.WriteLine(message);
}
Dictionary<ConsoleKey, string> quickCommands = new()
    {
        { ConsoleKey.F1, "help" },
        { ConsoleKey.F2, @"load extras\challenge.bin" },
        { ConsoleKey.F3, "run" },
        { ConsoleKey.F4, "" },
        { ConsoleKey.F5, "" },
        { ConsoleKey.F6, "" },
        { ConsoleKey.F7, "" },
        { ConsoleKey.F8, "load teleporter.syn9k" },
        { ConsoleKey.Escape, "exit" },
        { ConsoleKey.RightArrow, "! step" }
    };

try
{
    const string PROMPT = "SynOS > ";
    const string TITLE = "SynOS 9000";

    Synacor9000 syn9k = new();
    bool isDone = false;

    Console.Title = TITLE;
    Console.TreatControlCAsInput = true;

    Dictionary<string, (int Left, int Top)> curPos = new()
    {
        { "console", (0, Console.WindowTop +20) },
        { "gamearea", (0, 0) },
        { "debug", (0, Console.WindowTop +25) },
    };

    void DoSomething(string command)
    {
        string[] split = command.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        string args = (split.GetUpperBound(0) >= 1) ? split[1] : string.Empty;

        switch (split[0])
        {
            case "exit" or "quit":
                isDone = true;
                Console.WriteLine("Bye!");
                break;
            case "load":
                if (syn9k.Load((args == string.Empty) ? Synacor9000.DefaultLoadFile : args, out string results))
                    Console.WriteLine(results);
                else 
                    WriteError(results);
                break;
            case "save":
                if (syn9k.Save((args == string.Empty) ? Synacor9000.DefaultSaveFile : args, out results))
                    Console.WriteLine(results);
                else
                    WriteError(results); ;
                break;
            case "run":
                Console.SetCursorPosition(curPos["gamearea"].Left, curPos["gamearea"].Top);
                syn9k.Run();
                curPos["gamearea"] = Console.GetCursorPosition();
                break;
            case "help":
                Console.WriteLine("Yea, we need some, but for now, here are the defined quick keys:");
                foreach (var cmds in quickCommands)
                {
                    Console.WriteLine($"{cmds.Key} {cmds.Value}");
                }
                break;
            case "!": // pass all debugger commands through.
                Console.SetCursorPosition(curPos["gamearea"].Left, curPos["gamearea"].Top);
                bool debugResult = syn9k.DebugDispatcher(command[1..].Trim(), out List<string> debugResponse);
                curPos["gamearea"] = Console.GetCursorPosition();

                Console.SetCursorPosition(curPos["debug"].Left, curPos["debug"].Top);
                foreach (string s in debugResponse)
                {
                    if (debugResult)
                        Console.WriteLine(s.PadRight(Console.BufferWidth -1));
                    else
                        WriteError(s.PadRight(Console.BufferWidth - 1));
                }

                break;
            default:
                Console.WriteLine($"Command not recognized: {split[0]}");
                break;
        }
    }

    string ReadLine()
    {
        ConsoleKeyInfo cki;
        StringBuilder sb = new();
        bool doneInput = false;
        do
        {
            cki = Console.ReadKey();
            switch (cki.Key)
            {
                case ConsoleKey.F1:
                case ConsoleKey.F2:
                case ConsoleKey.F3:
                case ConsoleKey.F4:
                case ConsoleKey.F5:
                case ConsoleKey.F6:
                case ConsoleKey.F7:
                case ConsoleKey.F8:
                case ConsoleKey.Escape:
                case ConsoleKey.RightArrow:
                    if (quickCommands.TryGetValue(cki.Key, out string? cmd))
                    {
                        Console.Write(cmd);
                        sb.Clear();
                        sb.Append(cmd);
                        doneInput = true;
                    }
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

    Console.Clear();

    while (!isDone)
    {
        Console.SetCursorPosition(curPos["console"].Left, curPos["console"].Top);
        Console.WriteLine(new string(' ', Console.WindowWidth)); 
        Console.Write(PROMPT);
        Console.Write(new string(' ', Console.WindowWidth - PROMPT.Length));
        Console.CursorLeft = PROMPT.Length;
        string input = ReadLine();
        if (!string.IsNullOrEmpty(input)) DoSomething(input);
    }
}
catch (Exception e)
{
    Console.WriteLine(e);
}