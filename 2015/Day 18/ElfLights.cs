using BKH.AoC_Point2D;
using BKH.BaseGameOfLife;

namespace AoC_2015_Day_18
{
    internal class ElfLights(string[] puzzleInput) : GameOfLife(puzzleInput) 
    {
        private static readonly List<Point2D> _stuckLights = [ new(0,0),   new(0,99), new(99,0), new(99,99) ];

        override protected int NewValue(int x, int y, bool doPart2 = false)
        {
            const int upperBound = 99;

            // bounds check.
            if (x < 0 || y < 0) return 0;
            if (x > upperBound || y > upperBound) return 0;

            Point2D currentPoint = new (x,y);

            if (doPart2 && _stuckLights.Contains(currentPoint)) return 1;

            if (!_board[_currentBoard].TryGetValue(currentPoint, out int currentValue))
            {
                currentValue = 0;
            }

            int numNeighbors = currentPoint.GetAllNeighbors().Count(x => _board[_currentBoard].ContainsKey(x));

            //A light which is off turns on if exactly 3 neighbors are on, and stays off otherwise.
            if (currentValue == 0 && numNeighbors == 3) return 1;

            //A light which is on stays on when 2 or 3 neighbors are on, and turns off otherwise.
            if (currentValue == 1 && (numNeighbors == 2 || numNeighbors == 3)) return 1;

            return 0;
        }
    }
}
