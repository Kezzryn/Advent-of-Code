namespace AoC_2024_Day_21;
using BKH.Geometry;

internal class Robot
{
    public const int NumericKeypad = 0;
    public const int ArrowKeys = 1;

    private readonly Dictionary<int, Point2D> _numKeypad = new()
    {
        { '7', new(0,3) },        { '8', new(1,3) },        { '9', new(2,3) },
        { '4', new(0,2) },        { '5', new(1,2) },        { '6', new(2,2) },
        { '1', new(0,1) },        { '2', new(1,1) },        { '3', new(2,1) },
                                  { '0', new(1,0) },        { 'A', new(2,0) }
    };

    private readonly Dictionary<int, Point2D> _keypad = new()
    {
                                                    { (int)Robot.Direction.Up, new(1,1) },      { (int)Robot.Direction.Push, new(2,1) },
        { (int)Robot.Direction.Left, new(0,0) },    { (int)Robot.Direction.Down, new(1,0) },    { (int)Robot.Direction.Right, new(2,0) }
    };

    private readonly Point2D _emptySpace;
    
    private readonly Dictionary<(Direction, Direction), long> _moveCache = [];

    private readonly Robot? NextRobot = null;

    public Robot(int keypadType, int maxDepth)
    {
        if (keypadType == NumericKeypad) _keypad = _numKeypad;

        _emptySpace = (_keypad.ContainsKey('A') ? _keypad['A'] : _keypad[(int)Direction.Push]) + new Point2D(-2, 0);

        if (maxDepth > 0)   
        {
            NextRobot = new Robot(ArrowKeys, maxDepth - 1);
        }
    }

    public long ProcessDoorCode(string doorCode)
    {
        doorCode = "A" + doorCode;

        long returnValue = 0;

        for(int a = 1; a < doorCode.Length; a++)
        {
            returnValue += MoveToValue((Direction)doorCode[a-1], (Direction)doorCode[a]);
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

    public long MoveToValue(Direction fromPos, Direction toPos)
    {
        if(_moveCache.TryGetValue((fromPos, toPos), out long cacheValue)) return cacheValue;

        List<Direction> currentMoves = [];
        if (fromPos != toPos)
        {
            Point2D start = _keypad[(int)fromPos];
            Point2D end = _keypad[(int)toPos];
            Point2D diff = start - end;

            IEnumerable<Direction> updown = Enumerable.Repeat(
                    int.IsPositive(diff.Y) ? Direction.Down : Direction.Up,
                    Math.Abs(diff.Y));

            IEnumerable<Direction> leftright = Enumerable.Repeat(
                    int.IsPositive(diff.X) ? Direction.Left : Direction.Right,
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
        currentMoves.Add(Direction.Push);

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
                    rv += NextRobot.MoveToValue(Direction.Push, currentMoves[i]);
                else 
                    rv += NextRobot.MoveToValue(currentMoves[i - 1], currentMoves[i]);
            }
        }

        _moveCache.TryAdd((fromPos, toPos), rv);
        return rv;
    }

    public enum Direction
    {
        Up, Right, Down, Left, Push
    }
}
