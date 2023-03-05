using Synacor_Challenge;
using System.Diagnostics;
using System.Text;

try
{
    Synacor9000 syn9k = new();
    bool isDone = false;

    Console.TreatControlCAsInput = false;

    void WriteError(string message)
    {
        ConsoleColor ccFC = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("*** ERROR : ");
        Console.ForegroundColor = ccFC;
        Console.WriteLine(message);
    }

    void DoSomething(string command)
    {
        string[] split = command.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        switch (split[0])
        {
            case "exit":
                isDone = true;
                Console.WriteLine("Bye!");
                break;
            case "load" or "save":
                string fileName = (split.GetUpperBound(0) >= 1) ? split[1] : "challenge.bin";
                try
                {
                    if (split[0] == "save") syn9k.Save(fileName);
                    if (split[0] == "load") syn9k.Load(fileName);
                    Console.WriteLine($"{split[0]}ed {fileName}");
                }
                catch (Exception e)
                {
                    WriteError(e.Message);
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

    Console.Clear();
    Console.WriteLine("Welcome to the puzzle.");

    while (!isDone)
    {
            Console.WriteLine();
            Console.Write("s9k> ");
            string? input = Console.ReadLine();
            if (input != null) DoSomething(input);
    }
    

}
catch (Exception e)
{
    Console.WriteLine(e);
}