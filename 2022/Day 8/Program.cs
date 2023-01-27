//used to clarify GetLowerBound / GetUpperBound calls. 
const int ROW = 0; //rows are the first dimension of the forest array (array is 0 bound) 
const int COL = 1; //columsn are the second dimension of the forest array (array is 0 bound) 

//derived from the input file and should be the max row/col of the forest. 
int FOREST_ROW = -1;
int FOREST_COL = -1;

//global forest
int[,] forest;

//tree tracking for part one.
int[,] tree_visable;

int treehouse_row = -1;
int treehouse_col = -1;
void LoadForest()
{
    //note that we flip the map when we load it, 'cause top down file goes to 0 bound array
    //shouldn't matter since none of our answers rely on absolute positioning.
    const string PUZZLE_INPUT = "..\\..\\..\\..\\..\\Input Files\\Day 8.txt"; ;
    string[] puzzleInput = File.ReadAllLines(PUZZLE_INPUT);

    FOREST_COL = puzzleInput.Count();
    FOREST_ROW = puzzleInput[0].Count();

    forest = new int[FOREST_ROW, FOREST_COL];
    tree_visable = new int[FOREST_ROW, FOREST_COL];

    for (int row = 0; row < FOREST_ROW; row++)
    {
        for (int col = 0; col < FOREST_COL; col++)
        {
            forest[row, col] = int.Parse(puzzleInput[row].Substring(col, 1));
        }
    }
}

int GetSceneScore(int tree_row, int tree_col)
{
    //return the product of the number of trees visable in the four cardinal directions from the given position parameters. 
    //number of trees is a count that stops if you reach an edge or at the first tree that is the same height or taller than the tree under consideration.

    //scene score for each direction 
    int ss_left = 0;
    int ss_right = 0;
    int ss_top = 0;
    int ss_bot = 0;

    int maxHeight = forest[tree_row, tree_col];

    //scan up
    if (tree_row != forest.GetLowerBound(ROW))  //edge check
    {
        for (int i = tree_row - 1; i >= forest.GetLowerBound(ROW); i--)
        {
            ss_top++;
            if (forest[i, tree_col] >= maxHeight) break;
        }
    }

    //scan down
    if (tree_row != forest.GetUpperBound(ROW)) //edge check
    {
        for (int i = tree_row + 1; i <= forest.GetUpperBound(ROW); i++)
        {
            ss_bot++;
            if (forest[i, tree_col] >= maxHeight) break;
        }
    }

    //scan left
    if (tree_col != forest.GetLowerBound(COL)) // edge check
    {
        for (int i = tree_col - 1; i >= forest.GetLowerBound(COL); i--)
        {
            ss_left++;
            if (forest[tree_row, i] >= maxHeight) break;
        }
    }

    //scan right
    if (tree_col != forest.GetUpperBound(COL)) //edge check 
    {
        for (int i = tree_col + 1; i <= forest.GetUpperBound(COL); i++)
        {
            ss_right++;
            if (forest[tree_row, i] >= maxHeight) break;
        }
    }
    //Console.WriteLine($"({tree_row},{tree_col}) = {ss_left} * {ss_right} * {ss_top} * {ss_bot}");
    return ss_left * ss_right * ss_top * ss_bot;
}

int Part1()
{
    //A tree is visible if all of the other trees between it and an edge of the grid are shorter than it.
    //Only consider trees in the same row or column; that is, only look up, down, left, or right from any given tree.

    const int TALLEST_TREE = 9; //no trees can be taller than this.
    int tallestTreeSeen = -1;
    int returnValue = 0;

    //break this out so we're not duplicating too much code
    Action<int, int> TestTree = (row, col) =>
    {
        if (forest[row, col] > tallestTreeSeen)
        {
            tallestTreeSeen = forest[row, col];
            tree_visable[row, col] = 1;
        }
    };

    //make two big passes, checking horizontally then vertically, moving in from each each edge. 
    //searching top to bottom,
    for (int row = 0; row < FOREST_ROW; row++)
    {
        //left edge to right.
        tallestTreeSeen = -1;
        for (int col = 0; col < FOREST_COL; col++)
        {
            TestTree(row, col);
            if (tallestTreeSeen == TALLEST_TREE) break; // nothing taller than this one.
        }

        //right edge to left. 
        tallestTreeSeen = -1;
        for (int col = FOREST_COL - 1; col >= 0; col--)
        {
            TestTree(row, col);
            if (tallestTreeSeen == TALLEST_TREE) break; //no trees bigger than this one. all others are in it's shadow. 
        }
    }

    //searching left to right.
    for (int col = 0; col < FOREST_COL; col++)
    {
        //top to bottom 
        tallestTreeSeen = -1;
        for (int row = 0; row < FOREST_ROW; row++)
        {
            TestTree(row, col);
            if (tallestTreeSeen == TALLEST_TREE) break; //no trees bigger than this one. all others are in it's shadow. 
        }

        //bottom to top. 
        tallestTreeSeen = -1;
        for (int row = FOREST_ROW - 1; row >= 0; row--)
        {
            TestTree(row, col);
            if (tallestTreeSeen == TALLEST_TREE) break; //no trees bigger than this one. all others are in it's shadow.
        }
    }

    //count the visable trees. 
    //Cast()-ing flattens the array allowing the use of Sum(); 
    returnValue = tree_visable.Cast<int>().Sum();

    return returnValue;
}


int Part2()
{
    int bestTree = 0;
    int treeScore = 0;

    for (int row = 0; row < FOREST_ROW; row++)
    {
        for (int col = 0; col < FOREST_COL; col++)
        {
            treeScore = GetSceneScore(row, col);
            if (treeScore >= bestTree)
            {
                bestTree = treeScore;
                treehouse_row = row;
                treehouse_col = col;
            }
        }
    }
    return bestTree;
}

void DrawForest()
{
    for (int row = tree_visable.GetLowerBound(ROW); row <= tree_visable.GetUpperBound(ROW); row++)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        if (row < 10) { Console.Write($"0{row} "); } else { Console.Write($"{row} "); }

        for (int col = tree_visable.GetLowerBound(COL); col <= tree_visable.GetUpperBound(COL); col++)
        {
            if (tree_visable[row, col] == 1) Console.ForegroundColor = ConsoleColor.Green;
            if (row == treehouse_row && col == treehouse_col) Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(forest[row, col]);
            Console.ResetColor();
        }
        Console.WriteLine("");
    }
}

try
{
    LoadForest();

    Console.WriteLine($"The number of visible trees is: {Part1()}");
    Console.WriteLine($"The most scenic tree is: {Part2()}");

    DrawForest();

}
catch (Exception e)
{
    Console.WriteLine(e);
}


