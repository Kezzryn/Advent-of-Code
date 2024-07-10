using BKH.Geometry;

static int VirusHunt(string[] mapData, int numBursts, bool isEvolved = false)
{
    int returnValue = 0;

    Dictionary<Point2D, NodeState> theMap = [];
    Dictionary<NodeState, NodeState> nextState = [];

    if (isEvolved)
    {
        nextState.Add(NodeState.Clean,    NodeState.Weakened);
        nextState.Add(NodeState.Weakened, NodeState.Infected);
        nextState.Add(NodeState.Infected, NodeState.Flagged);
        nextState.Add(NodeState.Flagged,  NodeState.Clean);
    }
    else
    {
        nextState.Add(NodeState.Clean,    NodeState.Infected);
        nextState.Add(NodeState.Infected, NodeState.Clean);
    }

    theMap = mapData.SelectMany(
            (row, r) => row.Select(
                (col, c) => (X: r, Y: c, State: mapData[r][c] == '#' ? NodeState.Infected : NodeState.Clean)))
                .ToDictionary(key => new Point2D(key.X, key.Y), val => val.State);

    Cursor cursor = new(mapData[0].Length / 2, mapData.Length / 2, 0, 1);

    foreach (int step in Enumerable.Range(0, numBursts))
    {
        Point2D currentPosition = cursor.XYAsPoint2D;
        if (!theMap.TryGetValue(currentPosition, out NodeState state))
        {
            state = NodeState.Clean;
            theMap.Add(currentPosition, state);
        }

        if (state == NodeState.Clean) cursor.TurnLeft();
        if (state == NodeState.Infected) cursor.TurnRight();
        if (state == NodeState.Flagged) cursor.TurnAround();

        theMap[currentPosition] = nextState[state];
        if (nextState[state] == NodeState.Infected) returnValue++;

        cursor.Step();
    }

    return returnValue;
}

try
{
    const int PART_1_NUM_BURSTS = 10000;
    const int PART_2_NUM_BURSTS = 10000000;
    const bool IS_EVOLVED = true;

    const string PUZZLE_INPUT = "PuzzleInput.txt";

    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Reverse().ToArray(); //flip to put 0,0 in lower left.

    int part1Answer = VirusHunt(puzzleInput, PART_1_NUM_BURSTS);
    int part2Answer = VirusHunt(puzzleInput, PART_2_NUM_BURSTS, IS_EVOLVED);
    
    Console.WriteLine($"Part 1: The number of nodes that become infected after {PART_1_NUM_BURSTS} is {part1Answer}.");
    Console.WriteLine($"Part 2: After its evolution, the number of nodes that become infected after {PART_2_NUM_BURSTS} is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}

enum NodeState
{
    Clean,
    Weakened,
    Infected,
    Flagged
};