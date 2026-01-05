using BKH.Geometry;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const int NUM_LOOPS = 1000;
    List<Point3D> puzzleInput = [.. File.ReadAllLines(PUZZLE_INPUT).Select(x => new Point3D([.. x.Split(',').Select(y => int.Parse(y))]))];

    Dictionary<double, (Point3D A, Point3D B)> distances = [];

    for (int a = 0; a < puzzleInput.Count - 1; a++)
    {
        for (int b = a + 1; b < puzzleInput.Count; b++)
        {
            distances.Add(Point3D.EuclideanDistance3D(puzzleInput[a], puzzleInput[b]), (puzzleInput[a], puzzleInput[b]));
        }
    }

    List<List<Point3D>> chains = [];
    Dictionary<Point3D, int> chainIndex = [];   //Points to which chain a specific point is in. Used for faster searching so we don't have to iterate each chain every loop

    int loop = 0;
    int part1Answer = 0;
    long part2Answer = 0;
    
    foreach (var (k, (A, B)) in distances.OrderBy(x => x.Key))
    {
        loop++;
        
        switch (chainIndex.TryGetValue(A, out int indexA), chainIndex.TryGetValue(B, out int indexB))
        {
            case (true, true):
                if (indexA != indexB) //we're going to merge B into A and leave B a stub.
                {
                    chains[indexA].AddRange(chains[indexB]);
                    foreach (Point3D p in chains[indexB])
                    {
                        chainIndex[p] = indexA;
                    }
                    chains[indexB].Clear();
                }
                break;
            case (true, false): //Add B to A's chain
                chains[indexA].Add(B);
                chainIndex[B] = indexA;
                break;
            case (false, true): //Add A to B's chain
                chains[indexB].Add(A);
                chainIndex[A] = indexB;
                break;
            case (false, false): // Net new.
                chains.Add([A, B]);
                chainIndex[A] = chains.Count - 1;
                chainIndex[B] = chains.Count - 1;
                break;
            default:
                throw new Exception("How did we even get here?");
        }

        if (loop == NUM_LOOPS) part1Answer = chains.Select(x => x.Count).OrderByDescending(x => x).Take(3).Aggregate((x, y) => x * y);
        if (chains[indexA].Count == puzzleInput.Count || chains[indexB].Count == puzzleInput.Count)
        {
            part2Answer = (long)A.X * (long)B.X; //convert to long or overflow.
            break; 
        }
    }

    Console.WriteLine($"Part 1: The product of the largest three circuts after {NUM_LOOPS} is {part1Answer}.");
    Console.WriteLine($"Part 2: The product of the X coordinates of the last joined pair is {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}