﻿using System.Timers;

namespace AoC_2016_Day_05
{
    internal static class Globals
    {
        public static string part1Answer = "";
        public static char[] part2Answer = "????????".ToCharArray();
        public static string secretKey = "";
    }

    internal static class Cinematic
    {
        public static readonly bool GRAND_FINALE = true;
        private static int messagePinger = 0;
        private static readonly Random rnd = new ();
        private static readonly HashSet<int> msgTracker = [];
        private static int cursorTop = 0;

        private static readonly string[] BootMessages =
        [
            "Hackmaster 9000 loading.",
            " ",
            "Resonance Engine ... Online.",
            "Logistical Engine ... Verified.",
            "Countermeasures ... ... ... Initialized.",
            "Homing signal ... Searching ... ... ... No route detected.",
            "Matrix connection ... Follow the white rabbit.",
            "GPS ... Pinpointed.",
            "Discorporate Resonance Individual with Determinate Enforced Reasoning v5.71 ... ... Activated.",
            "Sequence start completed.",
            "All systems nominal."
        ];

        private static readonly string[] hackMessages =
        [
            "Asking Wintermute.",
            "Hacking the Gibson.",
            "Looking for post-it notes.",
            "Deploying logic bomb.",
            "Come on, baby.",
            "Too easy.",
            "In English, please!",
            "De-anonymizing Anonymous.",
            "Ignorance is bliss.",
            "Nothing is impossible.",
            "Turning off CAPS-LOCK.",
            "Is it \"swordfish\"?",
            "One, one, one... uh... one!",
            "\"12345?\" That's amazing!",
            "Trying \"password123.\"",
            "Trying \"password1234.\""
        ];

        public static void DoBoot()
        {
            Console.Clear();
            foreach (string message in BootMessages)
            {
                foreach (string chunk in message.Split("..."))
                {
                    Console.Write(chunk);
                    if (chunk[^1] != '.')
                    {
                        Console.Write("...");
                        Thread.Sleep(rnd.Next(150, 350));
                    }
                }
                Console.WriteLine();
                Thread.Sleep(rnd.Next(500,1000)); 
            }
            Console.WriteLine();
            Console.WriteLine("Enter secret key to begin.");
            Console.Write("HM9000> ");

            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && Globals.secretKey.Length > 0)
                {
                    Console.Write("\b \b");
                    Globals.secretKey = Globals.secretKey[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    Globals.secretKey += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            Console.WriteLine();
            Console.WriteLine();

            cursorTop = Console.CursorTop;
        }

        public static void DoTick(Object? source, ElapsedEventArgs e)
        {
            DoCinematic();
        }

        private static void DoMessages(bool isFinal = false)
        {
            Random rnd = new();

            Console.SetCursorPosition(0, cursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, cursorTop);

            if (isFinal)
            {
                Console.WriteLine($"We're in!");
            }
            else
            {
                if (msgTracker.Count == hackMessages.Length) msgTracker.Clear();
                int getNext = rnd.Next(0, hackMessages.Length);

                while (msgTracker.Contains(getNext)) getNext = rnd.Next(0, hackMessages.Length);

                Console.WriteLine($"{hackMessages[getNext]}");
                msgTracker.Add(getNext);
            }
        }

        public static void DoCinematic(bool isFinal = false)
        {
            var forgroundColor = Console.ForegroundColor;

            Random rnd = new();
            if (messagePinger++ % 4 == 0 || isFinal) DoMessages(isFinal);

            Console.SetCursorPosition(0, cursorTop+2);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(Globals.part1Answer);
            for (int i = 7; i >= Globals.part1Answer.Length; i--)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write((char)rnd.Next('0', 'z'));
            }

            Console.Write(' ');

            foreach (char ch in Globals.part2Answer)
            {
                if (ch == '?')
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write((char)rnd.Next('0', 'z'));
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(ch);
                }
            }
            Console.ForegroundColor = forgroundColor;

            if (isFinal)
            {
                Console.SetCursorPosition(0, cursorTop+2);

                Console.Write("The first door password is ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(Globals.part1Answer);
                Console.ForegroundColor = forgroundColor;
                Console.WriteLine(".");


                Console.Write("The second door password is ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(Globals.part2Answer);
                Console.ForegroundColor = forgroundColor;
                Console.WriteLine(".");
            }
        }
    }
}
