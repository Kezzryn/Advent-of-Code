using Synacor_Challenge;
using System.Diagnostics;
using System.Text;

try
{
    const string PROMPT = "> ";
    const string TITLE = "SynOS 9000";

    Dictionary<ConsoleKey, string> quickCommands = new()
        {
            { ConsoleKey.Escape,    $"{ProgCmds.CMD_CHAR}{ProgCmds.Exit}" },
            { ConsoleKey.F2,        $"{ProgCmds.CMD_CHAR}{ProgCmds.Load} challenge.bin" },
            { ConsoleKey.F3,        $"{ProgCmds.CMD_CHAR}{ProgCmds.Run}" },
            { ConsoleKey.F5,        $"{ProgCmds.CMD_CHAR}{ProgCmds.Load} teleporter.syn9k" },
            { ConsoleKey.F6,        $"{ProgCmds.CMD_CHAR}{ProgCmds.Solve_TP}" },
            { ConsoleKey.F7,        $"{ProgCmds.CMD_CHAR}{ProgCmds.Load} orb.syn9k" },
            { ConsoleKey.F8,        $"{ProgCmds.CMD_CHAR}{ProgCmds.Solve_ORB}" }
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
        Console.Write("*** : ");
        Console.WriteLine(message);
    }

    static void WriteProgramOutput(string message)
    {
        Console.Write(message);
    }

    static void WriteDebuggerOutput(string message, bool isSuccess)
    {
        if (isSuccess)
            Console.WriteLine(message.PadRight(Console.BufferWidth - 1));
        else
            WriteError(message.PadRight(Console.BufferWidth - 1));
    }

    void WriteHelp()
    {
        const int spc = -8;
        Console.WriteLine($"{TITLE} command list:");
        Console.WriteLine($"{"F1",spc} This message.");
        foreach (var cmd in ProgCmds.Command_Descriptions)
        {
            Console.WriteLine($"{cmd.Key,spc} {cmd.Value}");
        }
        Console.WriteLine();
        Console.WriteLine($"All text that is not preceeded by {ProgCmds.CMD_CHAR} is passed without modification to the running program.");
        Console.WriteLine();
        Console.WriteLine("Quick-keys:");
        foreach (KeyValuePair<ConsoleKey, string> cmd in quickCommands)
        {
            Console.WriteLine($"{cmd.Key} {cmd.Value}");
        }
        Console.WriteLine();
    }

    void DoConsoleCommand(string command)
    {
        command = command.Trim(ProgCmds.CMD_CHAR).ToLower();

        string[] split = command.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        string args = (split.GetUpperBound(0) >= 1) ? command[command.IndexOf(split[1])..] : string.Empty;

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
            case ProgCmds.Solve_TP:
                if (!isLoaded)
                {
                    WriteMessage($"No binary loaded.");
                    break;
                }
                
                WriteMessage($"Searching space and time.");

                Stopwatch sw = Stopwatch.StartNew();
                int TP_Code = PuzzleSolutions.Solve_Teleporter().Result;
                sw.Stop();

                WriteMessage($"The teleporter solution code found in {sw.ElapsedMilliseconds / 1000} seconds That's a lot less than a billion years! The code is: {TP_Code}.");
                WriteMessage($"Rewiring teleporter.");
                List<string> rewire = new()
                {
                    $"set register 7 {TP_Code}",
                    "set memory 5485 6",
                    "set memory 5489 21",
                    "set memory 5490 21"
                };
                foreach (string tp_Command in rewire)
                {
                    bool res = syn9k.DebugDispatcher(tp_Command);
                    while (syn9k.GetDebugOutput(out string output))
                    {
                        WriteDebuggerOutput(output, res);
                    }
                }
                break;
            case ProgCmds.Solve_ORB:
                List<string> solution = PuzzleSolutions.Solve_Orb();
                StringBuilder sb = new();
                foreach (string s in solution) sb.Append(s);
                WriteMessage($"The orb solution code is: {sb}");
                break;
            default:
                // pass all unknown !commands commands through. Maybe they're for the debugger?
                bool debugResult = syn9k.DebugDispatcher(command);
                if (isRunning)
                {
                    isRunning = false;
                    WriteMessage("Stopping run for debugging.");
                }
                while (syn9k.GetDebugOutput(out string output))
                {
                    WriteDebuggerOutput(output, debugResult);
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

                    if (quickCommands.TryGetValue(cki.Key, out string? cmd))
                    {
                        Console.Write(cmd);
                        sb.Clear();
                        sb.Append(cmd);
                        doneInput = true;
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

        if (input.StartsWith(ProgCmds.CMD_CHAR))
        {
            DoConsoleCommand(input);
            input = string.Empty;
        }

        if (isLoaded && isRunning)
        {
            if (input != string.Empty) syn9k.SetProgramInput(input);

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
    public const char CMD_CHAR = '!';
    public const string Load = "load";
    public const string Save = "save";
    public const string Run = "run";
    public const string Exit = "exit";
    public const string Solve_TP = "solve_teleporter";
    public const string Solve_ORB = "solve_orb";

    public static Dictionary<string, string> Command_Descriptions = new()
    {
        { Load, $"Loads a file. Default: {Synacor9000.DefaultLoadFile}" },
        { Save, $"Saves the running program state. Default: {Synacor9000.DefaultSaveFile}" },
        { Run,  "Starts the loaded program" },
        { Exit, "Ends the program." },
        { Solve_TP, "Generates the solution to the teleporter puzzle." },
        { Solve_ORB, "Generates the solution to the orb puzzle." }
    };

}