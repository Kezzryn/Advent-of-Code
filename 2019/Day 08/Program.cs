using System.Drawing;

try
{
    const int ROWS = 6; //y axis
    const int COLS = 25;//x axis  
    const int LAYER_SIZE = ROWS * COLS; 

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string puzzleInput = File.ReadAllText(PUZZLE_INPUT);

    Dictionary<Point, List<int>> sifImage = new();

    //Data load.
    for(int cursor = 0; cursor < puzzleInput.Length; cursor++)
    {
        int z = cursor / LAYER_SIZE;
        int y = (cursor / COLS) % ROWS;
        int x = cursor % COLS;
        Point key = new (x, y);

        if (z == 0) sifImage.Add(key, new List<int>());
        sifImage[key].Add(puzzleInput[cursor] - '0');
    } 

    // Part 1
    int lowestZero = int.MaxValue;
    int part1Answer = 0;

    for(int z = 0; z < sifImage.First().Value.Count; z++)
    {
        int countZero = 0;
        int countOne = 0; 
        int countTwo = 0;
        foreach(List<int> value in sifImage.Values)
        {
            if (value[z] == 0) countZero++;
            if (value[z] == 1) countOne++;
            if (value[z] == 2) countTwo++;
        }

        if (countZero < lowestZero)
        {
            lowestZero = countZero;
            part1Answer = countOne * countTwo;
        }
    }

    Console.WriteLine($"Part 1: The checksum is: {part1Answer}");
    
    Console.WriteLine($"Part 2: The image message is:");
    Console.WriteLine();

    foreach(Point key in from y in Enumerable.Range(0, ROWS)
                         from x in Enumerable.Range(0, COLS)
                         select new Point(x, y))
    {
        Console.Write(sifImage[key].Where(x => x != 2).First() == 0 ? ' ' : 'X');

        if (key.X == COLS - 1) Console.WriteLine();
    }
}
catch (Exception e)
{
    Console.WriteLine(e);
}