try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string santaSteps = File.ReadAllText(PUZZLE_INPUT);

    int partOne = santaSteps.Sum(x => x == '(' ? 1 : -1);
    Console.WriteLine($"Part 1: Santa is on floor: {partOne}");

    int currentFloor = 0; 
    int numSteps = 0;
    
    foreach (char floor in santaSteps)
    {
        numSteps++;
        currentFloor += floor switch {
            '(' => +1,
            ')' => -1,
            _ => throw new Exception($"WTF {floor}")
        };

        if (currentFloor < 0) break;
    }
    
    Console.WriteLine($"Part 2: Santa first enters the basement at {numSteps}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}