using System.Collections;
using System.Text.RegularExpressions;

const string PUZZLE_INPUT = "PuzzleInput.txt";
string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

const string CRLF = "\r\n"; // if we ever run this on another platform, fix this. 
const int CRANE = 0;


string[] cargoData = puzzleInput.Split(CRLF + CRLF)[0].Split(CRLF);
string[] movesData = puzzleInput.Split(CRLF + CRLF)[1].Split(CRLF);

Stack[] LoadCargo(bool verbose = false)
{
    Stack[] returnValue;

    //step one, initalize the cargo stacks. 

    //EXAMPLE input one space is between it and the moveset. 
    /* 
        [D]    
    [N] [C]    
    [Z] [M] [P]
     1   2   3 
    */

    //define our array of cargo stacks from 1 to the number of indexes found.
    //we're going to use the 0 position in the cargo stack array for our "crane" 
    string[] temp = Regex.Split(cargoData[cargoData.GetUpperBound(0)], @"\D+");

    int numStacks = -1;
    if (!int.TryParse(temp.Max(), out numStacks)) throw new Exception("Failed to parse numStacks");

    //initalize our return array. 
    returnValue = new Stack[numStacks + 1];
    for (int i = returnValue.GetLowerBound(0); i <= returnValue.GetUpperBound(0); i++)
    {
        returnValue[i] = new Stack();
    }

    //now that we have our cargo (barge?) we need to load it up.
    //We scan the bottom row of data for the indexes since they line up with the cargo values,
    //then scan up the records pushing each character to the stack.
    //ASSUMPTION: Max of 9 columns 

    for (int col = 0; col < cargoData[cargoData.GetUpperBound(0)].Length; col++)
    {
        if (cargoData[cargoData.GetUpperBound(0)][col] != ' ')
        {
            int aryIndex = cargoData[cargoData.GetUpperBound(0)][col] % 48; //char to int conversion via ASCII table modulo yay!
            for (int row = cargoData.GetUpperBound(0) - 1; row >= 0; row--)
            {
                if (col <= cargoData[row].Length && cargoData[row][col] != ' ')
                {
                    if (verbose) Console.WriteLine($"Stacking {cargoData[row][col]} on {aryIndex}");
                    returnValue[aryIndex].Push(cargoData[row][col]);
                }
            }
        }
    }

    return returnValue;
}

string Part1(bool verbose = false)
{
    Stack[] cargo = LoadCargo(verbose);
    string returnValue = "";

    //move X crates from stack Y to stack Z, one at a time. 
    //return the top crate from each stack as the result. 

    for (int i = 0; i < movesData.Length; i++)
    {
        //EG input
        //move 3 from 9 to 6
        string[] moveParse = movesData[i].Split(' ');

        int numCrates = int.Parse(moveParse[1]);
        int fromStack = int.Parse(moveParse[3]);
        int toStack = int.Parse(moveParse[5]);

        for (int j = 0; j < numCrates; j++)
        {
            if (verbose) Console.WriteLine($"pushing value {cargo[fromStack].Peek()} to {toStack} from {fromStack}");
            cargo[toStack].Push(cargo[fromStack].Pop());
        }
    }

    for (int i = 1; i < cargo.Length; i++) //skip element zero, since that's our reserved CRANE element for part2
    {
        returnValue += cargo[i].Peek();
    }

    return returnValue;
}


string Part2(bool verbose = false)
{
    //move X crates from stack Y to stack Z, X at a time. (IE: preserve the order) 
    //return the top crate from each stack as the result. 

    Stack[] cargo = LoadCargo(verbose);
    string returnValue = "";

    for (int i = 0; i < movesData.Length; i++)
    {
        //Example input
        //move 3 from 9 to 6
        string[] moveParse = movesData[i].Split(' ');

        int numCrates = int.Parse(moveParse[1]);
        int fromStack = int.Parse(moveParse[3]);
        int toStack = int.Parse(moveParse[5]);

        for (int j = 0; j < numCrates; j++)
        {
            if (verbose) Console.WriteLine($"pushing value {cargo[fromStack].Peek()} to CRANE from {fromStack}");
            cargo[CRANE].Push(cargo[fromStack].Pop());
        }

        for (int j = 0; j < numCrates; j++)
        {
            if (verbose) Console.WriteLine($"pushing value {cargo[CRANE].Peek()} to {toStack} from CRANE");
            cargo[toStack].Push(cargo[CRANE].Pop());
        }
    }

    for (int i = 1; i < cargo.Length; i++) //skip element zero, since that's our reserved CRANE element for part2
    {
        returnValue += cargo[i].Peek();
    }

    return returnValue;
}


try
{
    bool verbose = false;
    Console.WriteLine($"Part 1 {Part1(verbose)}");
    Console.WriteLine($"Part 2 {Part2(verbose)}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}