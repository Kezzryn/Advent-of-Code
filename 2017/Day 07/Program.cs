using AoC_2017_Day_07;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Tree theTree = new(puzzleInput);

    void PrintTree(string node, int depthLimit, int depth = 1)
    {
        string spacer = new(' ', depth);
        Node current = theTree.GetNode(node);
        Console.WriteLine($"{spacer}{current.ID} {current.SumChildren}::{current.Weight} {current.ParentID}");

        if (depth <= depthLimit)
        {
            foreach (string child in current.Children)
            {
                PrintTree(child, depthLimit, depth + 1);
            }
        }
    }

    int FindImbalance(string node, int imbalance = 0)
    {
        var groupedChildren = theTree.GetChildren(node).Select(x => (Name: x, Weight: theTree.GetNode(x).SumChildren)).GroupBy(g => g.Weight).OrderBy(o => o.Count());

        if (groupedChildren.Count() == 1) return theTree.GetNode(node).Weight - imbalance;

        //First().First() is the oneoff weight.  Last().First() is the target weight
        imbalance = groupedChildren.First().First().Weight - groupedChildren.Last().First().Weight;

        return FindImbalance(groupedChildren.First().First().Name, imbalance);
    }

    Console.WriteLine($"Part 1: The bottom (root) program is {theTree.RootNode}.");

    //Manual check methodoligy
    //PrintTree(theTree.RootNode, 1);
    //PrintTree("ggxgmci", 1);
    //PrintTree("anygv", 1);
    //PrintTree("fabacam", 1);

    Console.WriteLine($"Part 2: The weight of the unbalanced node needs to be adjusted to be {FindImbalance(theTree.RootNode)}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}