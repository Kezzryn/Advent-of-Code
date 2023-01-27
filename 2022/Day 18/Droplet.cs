namespace AoC_2022_Day_18
{
    internal class Droplet
    {
        private readonly HashSet<Point3D> _obsidian = new();
        private readonly HashSet<Point3D> _airPockets = new();

        private readonly List<Point3D> _neighbors = new()
        {
            new Point3D(-1, 0, 0),
            new Point3D( 1, 0, 0),
            new Point3D( 0,-1, 0),
            new Point3D( 0, 1, 0),
            new Point3D( 0, 0,-1),
            new Point3D( 0, 0, 1)
        };

        private readonly Point3D _minObsidian;
        private readonly Point3D _maxObsidian;

        public Droplet(string fileName)
        {
            _obsidian = File.ReadAllLines(fileName)
                    .Select(x =>
                    {
                        int[] i = x.Split(',').Select(int.Parse).ToArray();
                        return new Point3D(i[0], i[1], i[2]);
                    })
                    .ToHashSet();

            _minObsidian = new Point3D(
                   _obsidian.Select(x => x.X).Min(),
                   _obsidian.Select(y => y.Y).Min(),
                   _obsidian.Select(z => z.Z).Min()
               );

            _maxObsidian = new Point3D(
                    _obsidian.Select(x => x.X).Max(),
                    _obsidian.Select(y => y.Y).Max(),
                    _obsidian.Select(z => z.Z).Max()
                );
            FindAirPockets();
        }

        public int GetPointArea(Point3D currentPoint)
        {
            int returnValue = 0;

            foreach (Point3D n in _neighbors)
            {
                if (_obsidian.Contains(currentPoint + n)) returnValue++;
            }

            //If this is obsidian, we want the area that does not have adjacent obsidian, otherwise we want the area WITH obsidion bits
            return _obsidian.Contains(currentPoint) ? 6 - returnValue : returnValue;
        }

        public int GetObsidianArea() => _obsidian.Select(GetPointArea).Sum();
        public int GetAirPocketArea() => _airPockets.Select(GetPointArea).Sum();

        public void PrintDrop()
        {
            foreach (Point3D point in _obsidian)
                Console.WriteLine(point.ToString());
        }

        public void PrintAir()
        {
            foreach (Point3D point in _airPockets)
                Console.WriteLine(point.ToString());
        }

        private void FindAirPockets()
        {
            for (int x = _minObsidian.X; x <= _maxObsidian.X; x++)
            {
                for (int y = _minObsidian.Y; y <= _maxObsidian.Y; y++)
                {
                    for (int z = _minObsidian.Z; z <= _maxObsidian.Z; z++)
                    {
                        Point3D currentPoint = new(x, y, z);
                        if (_airPockets.Contains(currentPoint) || _obsidian.Contains(currentPoint) || RayTestNode(currentPoint)) continue;

                        if (IsEnclosed(currentPoint, _minObsidian + new Point3D(-1, -1, -1), out List<Point3D> finalPath))
                        {
                            _airPockets.Add(currentPoint);
                            foreach (Point3D p in finalPath)
                            {
                                _airPockets.Add(p);
                            }
                        }
                    }
                }
            }
        }

        public bool RayTestNode(Point3D testPoint)
        {
            //returns true if it can make a line from testpoint to an edge. 
            //simple pre-test to find if you can hit any edge from here in a straight line.

            foreach (Point3D neighbor in _neighbors)
            {
                bool isBlocked = false;
                for (Point3D ray = testPoint; ray >= _minObsidian && ray <= _maxObsidian + neighbor; ray += neighbor)
                {
                    if (_obsidian.Contains(ray))
                    {
                        isBlocked = true;
                        break;
                    }
                }
                if (!isBlocked) return true;
            }
            return false;
        }

        public bool IsEnclosed(Point3D start, Point3D dest, out List<Point3D> finalPath)
        {
            PriorityQueue<Point3D, int> searchQueue = new(); //we enque based on fScore + h, the distance travelled, plus taxi distance guess to destination.
            HashSet<Point3D> inSearchQueue = new(); //we add this because we don't have a away to query the queue to see if a specific item is in it.

            //gScore is value of the path from start to here
            //fScore is estimated from here to end. We're using Taxi Distance as our hieruistic 
            Dictionary<Point3D, (int gScore, int fScore, Point3D? parent)> stepCounter = new()
            {
                { start, (0, Point3D.TaxiDistance(start, dest), null) }
            };

            searchQueue.Enqueue(start, Point3D.TaxiDistance(start, dest));
            inSearchQueue.Add(start);

            while (searchQueue.Count > 0)
            {
                Point3D cursor = searchQueue.Dequeue();
                if (cursor == dest || cursor < _minObsidian || cursor > _maxObsidian)
                {
                    finalPath = new()
                    {
                        cursor
                    };

                    //unroll our history. 
                    Point3D? p = stepCounter[cursor].parent;

                    while (p != null)
                    {
                        finalPath.Add(p);
                        p = stepCounter[p].parent;
                    }

                    return false;
                }

                foreach (Point3D n in _neighbors)
                {
                    Point3D nextStep = cursor + n;
                    if (!_obsidian.Contains(nextStep))
                    {
                        if (!stepCounter.ContainsKey(nextStep)) stepCounter.Add(nextStep, (int.MaxValue, int.MaxValue, null));

                        //tentative_gScore := gScore[current] + d(current, neighbor)
                        int t_gScore = stepCounter[cursor].gScore + 1;

                        //if tentative_gScore < gScore[neighbor]
                        if (t_gScore < stepCounter[nextStep].gScore)
                        {
                            //cameFrom[neighbor] := current
                            //gScore[neighbor] := tentative_gScore
                            //fScore[neighbor] := tentative_gScore + h(neighbor)
                            stepCounter[nextStep] = (t_gScore, t_gScore + Point3D.TaxiDistance(nextStep, dest), cursor);

                            //if neighbor not in openSet openSet.add(neighbor) 
                            if (!inSearchQueue.Contains(nextStep))
                            {
                                searchQueue.Enqueue(nextStep, stepCounter[nextStep].fScore);
                                inSearchQueue.Add(nextStep);
                            }
                        }
                    }
                }
            }

            //Pull everything out of the step counter and return it as our path/search space. 
            finalPath = new();
            foreach (Point3D p in stepCounter.Keys)
            {
                finalPath.Add(p);
            }
            return true;
        }
    }
}
