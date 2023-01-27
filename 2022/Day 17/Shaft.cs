namespace AoC_2022_Day_17
{
    internal class Shaft
    {
        private readonly Jets jet;
        private readonly Rocks rock = new();

        private const int SHAFT_WIDTH = 7; // the x
        private const int SHAFT_WIDTH_ZB = SHAFT_WIDTH - 1; //x but zero bound. 
        private const int SHAFT_HEIGHT = 5000; // we need only 200 or so for the current window. making it larger just in case... 
        //private const int SHAFT_HEIGHT_ZB = SHAFT_HEIGHT - 1; // y but zero bound.  Never actually needed it. 

        private bool[,] theShaft = new bool[SHAFT_WIDTH, SHAFT_HEIGHT];
        private char[,] theDebug = new char[SHAFT_WIDTH, SHAFT_HEIGHT];

        private bool _verbose = false;

        public int ShaftHeight { get; set; }

        public Shaft(string puzzleData)
        {
            jet = new Jets(puzzleData);

            for (int y = 0; y < SHAFT_HEIGHT; y++)
                for (int x = 0; x < SHAFT_WIDTH; x++)
                    theShaft[x, y] = (y == 0) ? Rocks.ROCK : Rocks.AIR;

            ShaftHeight = 0;
        }

        public void ToggleVerbose() => _verbose = !_verbose;

        public void PrintShaft(int startHeight, int depth)
        {
            depth = int.Max(0, startHeight - depth);
            startHeight = int.Min(theShaft.GetLength(1), startHeight);

            for (int y = startHeight; y >= depth; y--)
            {
                for (int x = 0; x <= SHAFT_WIDTH_ZB; x++)
                {
                    if (theShaft[x, y])
                    {
                        Console.Write($"{theDebug[x, y]}");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine($"   {y}");
            }
            Console.WriteLine();
        }

        public string GetHash(int HashWindow = 20)
        {
            string returnValue = "";

            returnValue += jet.GetJetIndex();
            returnValue += rock.CurrentRockID();

            for (int y = ShaftHeight; y >= int.Max(0, ShaftHeight - HashWindow); y--)
            {
                for (int x = 0; x <= SHAFT_WIDTH_ZB; x++)
                {
                    returnValue += theShaft[x, y] ? "1" : "0";
                }
            }

            return returnValue;
        }

        public void DropRock()
        {
            //spawn a rock in two units away from the left side and three units above the current floor. 
            bool isDone = false;

            rock.NextRock();

            rock.HandleX = 2;
            rock.HandleY = ShaftHeight + 4;

            if (_verbose) Console.WriteLine($"Initial handle : {rock.HandleX} {rock.HandleY}");

            while (!isDone)
            {
                Push();
                if (_verbose) Console.WriteLine($"Handle after push : {rock.HandleX} {rock.HandleY}");

                isDone = Fall();
                if (_verbose && !isDone) Console.WriteLine($"Handle after fall : {rock.HandleX} {rock.HandleY}");
            }
        }

        private bool IsCollision(int xOffset, int yOffset)
        {
            for (int x = 0; x <= rock.RockWidth; x++)
            {
                for (int y = 0; y <= rock.RockHeight; y++)
                {
                    if (rock.GetCurrentRock()[x, y] && theShaft[x + xOffset, y + yOffset]) return true;
                }
            }
            return false;
        }

        private void PasteRock(int xOffset, int yOffset)
        {

            for (int x = 0; x <= rock.RockWidth; x++)
            {
                for (int y = 0; y <= rock.RockHeight; y++)
                {
                    if (rock.GetCurrentRock()[x, y])
                    {
                        theShaft[x + xOffset, y + yOffset] = Rocks.ROCK;
                        theDebug[x + xOffset, y + yOffset] = rock.CurrentRockID();
                    }
                }
            }
        }

        private void Push()
        {
            switch (jet.GetNextJet())
            {
                case Jets.Left:
                    if (_verbose) Console.Write($"Try move left: ");
                    if (rock.HandleX == 0)
                    {
                        if (_verbose) Console.WriteLine($"We're against a wall");
                        break;
                    }
                    if (!IsCollision(rock.HandleX - 1, rock.HandleY))
                    {
                        if (_verbose) Console.WriteLine($"No collision.");
                        rock.HandleX--;
                    }
                    break;
                case Jets.Right:
                    if (_verbose) Console.Write($"Try move right: ");
                    if (rock.HandleX + rock.RockWidth >= SHAFT_WIDTH_ZB)
                    {
                        if (_verbose) Console.WriteLine($"We're against a wall");
                        break;
                    }
                    if (!IsCollision(rock.HandleX + 1, rock.HandleY))
                    {
                        if (_verbose) Console.WriteLine($"No collision.");
                        rock.HandleX++;
                    }
                    break;
            }
        }

        private bool Fall()
        {
            //return false if it goes down, true if it gets stuck.
            bool returnValue = false;

            if (IsCollision(rock.HandleX, rock.HandleY - 1))
            {
                PasteRock(rock.HandleX, rock.HandleY);

                ShaftHeight = int.Max(ShaftHeight, rock.HandleY + rock.RockHeight);

                if (_verbose) Console.WriteLine($"Collision on drop. Pasting at {rock.HandleX} {rock.HandleY} New height {ShaftHeight}");
                returnValue = true;
            }
            else
            {
                rock.HandleY--;
            }

            return returnValue;
        }
    }
}
