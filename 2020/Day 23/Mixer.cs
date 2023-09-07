using System.Diagnostics;
using System.Text;

namespace AoC_2020_Day_23
{
    internal class Mixer
    {
        private readonly (int value, int nextIndex)[] _cups;

        private readonly int _indexOfOne = -1;
        private readonly int _maxCup = int.MinValue;
        private int _cursor = 0;

        public Mixer(string puzzleInput, int numNodes = -1)
        {
            if (numNodes == -1) numNodes = puzzleInput.Length;
            _cups = new (int, int)[numNodes];

            for (int i = 0; i < numNodes; i++)
            {
                _cups[i].nextIndex = i + 1;

                if(i < puzzleInput.Length)
                {
                    _cups[i].value = puzzleInput[i] - 48;
                }
                else
                {
                    _cups[i].value = i + 1;
                }

                if (_cups[i].value == 1) _indexOfOne = i;
            }

            _maxCup = numNodes;

            _cups[_cups.GetUpperBound(0)].nextIndex = 0;
        }

        public string Part1Answer()
        {
            StringBuilder sb = new();
            int j = 0;
            int currIndex = _cups[_indexOfOne].nextIndex;

            while (j < _cups.Length - 1)
            {
                sb.Append(_cups[currIndex].value);
                currIndex = _cups[currIndex].nextIndex;
                j++;
            }

            return sb.ToString();
        }

        public long Part2Answer()
        {
            int indexA = _cups[_indexOfOne].nextIndex;
            int indexB = _cups[indexA].nextIndex;

            long multA = _cups[indexA].value;
            long multB = _cups[indexB].value;
            return multA * multB;
        }

        public void Mix(int NUM_MIXES = 1)
        {
            for (int y = 1; y <= NUM_MIXES; y++)
            {
                //The crab picks up the three cups that are immediately clockwise of the current cup.They are removed from the circle; cup spacing is adjusted as necessary to maintain the circle.

                int pickUpIndex = _cups[_cursor].nextIndex; // Where the crab picks up from 

                int stichIndex = _cups[_cursor].nextIndex; // The cup to be joined to head. 
                stichIndex = _cups[stichIndex].nextIndex;
                stichIndex = _cups[stichIndex].nextIndex;
                stichIndex = _cups[stichIndex].nextIndex;

                _cups[_cursor].nextIndex = stichIndex;

                //The crab selects a destination cup: the cup with a label equal to the current cup's label minus one. If this would select one of the cups that was just picked up, the crab will keep subtracting one until it finds a cup that wasn't just picked up. If at any point in this process the value goes below the lowest value on any cup's label, it wraps around to the highest value on any cup's label instead.

                int tempIndex = pickUpIndex;
                int[] upCups = new int[3];  // values of the picked up cups.
                for (int i = 0;i < 3; i++)
                {
                    upCups[i] = _cups[tempIndex].value;
                    tempIndex = _cups[tempIndex].nextIndex;
                }

                int newCursorValue = _cups[_cursor].value;

                do
                {
                    newCursorValue--;
                    if (newCursorValue < 1) newCursorValue = _maxCup;
                } while (upCups[0] == newCursorValue || upCups[1] == newCursorValue || upCups[2] == newCursorValue);

                int insertIndex = -1;   // Where to insert the cups that were picked up
                if (newCursorValue > 9)
                {
                    insertIndex = newCursorValue - 1;
                }
                else
                {
                    for (int i = 0; i < 9; i++)
                    {
                        if (_cups[i].value == newCursorValue)
                        {
                            insertIndex = i;
                            break;
                        }
                    }
                }

                //The crab places the cups it just picked up so that they are immediately clockwise of the destination cup.They keep the same order as when they were picked up.
                int insertIndexNext = _cups[insertIndex].nextIndex;

                //join the head. 
                _cups[insertIndex].nextIndex = pickUpIndex;

                pickUpIndex = _cups[pickUpIndex].nextIndex;
                pickUpIndex = _cups[pickUpIndex].nextIndex;

                //join the tail
                _cups[pickUpIndex].nextIndex = insertIndexNext;

                //The crab selects a new current cup: the cup which is immediately clockwise of the current cup.
                _cursor = _cups[_cursor].nextIndex;
            }
        }
    }
}
