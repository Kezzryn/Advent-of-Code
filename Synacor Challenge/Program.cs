using Synacor_Challenge;
using System.Text;

try
{
    const string PROMPT = "> ";
    const string TITLE = "SynOS 9000";

    Dictionary<ConsoleKey, string> quickCommands = new()
        {
            { ConsoleKey.F2,        $"{ProgCmds.COMMAND_CHAR}{ProgCmds.Load} extras\\challenge.bin" },
            { ConsoleKey.F3,        $"{ProgCmds.COMMAND_CHAR}{ProgCmds.Run}" },
            { ConsoleKey.F5,        $"{ProgCmds.COMMAND_CHAR}{ProgCmds.Save}" },
            { ConsoleKey.F8,        $"{ProgCmds.COMMAND_CHAR}{ProgCmds.Load} teleporter.syn9k" },
            { ConsoleKey.Escape,    $"{ProgCmds.COMMAND_CHAR}{ProgCmds.Exit}" }
        };
    //Dictionary<string, (int Left, int Top)> curPos = new()
    //    {
    //        { "console", (0, Console.WindowTop +20) },
    //        { "gamearea", (0, 0) },
    //        { "debug", (0, Console.WindowTop +25) },
    //    };

    Synacor9000 syn9k = new();

    bool isDone = false;
    bool isLoaded = false;
    bool isRunning = false;

    static void WriteError(string message)
    {
        ConsoleColor ccFC = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("*** ERROR : ");
        Console.ForegroundColor = ccFC;
        Console.WriteLine(message);
    }
    static void WriteMessage(string message)
    {
        //ConsoleColor ccFC = Console.ForegroundColor;
        //Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("*** : ");
        //Console.ForegroundColor = ccFC;
        Console.WriteLine(message);
    }
    static void WriteProgramOutput(string message)
    {
        Console.Write(message);
    }
    static void WriteDebuggerOutput(string message, bool isSuccess)
    {
        if (isSuccess)
            Console.WriteLine(message);
        else
            WriteError(message);
    }
    void WriteHelp()
    {
        const int spc = -8;
        Console.WriteLine($"{TITLE} command list:");
        Console.WriteLine($"{"F1",spc} This message.");
        foreach (var cmd in ProgCmds.Command_Descriptions)
        {
            Console.WriteLine($"{cmd.Key, spc} {cmd.Value}");
        }
        Console.WriteLine();
        Console.WriteLine($"All text that is not preceeded by {ProgCmds.COMMAND_CHAR} is passed without modification to the running program.");
        Console.WriteLine("Quick-keys:");
        foreach(KeyValuePair<ConsoleKey, string> cmd in quickCommands)
        {
            Console.WriteLine($"{cmd.Key} {cmd.Value}");
        }
        Console.WriteLine();
    }

    void DoConsoleCommand(string command)
    {
        command = command.Trim(ProgCmds.COMMAND_CHAR).ToLower();

        string[] split = command.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        string args = (split.GetUpperBound(0) >= 1) ? split[1] : string.Empty;

        switch (split[0])
        {
            case ProgCmds.Exit:
                isDone = true;
                break;
            case ProgCmds.Load:
                isLoaded = syn9k.Load((args == string.Empty) ? Synacor9000.DefaultLoadFile : args, out string results);
                if (isLoaded)
                    WriteMessage(results);
                else
                    WriteError(results);
                break;
            case ProgCmds.Run:
                if (isLoaded)
                {
                    WriteMessage("Running the program.");
                    isRunning = true;
                }
                else
                {
                    WriteError("No program loaded.");
                }
                break;
            case ProgCmds.Save:
                if (!isRunning)
                {
                    WriteError("No running program to save.");
                    break;
                }

                if (syn9k.Save((args == string.Empty) ? Synacor9000.DefaultSaveFile : args, out results))
                    WriteMessage(results);
                else
                    WriteError(results);
                break;
            default:
                // pass all unknown !commands commands through. Maybe they're for the debugger?
                bool debugResult = syn9k.DebugDispatcher(command, out List<string> debugResponse);

                foreach (string s in debugResponse)
                {
                    WriteDebuggerOutput(s.PadRight(Console.BufferWidth - 1), debugResult);
                }
                break;
        }
    }
    string ReadInput()
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
                    WriteHelp();
                    doneInput = true;
                    break;
                case ConsoleKey.F2:
                case ConsoleKey.F3:
                case ConsoleKey.F4:
                case ConsoleKey.F5:
                case ConsoleKey.F6:
                case ConsoleKey.F7:
                case ConsoleKey.F8:
                case ConsoleKey.Escape:
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

    Console.Title = TITLE;
    Console.Clear();
    WriteHelp();

    while (!isDone)
    {
        Console.Write(PROMPT);
        string input = ReadInput();

        if (string.IsNullOrEmpty(input)) continue;

        if (input.StartsWith(ProgCmds.COMMAND_CHAR))
        {
            DoConsoleCommand(input);
            input = string.Empty;
        }

        if (isLoaded && isRunning)
        {
            syn9k.SetProgramInput(input);

            isRunning = syn9k.Run() != Synacor9000.State.Halted;

            while (syn9k.GetProgramOutput(out string output))
            {
                WriteProgramOutput(output);
            }

            if (!isRunning) WriteMessage("Program halted.");
        }
    }

    WriteMessage("Bye!");
}
catch (Exception e)
{
    Console.WriteLine(e);
}

static class ProgCmds
{
    public const char COMMAND_CHAR = '!';
    public const string Load = "load";
    public const string Save = "save";
    public const string Run = "run";
    public const string Exit = "exit";

    public static Dictionary<string, string> Command_Descriptions = new()
    {
        { Load, $"Loads a file. Default: {Synacor9000.DefaultLoadFile}" },
        { Save, $"Saves the running program state. Default: {Synacor9000.DefaultSaveFile}" },
        { Run,  "Starts the loaded program" },
        { Exit, "Ends the program." }
    };

}