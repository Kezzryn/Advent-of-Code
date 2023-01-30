using System.Drawing;

namespace AoC_2022_Day_9
{
    internal class Rope
    {
        private int HEAD { get; } = 0;
        private int TAIL { get; }

        private Point[] _segments;
        private HashSet<Point> _tailVisited = new();
        private bool _verbose = false;

        public Rope(int numSegments)
        {
            _segments = new Point[numSegments];
            TAIL = _segments.GetUpperBound(0);

            for (int i = 0; i < numSegments; i++)
            {
                _segments[i] = new Point(0, 0);
            }

            AddTailPos();
        }

        public void ToggleVerbose() => _verbose = !_verbose;

        public int NumTailPos()
        {
            return _tailVisited.Count;
        }

        private void AddTailPos()
        {
            if (_tailVisited.Add(_segments[TAIL]) && _verbose) Console.WriteLine($"Adding tail position: {_segments[TAIL]} count: {_tailVisited.Count}");
        }

        public void Move(IEnumerable<string> moveSet)
        {
            foreach (string move in moveSet)
            {
                //moveString is a direction and distance
                //eg: U 1  L 12
                //direction (U L D R) and distance (positive int), seperated by a space.

                if (!int.TryParse(move[1..], out int dist)) throw new Exception("Failed to parse moveString.");

                Point offset = move[0] switch
                {
                    'U' => new Point(0, 1),
                    'D' => new Point(0, -1),
                    'L' => new Point(-1, 0),
                    'R' => new Point(1, 0),
                    _ => new Point(0, 0),
                };

                YankChain(offset, dist);
            }
        }

        private void YankChain(Point headOffset, int dist)
        {
            if (_verbose) Console.WriteLine($"headoffset {headOffset} distance {dist} inital head POS {_segments[HEAD]}");

            for (int moves = 1; moves <= dist; moves++)
            {
                //move the head.
                _segments[HEAD].Offset(headOffset);
                if (_verbose) Console.WriteLine($"new head position {_segments[HEAD]} move {moves} of {dist}");

                //the head has moved, now move/check each body segment in sequence 
                for (int i = HEAD + 1; i <= TAIL; i++)
                {
                    if (IsAdjacent(_segments[i], _segments[i - 1]))
                    {
                        //once we find an adjacent position skip to the next move as nothing after this is going to move.
                        continue;
                    }
                    else
                    {
                        if (_verbose) Console.WriteLine($"{moves} {i}: Not Adjacent: {_segments[i]}, {_segments[i - 1]}");
                        //not adjacent, move i towards i-1 
                        MoveTowards(ref _segments[i], _segments[i - 1]);
                        if (_verbose) Console.WriteLine($"{moves} {i}: Moved a towards b: {_segments[i]}, {_segments[i - 1]}");
                    }
                }
                //For the puzzle solution, update the list of tail positions.
                AddTailPos();
            }
        }
        private static bool IsAdjacent(Point a, Point b)
        {
            //are two points on or next to each other? 
            Point z = a - (Size)b;

            if (z.X >= -1 && z.X <= 1 && z.Y >= -1 && z.Y <= 1) return true;

            return false;
        }
        private static void MoveTowards(ref Point a, Point b)
        {
            //Move A towards B.
            //assumption: We're never more than 2 away in any direction.  
            //improvement: update the class to teleport towards a point farther away.
            //  NB: this has implications for the solution though. 
            Point z = b - (Size)a;

            if (z.X > 0) a.X++;
            if (z.X < 0) a.X--;

            if (z.Y > 0) a.Y++;
            if (z.Y < 0) a.Y--;
        }
    }
}
