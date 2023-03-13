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
            { ConsoleKey.F2,        $"{ProgCmds.CMD_CHAR}break addy 5482" },
            { ConsoleKey.F3,        $"{ProgCmds.CMD_CHAR}set input use teleporter" },
            { ConsoleKey.F4,        $"{ProgCmds.CMD_CHAR}set register 7 1" },
            { ConsoleKey.F5,        $"{ProgCmds.CMD_CHAR}break run" },
            { ConsoleKey.F6,        $"{ProgCmds.CMD_CHAR}trace echo" },
            { ConsoleKey.F7,        $"{ProgCmds.CMD_CHAR}{ProgCmds.Solve_TP}" },
            { ConsoleKey.F8,        $"{ProgCmds.CMD_CHAR}{ProgCmds.Load} teleporter.syn9k" },
            { ConsoleKey.F9,        $"{ProgCmds.CMD_CHAR}{ProgCmds.Solve_ORB}" },
            { ConsoleKey.F10,       $"{ProgCmds.CMD_CHAR}step 5" },
            { ConsoleKey.F11,       $"{ProgCmds.CMD_CHAR}step 50" }
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
        Console.WriteLine($"All text that is not preceeded by {ProgCmds.CMD_CHAR} is passed without modification to the running program.");
        Console.WriteLine();
        Console.WriteLine("Quick-keys:");
        foreach(KeyValuePair<ConsoleKey, string> cmd in quickCommands)
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
                WriteMessage($"Searching space and time.");
                Stopwatch sw = Stopwatch.StartNew();

                /*
                 * 
                 * 
                  PAckAsync(int reg7)
        {
            // This is a procedural Ackermann implementation. 
            // Copied verbatum from Andy Christianson at https://github.com/NiXXeD/
            // A solution that I wouldn't have arrived at, but is obvious in hindsight. 

            // r0 goes from 4 to 0.  r1 can be between 0 and 32768, so that's our max search space.
            int[,] cache = new int[5, 32769];

            //   (0, _) => AddOne(r1)
            for (int i = 0; i <= 32768; i++)
            {
                cache[0, i] = (i + 1) % 32768;
            }

            // fill in each row of our solution space based on date from the previous rows.
            for (int r0 = 1; r0 <= 4; r0++)
            {
                //  (_, 0) => Ack(SubOne(r0), regSeven)
                cache[r0, 0] = cache[r0 - 1, reg7];

                // This one took me a hot minute. The second part represents the state that only exists in the first few calls of the function. 
                // After that's out of the way, r1 increases to the max of 32768 before rolling over to 0, which we handle on the previous line.
                // but, because we're counting up and not down, we need 
                for (int r1 = 1; (r1 < 32768 && r0 < 4) || (r1 < 2 && r0 == 4); r1++)
                {
                    if (cancellationToken.IsCancellationRequested) return Task.FromCanceled<int>(cancellationToken);
                    //Ack(SubOne(r0), Ack(r0, SubOne(r1)))
                    cache[r0, r1] = cache[r0 - 1, cache[r0, r1 - 1]];
                }
            }

            //and echo back the input that gave us the correct result. 
            return Task.FromResult(cache[4, 1] == TARGET_VALUE ? reg7 : -1);
        }
                 */

                int TP_Code = PuzzleSolutions.Solve().Result;
                sw.Stop();


                WriteMessage($"The teleporter solution code found in {sw.ElapsedMilliseconds / 1000} seconds. {TP_Code}");
                WriteMessage($"Rewiring teleporter.");
                List<string> rewire = new()
                {
                    $"set register 7 {TP_Code}",
                    "set register 0 6",
                    "set memory 5489 21",
                    "set memory 5490 21"
                };
                foreach(string tp_Command in rewire)
                {
                    List<string> tp_responses = new();
                    bool res = syn9k.DebugDispatcher(tp_Command, out tp_responses);
                    foreach(string debug_response in tp_responses)
                    {
                        WriteDebuggerOutput(debug_response, res);
                    }
                }

                break;
            case ProgCmds.Solve_ORB:
                WriteMessage($"The orb solution code is: {PuzzleSolutions.OrbSolver()}");
                break;
            default:
                // pass all unknown !commands commands through. Maybe they're for the debugger?
                bool debugResult = syn9k.DebugDispatcher(command, out List<string> debugResponse);
                if (isRunning)
                {
                    isRunning = false;
                    WriteMessage("Stopping run for debugging.");
                }
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