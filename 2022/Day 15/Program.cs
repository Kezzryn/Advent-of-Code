using AoC_2022_Day_15;
using System.Drawing;

try
{
    const string PUZZLE_INPUT = "..\\..\\..\\..\\..\\Input Files\\Day 15.txt";

    const int Part1Slice = 2000000;

    const int MAX_X = 4000000;
    const int MAX_Y = 4000000;
    const int MIN_X = 0;
    const int MIN_Y = 0;

    List<Sensor> sensors = File.ReadAllLines(PUZZLE_INPUT).Select(x => new Sensor(x)).ToList();

    //Part 1 
    //Where Y = 2000000, how many positions cannot contain a beacon?  (ie: No max/min X coordinate) 
    //build a list of ranges that represent the start and end of each "slice" of the map. 
    //then, order them by their start coordinate, and merge any overlapping lines.
    //then sum the result. 

    Slice slicePart1 = new();

    foreach (Sensor s in sensors)
    {
        int x = s.WidthAt(Part1Slice);
        if (x == 0) continue;

        slicePart1.AddSegment(s.X - ((x - 1) / 2), x);
    }

    //TODO: work this into the AddSegments, so we're always flattening.
    slicePart1.FlattenSegments();

    int coveredLocations = slicePart1.GetSegments().Select(y => y.Length).Sum();
    int numBeacons = sensors.Select(bl => bl.BeaconLocation.Y).Where(y => y == Part1Slice).Distinct().ToList().Count;
    int numSensors = sensors.Select(sl => sl.SensorLocation.Y).Where(y => y == Part1Slice).Distinct().ToList().Count;

    Console.WriteLine($"Part 1 answer: There are a total of {coveredLocations} locations containing {numBeacons} beacons and {numSensors} sensors, making {coveredLocations - numSensors - numBeacons} spots that cannot contain a beacon.");

    bool outOfRange = false;
    //Part 2
    //Find the spot in a 4000000 x 4000000 grid that isn't covered by a beacon/sensor.
    //We are going to walk the perimiter of each sensor, testing to see if that point is covered by another sensor. 
    //There was some experimentatoin with iterating through with the slices, but it was 2-3x slower. 
    foreach (Sensor s in sensors)
    {
        foreach (Point p in s.Perimeter().Where(x => x.X >= MIN_X && x.Y >= MIN_Y && x.X <= MAX_X && x.Y <= MAX_Y))
        {
            outOfRange = true;
            foreach (Sensor z in sensors)
            {
                if (z.IsCovered(p))
                {
                    outOfRange = false;
                    break;
                }
            }

            if (outOfRange)
            {
                Console.WriteLine($"Part 2: The tuning frequency of the beacon at {p} is {(4000000L * p.X) + p.Y}");
                break;
            }
        }
        if (outOfRange) break;
    }
}
catch (Exception e)
{
    Console.WriteLine(e);
}