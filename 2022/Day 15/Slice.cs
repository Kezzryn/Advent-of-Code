namespace AoC_2022_Day_15
{
    internal class Segment
    {
        public int Start { get; set; }
        public int Length { get; set; }

        public int End
        {
            get { return Start + Length; }
        }

        public Segment()
        {
            Start = 0;
            Length = 0;
        }

        public Segment(int start, int length)
        {
            Start = start;
            Length = length;
        }

        //if a is contained within b. 
        public bool Contains(Segment a) => Contains(a, this);
        public static bool Contains(Segment a, Segment b) => a.Start >= b.Start && a.End <= b.End;
        public bool Overlaps(Segment a) => Overlaps(this, a);
        public static bool Overlaps(Segment a, Segment b) => (a.Start >= b.Start || a.End >= b.Start) && (a.Start <= b.End || a.End <= b.End);

        public override string ToString() => $"Start = {Start} Length = {Length}";
    }

    internal class Slice
    {
        List<Segment> segments = new();

        public Slice()
        {

        }
        public void AddSegment(int start, int length)
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
                    int newStart = int.Min(a.Start, n.Start);
                    int newLength = int.Max(a.End, n.End) - newStart;

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
                    if (segments[i].Overlaps(segments[i + 1]))
                    {
                        int newStart = int.Min(segments[i].Start, segments[i + 1].Start);
                        int newLength = int.Max(segments[i].End, segments[i + 1].End) - newStart;

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
