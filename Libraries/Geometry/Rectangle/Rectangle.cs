namespace BKH.Geometry
{
    public class Rectangle(int x1, int y1, int x2, int y2)
    {
        public int X1 { get; private set; } = x1;
        public int X2 { get; private set; } = x2;
        public int Y1 { get; private set; } = y1;
        public int Y2 { get; private set; } = y2;

        public Rectangle(Rectangle o) : this(o.X1, o.Y1, o.X2, o.Y2) { }

        public long Area { get { return (long)(X2 - X1 + 1) * (long)(Y2 - Y1 + 1); } }

        public bool Contains(Rectangle other) => (X1 <= other.X1 && X2 >= other.X2) &&
                                                 (Y1 <= other.Y1 && Y2 >= other.Y2);
        public bool ContainedBy(Rectangle other) => other.Contains(this);

        public static Rectangle Empty { get => new(0, 0, 0, 0); }

        public List<Rectangle> Exclude(Rectangle other)
        {
            // Based on Cuboid code.

            if (ContainedBy(other)) return [];

            List<Rectangle> returnValue = [];
            Rectangle current = new(this);

            // Cut right side off, if one exists. 
            if (current.X1 <= other.X2 && other.X2 < current.X2)
            {
                returnValue.Add(new(other.X2 + 1, current.Y1, current.X2, current.Y2));
                current = new(current.X1, current.Y1, other.X2, current.Y2);
            }

            // Cut left side off, if one exists. 
            if (current.X1 < other.X1 && other.X1 <= current.X2)
            {
                returnValue.Add(new(current.X1, current.Y1, other.X1 - 1, current.Y2));
                current = new(other.X1, current.Y1, current.X2, current.Y2);
            }

            // Do it again, but for Y.
            if (current.Y1 <= other.Y2 && other.Y2 < current.Y2)
            {
                returnValue.Add(new(current.X1, other.Y2 + 1, current.X2, current.Y2));
                current = new(current.X1, current.Y1, current.X2, other.Y2);
            }

            // and other end of the Y.
            if (current.Y1 < other.Y1 && other.Y1 <= current.Y2)
            {
                returnValue.Add(new(current.X1, current.Y1, current.X2, other.Y1 - 1));
                //current = new(current.X1, other.Y1, current.X2, current.Y2);
            }

            return returnValue;
        }

        public bool Overlap(Rectangle other)
        {
            if (!EdgeOverlap(X1, X2, other.X1, other.X2)) return false;
            return EdgeOverlap(Y1, Y2, other.Y1, other.Y2);
        }

        public Rectangle Intersect(Rectangle other)
        {
            if (!Overlap(other)) return Empty;

            List<int> XList = [X1, X2, other.X1, other.X2];
            XList.Sort();
            List<int> YList = [Y1, Y2, other.Y1, other.Y2];
            YList.Sort();

            return new(XList[1], YList[1], XList[2], YList[2]);
        }
        public override string ToString() => $"({X1}, {Y1}), ({X2}, {Y2})";

        public static bool EdgeOverlap(int aStart, int aEnd, int bStart, int bEnd)
        {
            return (aStart >= bStart || aEnd >= bStart) && (aStart <= bEnd || aEnd <= bEnd);
        }
    }
}
