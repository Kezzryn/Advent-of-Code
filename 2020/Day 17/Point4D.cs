using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Numerics;

namespace AoC_Point4D
{
    internal struct Point4D :
        IComparable<Point4D>,
        IEqualityComparer<Point4D>,
        IEqualityOperators<Point4D, Point4D,bool>,
        IEqualityOperators<Point4D, Point, bool>,
        IAdditionOperators<Point4D, Point4D, Point4D>,
        ISubtractionOperators<Point4D, Point4D, Point4D>,
        IAdditionOperators<Point4D, Point, Point4D>,
        ISubtractionOperators<Point4D, Point, Point4D>,
        IAdditionOperators<Point4D, (int x, int y), Point4D>,
        ISubtractionOperators<Point4D, (int x, int y), Point4D>,
        IComparisonOperators<Point4D, Point4D, bool>
    {
        public static readonly Point4D Empty = new();

        //human readable return types
        private const int LessThan = -1;
        private const int EqualTo = 0;
        private const int GreaterThan = 1;

        private int _x;
        private int _y;
        private int _z;
        private int _w;

        public Point4D(int x, int y, int z, int w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        public Point4D(int[] xyzw)
        {
            _x = xyzw[0];
            _y = xyzw[1];
            _z = xyzw[2];
            _w = xyzw[3];
        }

        public Point4D()
        {
        }

        public Point4D(Point xy, int z = 0)
        {
            _x = xy.X;
            _y = xy.Y;
            _z = z;
            _w = 0;
        }

        public Point4D((int x, int y) xy, int z = 0)
        {
            _x = xy.x;
            _y = xy.y;
            _z = z;
            _w = 0;
        }

        public readonly bool IsEmpty
        {
            get
            {
                return _x == 0 && _y == 0 && _z == 0 && _w == 0;
            }
        }

        public int X
        {
            readonly get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }

        public int Y
        {
            readonly get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }

        public int Z
        {
            readonly get
            {
                return _z;
            }
            set
            {
                _z = value;
            }
        }

        public int W
        {
            readonly get
            {
                return _w;
            }
            set
            {
                _w = value;
            }
        }

        public override string ToString() => $"{{X={X},Y={Y},Z={Z},W={W}}}";

        public static int TaxiDistance4D(Point4D s, Point4D e) => Math.Abs(s.X - e.X) + Math.Abs(s.Y - e.Y) + Math.Abs(s.Z - e.Z) + Math.Abs(s.W - e.W);
        public static int TaxiDistance3D(Point4D s, Point4D e) => Math.Abs(s.X - e.X) + Math.Abs(s.Y - e.Y) + Math.Abs(s.Z - e.Z);
        public static int TaxiDistance2D(Point4D s, Point4D e) => Math.Abs(s.X - e.X) + Math.Abs(s.Y - e.Y);
        public static int TaxiDistance2D(Point s, Point e) => Math.Abs(s.X - e.X) + Math.Abs(s.Y - e.Y);

        public (int x, int y) As2D() => (X, Y);

        // IAdditionOperators, ISubtractionOperators
        public static Point4D operator +(Point4D left, Point4D right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
        public static Point4D operator -(Point4D left, Point4D right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
        public static Point4D operator +(Point4D left, Point right) => new(left.X + right.X, left.Y + right.Y, left.Z, left.W);
        public static Point4D operator -(Point4D left, Point right) => new(left.X - right.X, left.Y - right.Y, left.Z,left.W);
        public static Point4D operator +(Point4D left, (int x, int y) right) => new(left.X + right.x, left.Y + right.y, left.Z, left.W);
        public static Point4D operator -(Point4D left, (int x, int y) right) => new(left.X - right.x, left.Y - right.y, left.Z, left.W);


        // IEqualityOperators<Point4D, Point4D, bool>
        public static bool operator ==(Point4D left, Point4D right) => 
            left.X == right.X && 
            left.Y == right.Y && 
            left.Z == right.Z && 
            left.W == right.W;
        public static bool operator !=(Point4D left, Point4D right) => 
            !(left.X == right.X && 
              left.Y == right.Y && 
              left.Z == right.Z && 
              left.W == right.W);
        
        // IEqualityOperators<Point4D, Point, bool>
        public static bool operator ==(Point4D left, Point right) => left.X == right.X && left.Y == right.Y && left.Z == 0 && left.W == 0;
        public static bool operator !=(Point4D left, Point right) => !(left.X == right.X && left.Y == right.Y && left.Z == 0 && left.W == 0);

        ////IEqualityComparer
        public readonly bool Equals(Point4D x, Point4D y) => x == y;
        public override readonly bool Equals(object? obj)
        {
            if (obj == null) return false;

            if (obj is Point4D p4D)
                return this == p4D;
            else
                throw new ArgumentException("Object is not a Point4D");
        }

        // IComparisonOperators<Point4D, Point4D, bool>
        public static bool operator >(Point4D left, Point4D right) => Compare(left, right) == GreaterThan;
        public static bool operator <(Point4D left, Point4D right) => Compare(left, right) == LessThan;
        public static bool operator >=(Point4D left, Point4D right) => Compare(left, right) != LessThan;
        public static bool operator <=(Point4D left, Point4D right) => Compare(left, right) != GreaterThan;

        // IComparable
        public readonly int CompareTo(Point4D other) => Compare(this, other);

        // Hash Codes
        public override readonly int GetHashCode() => unchecked(_x ^ _y ^ _z ^ _w);
        public int GetHashCode([DisallowNull] Point4D obj) => GetHashCode();

        // Custom comparison function. 
        private static int Compare(Point4D left, Point4D right)
        {
            // This is imperfect, but good enough. We shouldn't collide with things like comparing 0,1,0 and 1,0,0
            // x is closer than y then z

            // We need equality to happen first.
            if (left == right) return EqualTo;

            //then an absolute cull 
            if (left.X < right.X && left.Y < right.Y && left.Z < right.Z && left.W < right.W) return LessThan;
            if (left.X > right.X && left.Y > right.Y && left.Z > right.Z && left.W > right.W) return GreaterThan;

            // Now we'll get the sum of our points (IE the distance from 0,0,0 and compare them to see who's "closer" 
            // We don't want to return that 0,1,0 == 1,0,0 as true, so we'll do an priority check. 

            long leftSum = left.X + left.Y + left.Z + left.W;
            long rightSum = right.X + right.Y + right.Z + right.W;

            if (leftSum == rightSum)
            {
                return (left.X < right.X || left.Y < right.Y || left.Z < right.Z || left.W < right.W) ? LessThan : GreaterThan;
            }
            return (leftSum < rightSum) ? LessThan : GreaterThan;
        }
    }
}