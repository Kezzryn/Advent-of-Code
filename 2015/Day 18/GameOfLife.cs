namespace AoC_2015_Day_18
{
    internal class GameOfLife
    {
        private readonly (int x, int y)[] _neighbors = new[]
        {
            (-1, 1), (0, 1),(1, 1),
            (-1, 0),        (1, 0),
            (-1,-1), (0,-1),(1,-1)
        };

        private readonly (int x, int y)[] _stuckLights = new(int x, int y)[4];

        private readonly bool[,,] _lights;

        // we use the 3rd dimension of lights to build our next puzzle state. This tracks which one is "live" 
        private int _targetBoard = 0;

        public GameOfLife(string[] puzzleInput)
        {
            _lights = new bool[puzzleInput[0].Length, puzzleInput.Length, 2];

            int y = 0;
            // flip this so that our text file and array display are in sync for easier debugging. 
            foreach (string line in puzzleInput.Reverse())
            {
                for (int x = 0; x < line.Length; x++)
                {
                    _lights[x, y, _targetBoard] = puzzleInput[y][x] == '#';
                }
                y++;
            }

            // Setup for part two. 
            _stuckLights[0] = (_lights.GetLowerBound(0), _lights.GetLowerBound(1));
            _stuckLights[1] = (_lights.GetUpperBound(0), _lights.GetLowerBound(1));
            _stuckLights[2] = (_lights.GetLowerBound(0), _lights.GetUpperBound(1));
            _stuckLights[3] = (_lights.GetUpperBound(0), _lights.GetUpperBound(1));
        }

        private bool NewValue(int x, int y, int board)
        {
            int numNeighbors = 0;

            foreach (var neighbor in _neighbors)
            {
                // rather than a bunch of bounds checking logic, just catch the error.
                // this probalby isn't the most performance optimal solution, but it makes the code easy.
                try
                {
                    numNeighbors += _lights[x + neighbor.x, y + neighbor.y, board] ? 1 : 0;
                }
                catch (IndexOutOfRangeException)
                {
                    continue;
                }

                if (numNeighbors >= 4) return false; // death! 
            }

            //A light which is on stays on when 2 or 3 neighbors are on, and turns off otherwise.
            //A light which is off turns on if exactly 3 neighbors are on, and stays off otherwise.
            return _lights[x, y, board] ? (numNeighbors == 2 || numNeighbors == 3) : numNeighbors == 3;
        }


        public void Step(bool stuckLights = false)
        {
            // flip flops between 1 and 0 
            _targetBoard = (_targetBoard + 1) % 2;

            for (int x = 0; x <= _lights.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= _lights.GetUpperBound(1); y++)
                {
                    // NB: in the NewValue call, we flip _targetBoard so we're looking at our "past" state, while still writing to the "current" one.
                    _lights[x, y, _targetBoard] = NewValue(x, y, (_targetBoard + 1) % 2); 
                }
            }

            if (stuckLights) // part 2 tweak. 
            {
                foreach(var (x, y) in _stuckLights)
                {
                    _lights[x, y, _targetBoard] = true;
                }
            }
        }

        public int CountLights()
        {
            int returnvalue = 0;
            for (int x = 0; x <= _lights.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= _lights.GetUpperBound(1); y++)
                {
                    returnvalue += _lights[x, y, _targetBoard] ? 1 : 0;
                }
            }
            return returnvalue;
        }

        public void PrintLights()
        {
            for (int y = 0; y <= _lights.GetUpperBound(1); y++)
            {
                for (int x = 0; x <= _lights.GetUpperBound(0); x++)
                {
                    if (_lights[x, y, _targetBoard]) Console.Write('#'); else Console.Write('.');
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
