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
        private readonly List<Elf> _elves = new();
        private readonly LinkedList<Directions> _directions = new();

        public Grove(string[] initalState)
        {
            string[] puzzleInput = initalState.Reverse().ToArray();


            for (int y = 0; y < puzzleInput.Length; y++)
            {
                for (int x = 0; x < puzzleInput[y].Length; x++)
                {
                    if (puzzleInput[y][x] == '#') _elves.Add(new Elf($"x{x}y{y}", x, y));
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
            Dictionary<(int X, int Y), List<string>> proposedMoves = new();

            foreach (Elf e in _elves)
            {
                // we should be the only result.
                //if (_elves.FindAll(x => x.Y >= e.Y - 1 && x.Y <= e.Y + 1 && x.X >= e.X - 1 && x.X <= e.X + 1).Count == 1) continue; //nobody around us, do nothing.
                // 68990 ticks-ish.  50% time. 
                if (_elves.Where(x => x.Y >= e.Y - 1 && x.Y <= e.Y + 1 && x.X >= e.X - 1 && x.X <= e.X + 1).ToList().Count == 1) continue;

                // Honestly, if this is null here, we've got bigger issues than no error checking. 
                LinkedListNode<Directions>? currentDirection = _directions.First;

                while(currentDirection is not null)
                {
                    bool isOtherElf = currentDirection.Value switch
                    {
                        Directions.North => _elves.Find(rv => rv.Y == e.Y + 1 && rv.X >= e.X - 1 && rv.X <= e.X + 1) != null,
                        Directions.South => _elves.Find(rv => rv.Y == e.Y - 1 && rv.X >= e.X - 1 && rv.X <= e.X + 1) != null,
                        Directions.East => _elves.Find(rv => rv.X == e.X + 1 && rv.Y >= e.Y - 1 && rv.Y <= e.Y + 1) != null,
                        Directions.West => _elves.Find(rv => rv.X == e.X - 1 && rv.Y >= e.Y - 1 && rv.Y <= e.Y + 1) != null,
                        _ => false
                    };

                    if (!isOtherElf)
                    {
                        (int X, int Y) moveKey = currentDirection.Value switch
                        {
                            Directions.North => (e.X, e.Y + 1),
                            Directions.South => (e.X, e.Y - 1),
                            Directions.East => (e.X + 1, e.Y),
                            Directions.West => (e.X - 1, e.Y),
                            _ => (e.X, e.Y)
                        };

                        if (proposedMoves.TryGetValue(moveKey, out List<string> dest))
                        {
                            dest.Add(e.ID); // Uh-oh, we've already got this key. Time to meet our neighbor
                        }
                        else
                        {
                            proposedMoves.Add(moveKey, new List<string> { e.ID }); // All clear... for now. 
                        }

                        break; // we've got a move, next elf time. 
                    } 
                    
                    currentDirection = currentDirection.Next;
                    
                } //end while
            } // end for each elf

            foreach (KeyValuePair<(int X, int Y), List<string>> kvp in proposedMoves)
            {
                if (kvp.Value.Count > 1) continue; // more than one elf. Bounce 'em.

                Elf? elf = _elves.Find(x => kvp.Value.First() == x.ID);

                if (elf != null)
                {
                    elf.X = kvp.Key.X;
                    elf.Y = kvp.Key.Y;
                }
            }

            // pop off FIRST value of the linked list and stick it on the end. 
            _directions.AddLast(_directions.FirstOrDefault());
            _directions.RemoveFirst();

            // Did we do any moves?
            return proposedMoves.Count != 0;
        }
    }
}
