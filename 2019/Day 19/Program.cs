using AoC_2019_IntcodeVM;

try
{
    const int PART_ONE_MAX = 50;
    const string PUZZLE_INPUT = "PuzzleInput.txt";

    //Load the VM
    long[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(',').Select(long.Parse).ToArray();
    IntcodeVM tractorBot = new(puzzleInput);

    bool IsBeam(int x, int y)
    {
        tractorBot.Reboot();
        tractorBot.SetInput(x);
        tractorBot.SetInput(y);
        tractorBot.Run();
        tractorBot.GetOutput(out long output);
        return output == 1;
    }

    //********* PART ONE
    Dictionary<(int x, int y), MapSymbols> theMap = new();

    foreach((int x,int y) in from a in Enumerable.Range(0, PART_ONE_MAX)
                             from b in Enumerable.Range(0, PART_ONE_MAX)
                             select (a,b))
    {
        theMap.Add((x, y), IsBeam(x,y) ? MapSymbols.Beam: MapSymbols.Space);
    }

    int part1Answer = theMap.Count(x => x.Value == MapSymbols.Beam);
    Console.WriteLine($"Part 1: The beam will affect {part1Answer} points in a 50x50 area.");


    //********* PART TWO
    const int SANTA_SHIP_SIZE = 99; //99 to account for the test point being inclusive to the dataset. 

    // This slope is close enough to the true slope for our needs. 
    double slopeMIN = (double)theMap.Where(x => x.Value == MapSymbols.Beam && x.Key.y == PART_ONE_MAX - 1).Min(m => m.Key.x) / PART_ONE_MAX;

    // this will be under-sloped at this point, but we'll adjust it later. 
    double slopeMAX = (double)theMap.Where(x => x.Value == MapSymbols.Beam && x.Key.y == PART_ONE_MAX - 1).Max(m => m.Key.x) / PART_ONE_MAX;

    int y_part2 = PART_ONE_MAX;
    bool isDone = false;
    int part2Answer = -1;

    while(!isDone)
    {
        y_part2++;
        // make a guess first. 
        int maxX = (int)(y_part2 * slopeMAX);
        int minX = (int)(y_part2 * slopeMIN);

        // ballpark filter
        if ((maxX - minX) < SANTA_SHIP_SIZE) continue;

        // refine our guess on the max beam
        // our guess is in space pull towards the beam.
        while (!IsBeam(maxX, y_part2)) maxX--;

        // our guess is in the beam, do an edge check.
        while (IsBeam(maxX, y_part2) && IsBeam(maxX + 1, y_part2)) maxX++;

        //adjust the slope for less future adjustments.
        slopeMAX = (double)maxX / y_part2;
        
        int shipMinX = maxX - SANTA_SHIP_SIZE;
        int shipMaxY = y_part2 + SANTA_SHIP_SIZE;

        if (IsBeam(shipMinX, shipMaxY))
        {
            part2Answer = (shipMinX * 10000) + y_part2;
            isDone = true;
        }

        if (y_part2 >= 1200) isDone = true; // saftey catch.
    }

    Console.WriteLine($"Part 2: The hash value of the coordiante closest for Santa's Ship to fit in is: {part2Answer}");

}
catch (Exception e)
{
    Console.WriteLine(e);
}

enum MapSymbols
{
    Space,
    Beam
}