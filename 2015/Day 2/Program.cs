int GetArea(int l, int w, int h) => (2 * l * w) + (2 * w * h) + (2 * h * l);
int GetVolume(int l, int w, int h) => l * w * h;

int GetSlack(int l, int w, int h)
{
    if (h >= l && h >= w) return l * w;
    if (w >= l && w >= h) return h * l;
    if (l >= h && l >= w) return h * w;
    return 0;
}

int GetPerimeter(int l, int w, int h)
{
    if (h >= l && h >= w) return 2 * (l + w);
    if (w >= l && w >= h) return 2 * (h + l);
    if (l >= h && l >= w) return 2 * (h + w);
    return 0;
}

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
        string[] s = box.Split('x');
        wrappingPaperArea += GetArea(int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2]));
        wrappingPaperSlack += GetSlack(int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2]));

        ribbonWrap += GetPerimeter(int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2]));
        ribbonBow += GetVolume(int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2]));
    }

    Console.WriteLine("Part 1:"); 
    Console.WriteLine($"We will need {wrappingPaperArea} cubic feet of wrapping paper and an extra {wrappingPaperSlack} of slack paper.");
    Console.WriteLine($"The total paper we will need is {wrappingPaperArea + wrappingPaperSlack}.");

    Console.WriteLine("Part 2:");
    Console.WriteLine($"We will need {ribbonWrap} feet of ribbon for wrapping and {ribbonBow} for the bow.");
    Console.WriteLine($"The total amount of ribbon we will need is {ribbonWrap + ribbonBow}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}