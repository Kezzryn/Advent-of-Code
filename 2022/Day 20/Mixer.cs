namespace AoC_2022_Day_20
{
    internal class Mixer
    {
        private readonly (int prevIndex, int nextIndex, long value)[] code;

        private readonly int _indexOfZero = -1;
        private int _indexOfHead = -1;
        private bool _verbose = false;

        public Mixer(string[] puzzleInput, int decryptionKey = 1)
        {
            code = new (int, int, long)[puzzleInput.Length];

            for (int i = 0; i < puzzleInput.Length; i++)
            {
                code[i].prevIndex = (i - 1 < puzzleInput.GetLowerBound(0)) ? puzzleInput.GetUpperBound(0) : i - 1;
                code[i].nextIndex = (i + 1 > puzzleInput.GetUpperBound(0)) ? puzzleInput.GetLowerBound(0) : i + 1;
                code[i].value = long.Parse(puzzleInput[i]) * decryptionKey;
                if (code[i].value == 0) _indexOfZero = i;
            }
            _indexOfHead = code.GetLowerBound(0);
        }

        public void ToggleVerbose() => _verbose = !_verbose;

        public void PrintArray(bool fromHead = true)
        {
            int j = 0;
            int currIndex = (fromHead) ? _indexOfHead : _indexOfZero;

            while (j < code.Length)
            {
                Console.Write($"{code[currIndex].value} ");
                currIndex = code[currIndex].nextIndex;
                j++;
            }
            Console.WriteLine();
        }

        private int FindInsert(int startIndex, long offset)
        {
            int returnValue = startIndex;

            long steps = offset % (code.LongLength - 1); // -1 to account for the element we've yoinked. 
            if (_verbose) Console.WriteLine($"steps :: {steps} = {offset} % {code.LongLength - 1}");
            bool GoLeft = steps < 0;
            int i = 0;

            if (steps == 0) return startIndex; //this could throw an index out of bounds elsewhere but that's fine, it means things are really messed up. 

            while (i < Math.Abs(steps))
            {
                returnValue = (GoLeft) ? code[returnValue].prevIndex : code[returnValue].nextIndex;
                i++;
            }

            if (GoLeft) returnValue = code[returnValue].prevIndex; //one more time, to make sure our cursor is properly placed "before" our insert point. 

            return returnValue;
        }

        public void Mix(int NUM_MIXES = 1)
        {
            //mix master
            for (int y = 1; y <= NUM_MIXES; y++)
            {
                if (_verbose)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"INITIAL STATE {y}:");
                    Console.ResetColor();
                    PrintArray();
                }
                for (int i = code.GetLowerBound(0); i <= code.GetUpperBound(0); i++)
                {
                    if (code[i].value == 0) continue; // skip this, we don't change anything. 

                    // remove ourself from our current location, and join the break in the chain. 
                    // we do this now so massive loop iterations skip over us. 
                    code[code[i].prevIndex].nextIndex = code[i].nextIndex;
                    code[code[i].nextIndex].prevIndex = code[i].prevIndex;


                    // find our new insert point. 
                    int delta = FindInsert(i, code[i].value);
                    if (_verbose) Console.WriteLine($"{i}) Moving value {code[i].value} to after intitial index {delta} ");

                    if (delta == -1 || delta == i)
                    {
                        if (_verbose) Console.WriteLine("Trap card! We're not moving. Stuff ourselves right back in the chain and continue...");
                        code[code[i].prevIndex].nextIndex = i;
                        code[code[i].nextIndex].prevIndex = i;

                        continue;
                    }

                    //update our head marker if it moved. 
                    if (i == _indexOfHead) _indexOfHead = code[i].nextIndex;


                    //we insert ourselves "between" the delta target and the next link.
                    code[i].prevIndex = delta;
                    code[i].nextIndex = code[delta].nextIndex;

                    //and update the links to us from each end.
                    code[code[i].nextIndex].prevIndex = i;
                    code[delta].nextIndex = i;

                    if (_verbose)
                    {
                        Console.Write($"State After move: ");
                        PrintArray();
                        Console.WriteLine();
                    }
                }

                if (_verbose)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"After {y} Mix:");
                    Console.ResetColor();
                    PrintArray();
                    Console.WriteLine();
                }
            }
        }
        public long ValueAt(int index)
        {
            int currentIndex = _indexOfZero;
            long answer = 0;
            for (int i = 1; i <= index; i++)
            {
                currentIndex = code[currentIndex].nextIndex;
                if (i % index == 0)
                {
                    answer = code[currentIndex].value;
                }
            }
            return answer;
        }
    }
}
