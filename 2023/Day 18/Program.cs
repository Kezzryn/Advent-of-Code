global using Point2D = (long X, long Y); // Oh shiny!
static Point2D AddPoint2D(Point2D A, Point2D B) => ((A.X + B.X), (A.Y + B.Y));
static Point2D MultPoint2D(Point2D A, int mult) => ((A.X * mult), (A.Y * mult));

Dictionary<char, Point2D> directions = new()
{
    { 'U', (0,1) },
    { 'D', (0,-1) },
    { 'L', (-1,0) },
    { 'R', (1,0) },
    //Below is translated for part 2
    //0 means R, 1 means D, 2 means L, and 3 means U.
    { '0', (1,0) },
    { '1', (0,-1) },
    { '2', (-1,0) },
    { '3', (0,1) },
};

static long GetArea(LinkedList<Point2D> theMap)
{
    // This thread was helpful in untangling how Pick's Theorm and the shoelace formula work.
    // https://www.reddit.com/r/adventofcode/comments/18lj7wx/2023_day_18_how_do_you_adapt_the_shoelace_formula/

    LinkedListNode<Point2D>? cursor = theMap.First ?? throw new NullReferenceException();
    LinkedListNode<Point2D>? cursorNext = null;

    long innerArea = 0;
    long perimeter = 0;
    do
    {
        //using the linked list for easy looping.
        cursorNext = cursor.Next ?? theMap.First ?? throw new NullReferenceException();

        //This is basically taxicab distance.
        perimeter +=
            Math.Abs((cursor.Value.X - cursorNext.Value.X)) +
            Math.Abs((cursor.Value.Y - cursorNext.Value.Y));

        //Shoelace formula.
        innerArea +=
            (cursor.Value.X * cursorNext.Value.Y) -
            (cursorNext.Value.X * cursor.Value.Y);

        cursor = cursor.Next;
    } while (cursor is not null);

    //This is Pick's Theorm, using shoelace formula output as the input for the inner value.
    return (innerArea / 2) + (perimeter / 2) + 1;
}

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    LinkedList<Point2D> theMapP1 = new();
    Point2D cursorP1 = (0, 0);

    LinkedList<Point2D> theMapP2 = new();
    Point2D cursorP2 = (0, 0);

    //load the lists.
    foreach (string line in puzzleInput)
    {
        //part 1 
        char dir = line[0];
        int numSteps = int.Parse(line[2..(line.IndexOf('(') - 1)]);

        cursorP1 = AddPoint2D(cursorP1, MultPoint2D(directions[dir], numSteps));
        theMapP1.AddFirst(cursorP1);

        // part 2
        string hexCodes = line[(line.IndexOf('#')+1)..^1];
        dir = hexCodes[^1];
        numSteps = Convert.ToInt32(hexCodes[..^1], 16);
       
        cursorP2 = AddPoint2D(cursorP2, MultPoint2D(directions[dir], numSteps));
        theMapP2.AddFirst(cursorP2);
    }

    long part1Answer = GetArea(theMapP1);
    long part2Answer = GetArea(theMapP2);

    Console.WriteLine($"Part 1: The inital instructions fill an area of {part1Answer}.");
    Console.WriteLine($"Part 2: The revised instructions fill {part2Answer} cubic meters.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}