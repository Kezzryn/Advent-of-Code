namespace AoC_2024_Dat_16;
using BKH.BaseMap;
using BKH.Geometry;
using System.Collections.Generic;

internal class ReindeerMazes(string[] puzzleInput) : BaseMapCursor(puzzleInput)
{
    const int WALL = 0;
    const int OPEN = 1;

    protected override void LoadMap(string[] puzzleInput)
    {
        foreach (Point2D p in from y in Enumerable.Range(0, puzzleInput.Length)
                              from x in Enumerable.Range(0, puzzleInput[0].Length)
                              select new Point2D(x, y))
        {
            char c = puzzleInput[p.Y][p.X];
            _theMap[p] = c == '#' ? WALL : OPEN;
            if (c == 'S') StartPosition = new(p.X, p.Y, 1, 0);
            if (c == 'E') EndPosition = p;
        }
    }

    protected override bool TestStep(Cursor nextStep)
    {
        if((_theMap.TryGetValue(nextStep.XYAsPoint2D, out int mapTile) && mapTile != WALL))
        {
            stepCounter.TryAdd(nextStep, (int.MaxValue, int.MaxValue, null));
            return true;
        }
        return false;
    }

    public int DoPart = 1;
    protected override bool TestGScore(int t_gScore, int gScore)
    {
        if (DoPart == 1) return t_gScore < gScore;
        return t_gScore <= gScore;
    }

    protected override List<(Cursor, int)> NextSteps(Cursor cursor)
    {
        List<(Cursor, int)> returnValue = [];

        returnValue.Add((cursor.ReturnCloneTurnLeft(), 1000));
        returnValue.Add((cursor.ReturnCloneTurnRight(), 1000));
        returnValue.Add((cursor.ReturnCloneNextStep(), 1));

        return returnValue;
    }

    protected override char DisplayMapSymbol(int mapValue)
    {
        if (mapValue == WALL) return '#';
        if (mapValue == OPEN) return '.';
        return 'x';
    }
}
