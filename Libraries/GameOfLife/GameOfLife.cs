using BKH.AoC_Point2D;

namespace BKH.BaseGameOfLife
{
    public class GameOfLife
    {
        protected readonly List<Dictionary<Point2D, int>> _board = [[], []];
        protected int _targetBoard = 1;
        protected int _currentBoard = 0;

        public GameOfLife(string[] puzzleInput)
        {
            int y = 0;
            // flip this so that our text file and array display are in sync for easier debugging. 
            foreach (string line in puzzleInput.Reverse())
            {
                for (int x = 0; x < line.Length; x++)
                {
                    if (puzzleInput[y][x] == '#') _board[_currentBoard].Add(new(x, y), 1);
                }
                y++;
            }

            //FlipBoard();
        }

        virtual protected int NewValue(int x, int y, bool doPart2 = false)
        {
            throw new NotImplementedException("Need to override NewValue()");
        }

        private void FlipBoard()
        {
            (_targetBoard, _currentBoard) = (_currentBoard, _targetBoard);
        }
        public void Step(bool doPart2 = false)
        {
            int lowerX = _board[_currentBoard].Keys.Min(x => x.X) -1;
            int upperX = _board[_currentBoard].Keys.Max(x => x.X) +1;
            int lowerY = _board[_currentBoard].Keys.Min(x => x.Y) -1;
            int upperY = _board[_currentBoard].Keys.Max(x => x.Y) +1;

            _board[_targetBoard].Clear(); 
            for (int x = lowerX; x <= upperX; x++)
            {
                for (int y = lowerY; y <= upperY; y++)
                {
                    int newValue = NewValue(x, y, doPart2);
                    if (newValue != 0)
                    {
                        _board[_targetBoard].Add(new(x, y), newValue);
                    }
                }
            }

            FlipBoard();
        }

        public int CountCells => _board[_currentBoard].Count;

        public void PrintCells()
        {
            int lowerX = _board[_currentBoard].Keys.Min(x => x.X);
            int upperX = _board[_currentBoard].Keys.Max(x => x.X);
            int lowerY = _board[_currentBoard].Keys.Min(x => x.Y);
            int upperY = _board[_currentBoard].Keys.Max(x => x.Y);

            for (int y = lowerY; y <= upperY; y++)
            {
                for (int x = lowerX; x <= upperX; x++)
                {
                    if (_board[_currentBoard].ContainsKey(new(x, y))) Console.Write('#'); else Console.Write('.');
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
