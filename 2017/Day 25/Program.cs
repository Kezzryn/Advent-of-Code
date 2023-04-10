try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string CRLF = "\r\n";
    const int STATE_TRUE = 1024;

    string[][] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(CRLF+CRLF).Select(x => x.Split(CRLF).ToArray()).ToArray();

    int currentState = 0;
    int numLoops = 0;
    int cursor = 0;

    HashSet<int> tape = new();
    Dictionary<int, (int nextValue, int nextState, int nextPOS)> nextStep = new();

    for (int i = 0; i < puzzleInput.Length; i++)
    {
        if (i == 0)
        {
            //Begin in state X.
            //Perform a diagnostic checksum after 1234 steps.

            currentState = puzzleInput[i][0].ToCharArray().TakeLast(2).First();
            numLoops =     int.Parse(puzzleInput[i][1].Split(' ')[5]);
        }
        else
        {
            //In state X:                               line 0
            //  If the current value is 0:              line 1
            //    - Write the value 1.                  line 2
            //    - Move one slot to the [right/left].  line 3
            //    - Continue with state Y.              line 4

            int thisState = puzzleInput[i][0].ToCharArray().TakeLast(2).First();
            int value =     puzzleInput[i][2].ToCharArray().TakeLast(2).First() == '1' ? 1 : 0;
            int POS =       puzzleInput[i][3].Contains("right") ? 1 : -1;
            int state =     puzzleInput[i][4].ToCharArray().TakeLast(2).First(); 

            nextStep.Add(thisState, (value, POS, state));

            //  If the current value is 1:              line 5
            //    - Write the value 1.                  line 6
            //    - Move one slot to the [right/left].  line 7
            //    - Continue with state Y.              line 8
            
            value =         puzzleInput[i][6].ToCharArray().TakeLast(2).First() == '1' ? 1 : 0;
            POS =           puzzleInput[i][7].Contains("right") ? 1 : -1;
            state =         puzzleInput[i][8].ToCharArray().TakeLast(2).First();
                
            nextStep.Add(thisState | STATE_TRUE, (value, POS, state));
        }
    }

    for (int i = 1; i <= numLoops; i++)
    {
        int key = currentState | (tape.Contains(cursor) ? STATE_TRUE : 0);
        
        if (!nextStep.TryGetValue(key, out (int nextValue, int nextPos, int nextState) ns)) throw new Exception();

        if (ns.nextValue == 1) tape.Add(cursor);
        if (ns.nextValue == 0) tape.Remove(cursor);

        cursor += ns.nextPos;
        currentState = ns.nextState;
    }

    int part1Answer = tape.Count;
 
    Console.WriteLine($"Part 1: The checksum is {part1Answer}.");
 }
catch (Exception e)
{
    Console.WriteLine(e);
}