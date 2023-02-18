using System.Drawing;

namespace AoC_2022_Day_23
{
    internal enum Directions
    {
        North = 0,
        South = 1,
        West = 2,
        East = 3
    };

    internal class Grove
    {
        private readonly HashSet<Point> _elves = new();
        private readonly LinkedList<Directions> _directions = new();

        public Grove(string[] initalState)
        {
            string[] puzzleInput = initalState.Reverse().ToArray();

            for (int y = 0; y < puzzleInput.Length; y++)
            {
                for (int x = 0; x < puzzleInput[y].Length; x++)
                {
                    if (puzzleInput[y][x] == '#') _elves.Add(new Point(x, y));
                }
            }

            _directions.AddFirst(Directions.North);
            _directions.AddLast(Directions.South);
            _directions.AddLast(Directions.West);
            _directions.AddLast(Directions.East);
        }

        public void SpreadElves(int numRounds)
        {
            for (int i = 0; i < numRounds; i++)
            {
                if (!TakeStep()) break;
            }
        }

        public int SpreadElves()
        {
            int numSteps = 1;

            while (TakeStep()) numSteps++;

            return numSteps;
        }

        public int GetArea() => ((_elves.Max(rv => rv.X) - _elves.Min(rv => rv.X) + 1) * (_elves.Max(rv => rv.Y) - _elves.Min(rv => rv.Y) + 1)) - _elves.Count;

        private bool TakeStep() // return false if no steps are taken. 
        {
            Dictionary<Point, List<Point>> proposedMoves = new();

            foreach (Point e in _elves)
            {
                HashSet<Point> neighbors = new()
                {
                    new Point(e.X-1, e.Y+1),    new Point(e.X, e.Y+1),  new Point(e.X+1, e.Y+1),
                    new Point(e.X-1, e.Y),                              new Point(e.X+1, e.Y),
                    new Point(e.X-1, e.Y-1),    new Point(e.X, e.Y-1),  new Point(e.X+1, e.Y-1)
                };

                if (!neighbors.Any(_elves.Contains)) continue;

                // Honestly, if this is null here, we've got bigger issues than no error checking. 
                LinkedListNode<Directions>? currentDirection = _directions.First;

                while(currentDirection is not null)
                {
                    bool isOtherElf = currentDirection.Value switch
                    {
                        Directions.North => neighbors.Where(n => n.Y == e.Y + 1).Any(_elves.Contains),
                        Directions.South => neighbors.Where(n => n.Y == e.Y - 1).Any(_elves.Contains),
                        Directions.East => neighbors.Where(n => n.X == e.X + 1).Any(_elves.Contains),
                        Directions.West => neighbors.Where(n => n.X == e.X - 1).Any(_elves.Contains),
                        _ => false
                    };

                    if (!isOtherElf)
                    {
                        Point moveKey = currentDirection.Value switch
                        {
                            Directions.North => new Point(e.X, e.Y + 1),
                            Directions.South => new Point(e.X, e.Y - 1),
                            Directions.East => new Point(e.X + 1, e.Y),
                            Directions.West => new Point(e.X - 1, e.Y),
                            _ => throw new NotSupportedException($"What direction? {currentDirection.Value}")
                        };

                        if (proposedMoves.TryGetValue(moveKey, out List<Point>? dest))
                        {
                            dest.Add(e); // Uh-oh, we've already got this key. Time to meet our neighbor
                        }
                        else
                        {
                            proposedMoves.Add(moveKey, new List<Point> { e }); // All clear... for now. 
                        }

                        break; // we've got a move, next elf time. 
                    } 
                    
                    currentDirection = currentDirection.Next;
                    
                } //end while
            } // end for each elf

            foreach (KeyValuePair<Point, List<Point>> kvp in proposedMoves)
            {
                if (kvp.Value.Count > 1) continue; // more than one elf wants to go here? Nope. 

                _elves.Remove(kvp.Value.First());
                _elves.Add(kvp.Key);
            }

            // pop off FIRST value of the linked list and stick it on the end. 
            _directions.AddLast(_directions.FirstOrDefault());
            _directions.RemoveFirst();

            // Did we do any moves?
            return proposedMoves.Count != 0;
        }
    }
}
