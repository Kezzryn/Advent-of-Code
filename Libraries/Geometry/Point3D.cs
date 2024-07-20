namespace BKH.Geometry;

using System;
using System.Numerics;

public struct Point3D :
    IComparable<Point3D>,
    IEquatable<Point3D>,
    IEqualityComparer<Point3D>,
    IEqualityOperators<Point3D, Point3D, bool>,
    IEqualityOperators<Point3D, Point2D, bool>,
    IAdditionOperators<Point3D, Point3D, Point3D>,
    ISubtractionOperators<Point3D, Point3D, Point3D>,
    IAdditionOperators<Point3D, Point2D, Point3D>,
    ISubtractionOperators<Point3D, Point2D, Point3D>,
    IAdditionOperators<Point3D, (int x, int y), Point3D>,
    ISubtractionOperators<Point3D, (int x, int y), Point3D>,
    IComparisonOperators<Point3D, Point3D, bool>
{
    public static readonly Point3D Empty = new();

    //human readable return types
    private const int LessThan = -1;
    private const int EqualTo = 0;
    private const int GreaterThan = 1;

    private int _x;
    private int _y;
    private int _z;

    public Point3D(int x, int y, int z)
    {
        _x = x;
        _y = y;
        _z = z;
    }

    public Point3D(int[] xyz)
    {
        _x = xyz[0];
        _y = xyz[1];
        _z = xyz[2];
    }

    public Point3D()
    {
    }

    public Point3D(Point2D xy, int z = 0)
    {
        _x = xy.X;
        _y = xy.Y;
        _z = z;
    }

    public Point3D((int x, int y) xy, int z = 0)
    {
        _x = xy.x;
        _y = xy.y;
        _z = z;
    }

    public readonly bool IsEmpty
    {
        get
        {
            return _x == 0 && _y == 0 && _z == 0;
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

    public readonly (int x, int y) As2D() => (X, Y);
    public readonly Point2D AsPoint2D() => new(X, Y);
    public static int TaxiDistance3D(Point3D s, Point3D e) => Math.Abs(s.X - e.X) + Math.Abs(s.Y - e.Y) + Math.Abs(s.Z - e.Z);
    public static int TaxiDistance3D(Point3D s) => TaxiDistance3D(s, new Point3D(0, 0, 0));
    public static int TaxiDistance2D(Point3D s, Point3D e) => Math.Abs(s.X - e.X) + Math.Abs(s.Y - e.Y);
    public override readonly string ToString() => $"{{X={X},Y={Y},Z={Z}}}";

    //IAdditionOperators
    public static Point3D operator +(Point3D left, Point3D right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    public static Point3D operator +(Point3D left, Point2D right) => new(left.X + right.X, left.Y + right.Y, left.Z);
    public static Point3D operator +(Point3D left, (int x, int y) right) => new(left.X + right.x, left.Y + right.y, left.Z);

    //ISubtractionOperators
    public static Point3D operator -(Point3D left, Point3D right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z); 
    public static Point3D operator -(Point3D left, Point2D right) => new(left.X - right.X, left.Y - right.Y, left.Z);
    public static Point3D operator -(Point3D left, (int x, int y) right) => new(left.X - right.x, left.Y - right.y, left.Z);

    // IEqualityOperators<Point3D, Point3D, bool>
    public static bool operator ==(Point3D left, Point3D right) => left.X == right.X && left.Y == right.Y && left.Z == right.Z;
    public static bool operator !=(Point3D left, Point3D right) => !(left.X == right.X && left.Y == right.Y && left.Z == right.Z);

    // IEqualityOperators<Point3D, Point, bool>
    public static bool operator ==(Point3D left, Point2D right) => left.X == right.X && left.Y == right.Y;
    public static bool operator !=(Point3D left, Point2D right) => !(left.X == right.X && left.Y == right.Y);

    //IEqualityComparer
    public readonly bool Equals(Point3D x, Point3D y) => x == y;
    public override readonly bool Equals(object? obj)
    {
        if (obj == null) return false;

        if (obj is Point3D p3D)
            return this == p3D;
        else
            throw new ArgumentException("Object is not a Point3D");
    }

    //IComparor
    public bool Equals(Point3D other) => this == other;

    // IComparisonOperators<Point3D, Point3D, bool>
    public static bool operator >(Point3D left, Point3D right) => Compare(left, right) == GreaterThan;
    public static bool operator <(Point3D left, Point3D right) => Compare(left, right) == LessThan;
    public static bool operator >=(Point3D left, Point3D right) => Compare(left, right) != LessThan;
    public static bool operator <=(Point3D left, Point3D right) => Compare(left, right) != GreaterThan;

    // IComparable
    public readonly int CompareTo(Point3D other) => Compare(this, other);

    // Hash Codes
    public override readonly int GetHashCode() => Tuple.Create(X, Y, Z).GetHashCode();
    readonly int IEqualityComparer<Point3D>.GetHashCode(Point3D obj) => Tuple.Create(obj.X, obj.Y, obj.Z).GetHashCode();
   
    // Custom comparison function. 
    private static int Compare(Point3D left, Point3D right)
    {
        // This is imperfect, but good enough. We shouldn't collide with things like comparing 0,1,0 and 1,0,0
        // x is closer than y then z

        // We need equality to happen first.
        if (left == right) return EqualTo;

        //then an absolute cull 
        if (left.X < right.X && left.Y < right.Y && left.Z < right.Z) return LessThan;
        if (left.X > right.X && left.Y > right.Y && left.Z > right.Z) return GreaterThan;

        // Now we'll get the sum of our points (IE the distance from 0,0,0 and compare them to see who's "closer" 
        // We don't want to return that 0,1,0 == 1,0,0 as true, so we'll do an priority check. 

        long leftSum = left.X + left.Y + left.Z;
        long rightSum = right.X + right.Y + right.Z;

        if (leftSum == rightSum)
        {
            return (left.X < right.X || left.Y < right.Y || left.Z < right.Z) ? LessThan : GreaterThan;
        }
        return (leftSum < rightSum) ? LessThan : GreaterThan;
    }
}