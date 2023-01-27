namespace AoC_2022_Day_17
{
    internal class Rocks
    {
        // the current rock that's being thrown down the shaft, as well as all related rock data for the current rock.  

        public static readonly bool AIR = false;
        public static readonly bool ROCK = true;

        private static readonly bool[][,] theRocks = new bool[][,]
        {
            new bool[4, 1] { { ROCK }, { ROCK }, { ROCK }, { ROCK } },                          //Horizontal line
            new bool[3, 3] { { AIR,  ROCK, AIR }, { ROCK, ROCK, ROCK }, { AIR,  ROCK, AIR } },  //Plus
            new bool[3, 3] { { ROCK, AIR, AIR }, { ROCK, AIR, AIR }, { ROCK, ROCK, ROCK } },    //L
            new bool[1, 4] { { ROCK, ROCK, ROCK, ROCK } },                                      //Vertical line
            new bool[2, 2] { { ROCK, ROCK }, { ROCK, ROCK } }                                   //Square 
        };

        private int _currentRock = -1;

        public int HandleX { get; set; } = 0;
        public int HandleY { get; set; } = 0;

        public int RockWidth { get { return theRocks[_currentRock].GetUpperBound(0); } }
        public int RockHeight { get { return theRocks[_currentRock].GetUpperBound(1); } }

        public Rocks()
        {
        }

        public char CurrentRockID()
        {
            return _currentRock switch
            {
                1 => 'H',
                2 => '+',
                3 => 'L',
                4 => 'V',
                5 => 'S',
                _ => '?',
            };
        }

        public bool[,] GetCurrentRock() => GetRock(_currentRock);
        public static bool[,] GetRock(int rockNum)
        {
            if (rockNum >= theRocks.GetLowerBound(0) && rockNum <= theRocks.GetUpperBound(0))
            {
                return theRocks[rockNum];
            }
            else
            {
                return new bool[,] { { AIR } };
            }
        }

        public void NextRock()
        {
            _currentRock++;
            if (_currentRock > theRocks.GetUpperBound(0)) _currentRock = theRocks.GetLowerBound(0);
        }

        public bool[,] GetNextRock()
        {
            NextRock();
            return GetRock(_currentRock);
        }
    }
}
