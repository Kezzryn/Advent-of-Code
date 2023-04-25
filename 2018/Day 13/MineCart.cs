using System.Numerics;

namespace AoC_2018_Day_13
{
    enum MapSymbols
    {
        Straight,
        Curve_A,   // '/' 
        Curve_B,   // '\'
        Intersection,
        Out_Of_Bounds
    }

    internal class MineCart
    {
        public Complex Position { get; set; }

        public Complex Direction { get; set; }

        public int ID { get; }

        public bool IsCrashed { get; set; } = false;

        private int _turnDirection = 1; // start with left.  % 3 => 1, 2, 0  Left, Straight, Right. 

        public MineCart(int x, int y, char initDirection, int id)
        {
            ID = id;
            Position = new Complex(x, y);
            Direction = initDirection switch
            {
                '>' => new Complex(1,  0),
                '<' => new Complex(-1, 0),
                '^' => new Complex(0, -1),
                'v' => new Complex(0,  1),
                _ => throw new NotImplementedException()
            };
        }

        public void Step() => Position += Direction;

        private void TurnLeft() => Direction *= -Complex.ImaginaryOne;
        
        private void TurnRight() => Direction *= Complex.ImaginaryOne;

        public void Intersection()
        {
            switch (_turnDirection) 
            {
                case 0:
                    TurnRight();
                    break;
                case 1:
                    TurnLeft();
                    break;
                case 2:
                    // go straight
                default:
                    break;
            };

            _turnDirection = (_turnDirection + 1) % 3;
        }

        public void FollowCurve(MapSymbols curveType)
        {
            switch (curveType)
            {
                case MapSymbols.Curve_A: // '/'
                    if (Direction.Real == 0)
                    {
                        TurnRight();
                    }
                    else if (Direction.Imaginary == 0)
                    {
                        TurnLeft();
                    }
                    break;
                case MapSymbols.Curve_B: // '\'
                    if (Direction.Real == 0)
                    {
                        TurnLeft();
                    }
                    else if(Direction.Imaginary == 0)
                    {
                        TurnRight();
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
