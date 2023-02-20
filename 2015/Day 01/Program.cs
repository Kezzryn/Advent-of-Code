try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string santaSteps = File.ReadAllText(PUZZLE_INPUT);

    int currentFloor = 0; 
    int numSteps = 0;
    int firstBasement = -1;

    foreach (char floor in santaSteps)
    {
        numSteps++;
        currentFloor += floor switch {
            '(' => +1,
            ')' => -1,
            _ => throw new Exception($"WTF {floor}")
        };

        firstBasement = (firstBasement == -1 && currentFloor < 0) ? numSteps : firstBasement;
    }

    Console.WriteLine($"Santa is on floor: {currentFloor}");
    Console.WriteLine($"Santa enters the basement at {firstBasement}");
}
catch (Exception e)
{
    Console.WriteLine(e);
}