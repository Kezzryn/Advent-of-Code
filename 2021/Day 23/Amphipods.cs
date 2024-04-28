using System.Text;

namespace AoC_2021_Day_23
{ 
    internal class Amphipods
    {
        private const int EMPTY = 0;
        public const int AMBER_POD = 1; 
        public const int BRONZE_POD = 2;
        public const int COPPER_POD = 3;
        public const int DESERT_POD = 4;

        // key is room tile and hallway tile, in that order.
        // Precomputed and made static so it doesn't fill up memory with each instance.
        static private readonly Dictionary<(int room, int hallway), int> _numSteps = new()
        {
            //Amber
            {(7,0), 3}, {(7,1), 2}, {(7,2), 2}, {(7,3), 4}, {(7,4), 6}, {(7,5), 8}, {(7,6), 9},
            {(8,0), 4}, {(8,1), 3}, {(8,2), 3}, {(8,3), 5}, {(8,4), 7}, {(8,5), 9}, {(8,6), 10},
            {(9,0), 5}, {(9,1), 4}, {(9,2), 4}, {(9,3), 6}, {(9,4), 8}, {(9,5), 10}, {(9,6), 11},
            {(10,0), 6}, {(10,1), 5}, {(10,2), 5}, {(10,3), 7}, {(10,4), 9}, {(10,5), 11}, {(10,6), 12},
            //Bronze
            {(11,0), 5}, {(11,1), 4}, {(11,2), 2}, {(11,3), 2}, {(11,4), 4}, {(11,5), 6}, {(11,6), 7},
            {(12,0), 6}, {(12,1), 5}, {(12,2), 3}, {(12,3), 3}, {(12,4), 5}, {(12,5), 7}, {(12,6), 8},
            {(13,0), 7}, {(13,1), 6}, {(13,2), 4}, {(13,3), 4}, {(13,4), 6}, {(13,5), 8}, {(13,6), 9},
            {(14,0), 8}, {(14,1), 7}, {(14,2), 5}, {(14,3), 5}, {(14,4), 7}, {(14,5), 9}, {(14,6), 10},
            //Copper
            {(15,0), 7}, {(15,1), 6}, {(15,2), 4}, {(15,3), 2}, {(15,4), 2}, {(15,5), 4}, {(15,6), 5},
            {(16,0), 8}, {(16,1), 7}, {(16,2), 5}, {(16,3), 3}, {(16,4), 3}, {(16,5), 5}, {(16,6), 6},
            {(17,0), 9}, {(17,1), 8}, {(17,2), 6}, {(17,3), 4}, {(17,4), 4}, {(17,5), 6}, {(17,6), 7},
            {(18,0), 10}, {(18,1), 9}, {(18,2), 7}, {(18,3), 5}, {(18,4), 5}, {(18,5), 7}, {(18,6), 8},
            //Desert
            {(19,0), 9}, {(19,1), 8}, {(19,2), 6}, {(19,3), 4}, {(19,4), 2}, {(19,5), 2}, {(19,6), 3},
            {(20,0), 10}, {(20,1), 9}, {(20,2), 7}, {(20,3), 5}, {(20,4), 3}, {(20,5), 3}, {(20,6), 4},
            {(21,0), 11}, {(21,1), 10}, {(21,2), 8}, {(21,3), 6}, {(21,4), 4}, {(21,5), 4}, {(21,6), 5},
            {(22,0), 12}, {(22,1), 11}, {(22,2), 9}, {(22,3), 7}, {(22,4), 5}, {(22,5), 5}, {(22,6), 6}
        };  

        //podtype = room index in _positions
        static private readonly Dictionary<int, int> _baseRoom = new()
        {
            {AMBER_POD, 7 },
            {BRONZE_POD, 11 },
            {COPPER_POD, 15 },
            {DESERT_POD, 19 },
        };

        static private readonly Dictionary<int, int> _energyMultiplier = new()
        {
            {AMBER_POD, 1 },
            {BRONZE_POD, 10 },
            {COPPER_POD, 100 },
            {DESERT_POD, 1000 },
        };

        public int Energy { get; private set; }
        
        public List<(int moveFrom, int moveTo)> MoveHistory;

        //0..6 are valid hallway placements. 
        //7..10 Amber, 11..14 Bronze, 15..18 Copper, 19..22 Desert
        private readonly bool[] _atHome = new bool[23];
        private readonly int[] _positions = new int[23];

        public Amphipods(int[] a, int[] b, int[] c, int[] d)
        {
            int expectedLength = 4;
            if (a.Length != expectedLength) throw new ArgumentException($"a.Length is {a.Length} Expected to be {expectedLength}.");
            if (b.Length != expectedLength) throw new ArgumentException($"b.Length is {b.Length} Expected to be {expectedLength}.");
            if (c.Length != expectedLength) throw new ArgumentException($"c.Length is {c.Length} Expected to be {expectedLength}.");
            if (d.Length != expectedLength) throw new ArgumentException($"d.Length is {d.Length} Expected to be {expectedLength}.");

            Array.Copy(a, 0, _positions, _baseRoom[AMBER_POD], expectedLength);
            Array.Copy(b, 0, _positions, _baseRoom[BRONZE_POD], expectedLength);
            Array.Copy(c, 0, _positions, _baseRoom[COPPER_POD], expectedLength);
            Array.Copy(d, 0, _positions, _baseRoom[DESERT_POD], expectedLength);

            // Find and set any initial critters that are at home. This is mainly for part one since that data has known at home critters.
            for (int podType = 1; podType <= 4; podType++)
            {
                for(int roomIndex = _baseRoom[podType] + 3; roomIndex >= _baseRoom[podType]; roomIndex--)
                {
                    if (_positions[roomIndex] != podType) break;
                    _atHome[roomIndex] = true;
                }
            }

            MoveHistory = [];
        }

        public Amphipods(Amphipods cloneFrom)
        {
            Array.Copy(cloneFrom._positions, _positions, cloneFrom._positions.Length);
            Array.Copy(cloneFrom._atHome, _atHome, cloneFrom._atHome.Length);
            Energy = cloneFrom.Energy;
            MoveHistory = new(cloneFrom.MoveHistory);
        }

        private int CanMoveHome(int PodType)
        {
            int returnValue = -1;
            for (int i = _baseRoom[PodType]; i < _baseRoom[PodType] + 4; i++)
            {
                if (_positions[i] == EMPTY)
                {
                    returnValue = i;
                    continue;
                }
                if (_positions[i] != PodType) return -1;
            }
            return returnValue;
        }

        static private bool IsHallwayPosition(int positionIndex) => positionIndex < _baseRoom[AMBER_POD];

        private bool IsPath(int source, int dest)
        {
            if (_positions[dest] != EMPTY) return false; //quick early exit check.

            if (IsHallwayPosition(source) && IsHallwayPosition(dest)) return false; //both in the hallway. 
            if (!IsHallwayPosition(source) && !IsHallwayPosition(dest)) return false; //neither in the hallway

            //organize our arguments to make our check always from a room to the hallway.
            (int fromRoom, int toHallway) = IsHallwayPosition(source) ? (dest, source) : (source, dest);

            int hallStartIndex = 0;

            //01 A 2 B 3 C 4 D 56
            //This is a bit of data trickery here, where the room pod values happen to line up with the exit indexes of the room positions.
            if (fromRoom >= _baseRoom[AMBER_POD]) hallStartIndex = AMBER_POD;
            if (fromRoom >= _baseRoom[BRONZE_POD]) hallStartIndex = BRONZE_POD;
            if (fromRoom >= _baseRoom[COPPER_POD]) hallStartIndex = COPPER_POD;
            if (fromRoom >= _baseRoom[DESERT_POD]) hallStartIndex = DESERT_POD;

            // check the side room to hallway
            for (int i = fromRoom; i >= _baseRoom[hallStartIndex]; i--) //Remember the hallStartindex matches a side room. 
            {
                if (_positions[i] == EMPTY) continue;
                if (i != source) return false;
            }

            // check hallway exit to the destination 
            if (toHallway <= hallStartIndex)
            {
                //moving left, towards zero.
                for (int i = hallStartIndex; i >= toHallway; i--)
                {
                    if (_positions[i] == EMPTY) continue;
                    if (i != source) return false;
                }
            }
            else
            {
                //moving right, up the hall
                for (int i = ++hallStartIndex; i <= toHallway; i++)
                {
                    if (_positions[i] == EMPTY) continue;
                    if (i != source) return false;
                }
            }

            return true;
        }

        public bool AllAtHome()
        {
            for (int i = _baseRoom[AMBER_POD]; i <= _atHome.GetUpperBound(0); i++)
            {
                if (!_atHome[i]) return false;
            }
            return true;
        }

        public List<(int, int)> BuildMoveList()
        {
            List<(int moveFrom, int moveTo)> moveList = [];

            for (int positionIndex = 0; positionIndex <= _positions.GetUpperBound(0); positionIndex++)
            {
                if (_positions[positionIndex] == EMPTY) continue;
                if (IsHallwayPosition(positionIndex))
                {
                    int moveHome = CanMoveHome(_positions[positionIndex]);
                    if (moveHome != -1 && IsPath(positionIndex, moveHome))
                        moveList.Add((positionIndex, moveHome));
                }
                else
                {
                    if (_atHome[positionIndex]) continue;
                    for (int hallwayStep = 0; hallwayStep < _baseRoom[AMBER_POD]; hallwayStep++)
                    {
                        if (_positions[hallwayStep] != EMPTY) continue;

                        if (IsPath(positionIndex, hallwayStep))
                        {
                            moveList.Add((positionIndex, hallwayStep));
                        }
                    }
                }
            }
            return moveList;
        }

        public Int128 GetHash()
        {
            Int128 returnValue = 0;
            for(int i = 0; i <= _positions.GetUpperBound(0); i++)
            {
                returnValue <<= 3;
                returnValue += _positions[i];
            }
            return returnValue;
        }

        public void MoveAmphipod(int source, int dest)
        {
            Energy += _energyMultiplier[_positions[source]] * source switch
            {
                <= 6 => _numSteps[(dest,source)],
                _ => _numSteps[(source, dest)],
            };

            (_positions[source], _positions[dest]) = (_positions[dest], _positions[source]);
            
            if (!IsHallwayPosition(dest)) _atHome[dest] = true;
            MoveHistory.Add((source, dest));
        }

        override public string ToString()
        {
            StringBuilder sb = new();

            for(int i = 0; i < 7; i++)
            {
                if (i >= 2 && i <= 5) sb.Append($" ..");
                sb.Append($" {_positions[i]}");
            }
            sb.Append('\n');

            for (int i = 0; i < 4; i++)
            {
                sb.Append("  ");
                for (int j = 1; j <= 4; j++)
                {
                    sb.Append($"    {_positions[_baseRoom[j] + i]}");
                }
                sb.Append('\n');
            }
            return sb.ToString();
        }
    }
}
