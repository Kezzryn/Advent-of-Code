try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    List<List<int>> puzzleInput = File.ReadAllLines(PUZZLE_INPUT)
        .Select(x => x.Split(' ').Select(int.Parse).ToList())
            .ToList();

    static bool IsSafe(List<int> report)
    {
        bool isAscending = true; 
        bool isDecending = true;
        
        for (int i = 1; i < report.Count; i++)
        {
            if (Math.Abs(report[i - 1] - report[i]) > 3) return false; //Potential early exit.

            isAscending = isAscending && (report[i - 1] < report[i]);
            isDecending = isDecending && (report[i - 1] > report[i]);
        }

        return isAscending || isDecending;
    }

    static bool CanMakeSafe(List<int> report)
    {
        if(IsSafe(report)) return true;  //Check for safe.

        //Otherwise, try to make it safe.
        for (int i = 0; i < report.Count; i++)
        {
            List<int> safetyCheck = new(report);
            safetyCheck.RemoveAt(i);
            if (IsSafe(safetyCheck)) return true;
        }

        return false;
    }

    int part1Answer = puzzleInput.Count(IsSafe);
    int part2Answer = puzzleInput.Count(CanMakeSafe);

    Console.WriteLine($"Part 1: There are {part1Answer} safe reports.");
    Console.WriteLine($"Part 2: Accounting for the Problem Dampener there are now {part2Answer} safe reports.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}