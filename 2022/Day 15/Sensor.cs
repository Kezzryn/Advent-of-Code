using System.Drawing;
using System.Text.RegularExpressions;

namespace AoC_2022_Day_15
{
    partial class Sensor
    {
        public Point SensorLocation { get; }
        public Point BeaconLocation { get; }
        public int DistanceToBeacon { get; }

        public Sensor(string input)
        {
            //example input: Sensor at x=2, y=18: closest beacon is at x=-2, y=15

            string[] coords = ReturnNumbers().Split(input);

            SensorLocation = new Point(int.Parse(coords[1]), int.Parse(coords[2]));
            BeaconLocation = new Point(int.Parse(coords[3]), int.Parse(coords[4]));

            DistanceToBeacon = CalcDistance(SensorLocation, BeaconLocation);
        }

        public Sensor(int sensorX, int sensorY, int beaconX, int beaconY)
        {
            //debug use
            SensorLocation = new Point(sensorX, sensorY);
            BeaconLocation = new Point(beaconX, beaconY);

            DistanceToBeacon = CalcDistance(SensorLocation, BeaconLocation);
        }

        public override string ToString() => $"{SensorLocation} {BeaconLocation} = {DistanceToBeacon}";

        public static int CalcDistance(Point a, Point b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

        public bool IsCovered(Point test) => CalcDistance(SensorLocation, test) <= DistanceToBeacon;

        public int WidthAt(int Y) => int.Max((2 * DistanceToBeacon) + 1 - 2 * Math.Abs(SensorLocation.Y - Y), 0);

        public int HeightAt(int X) => int.Max((2 * DistanceToBeacon) + 1 - 2 * Math.Abs(SensorLocation.X - X), 0);

        public int X => this.SensorLocation.X;
        public int Y => this.SensorLocation.Y;

        public List<Point> Perimeter()
        {
            List<Point> returnValue = new();

            int edgeDistance = DistanceToBeacon + 1;

            for (int x_offset = 0; x_offset <= edgeDistance; x_offset++)
            {
                int y_offset = edgeDistance - x_offset;

                returnValue.Add(new Point(SensorLocation.X + x_offset, SensorLocation.Y + y_offset));
                if (x_offset != 0 && y_offset != 0)
                {
                    returnValue.Add(new Point(SensorLocation.X - x_offset, SensorLocation.Y + y_offset));
                    returnValue.Add(new Point(SensorLocation.X + x_offset, SensorLocation.Y - y_offset));
                }
                returnValue.Add(new Point(SensorLocation.X - x_offset, SensorLocation.Y - y_offset));
            }

            return returnValue;
        }

        [GeneratedRegex("[^0-9-]+")]
        private static partial Regex ReturnNumbers();
    }
}
