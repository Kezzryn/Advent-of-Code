namespace AoC_2018_Day_23
{
    internal class Box
    {
        public Point3D Min = new();
        public Point3D Max = new();
        public List<Point3D> NanobotsInRange = new();

        public Box() { }

        public Box(long minX, long minY, long minZ, long maxX, long maxY, long maxZ)
        {
            Min = new(minX, minY, minZ);
            Max = new(maxX, maxY, maxZ);
        }

        public bool BoxIntersectsSphere(Point3D S, long R)
        {
            //Subtract the distance the edges of the box is offset from the radius.
            //Since the radius is also the max taxi distance, if the result is >=0 then they intersect. 

            if (S.X < Min.X)
                R -= Min.X - S.X;
            else
                if (S.X > Max.X)
                  R -= S.X - Max.X;

            if (S.Y < Min.Y)
                R -= Min.Y - S.Y;
            else
                if (S.Y > Max.Y)
                   R -= S.Y - Max.Y;

            if (S.Z < Min.Z)
                R -= Min.Z - S.Z;
            else
                if (S.Z > Max.Z)
                  R -= S.Z - Max.Z;

            return R >= 0;
        }

        public List<Box> Subdivide()
        {
            //Cut each box into 8 smaller boxes.
            if (Min == Max) return new List<Box>(); //can't go any lower.

            List<Box> returnValue = new();

            long halfX = Min.X + ((Max.X - Min.X) / 2);
            long halfY = Min.Y + ((Max.Y - Min.Y) / 2);
            long halfZ = Min.Z + ((Max.Z - Min.Z) / 2);

            returnValue.Add(new(Min.X,     Min.Y,     Min.Z,     halfX, halfY, halfZ));
            returnValue.Add(new(Min.X,     Min.Y,     halfZ + 1, halfX, halfY, Max.Z));

            returnValue.Add(new(halfX + 1, Min.Y,     Min.Z, Max.X, halfY, halfZ));
            returnValue.Add(new(halfX + 1, Min.Y, halfZ + 1, Max.X, halfY, Max.Z));
            
            returnValue.Add(new(Min.X,     halfY + 1, Min.Z,     halfX, Max.Y, halfZ));
            returnValue.Add(new(Min.X,     halfY + 1, halfZ + 1, halfX, Max.Y, Max.Z));

            returnValue.Add(new(halfX + 1, halfY + 1, Min.Z,     Max.X, Max.Y, halfZ));
            returnValue.Add(new(halfX + 1, halfY + 1, halfZ + 1, Max.X, Max.Y, Max.Z));

            return returnValue;
        }

        public override string ToString()
        {
            return $"{Min} {Max}";
        }
    }
}
