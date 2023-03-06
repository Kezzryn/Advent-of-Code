using System.Text;

namespace Synacor_Challenge
{
    internal partial class Synacor9000
    {
        /*
         * This file contains the code for getting and processing our console input.
         * Nearly all of this is for opcode 20, but some does touch on flow control. 
         */

        private string _inputBuffer = string.Empty;
        private string GetConsoleInput()
        {
            const char NEWLINE = '\n';
            const string CMD_EXIT = "exit";

            string returnValue;

            StringBuilder sb = new();

            Dictionary<ConsoleKey, string> quickCommands = new()
            {
                { ConsoleKey.RightArrow, "east" },
                { ConsoleKey.LeftArrow, "west" },
                { ConsoleKey.UpArrow, "north" },
                { ConsoleKey.DownArrow, "south" },
                { ConsoleKey.Escape, CMD_EXIT },
            };

            ConsoleKeyInfo cki;
            bool doneInput = false;
            do
            {
                cki = Console.ReadKey();
                switch (cki.Key)
                {
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.Escape:
                        if (quickCommands.TryGetValue(cki.Key, out string? cmd))
                        {
                            Console.Write(cmd);
                            sb.Clear();
                            sb.Append(cmd);
                            sb.Append(NEWLINE);
                            doneInput = true;
                        }
                        break;
                    case ConsoleKey.Enter:
                        Console.Write(NEWLINE);
                        sb.Append(NEWLINE);
                        doneInput = true;
                        break;
                    case ConsoleKey.Backspace:
                        if (sb.Length > 0) sb = sb.Remove(sb.Length - 1, 1);
                        // backspace moves the cursor back, so whitespace to clear, then \b to back the cursor up again. 
                        Console.Write(" \b");
                        break;
                    default:
                        if ((cki.Modifiers & ConsoleModifiers.Control) != 0 && cki.Key == ConsoleKey.C)
                        {
                            sb.Clear();
                            sb.Append(CMD_EXIT);
                            doneInput = true;
                            break;
                        }
                        sb.Append(cki.KeyChar);
                        break;
                }
            } while (!doneInput);

            // don't pass back ENTER only.
            // capture other stuff. 
            if (string.IsNullOrEmpty(sb.ToString()) || sb.ToString()[0] == NEWLINE)
            {
                returnValue = string.Empty;
            }
            else
            {
                returnValue = sb.ToString();
                switch (returnValue[..^1].ToLower()) // strip newline for testing. 
                {
                    case CMD_EXIT:
                        _stopExecution = true;
                        break;
                    default:
                        break;
                }
            }

            return returnValue;
        }
    }
}
