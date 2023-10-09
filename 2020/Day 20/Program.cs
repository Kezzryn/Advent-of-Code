using AoC_2020_Day_20;

try
{
    const int MATCH_ANY = -1;
    const int MATCH_EDGE = 0;

    const string PUZZLE_INPUT = "PuzzleInput.txt";
    const string CRLF = "\r\n";

    //The sea monster 0,0 is in the bottom left corner.
    List<(int x, int y)> seaMonster = new()
    {
        // bottom
        (1,0),
        (4,0),
        (7,0),
        (10,0),
        (13,0),
        (16,0),
        // middle 
        (0, 1),
        (5, 1),
        (6, 1),
        (11, 1),
        (12, 1),
        (17, 1),
        (18, 1),
        (19, 1),
        // top
        (18, 2)
    };
    int seaMonsterSizeX = seaMonster.Select(sm => sm.x).Max();
    int seaMonsterSizeY = seaMonster.Select(sm => sm.y).Max();

    string[] puzzleInput = File.ReadAllText(PUZZLE_INPUT).Split(CRLF + CRLF).ToArray();

    Dictionary<int, PuzzlePiece> tiles = new();

    foreach (string inputTiles in puzzleInput)
    {
        string[] tile = inputTiles.Split(CRLF);
        int rowlen = tile[1].Length;

        int tileNum = int.Parse(tile[0][5..^1]);
        string top = tile[1];
        string bottom = tile[tile.GetUpperBound(0)];
        string left = "";
        string right = "";
        string center = "";

        for (int i = 1; i < tile.Length; i++)
        {
            left += tile[i][0];
            right += tile[i][rowlen - 1];
            if (i > 1 && i < tile.Length - 1)
            {
                center += tile[i][1..^1];
                center += CRLF;
            }
        }

        tiles.Add(tileNum, new(top, bottom, left, right, center));
    }

    HashSet<int> corners = new();
    long part1Answer = 1;
    int part2Answer = 0;
    Dictionary<int, HashSet<int>> matchSet = new();

    // find the corners and matching edges
    foreach ((int currentValue, PuzzlePiece piece) in tiles)
    {
        matchSet.TryAdd(currentValue, new());
        foreach ((int otherValue, PuzzlePiece otherPiece) in tiles.Where(x => x.Key != currentValue))
        {
            foreach (int i in piece.AllNums())
            {
                if (otherPiece.AllNums().Contains(i)) matchSet[currentValue].Add(otherValue);
            }
        }

        if (matchSet[currentValue].Count == 2)
        {
            corners.Add(currentValue);
            part1Answer *= currentValue;
        }

        if (matchSet[currentValue].Count == 2 || matchSet[currentValue].Count == 3)
        {
            List<int> edgesToClear = new(tiles[currentValue].AllNums());
            foreach (int tileNum in matchSet[currentValue])
            {
                var linkNums = tiles[tileNum].AllNums().Intersect(tiles[currentValue].AllNums());
                edgesToClear.RemoveAll(x => linkNums.Contains(x));
            }
            tiles[currentValue].ClearEdges(edgesToClear);
        }
    }

    Console.WriteLine($"Part 1: The product of the corner IDs is {part1Answer}.");

    int puzzleSize = (int)Math.Sqrt(tiles.Count);
    int tileSize = tiles.First().Value.TileSize;
    int[,] puzzleGrid = new int[puzzleSize, puzzleSize];
    int maxX = puzzleGrid.GetUpperBound(0);
    int maxY = puzzleGrid.GetUpperBound(1);

    foreach ((int corner, int doFlip) in from c in corners
                                         from f in Enumerable.Range(0, 2)
                                         select (c, f))
    {
        //reset our run.
        Array.Clear(puzzleGrid);
        part2Answer = 0;

        //grab one of the corners we've identified as a starting point.
        puzzleGrid[0, 0] = corner;
        if (doFlip == 1) tiles[corner].Flip();

        foreach ((int X, int Y) currentTile in from x in Enumerable.Range(0, puzzleSize)
                                               from y in Enumerable.Range(0, puzzleSize)
                                               select (x, y))
        {
            int currentTileID = puzzleGrid[currentTile.X, currentTile.Y];

            int matchDown = currentTile.Y == 0 ? MATCH_EDGE : tiles[puzzleGrid[currentTile.X, currentTile.Y - 1]].Up;

            int matchLeft = currentTile.X == 0 ? MATCH_EDGE : tiles[puzzleGrid[currentTile.X - 1, currentTile.Y]].Right;

            int matchRight = currentTile.X == maxX ? MATCH_EDGE : MATCH_ANY;
            int matchUp = currentTile.Y == maxY ? MATCH_EDGE : MATCH_ANY;

            if (!tiles[currentTileID].MakeFit(matchUp, matchRight, matchDown, matchLeft))
            {
                Console.WriteLine($"Unable to make tile {currentTileID} at {currentTile.X},{currentTile.Y} fit.");
            }

            //figure out where our adjacent pieces are going to go. 
            for (int side = 0; side < 4; side++)
            {
                int newX = 0;
                int newY = 0;
                int link = 0;

                switch (side)
                {
                    case 0:
                        link = tiles[currentTileID].Up;
                        newX = currentTile.X;
                        newY = currentTile.Y + 1;
                        break;
                    case 1:
                        link = tiles[currentTileID].Right;
                        newX = currentTile.X + 1;
                        newY = currentTile.Y;
                        break;
                    case 2:
                        link = tiles[currentTileID].Down;
                        newX = currentTile.X;
                        newY = currentTile.Y - 1;
                        break;
                    case 3:
                        link = tiles[currentTileID].Left;
                        newX = currentTile.X - 1;
                        newY = currentTile.Y;
                        break;
                    default:
                        throw new NotImplementedException();
                }

                if (link == 0) continue;
                if (newX < 0 || newY < 0 || newX > maxX || newY > maxY) continue;
                if (puzzleGrid[newX, newY] != 0) continue;

                foreach (int tileID in matchSet[currentTileID])
                {
                    if (tiles[tileID].AllNums().Contains(link))
                    {
                        puzzleGrid[newX, newY] = tileID;
                        break; // found a match, early exit.
                    }
                }
            }
        }


        // convert the filled out grid to an oceanMap
        char[,] oceanMap = new char[(puzzleSize * tileSize), (puzzleSize * tileSize)];

        foreach ((int x, int y) in from yOcean in Enumerable.Range(0, (puzzleSize * tileSize))
                                   from xOcean in Enumerable.Range(0, (puzzleSize * tileSize))
                                   select (xOcean, yOcean))
        {
            int id = puzzleGrid[(x / tileSize), (y / tileSize)];

            oceanMap[x, y] = tiles[puzzleGrid[(x / tileSize), (y / tileSize)]].GetTileFragment((x % tileSize), (y % tileSize));
            if (oceanMap[x, y] == '#') part2Answer++;
        }

        // grid is filled out. Can we find a sea monster? 
        bool foundMonster = false;
        HashSet<(int x, int y)> seaMonsterLocations = new();

        foreach ((int X, int Y) offset in from xSM in Enumerable.Range(0, (puzzleSize * tileSize) - seaMonsterSizeX)
                                          from ySM in Enumerable.Range(0, (puzzleSize * tileSize) - seaMonsterSizeY)
                                          select (xSM, ySM))
        {
            int countMatches = 0;
            foreach ((int X, int Y) seaM in seaMonster)
            {
                if (oceanMap[offset.X + seaM.X, offset.Y + seaM.Y] == '#') countMatches++;
            }

            if (countMatches == seaMonster.Count)
            {
                part2Answer -= seaMonster.Count;
                seaMonsterLocations.Add((offset.X, offset.Y));
                foundMonster = true;
            }
        }

        if (foundMonster)
        {
            Console.WriteLine();
            for (int y = puzzleGrid.GetUpperBound(1); y >= 0; y--)
            {
                for (int x = 0; x <= puzzleGrid.GetUpperBound(0); x++)
                {
                    Console.Write($"{puzzleGrid[x, y]}  ");
                }
                Console.WriteLine();
            }

            foreach((int X, int Y) foundSeaMonster in seaMonsterLocations)
            {
                foreach((int smX, int smY) in seaMonster)
                {
                    oceanMap[smX + foundSeaMonster.X, smY + foundSeaMonster.Y] = 'O';
                }
            }


            Console.WriteLine();
            for (int y = oceanMap.GetUpperBound(1); y >= 0; y--)
            {
                for (int x = 0; x <= oceanMap.GetUpperBound(0); x++)
                {
                    char c = oceanMap[x, y];
                    ConsoleColor temp = Console.ForegroundColor;
                    Console.ForegroundColor = c switch
                    {
                        'O' => ConsoleColor.Green,
                        '.' => ConsoleColor.Black,
                        '#' => ConsoleColor.Blue,
                        _ => ConsoleColor.Red
                    };
                    Console.Write(oceanMap[x, y]);
                    Console.ForegroundColor = temp;
                }
                Console.WriteLine();
            }

            break;
        }
    }

    Console.WriteLine();
    Console.WriteLine($"Part 2: The water roughness is measured as {part2Answer}.");
}
catch (Exception e)
{
    Console.WriteLine(e);
}