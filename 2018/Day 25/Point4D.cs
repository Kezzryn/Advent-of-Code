using System.Diagnostics.CodeAnalysis;

namespace AoC_2018_Day_25
{

    internal class Point4D : IEqualityComparer<Point4D>
    {
        public int X {  get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int W { get; set; }

        public int DistFromZero => TaxiDistance4D(this, new(0, 0, 0, 0));
        public static int TaxiDistance4D(Point4D a, Point4D b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z) + Math.Abs(a.W - b.W);
        }

        public bool Equals(Point4D? x, Point4D? y)
        {
            if(x == null || y == null) return false; 
            if(x == null && y == null) return true;

            if (x!.X == y.X &&
                x.Y == y.Y &&
                x.Z == y.Z &&
                x.W == y.W) return true;

            return false;
        }

        public int GetHashCode([DisallowNull] Point4D obj)
        {
            return obj.X ^ obj.Y ^ obj.Z ^ obj.W;
        }

        public Point4D(string input)
        {
            int[] coords = input.Split(',').Select(int.Parse).ToArray();
            X = coords[0];
            Y = coords[1];
            Z = coords[2];
            W = coords[3];
        }

        public Point4D(int x, int y, int z, int w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }   
    }
}
