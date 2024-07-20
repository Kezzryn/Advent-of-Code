using System.Text.RegularExpressions;
using BKH.Geometry;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Dictionary<int, Point2D> starLocation = [];
    Dictionary<int, Point2D> starVelocity = [];

    for (int i = 0; i < puzzleInput.Length; i++)
    {
        int[] matches = Regex.Matches(puzzleInput[i], @"[-]?\d+")
            .Select(x => int.Parse(x.Value)).ToArray();

        starLocation.Add(i, new Point2D(matches[0], matches[1]));
        starVelocity.Add(i, new Point2D(matches[2], matches[3]));
    }
    
    bool doStepping = false;
    bool doReverse = false;
    bool isDone = false;
    int numSteps = 0;

    while(!isDone)
    {
        numSteps += doReverse ? -1 : 1;

        foreach (int key in starLocation.Keys)
        {
            if (doReverse)
            {
                starLocation[key] -= starVelocity[key];
            }
            else
            {
                starLocation[key] += starVelocity[key];
            }
        }

        Point2D min = new(starLocation.Min(x => x.Value.X), starLocation.Min(x => x.Value.Y));
        Point2D max = new(starLocation.Max(x => x.Value.X), starLocation.Max(x => x.Value.Y));

        long area = (long)(max.X - min.X) * (long)(max.Y - min.Y);  //explicit cast to avoid overflow issues.

        // Switch to manual inspection once the cloud starts to come together.
        if (area < 5000) doStepping = true;

        if (doStepping)
        {
            //translate to screen coordinates. 
            bool[,] starmap = new bool[(max.X - min.X) + 1, (max.Y - min.Y) + 1];
        
            foreach (Point2D point in starLocation.Values)
            {
                starmap[point.X - min.X, point.Y - min.Y] = true;
            }

            Console.Clear();
            for (int y = 0; y <= starmap.GetUpperBound(1); y++)
            {
                for (int x = 0; x <= starmap.GetUpperBound(0); x++)
                {
                    Console.Write(starmap[x, y] ? '#' : ' ');
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine($"As seen on tick {numSteps}.");
            Console.WriteLine();
            Console.WriteLine("ESC to quit. Left and Right arrow to step.");
            ConsoleKeyInfo keyPress = Console.ReadKey();
            switch (keyPress.Key)
            {
                case ConsoleKey.Escape:
                    isDone = true;
                    break;
                case ConsoleKey.LeftArrow:
                    doReverse = true;
                    break;
                case ConsoleKey.RightArrow:
                default:
                    doReverse = false;
                    break;
            }
        }
    }
}
catch (Exception e)
{
    Console.WriteLine(e);
}