using System.Text;

namespace Synacor_Challenge
{
    internal partial class Synacor9000
    {
        /*
         * This file contains the code for getting and processing our console input.
         * Nearly all of this is for opcode 20, but some does touch on flow control. 
         */

        private const char NEWLINE = '\n';

        private string _inputBuffer = string.Empty;
        private string GetConsoleInput()
        {
            
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
                    case ConsoleKey.A:
                    case ConsoleKey.B:
                    case ConsoleKey.C:
                    case ConsoleKey.D:
                    case ConsoleKey.E:
                    case ConsoleKey.F:
                    case ConsoleKey.G:
                    case ConsoleKey.H:
                    case ConsoleKey.I:
                    case ConsoleKey.J:
                    case ConsoleKey.K:
                    case ConsoleKey.L:
                    case ConsoleKey.M:
                    case ConsoleKey.N:
                    case ConsoleKey.O:
                    case ConsoleKey.P:
                    case ConsoleKey.Q:
                    case ConsoleKey.R:
                    case ConsoleKey.S:
                    case ConsoleKey.T:
                    case ConsoleKey.U:
                    case ConsoleKey.V:
                    case ConsoleKey.W:
                    case ConsoleKey.X:
                    case ConsoleKey.Y:
                    case ConsoleKey.Z:
                        sb.Append(cki.KeyChar);
                        break;
                    default:
                        if ((cki.Modifiers & ConsoleModifiers.Control) != 0 && cki.Key == ConsoleKey.C)
                        {
                            sb.Clear();
                            sb.Append(CMD_EXIT);
                            doneInput = true;
                            break;
                        }
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
