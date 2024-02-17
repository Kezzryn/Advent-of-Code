using System.Numerics;

namespace AoC_2023_Day_24
{
    internal class VecDecimal(decimal x, decimal y, decimal z) :
        IMultiplyOperators<VecDecimal, decimal, VecDecimal>,
        IAdditionOperators<VecDecimal, VecDecimal, VecDecimal>,
        IAdditionOperators<VecDecimal, decimal, VecDecimal>,
        IDivisionOperators<VecDecimal, decimal, VecDecimal>
    {
        private readonly decimal _x = x;
        private readonly decimal _y = y;
        private readonly decimal _z = z;

        public decimal X { get { return _x; } }
        public decimal Y { get { return _y; } }
        public decimal Z { get { return _z; } }

        public VecDecimal(decimal x, decimal y) : this(x, y, 0)
        {
            // passes through to base constructor.
        }
        public static VecDecimal operator +(VecDecimal left, decimal right)
        {
            return new(left.X + right, left.Y + right, left.Z + right);
        }

        public static VecDecimal operator +(VecDecimal left, VecDecimal right)
        {
            return new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        public static VecDecimal operator *(VecDecimal left, decimal right)
        {
            return new(left.X * right, left.Y * right, left.Z * right);
        }

        public static VecDecimal operator /(VecDecimal left, decimal right)
        {
            return new(left.X / right, left.Y / right, left.Z / right);
        }
    }
}
