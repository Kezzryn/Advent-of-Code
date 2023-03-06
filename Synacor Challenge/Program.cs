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

try
{
    const string PROMPT = "SynOS > ";
    const string TITLE = "SynOS 9000";

    Dictionary<ConsoleKey, string> quickCommands = new()
    {
        { ConsoleKey.F1, "help" },
        { ConsoleKey.F2, "load" },
        { ConsoleKey.F3, "run" },
        { ConsoleKey.F4, "debugger" },
        { ConsoleKey.F5, "save quicksave.syn9k" },
        { ConsoleKey.F6, "load teleporter.syn9k" },
        { ConsoleKey.F7, "load quicksave.syn9k" },
        { ConsoleKey.F8, "help" },
        { ConsoleKey.Escape, "exit" },

    };

    Synacor9000 syn9k = new();
    bool isDone = false;

    Console.Title = TITLE;
    Console.TreatControlCAsInput = true;
    
    void DoSomething(string command)
    {
        string[] split = command.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        switch (split[0])
        {
            case "exit" or "quit":
                isDone = true;
                Console.WriteLine("Bye!");
                break;
            case "load":
                string fileName = (split.GetUpperBound(0) >= 1) ? split[1] : Synacor9000.DefaultLoadFile;
                if (syn9k.Load(fileName, out string? error))
                {
                    Console.WriteLine($"{split[0]} of {fileName} done.");
                }
                else
                {
                    WriteError(error!);
                }
                break;
            case "save":
                fileName = (split.GetUpperBound(0) >= 1) ? split[1] : Synacor9000.DefaultSaveFile;
                if (syn9k.Save(fileName, out error))
                {
                    Console.WriteLine($"{split[0]} of {fileName} done.");
                }
                else
                {
                    WriteError(error!);
                }
                break;
            case "run":
                syn9k.Run();
                break;
            case "help":
                Console.WriteLine("Yea, we need some, but for now, here are the defined quick keys:");
                foreach (var cmds in quickCommands)
                {
                    Console.WriteLine($"{cmds.Key} {cmds.Value}");
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
    Console.WriteLine("Welcome to the puzzle.");

    while (!isDone)
    {
            Console.WriteLine();
            Console.Write(PROMPT);
            string input = ReadLine();
            if (!string.IsNullOrEmpty(input)) DoSomething(input);
    }
}
catch (Exception e)
{
    Console.WriteLine(e);
}