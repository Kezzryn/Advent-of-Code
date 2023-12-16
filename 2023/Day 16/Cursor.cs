using System.Numerics;

namespace AoC_2023_Day_16
{
    internal class Cursor
    {
        private Complex _pos;
        private Complex _dir;

        public int X { get { return (int)_pos.Real; } }
        public int Y { get { return (int)_pos.Imaginary; } }
        public (int X, int Y) XY { get { return (X, Y); } }
        public (int X, int Y) GetDir { get { return ((int)_dir.Real, (int)_dir.Imaginary); } }
        public bool IsMovingRight { get { return _dir.Imaginary == 0 && double.IsPositive(_dir.Real); } }
        public bool IsMovingLeft { get { return _dir.Imaginary == 0 && double.IsNegative(_dir.Real); } }
        public bool IsMovingUp { get { return _dir.Real == 0 && double.IsNegative(_dir.Imaginary); } } // remember map inversion.
        public bool IsMovingDown { get { return _dir.Real == 0 && double.IsPositive(_dir.Imaginary); } }
        public bool IsHorizontal { get { return IsMovingRight || IsMovingLeft; } }
        public bool IsVertical { get { return IsMovingUp || IsMovingDown; } }
        public Cursor(Complex position, Complex direction)
        {
            _pos = position;
            _dir = direction;
        }

        public Cursor(int X, int Y, int xV, int yV)
        {
            _pos = new Complex(X, Y);
            _dir = new Complex(xV, yV);
        }

        public Cursor(Cursor other)
        {
            _pos = new(other._pos.Real, other._pos.Imaginary);
            _dir = new(other._dir.Real, other._dir.Imaginary);
        }

        public (int X, int Y) NextStep()
        {
            Complex temp = _pos + _dir;
            return ((int)temp.Real, (int)temp.Imaginary);
        }

        public void Step() => _pos += _dir;

        // NOTE: Turn Left/Right assume a "text" map where "North" is a LOWER Y digit.
        // This is reveresd from a "normal" 2d grid where Y increases as it goes "up".
        
        public void TurnLeft() => _dir *= -Complex.ImaginaryOne;
        
        public void TurnRight() => _dir *= Complex.ImaginaryOne;
        
        public override string ToString() => $"Pos: {_pos}  Dir: {_dir}";
    }
}
