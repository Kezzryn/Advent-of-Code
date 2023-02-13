using AoC_2022_Day_16;

try
{
    // Implementaion is a translation of the solution found: 
    // https://github.com/juanplopes/advent-of-code-2022/blob/main/day16.py
    // as done by reddit user  u/juanplopes 
    // I had attempted a branch and bound solution.
    // However, due to the shifting topography of the solution, I could not determine a bounding function that would reliably lead to a solution.
    // I did get a brute force to work on the example data, but it failed on the puzzle input. 
    // I'm leaving my older code here as a great example of what does NOT work. :) 

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int totalTimePart1 = 30;
    const int totalTimePart2 = 26;

    string[] caveData = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<string, string[]> theCaves = new(); //G
    Dictionary<string, double> theFlows = new(); // F
    Dictionary<string, int> theValves = new(); // I
    Dictionary<string, Dictionary<string, double>>  theMap = new(); // T

    foreach (string cave in caveData)
    {
        // Valve VS has flow rate=0; tunnels lead to valves FF, GC
        // Valve OI has flow rate=20; tunnel leads to valve SY
        // 012345678901234567890123456789012345678901234567890123456789
        
        string caveName = cave[6..8];
        int flowRate = int.Parse(cave[(cave.IndexOf('=') + 1)..cave.IndexOf(';')]);

        //The index of the space after the word valve, adding 1 move the index after the space, then split the rest of the string on commas and return a string array.
        theCaves.Add(caveName, cave[(cave.IndexOf(' ', cave.IndexOf("valve")) + 1)..].Split(", "));

        if (flowRate > 0)
        {
            theFlows.Add(caveName, flowRate);
            theValves.Add(caveName, 1 << theFlows.Count);
        }
    }

    // Initalize our cave map. this should create all our nodes, if not, we'll throw errors later. 
    // Spoiler alert, we didn't throw errors. 
    // Note that AA -> AA type links are cost "2", where they should be set to 0. However, we never hit them, so they're not getting touched. 
    foreach(string caveX in theCaves.Keys)
    {
        theMap.Add(caveX, new());
        foreach (string caveY in theCaves.Keys)
        {
            theMap[caveX].Add(caveY, theCaves[caveX].Contains(caveY) ? 1 : double.PositiveInfinity);
        }
    }

    // Floyd-Warshall the double dictionary. 
    foreach (string k in theMap.Keys)
    {
        foreach (string i in theMap.Keys)
        {
            foreach (string j in theMap.Keys)
            {
                theMap[i][j] = double.Min(theMap[i][j], theMap[i][k] + theMap[k][j]);
            }
        }
    }

    Dictionary<int, double> visit(string currentCave, double budget, int state, double flow, Dictionary<int, double> answer)
    {
        //add the max of existing state (defaulting to 0) to the dictionary with key state 
        if (answer.TryGetValue(state, out double stateFlow))
        {
            answer[state] = double.Max(stateFlow, flow);
        }
        else
        {
            answer.Add(state, flow);
        }

        foreach (string valveCave in theFlows.Keys)
        {
            //newbudget = budget - T[v][u] - 1
            double newBudget = budget - theMap[currentCave][valveCave] - 1;

            //Check the bitwise & between our valve map and our state so we don't double visit somewhere. 
            if ((theValves[valveCave] & state) != 0 || newBudget <= 0 ) continue;

            //Recursive call for the next cave, while updating our statekey with a bit hack on the valves 
            visit(valveCave, newBudget, state | theValves[valveCave], flow + newBudget * theFlows[valveCave], answer);
        }
        return answer;
    }

    //Why am I allowed to do  all this inlining? Feels weird. :)  
    double part1 = visit("AA", totalTimePart1, 0, 0, new()).Values.ToList().Max();

    Console.WriteLine($"Part 1: The most pressure that can be released in {totalTimePart1} minutes is {part1}.");

    Dictionary<int, double> answerKey = visit("AA", totalTimePart2, 0, 0, new());

    // I REALLY need to be supervised, this LINQ is crazy. 
    // Create a Cartesian product of the answer key, filtering where the keys are "mesh" ( x & y == 0 )
    // This is a super nifty solve for the multiple agents problem. 
    double part2 = answerKey.SelectMany(x => answerKey, (x, y) => new { x, y })
            .Where(pair => (pair.x.Key & pair.y.Key) == 0)
            .Select(pair => pair.x.Value + pair.y.Value)
            .ToList()
            .Max();

    Console.WriteLine($"Part 2: With the aid of an elephant, we can release {part2} pressure in {totalTimePart2} minutes.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}
