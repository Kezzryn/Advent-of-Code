namespace AoC_2024_Day_21;
using BKH.Geometry;

internal class Robot
{
    public const int NumericKeypad = 0;
    public const int ArrowKeys = 1;

    private readonly Dictionary<KeySymbols, Point2D> _numKeypad = new()
    {
        { KeySymbols.Seven, new(0,3) },  { KeySymbols.Eight, new(1,3) },  { KeySymbols.Nine, new(2,3) },
        { KeySymbols.Four, new(0,2) },   { KeySymbols.Five, new(1,2) },   { KeySymbols.Six, new(2,2) },
        { KeySymbols.One, new(0,1) },    { KeySymbols.Two, new(1,1) },    { KeySymbols.Three, new(2,1) },
                                         { KeySymbols.Zero, new(1,0) },   { KeySymbols.Push, new(2,0) }
    };

    private readonly Dictionary<KeySymbols, Point2D> _keypad = new()
    {
                                        { KeySymbols.Up, new(1,1) },    { KeySymbols.Push, new(2,1) },
        { KeySymbols.Left, new(0,0) },  { KeySymbols.Down, new(1,0) },  { KeySymbols.Right, new(2,0) }
    };

    private readonly Point2D _emptySpace;
    
    private readonly Dictionary<(KeySymbols, KeySymbols), long> _moveCache = [];

    private readonly Robot? NextRobot = null;

    public Robot(int keypadType, int maxDepth)
    {
        if (keypadType == NumericKeypad) _keypad = _numKeypad;

        _emptySpace = _keypad[KeySymbols.Push] + new Point2D(-2, 0);

        if (maxDepth > 0)   
        {
            NextRobot = new Robot(ArrowKeys, maxDepth - 1);
        }
    }

    public long ProcessDoorCode(string doorCode)
    {
        long returnValue = 0;

        for(int a = 0; a < doorCode.Length; a++)
        {
            if(a == 0 )
            {
                returnValue += MoveToValue(KeySymbols.Push, (KeySymbols)doorCode[a]);
            }
            else 
            {
                returnValue += MoveToValue((KeySymbols)doorCode[a - 1], (KeySymbols)doorCode[a]); 
            }
        }

        return returnValue;
    }

    private bool DoHorizFirst(Point2D start, Point2D end, Point2D diff)
    {
        /* From Reddit.
         * https://www.reddit.com/r/adventofcode/comments/1hj7f89/comment/m34erhg/
         * if empty space forces a path, do that.
         * Otherwise if moving left, horiz then vert
         * Otherwise vert then horiz.
         */

        if (_emptySpace.Y == start.Y && _emptySpace.X == end.X) return false; //moving left into the empty space.
        if (_emptySpace.X == start.X && _emptySpace.Y == end.Y) return true;  //moving vertially into the empty space.

        return int.IsPositive(diff.X); // prefer horiz only if moving left. 
    }

    public long MoveToValue(KeySymbols fromPos, KeySymbols toPos)
    {
        if(_moveCache.TryGetValue((fromPos, toPos), out long cacheValue)) return cacheValue;

        List<KeySymbols> currentMoves = [];
        if (fromPos != toPos)
        {
            Point2D start = _keypad[fromPos];
            Point2D end = _keypad[toPos];
            Point2D diff = start - end;

            IEnumerable<KeySymbols> updown = Enumerable.Repeat(
                    int.IsPositive(diff.Y) ? KeySymbols.Down : KeySymbols.Up,
                    Math.Abs(diff.Y));

            IEnumerable<KeySymbols> leftright = Enumerable.Repeat(
                    int.IsPositive(diff.X) ? KeySymbols.Left : KeySymbols.Right,
                    Math.Abs(diff.X));

            if (DoHorizFirst(start, end, diff))
            {
                currentMoves.AddRange(leftright);
                currentMoves.AddRange(updown);
            }
            else
            {
                currentMoves.AddRange(updown);
                currentMoves.AddRange(leftright);
            }
        }
        currentMoves.Add(KeySymbols.Push);

        long rv = 0; 

        if (NextRobot == null)
        {
            rv = currentMoves.Count;
        }
        else
        {
            for (int i = 0; i < currentMoves.Count; i++)
            {
                if(i == 0)
                { 
                    rv += NextRobot.MoveToValue(KeySymbols.Push, currentMoves[i]);
                }
                else 
                {
                    rv += NextRobot.MoveToValue(currentMoves[i - 1], currentMoves[i]);
                }
            }
        }

        _moveCache.TryAdd((fromPos, toPos), rv);
        return rv;
    }

    public enum KeySymbols
    {
        Up, 
        Right, 
        Down, 
        Left, 
        Push = 'A',
        One = '1',
        Two = '2',
        Three = '3',
        Four = '4',
        Five = '5',
        Six = '6',
        Seven = '7',
        Eight = '8',
        Nine = '9',
        Zero = '0'
    }
}
