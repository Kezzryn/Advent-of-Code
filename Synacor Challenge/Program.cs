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
    Synacor9000 syn9k = new();
    bool isDone = false;

    Console.Title = "SynOS 9000";
    Console.TreatControlCAsInput = true;
    
    void DoSomething(string command)
    {
        string[] split = command.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        switch (split[0])
        {
            case "exit":
                isDone = true;
                Console.WriteLine("Bye!");
                break;
            case "load":
                string fileName = (split.GetUpperBound(0) >= 1) ? split[1] : "";
                if (syn9k.Load(fileName, out string? error))
                {
                    Console.WriteLine($"{split[0]} of {(fileName == string.Empty ? Synacor9000.DefaultLoadFile : fileName)} done.");
                }
                else
                {
                    WriteError(error!);
                }
                break;
            case "save":
                fileName = (split.GetUpperBound(0) >= 1) ? split[1] : "";
                if (syn9k.Save(fileName, out error))
                {
                    Console.WriteLine($"{split[0]} of {(fileName == string.Empty ? Synacor9000.DefaultSaveFile : fileName)} done.");
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
                Console.WriteLine("Yea, we need some.");
                break;
            default:
                Console.WriteLine(split[0]);
                Console.WriteLine("Command not recognized.");
                break;
        }
    }

    string ReadLine()
    {
        ConsoleKeyInfo cki;
        StringBuilder sb = new();
        bool breakOut = false;
        do
        {
            cki = Console.ReadKey();
            switch (cki.Key)
            {
                case ConsoleKey.F1:
                    Console.Write("help");
                    sb.Clear();
                    sb.Append("help");
                    breakOut = true;
                    break;
                case ConsoleKey.F2:
                    Console.Write("load");
                    sb.Clear();
                    sb.Append("load");
                    breakOut = true;
                    //Save("quicksave.bin");
                    break;
                case ConsoleKey.F3:
                    Console.Write("run");
                    sb.Clear();
                    sb.Append("run");
                    breakOut = true;
                    break;
                case ConsoleKey.Escape:
                    isDone = true;
                    breakOut = true;
                    break;
                case ConsoleKey.Backspace:
                    if (sb.Length > 0) sb = sb.Remove(sb.Length - 1, 1);
                    // backspace moves the cursor back, so whitespace, then \b to back the cursor up again. 
                    Console.Write(" \b");
                    break;
                case ConsoleKey.Enter:
                    breakOut = true;
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
        } while (!breakOut);
        Console.WriteLine();
        return sb.ToString();
    }

    Console.Clear();
    Console.WriteLine("Welcome to the puzzle.");

    while (!isDone)
    {
            Console.WriteLine();
            Console.Write("SynOS > ");
            string input = ReadLine();
            if (!string.IsNullOrEmpty(input)) DoSomething(input);
    }
}
catch (Exception e)
{
    Console.WriteLine(e);
}