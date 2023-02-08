using System.Numerics;
using System.Text;

namespace AoC_2022_Day_22
{
    internal static class CubeMapTypes
    {
        public const int FlatMap = 0;
        public const int CubeMap = 1;
    }

    internal class Cube
    {
        private readonly Map[] _theCube;
        private Cursor _cursor;
        private bool _verbose = false;

        public Cube(string[] cubeData, int mapType) // Part 1 is flat. part2 is not-flat ie folded. 
        {
            int cubeSize = Math.Abs(cubeData.Length - cubeData.Select(x => x.Length).Max());
            int cubeIndex = 0;
            _theCube = new Map[6];

            for (int y = 0; y < cubeData.Length; y += cubeSize)
            {
                int quadY = y / cubeSize;

                for (int x = 0; x < cubeData[y].Length; x += cubeSize)
                {

                    if (cubeData[y][x] == ' ') continue; //nothing in this quadrant. 

                    StringBuilder sb = new();
                    for (int sideY = y; sideY < y + cubeSize; sideY++)
                    {
                        sb.Append(cubeData[sideY][x..(x + cubeSize)]);
                    }

                    int quadX = x / cubeSize;
                    _theCube[cubeIndex] = new Map(sb.ToString(), cubeSize, quadX, quadY, cubeIndex);
                    cubeIndex++;
                }
            }
            _cursor = new();

            switch (mapType)
            {
                case CubeMapTypes.FlatMap: 
                    FoldFlat();
                    break;
                case CubeMapTypes.CubeMap: 
                    FoldCube();
                    break;
                default:
                    throw new NotImplementedException($"mapType");
            }
            
        }
        private void FoldCube()
        {
            FoldFlat();
        }
        private void FoldFlat()
        {
            // setup linkages between the cube sides.
            // for each grid space, test up/down/left/right. If there is a square there, record it, if not find the first/last one in the row as appropriate,
            // EVEN IF IT'S YOURSELF.

            foreach (Map m in _theCube)
            {
                List<Map> horizSlice = _theCube.Where(x => x.GridY == m.GridY).OrderBy(x => x.GridX).ToList();
                List<Map> vertSlice = _theCube.Where(x => x.GridX == m.GridX).OrderBy(x => x.GridY).ToList();

                if (_verbose) Console.WriteLine($"Linking ID: {m.ID} ({m.GridX}, {m.GridY})");

                // set left 
                Map link = (m.GridX > horizSlice.First().GridX) 
                    ?   horizSlice.Where(x => x.GridX == m.GridX - 1).First()
                    :   horizSlice.Last();

                if (_verbose) Console.WriteLine($"--LEFT ID: {link.ID} ({link.GridX}, {link.GridY})");
                m.SetLink(Directions.Left, link.ID);

                // set right
                link = (m.GridX < horizSlice.Last().GridX)
                  ? horizSlice.Where(x => x.GridX == m.GridX + 1).First()
                  : horizSlice.First();

                if (_verbose) Console.WriteLine($"--RIGHT ID: {link.ID} ({link.GridX}, {link.GridY})");
                m.SetLink(Directions.Right, link.ID);

                // set up NB: "Up" is -Y on the grid. 
                link = (m.GridY > vertSlice.First().GridY)
                  ? vertSlice.Where(x => x.GridY == m.GridY - 1).First()
                  : vertSlice.Last();

                if (_verbose) Console.WriteLine($"--UP ID: {link.ID} ({link.GridX}, {link.GridY})");
                m.SetLink(Directions.Up, link.ID);

                // set down NB: "Down" is +Y on the grid. 
                link = (m.GridY < vertSlice.Last().GridY)
                  ? vertSlice.Where(x => x.GridY == m.GridY + 1).First()
                  : vertSlice.First();

                if (_verbose) Console.WriteLine($"--DOWN ID: {link.ID} ({link.GridX}, {link.GridY})");
                m.SetLink(Directions.Down, link.ID);
            }
        }

        public static List<string> TranslateMoveset(string moveData)
        {
            // Accepts the moveData from the input file and return a list of moves back for the main program to enumerate on.

            List<string> returnValue = new();
            int cursor;

            for(int i = 0;i< moveData.Length; i++)
            {
                cursor = moveData.IndexOfAny("LR".ToCharArray(), i);
                if (cursor == -1)
                {
                    returnValue.Add(moveData[i..]);
                    break;
                }

                returnValue.Add(moveData[i..cursor]);
                returnValue.Add(moveData[cursor].ToString());
                i = cursor;
            }

            return returnValue;
        }

        public void Move(string move)
        {
            if (!int.TryParse(move, out int dist))
            {
                // not a numeric value, we should be able to turn. 
                _cursor.Turn(move);
            }
            else
            {
                for (int i = 0; i < dist; i++)
                {
                    Cursor nextStep = _cursor.ShallowCopy();
                    nextStep.Step();
                    if (!_theCube[nextStep.MapIndex].IsOnMap(nextStep))
                    {
                        if (_verbose) Console.WriteLine($"We're off the map. {nextStep}");
                        nextStep.MapIndex = _theCube[_cursor.MapIndex].GetLink(_cursor.Facing);
                        int newMapRotation = _theCube[_cursor.MapIndex].GetRotation(_cursor.Facing);

                        switch (_cursor.Facing)
                        {
                            case Directions.Left:
                                nextStep.X = _theCube[nextStep.MapIndex].GetInterval();
                                break;
                            case Directions.Right:
                                nextStep.X = 1;
                                break;
                            case Directions.Up:
                                nextStep.Y = _theCube[nextStep.MapIndex].GetInterval();
                                break;
                            case Directions.Down:
                                nextStep.Y = 1;
                                break;
                            default:
                                throw new NotImplementedException($"_cursor.Facing = {_cursor.Facing}");
                        }
                        //now rotate the new map X based on the rotation.
                        if (newMapRotation != 0) TranslatePosition(ref nextStep, newMapRotation);
                        if (_verbose) Console.WriteLine($"Moves and translatons's done. {nextStep}");
                    }

                    // got our maps sorted, Check move forward. 
                    if (_theCube[nextStep.MapIndex].IsWall(nextStep))
                    {
                        if (_verbose) Console.WriteLine($"Wall detected at: {nextStep}");
                        break;
                    }
                    else
                    {
                        // if we made it here, it's not a wall and we can update our _cursor with our step.
                        _cursor = nextStep;
                        if (_verbose) Console.WriteLine($"Updating _cursor new values: {_cursor}");
                    }
                }
            }
        }

        private void TranslatePosition(ref Cursor cursor, int rotation)
        {
            // yes System.Windows.Media.Transform exists. 

            float centerPoint = (float)_theCube[cursor.MapIndex].GetInterval() / 2;

            Vector2 coords = new(cursor.X, cursor.Y);

            Matrix3x2 rotationMatrix = rotation switch
            {
                90 or -270 => Matrix3x2.CreateRotation(-(float)(Math.PI / 2), new Vector2(centerPoint, centerPoint)),
                180 or -180 => Matrix3x2.CreateRotation((float)(Math.PI), new Vector2(centerPoint, centerPoint)),
                270 or -90 => Matrix3x2.CreateRotation(-(float)((3*Math.PI) / 2), new Vector2(centerPoint, centerPoint)),
                _ => throw new NotImplementedException($"Unknown rotaton. {rotation}")
            } ;

            coords = Vector2.Subtract(coords, Vector2.One); // normalize to zero. 

            coords = Vector2.Transform(coords, rotationMatrix); // rotate

            coords = Vector2.Add(coords, Vector2.One); // shift back to match our 1,1 based grid. 

            cursor.X = (int)coords.X;
            cursor.Y = (int)coords.Y;

            switch (rotation)
            {
                case 90 or -270:
                    cursor.Turn("R");
                    break;
                case 180 or -180:
                    cursor.Turn("R");
                    cursor.Turn("R");
                    break;
                case 270 or -90:
                    cursor.Turn("L");
                    break;
                default:
                    throw new NotImplementedException($"Unknown rotaton. {rotation}");
            };
        }

        public Tuple<int, int, int> CursorPosition()
        {
            Tuple<int, int> xy = _theCube[_cursor.MapIndex].TranslateToSource(_cursor);
            return new(xy.Item1, xy.Item2, _cursor.Facing);
        }

        public void ToggleVerbose() => _verbose = !_verbose;
    }
}
