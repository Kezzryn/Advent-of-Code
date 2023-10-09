using System.Diagnostics;
using System.Numerics;
using System.Xml.XPath;

namespace AoC_2020_Day_20
{
    internal class PuzzlePiece
    {
        private int _up;
        private int _down;
        private int _left;
        private int _right;

        private readonly int _len = 0;
        private readonly string[] _tile;
        private readonly Dictionary<int, int> _reverse = new();

        public List<int> AllNums() => _reverse.Values.Where(x => x != 0).ToList();
        public string[] Tile => _tile;
        public int TileSize => _tile.Length;
        public int Up { get { return _up; } }
        public int Down { get { return _down; } }
        public int Right { get { return _right; } }
        public int Left { get { return _left; } }

        public string Orientation {  get { return $"{_totalRotation} {(_isFlipped ? "T" : "F")}"; } }

        public PuzzlePiece(string top, string bottom, string left, string right, string center)
        {
            int temp;

            _len = top.Length;

            //Reverse 'cause y=0 is "down", but the file is passed in top first. 
            _tile = center.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Reverse().ToArray();

            _up = Convert.ToInt32(top.Replace('.', '0').Replace('#', '1'), 2);
            temp = ReverseBinaryInt(_up);
            _reverse.TryAdd(_up, temp);
            _reverse.TryAdd(temp, _up);

            _down = Convert.ToInt32(bottom.Replace('.', '0').Replace('#', '1'), 2);
            temp = ReverseBinaryInt(_down);
            _reverse.TryAdd(_down, temp);
            _reverse.TryAdd(temp, _down);

            _left = Convert.ToInt32(left.Replace('.', '0').Replace('#', '1'), 2);
            temp = ReverseBinaryInt(_left);
            _reverse.TryAdd(_left, temp);
            _reverse.TryAdd(temp, _left);

            _right = Convert.ToInt32(right.Replace('.', '0').Replace('#', '1'), 2);
            temp = ReverseBinaryInt(_right);
            _reverse.TryAdd(_right, temp);
            _reverse.TryAdd(temp, _right);
        }

        private int ReverseBinaryInt(int value)
        {
            string temp = Convert.ToString(value, 2);
            char[] tempAry = temp.PadLeft(_len, '0').ToCharArray();
            Array.Reverse(tempAry);
            return Convert.ToInt32(new(tempAry), 2);
        }

        public void ClearEdges(List<int> nonMatches)
        {
            void UpdateInternals(int value)
            {
                _reverse.Remove(value);
                _reverse.Remove(ReverseBinaryInt(value));
                _reverse.TryAdd(0, 0);
            }

            foreach (int value in nonMatches)
            {
                if (_up == value)
                {
                    _up = 0;
                    UpdateInternals(value);
                }

                if (_down == value)
                {
                    _down = 0;
                    UpdateInternals(value);
                }

                if (_left == value)
                {
                    _left = 0;
                    UpdateInternals(value);
                }

                if (_right == value)
                {
                    _right = 0;
                    UpdateInternals(value);
                }
            }
        }

        public bool MakeFit(int top, int right, int bottom, int left)
        {
            for (int i = 1; i <= 8; i++)
            {
                int sidesMatch = 0;

                if (i == 5) Flip();

                sidesMatch += _up == top || top == -1 ? 1 : 0;
                sidesMatch += _right == right || right == -1 ? 1 : 0;
                sidesMatch += _down == bottom || bottom == -1 ? 1 : 0;
                sidesMatch += _left == left || left == -1 ? 1 : 0;

                if (sidesMatch == 4) return true; // early successful exit

                Rotate();
            }

            return false;
        }

        public void Flip()
        {
            _isFlipped = !_isFlipped;
            if (Math.Abs(_totalRotation) == 90 || Math.Abs(_totalRotation) == 270) _totalRotation = (_totalRotation - 180) % 360;

            // horizontal flip
            _up = _reverse[_up];          //top reverse 
            _down = _reverse[_down];    //bottom reverse
            (_left, _right) = (_right, _left);      // left <-> right 
        }

        private void Rotate()
        {

            _totalRotation = (_totalRotation - 90) % 360;

            int temp = _up;

            //CCW to match rotation matrix thing.
            _up = _right;
            _right = _reverse[_down];
            _down = _left;
            _left = _reverse[temp];

            //CW
            //_up = _reverse[_left];
            //_left = _down;
            //_down = _reverse[_right];
            //_right = temp; 
        }

        public void PrintTile(bool doLineBreak = true)
        {
            Console.WriteLine($"U:{_up} R:{_right} D:{_down} L:{_left} Rot:{_totalRotation} Fl:{_isFlipped}");
            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                {
                    Console.Write($"{GetTileFragment(x, y)}");
                }
                Console.WriteLine();
            }

            if (doLineBreak) Console.WriteLine();
        }

        public char GetTileFragment(int x, int y)
        {
            (int newX, int newY) = TranslatePosition(x, y);
            return _tile[newY][newX];
        }

        int _totalRotation = 0;
        bool _isFlipped = false;

        public (int x, int y) TranslatePosition(int currX, int currY)
        {
            //Offset that centerpoint 
            float centerPoint = (float)(TileSize - 1) / 2;

            Vector2 coords = new(currX, currY);

            // positive angles indicates a CCW rotation, negative, CW.
            Matrix3x2 rotationMatrix = _totalRotation switch
            {
                0 => Matrix3x2.CreateRotation(0, new Vector2(centerPoint, centerPoint)),
                90 or -270 => Matrix3x2.CreateRotation((float)(Math.PI / 2), new Vector2(centerPoint, centerPoint)),
                180 or -180 => Matrix3x2.CreateRotation((float)Math.PI, new Vector2(centerPoint, centerPoint)),
                270 or -90 => Matrix3x2.CreateRotation((float)((3 * Math.PI) / 2), new Vector2(centerPoint, centerPoint)),
                _ => throw new NotImplementedException($"Unknown rotaton. {_totalRotation}")
            };

            coords = Vector2.Transform(coords, rotationMatrix); // rotate the grid around the center point. 

            //flip AFTER rotation.
            if (_isFlipped ) coords.X = -coords.X + (TileSize - 1);

            return ((int)coords.X, (int)coords.Y);
        }
    }
}