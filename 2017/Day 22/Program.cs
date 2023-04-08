using System.Numerics;

static int VirusHunt(string[] mapData, int numBursts, bool isEvolved = false)
{
    int returnValue = 0;

    Dictionary<Complex, NodeState> theMap = new();
    Dictionary<NodeState, NodeState> nextState = new();

    if (isEvolved)
    {
        nextState.Add(NodeState.Clean, NodeState.Weakened);
        nextState.Add(NodeState.Weakened, NodeState.Infected);
        nextState.Add(NodeState.Infected, NodeState.Flagged);
        nextState.Add(NodeState.Flagged, NodeState.Clean);
    }
    else
    {
        nextState.Add(NodeState.Clean,    NodeState.Infected);
        nextState.Add(NodeState.Infected, NodeState.Clean);
    }

    for (int row = 0; row < mapData.Length; row++)
    {
        for (int col = 0; col < mapData[row].Length; col++)
        {
            theMap.Add(new Complex(col, row),
                mapData[row][col] == '#' ? NodeState.Infected : NodeState.Clean);
        }
    }

    Complex cursor = new(mapData[0].Length / 2, mapData.Length / 2);
    Complex direction = new(0, 1);

    foreach (int step in Enumerable.Range(0, numBursts))
    {
        if (!theMap.TryGetValue(cursor, out NodeState state))
        {
            state = NodeState.Clean;
            theMap.Add(cursor, state);
        }

        direction *= state switch
        {
            NodeState.Clean => Complex.ImaginaryOne,    //turn right
            NodeState.Infected => -Complex.ImaginaryOne,//turn left
            NodeState.Weakened => Complex.One,          //straight 
            NodeState.Flagged => -Complex.One,          //reverse
            _ => throw new NotImplementedException()
        };

        theMap[cursor] = nextState[theMap[cursor]];

        if (theMap[cursor] == NodeState.Infected) returnValue++;
        
        cursor += direction;
    }

    return returnValue;
}

try
{
    const int PART_1_NUM_BURSTS = 10000;
    const int PART_2_NUM_BURSTS = 10000000;
    const bool IS_EVOLVED = true;

    const string PUZZLE_INPUT = "PuzzleInput.txt";

    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT).Reverse().ToArray();

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