using System.Numerics;

namespace AoC_2022_Day_18
{
    internal static class ReturnValues
    {
        //human readable return types
        public const int LessThan = -1;
        public const int EqualTo = 0;
        public const int GreaterThan = 1;
        public const int Unknown = 2;
    }

    internal record Point3D :
        IComparable<Point3D>,
        IComparisonOperators<Point3D, Point3D, bool>,
        IAdditionOperators<Point3D, Point3D, Point3D>,
        ISubtractionOperators<Point3D, Point3D, Point3D>
    {
        // A point, but with a Z dimension. 
        public int X = 0;
        public int Y = 0;
        public int Z = 0;
        public Point3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point3D()
        {
        }

        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }

        private static int Compare(Point3D? left, Point3D? right)
        {
            // This is imperfect, but good enough. We shouldn't collide with things like comparing 0,1,0 and 1,0,0
            if (left is null && right is not null) return ReturnValues.LessThan;
            if (left is not null && right is null) return ReturnValues.GreaterThan;
            if (left is null && right is null) return ReturnValues.EqualTo;

            // Let's get rid of all the compiler warnings about nulls...  NB: this should never trigger 
            if (left is null || right is null) return ReturnValues.EqualTo;

            // We are making up comparisons now. 
            // We need equality to happen first.
            if (left.X == right.X && left.Y == right.Y && left.Z == right.Z) return ReturnValues.EqualTo;

            //then an absolute cull 
            if (left.X < right.X && left.Y < right.Y && left.Z < right.Z) return ReturnValues.LessThan;
            if (left.X > right.X && left.Y > right.Y && left.Z > right.Z) return ReturnValues.GreaterThan;

            // Now we'll get the sum of our points (IE the distance from 0,0,0 and compare them to see who's "closer" 
            // We don't want to return that 0,1,0 == 1,0,0 as true, so we'll do an priority check. 

            int leftSum = left.X + left.Y + left.Z;
            int rightSum = right.X + right.Y + right.Z;

            if (leftSum == rightSum)
            {
                if (left.X < right.X || left.Y < right.Y || left.Z < right.Z) return ReturnValues.LessThan;
                return ReturnValues.GreaterThan;
            }
            return (leftSum < rightSum) ? ReturnValues.LessThan : ReturnValues.GreaterThan;
        }
        public static int TaxiDistance(Point3D s, Point3D e)
        {
            return Math.Abs(s.X - e.X) + Math.Abs(s.Y - e.Y) + Math.Abs(s.Z - e.Z);
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (obj is Point3D)
                return CompareTo(obj);
            else
                throw new ArgumentException("Object is not a Point3D");
        }

        public int CompareTo(Point3D? other) => Compare(this, other);

        public static bool operator >(Point3D left, Point3D right)
        {
            return Compare(left, right) == ReturnValues.GreaterThan;
        }

        public static bool operator >=(Point3D left, Point3D right)
        {
            int rv = Compare(left, right);
            return (rv == ReturnValues.GreaterThan || rv == ReturnValues.EqualTo);
        }

        public static bool operator <(Point3D left, Point3D right)
        {
            return Compare(left, right) == ReturnValues.LessThan;
        }

        public static bool operator <=(Point3D left, Point3D right)
        {
            int rv = Compare(left, right);
            return (rv == ReturnValues.LessThan || rv == ReturnValues.EqualTo);
        }

        public static Point3D operator +(Point3D left, Point3D right)
        {
            return new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        public static Point3D operator -(Point3D left, Point3D right)
        {
            return new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }
    }
}
