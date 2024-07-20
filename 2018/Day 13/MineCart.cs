namespace AoC_2018_Day_13;
using BKH.Geometry;

public enum MapSymbols
{
    Straight,
    Curve_A,   // '/' 
    Curve_B,   // '\'
    Intersection,
    Out_Of_Bounds
}

internal class MineCart
{
    private readonly Cursor _cursor;
    public Point2D Position => _cursor.XYAsPoint2D;

    public bool IsCrashed { get; set; } = false;

    private int _turnDirection = 1; // start with left.  % 3 => 1, 2, 0  Left, Straight, Right. 

    public MineCart(int x, int y, char initDirection)
    {
        _cursor = initDirection switch
        {
            '>' => new(x, y, 1, 0),
            '<' => new(x, y, -1, 0),
            '^' => new(x, y, 0, -1),
            'v' => new(x, y, 0, 1),
            _ => throw new NotImplementedException()
        };

        _cursor.DoMirrorTurns = true;
    }

    public MineCart(MineCart clone)
    {
        _cursor = new(clone._cursor);
        _turnDirection = clone._turnDirection;
    }

    public Point2D TryStep(MapSymbols currentSymbol)
    {
        MineCart cloneCart = new(this);
        cloneCart.Step(currentSymbol);
        return cloneCart.Position;
    }

    public void Step(MapSymbols currentSymbol)
    {
        //Remember, the map has Y=0 at the top left, but the Cursor assumes 0 at the bottom left, so the direction changes need to reflect that.
        //In practice this means the left/right turns are reversed.
        switch (currentSymbol)
        {
            case MapSymbols.Straight:
                _cursor.Step();
                break;
            case MapSymbols.Curve_A:    //  /
                if (_cursor.IsHorizontal)
                    _cursor.TurnLeft();
                else
                    _cursor.TurnRight();

                _cursor.Step();
                break;

            case MapSymbols.Curve_B:    //  \
                if (_cursor.IsHorizontal)
                    _cursor.TurnRight();
                else
                    _cursor.TurnLeft();
                
                _cursor.Step();
                break;
            case MapSymbols.Intersection:
                switch (_turnDirection)
                {
                    case 0:
                        _cursor.TurnRight();
                        break;
                    case 1:
                        _cursor.TurnLeft();
                        break;
                    case 2:
                         //no turn, we're going straight through.
                    default:
                        break;
                };
                _turnDirection = (1 + _turnDirection) % 3;
                _cursor.Step();
                break;
            default:
                throw new NotImplementedException($"Unknown symbol: {currentSymbol}");
        }
    } 
    public char Direction
    { 
        get
        {
            if (_cursor.IsMovingDown) return 'v';
            if (_cursor.IsMovingUp) return '^';
            if (_cursor.IsMovingLeft) return '<';
            if (_cursor.IsMovingRight) return '>';

            return '\0';
        }
    }
}