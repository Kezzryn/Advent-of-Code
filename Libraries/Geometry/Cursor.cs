namespace BKH.Geometry;
using System.Numerics;

public class Cursor : IEquatable<Cursor>
{
    private Complex _pos;
    private Complex _dir;
    
    public bool DoMirrorTurns = false; // True for grids where the Y axis is 0 and increases "down".

    public int X { get { return (int)_pos.Real; } }
    public int Y { get { return (int)_pos.Imaginary; } }
    public (int X, int Y) XY { get { return (X, Y); } }
    public Point2D XYAsPoint2D { get { return new Point2D(X, Y); } }
    public Point2D DirAsPoint2D { get { return new Point2D(GetDir.X, GetDir.Y); } }
    public (int X, int Y) GetDir { get { return ((int)_dir.Real, (int)_dir.Imaginary); } }
    public bool IsMovingRight { get { return _dir.Imaginary == 0 && double.IsPositive(_dir.Real); } }
    public bool IsMovingLeft { get { return _dir.Imaginary == 0 && double.IsNegative(_dir.Real); } }
    public bool IsMovingUp { get { return _dir.Real == 0 && double.IsNegative(_dir.Imaginary); } } 
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
        DoMirrorTurns = other.DoMirrorTurns;
    }

    public (int X, int Y) NextStep()
    {
        Complex temp = _pos + _dir;
        return ((int)temp.Real, (int)temp.Imaginary);
    }

    public void Step(int num = 1)
    {
        if (num <= 0) return;
        _pos += (_dir * num);
    }
    
    //NOTE: This assumes a "normal" 2d grid where Y increases as it goes "up".
    //MirrorTurns is for Y increases "down" 
    public void TurnLeft()
    {
        if(DoMirrorTurns)
            _dir *= -Complex.ImaginaryOne;
        else
            _dir *= Complex.ImaginaryOne;
    }

    public void TurnRight()
    {
        if(DoMirrorTurns)
             _dir *= Complex.ImaginaryOne;
        else
            _dir *= -Complex.ImaginaryOne;
    }

    public void TurnAround() => _dir *= -1;

    public Cursor ReturnCloneTurnRight()
    {
        Cursor temp = new(this);
        temp.TurnRight();
        return temp;
    }

    public Cursor ReturnCloneTurnLeft()
    {
        Cursor temp = new(this);
        temp.TurnLeft();
        return temp;
    }

    public override string ToString() => $"Pos: {_pos}  Dir: {_dir}";

    public override int GetHashCode() => HashCode.Combine(_pos.GetHashCode(), _dir.GetHashCode());

    public bool Equals(Cursor? other)
    {
        if (other is null) return false;
        return _pos == other._pos && _dir == other._dir;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Cursor);
    }
}