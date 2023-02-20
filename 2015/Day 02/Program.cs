int GetArea(int l, int w, int h) => (2 * l * w) + (2 * w * h) + (2 * h * l);
int GetVolume(int l, int w, int h) => l * w * h;
int GetSlack(int l, int w, int h) => new[] { l, w, h }.OrderBy(x => x).Take(2).Aggregate((a,b) => a * b);
int GetPerimeter(int l, int w, int h) => new[] { l, w, h }.OrderBy(x => x).Take(2).Aggregate((a, b) => 2 * (a + b));

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] boxes = File.ReadAllLines(PUZZLE_INPUT);

    int wrappingPaperArea = 0;
    int wrappingPaperSlack = 0;
    int ribbonWrap = 0;
    int ribbonBow = 0;

    foreach (string box in boxes)
    {
        int[] s = box.Split('x').Select(int.Parse).ToArray();

        wrappingPaperArea += GetArea(s[0], s[1], s[2]);
        wrappingPaperSlack += GetSlack(s[0], s[1], s[2]);

        ribbonWrap += GetPerimeter(s[0], s[1], s[2]);
        ribbonBow += GetVolume(s[0], s[1], s[2]);
    }

    Console.WriteLine("Part 1:"); 
    Console.WriteLine($"We will need {wrappingPaperArea} cubic feet of wrapping paper and an extra {wrappingPaperSlack} of slack paper.");
    Console.WriteLine($"The total paper we will need is {wrappingPaperArea + wrappingPaperSlack} cubit feet.");
    Console.WriteLine();
    Console.WriteLine("Part 2:");
    Console.WriteLine($"We will need {ribbonWrap} feet of ribbon for wrapping and {ribbonBow} for the bow.");
    Console.WriteLine($"The total amount of ribbon we will need is {ribbonWrap + ribbonBow} feet.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}