namespace BKH.Geometry;

using System.Numerics;

public struct Point2D :
        IComparable<Point2D>,
        IEqualityComparer<Point2D>,
        IEqualityOperators<Point2D, Point2D, bool>,
        IAdditionOperators<Point2D, Point2D, Point2D>,
        ISubtractionOperators<Point2D, Point2D, Point2D>,
        IAdditionOperators<Point2D, (int x, int y), Point2D>,
        ISubtractionOperators<Point2D, (int x, int y), Point2D>,
        IMultiplyOperators<Point2D, int, Point2D>,
        IComparisonOperators<Point2D, Point2D, bool>
{
    public static Point2D Origin { get { return new(0, 0); } }

    //human readable return types for comparison operators
    private const int LessThan = -1;
    private const int EqualTo = 0;
    private const int GreaterThan = 1;

    private int _x;
    private int _y;

    private readonly int _hashCode; 

    public Point2D(int x, int y)
    {
        _x = x;
        _y = y;
        _hashCode = Tuple.Create(X, Y).GetHashCode();
    }

    public Point2D(int[] xy)
    {
        _x = xy[0];
        _y = xy[1];
        _hashCode = Tuple.Create(X, Y).GetHashCode();
    }

    public Point2D()
    {
        _hashCode = Tuple.Create(X, Y).GetHashCode();
    }

    public Point2D((int x, int y) xy)
    {
        _x = xy.x;
        _y = xy.y;
        _hashCode = Tuple.Create(X, Y).GetHashCode();
    }

    public readonly bool IsEmpty
    {
        get
        {
            return _x == 0 && _y == 0;
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

    public readonly IEnumerable<Point2D> GetAllNeighbors()
    {
        foreach ((int x, int y) in from y in Enumerable.Range(-1, 3)
                                   from x in Enumerable.Range(-1, 3)
                                   where (!(x == 0 && y == 0))
                                   select (x, y))
        {
            yield return new Point2D(x + X, y + Y);
        }
    }

    public readonly IEnumerable<Point2D> GetOrthogonalNeighbors()
    {
        foreach(Direction dir in Enum.GetValues(typeof(Direction)))
        {
            yield return OrthogonalNeighbor(dir);
        }
    }

    public override readonly string ToString() => $"{{X={X},Y={Y}}}";

    public static int TaxiDistance2D(Point2D s, Point2D e) => Math.Abs(s.X - e.X) + Math.Abs(s.Y - e.Y);

    // IAdditionOperators, ISubtractionOperators
    public static Point2D operator +(Point2D left, Point2D right) => new(left.X + right.X, left.Y + right.Y);
    public static Point2D operator -(Point2D left, Point2D right) => new(left.X - right.X, left.Y - right.Y);

    public static Point2D operator +(Point2D left, (int x, int y) right) => new(left.X + right.x, left.Y + right.y);
    public static Point2D operator -(Point2D left, (int x, int y) right) => new(left.X - right.x, left.Y - right.y);

    //IMultiplyOperators
    public static Point2D operator *(Point2D left, int right) => new(left.X * right, left.Y * right);

    // IEqualityOperators<Point3D, Point3D, bool>
    public static bool operator ==(Point2D left, Point2D right) => left.X == right.X && left.Y == right.Y;
    public static bool operator !=(Point2D left, Point2D right) => !(left.X == right.X && left.Y == right.Y);

    ////IEqualityComparer
    public readonly bool Equals(Point2D x, Point2D y) => x == y;
    public readonly override bool Equals(object? obj)
    {
        if (obj == null) return false;

        if (obj is Point2D p2D)
            return this == p2D;
        else
            throw new ArgumentException("Object is not a Point2D");
    }

    // IComparisonOperators<Point3D, Point3D, bool>
    public static bool operator >(Point2D left, Point2D right) => Compare(left, right) == GreaterThan;
    public static bool operator <(Point2D left, Point2D right) => Compare(left, right) == LessThan;
    public static bool operator >=(Point2D left, Point2D right) => Compare(left, right) != LessThan;
    public static bool operator <=(Point2D left, Point2D right) => Compare(left, right) != GreaterThan;

    // IComparable
    public readonly int CompareTo(Point2D other) => Compare(this, other);

    // Hash Codes
    //public readonly override int GetHashCode() => unchecked(_x ^ _y);
    //public readonly int GetHashCode([DisallowNull] Point2D obj) => GetHashCode();
    public override readonly int GetHashCode() => _hashCode;
    readonly int IEqualityComparer<Point2D>.GetHashCode(Point2D obj) => _hashCode;

    private static int Compare(Point2D left, Point2D right)
    {
        // This is imperfect, but good enough. We shouldn't collide with things like comparing 0,1 and 1,0
        // x is closer than y

        // We need equality to happen first.
        if (left == right) return EqualTo;

        //then an absolute cull 
        if (left.X < right.X && left.Y < right.Y) return LessThan;
        if (left.X > right.X && left.Y > right.Y) return GreaterThan;

        // Now we'll get the sum of our points (IE the distance from 0,0 and compare them to see who's "closer" 
        // We don't want to return that 0,1 == 1,0 as true, so we'll do an priority check. 

        long leftSum = left.X + left.Y;
        long rightSum = right.X + right.Y;

        if (leftSum == rightSum)
        {
            return (left.X < right.X || left.Y < right.Y) ? LessThan : GreaterThan;
        }
        return (leftSum < rightSum) ? LessThan : GreaterThan;
    }

    public readonly Point2D OrthogonalNeighbor(Direction dir)
    {
        return dir switch
        {
            Direction.Up => this + (0, 1),
            Direction.Down => this + (0, -1),
            Direction.Left => this + (-1, 0),
            Direction.Right => this + (1, 0),
            _ => throw new NotImplementedException($"Unknown direction {dir}")
        };
    }

    /// <summary>
    /// Up/Down Left/Right assumes Positive Y is "up" and Positive X is "right"
    /// </summary>
    /// <param name="other">The Point2D to compare against</param>
    /// <param name="quadrent">The area to test occupancy with.</param>
    /// <returns>If the other point is in the given quadrenet in relation to this point. Returns false for all numbers on the same X/Y line as the existing point.</returns>
    public readonly bool IsInQuadrant(Point2D other, Quadrant quadrent)
    {
        var result = RelativeQuadrent(other);
        if (result == Quadrant.OnGridLine) return false;
        return result == quadrent;
    }

    public readonly Quadrant RelativeQuadrent(Point2D other)
    {
        if (X < other.X && Y < other.Y) return Quadrant.UpperRight; //+ + 
        if (X > other.X && Y < other.Y) return Quadrant.UpperLeft; //- + 
        if (X > other.X && Y > other.Y) return Quadrant.LowerLeft; // - - 
        if (X < other.X && Y > other.Y) return Quadrant.LowerRight; //+ -
        return Quadrant.OnGridLine;
        //throw new NotImplementedException($"On the same X or Y line as the source point.{this} {other}");
    }

    public enum Direction
    {
        Up, Right, Down, Left
    }

    public enum Quadrant
    {
        UpperRight, UpperLeft, LowerRight, LowerLeft, OnGridLine
    }
}

