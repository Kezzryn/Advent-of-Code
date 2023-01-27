try
{
    const string PUZZLE_INPUT = @"..\..\..\..\Input Files\Day 2.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    //Playing Rock Paper Scissors. 
    //File is in A X format, where A is opponent move, and X is your move.
    //Score is 6 points for a win, 3 for a draw, 0 for a loss plus 1 for throwing rock, 2 for paper, or 3 for scissors.

    //A for Rock, B for Paper, and C for Scissors.
    //X for Rock, Y for Paper, and Z for Scissors.
    //the scores for each combo of throws. [my throw,opponent throw]
    int[,] resultsPart1 = { { 4, 1, 7 }, { 8, 5, 2 }, { 3, 9, 6 } };
    //                             R{R,P,S} P{R,P,S} S{R,P,S} 
    //                              {T,L,W}  {W,T,L}  {L,W,T}

    //A for Rock, B for Paper, and C for Scissors.
    //X for Loss, Y for Draw, and Z for Win.
    //the scores for each combo of throws. [desired outcome,opponent throw]
    int[,] resultsPart2 = { { 3, 1, 2 }, { 4, 5, 6 }, { 8, 9, 7 } };
    //                             L{R,P,S} D{R,P,S} W{R,P,S}   throw 
    //                              {S,R,P}  {R,P,S}  {P,S,R}


    //UNICODE / ASCII charts have char 'A' = 65, which is 'A' % 32 = 1  (65/32 = 2 R 1 )
    //works for lowercase as well, but our input is all upper case. 
    int scorePart1 = puzzleInput.Select(x =>
    {
        return resultsPart1[
             (x[2] % 32) - 24,  //downshift so X = 0, Y = 1, Z = 2;  ('X' % 32) = 24
            (x[0] % 32) - 1]; //downshift so A = 0, B = 1, C = 2; ('A' % 32) = 1
    })
        .Sum();

    int scorePart2 = puzzleInput.Select(x =>
    {
        return resultsPart2[
            (x[2] % 32) - 24,  //downshift so X = 0, Y = 1, Z = 2;  ('X' % 32) = 24
            (x[0] % 32) - 1]; //downshift so A = 0, B = 1, C = 2; ('A' % 32) = 1
    })
        .Sum();

    Console.WriteLine($"Part one: {scorePart1}");
    Console.WriteLine($"Part two: {scorePart2}");

    /*
     * This runs approx 140x faster than the Select().Sum() syntax.
     
    for (int i = 0;i < puzzleInput.Length; i++)
    {
        
        int myThrow = (puzzleInput[i][2] % 32) - ('X' % 32); 
        int oppThrow = (puzzleInput[i][0] % 32) - ('A' % 32); 

        scorePart1 += resultsPart1[myThrow, oppThrow];
        scorePart2 += resultsPart2[myThrow, oppThrow];
    }

    Console.WriteLine($"Part one: {scorePart1}");
    Console.WriteLine($"Part two: {scorePart2}");
    */
}
catch (Exception e)
{
    Console.WriteLine(e);
}