using System.Runtime.CompilerServices;

namespace AoC_2016_Day_20
{
    internal class Segment
    {
        public uint Start { get; set; }
        public uint Length { get; set; }

        public uint End
        {
            get { return Start + Length; }
        }

        public Segment()
        {
            Start = 0;
            Length = 0;
        }

        public Segment(uint start, uint length)
        {
            Start = start;
            Length = length;
        }

        //if a is contained within b. 
        public bool Contains(Segment a) => Contains(a, this);
        public static bool Contains(Segment a, Segment b) => a.Start >= b.Start && a.End <= b.End;
        public bool Overlaps(Segment a) => Overlaps(a, this);
        public static bool Overlaps(Segment a, Segment b) => (a.Start >= b.Start || a.End >= b.Start) && (a.Start <= b.End || a.End <= b.End);
        public bool Adjacent(Segment a) => Adjacent(a, this);
        public static bool Adjacent(Segment a, Segment b) => (a.End + 1 == b.Start || b.End + 1 == a.Start);
        public override string ToString() => $"Start = {Start} Length = {Length}";
    }

    internal class Slice
    {
        List<Segment> segments = new();

        public Slice()
        {

        }

        public void AddRange(uint start, uint end)
        {
            AddSegment(start, end - start);
        }

        public void AddSegment(uint start, uint length)
        {
            Segment n = new(start, length);
            if (segments.Count == 0)
            {
                segments.Add(n);
                return;
            }

            foreach (Segment a in segments)
            {
                if (a.Contains(n)) break; // do nothing, we've got this covered. 
                if (n.Contains(a) || a.Overlaps(n)) //new one covers our existing one, stretch our exising one out.
                {
                    //Stretch the existing to cover the new segment
                    uint newStart = uint.Min(a.Start, n.Start);
                    uint newLength = uint.Max(a.End, n.End) - newStart;

                    a.Start = newStart;
                    a.Length = newLength;
                    break;
                }
                else
                {
                    // a net new unique segment.
                    segments.Add(n);
                    break;
                }
            }
        }

        public void FlattenSegments()
        {
            segments = segments.OrderBy(x => x.Start).ToList();

            bool isDone = false;
            while (!isDone)
            {
                if (segments.Count <= 1) break;
                for (int i = 0; i < segments.Count - 1; i++)
                {
                    if (segments[i].Contains(segments[i + 1]))
                    {
                        segments.Remove(segments[i + 1]);
                        isDone = false;
                        break;
                    }
                    if (segments[i].Overlaps(segments[i + 1]) || segments[i].Adjacent(segments[i + 1]))
                    {
                        uint newStart = uint.Min(segments[i].Start, segments[i + 1].Start);
                        uint newLength = uint.Max(segments[i].End, segments[i + 1].End) - newStart;

                        segments[i].Start = newStart;
                        segments[i].Length = newLength;

                        segments.Remove(segments[i + 1]);
                        isDone = false;
                        break;
                    }
                    isDone = true;
                }
            }
        }
        public List<Segment> GetSegments() => segments;
    }
}
