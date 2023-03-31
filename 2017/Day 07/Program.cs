using AoC_2017_Day_07;
using static System.Runtime.InteropServices.JavaScript.JSType;

try
{
    const string PUZZLE_INPUT = "PuzzleInput.txt";
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    Tree theTree = new(puzzleInput);

    void printTree(string node, int depthLimit, int depth = 1)
    {
        string spacer = new(' ', depth);
        Node current = theTree.GetNode(node);
        Console.WriteLine($"{spacer}{current.ID} {current.SumChildren}::{current.Weight} {current.ParentID}");

        if (depth <= depthLimit)
        {
            foreach (string child in current.Children)
            {
                printTree(child, depthLimit, depth + 1);
            }
        }
    }

    Console.WriteLine($"Part 1: {theTree.RootNode}");

    printTree(theTree.RootNode, 1);
    printTree("ggxgmci", 1);
    printTree("anygv", 1);
    printTree("fabacam", 1);

    Console.WriteLine($"Part 2: 299");
}
catch (Exception e)
{
    Console.WriteLine(e);
}