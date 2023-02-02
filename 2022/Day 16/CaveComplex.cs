namespace AoC_2022_Day_16
{
    class CaveComplex
    {
        private const string rootNode = "AA";
        private bool _verbose = false;

        public readonly Dictionary<string, Cave> _caves = new();
        
        public CaveComplex(string[] caveData)
        {
            _caves = new Dictionary<string, Cave>();

            foreach (string cave in caveData)
            {
                // Valve VS has flow rate=0; tunnels lead to valves FF, GC
                // Valve OI has flow rate=20; tunnel leads to valve SY
                // 012345678901234567890123456789012345678901234567890123456789
                string caveName = cave[6..8];
                int flowRate = int.Parse(cave[(cave.IndexOf('=') + 1)..cave.IndexOf(';')]);

                string[] tunnels = cave[
                     (cave.IndexOf(' ', cave.IndexOf("valve")) + 1)..].Split(", ");

                if (!_caves.ContainsKey(caveName)) _caves.Add(caveName, new Cave(flowRate));

                foreach (string link in tunnels) LinkCave(caveName, link);
            }

            Simplify();
            FullLinkage();
        }

        public void ToggleVerbose() => _verbose = !_verbose;
        public List<string> AllCaves() => _caves.OrderBy(x => x.Value.FlowRate).Select(x => x.Key).ToList();
        private void FullLinkage()
        {
            foreach (string node in _caves.Keys)
            {
                Dijkstra(node, out Dictionary<string, (string prev, int dist)> path);

                foreach (string exit in path.Keys.Except(_caves[node].ExitList()))
                {
                    if (node == exit) continue;
                    _caves[node].AddExit(exit, path[exit].dist);
                }
            }
        }
        private void LinkCave(string caveOne, string caveTwo, int distance = 1)
        {
            if (caveOne == caveTwo) return; //never link to ourselves.

            if (_caves.TryGetValue(caveOne, out Cave? caveA)) caveA.AddExit(caveTwo, distance);
            if (_caves.TryGetValue(caveTwo, out Cave? caveB)) caveB.AddExit(caveOne, distance);
        }
        private void RemoveLink(string caveOne, string caveTwo)
        {
            if (_caves.TryGetValue(caveOne, out Cave? caveA)) caveA.RemoveExit(caveTwo);
            if (_caves.TryGetValue(caveTwo, out Cave? caveB)) caveB.RemoveExit(caveOne);
        }
        private void RemoveCave(string name)
        {
            if (!_caves.TryGetValue(name, out Cave? value)) return;

            foreach (string cave in value.ExitList())
            {
                RemoveLink(name, cave);
            }
            _caves.Remove(name);
        }
        private void Simplify()
        {
            // Remove all non-root nodes with a flow rate of 0.
            // We don't care about them, they're just extra distance on the graph.
            Stack<string> removeList = new();

            foreach (string s in _caves.Keys)
            {
                if (s == rootNode) continue;
                if (_caves[s].FlowRate == 0)
                {
                    //crosslink everything in the exit list
                    foreach (string e1 in _caves[s].ExitList())
                    {
                        foreach (string e2 in _caves[s].ExitList())
                        {
                            if (e1 == e2) continue; // don't link to ourselves
                            LinkCave(e1, e2, _caves[s].ExitDistance(e1) + _caves[s].ExitDistance(e2));
                        }
                    }
                    removeList.Push(s);
                }
            }
            while (removeList.Count > 0)
            {
                RemoveCave(removeList.Pop());
            }
        }
        public void PrintLayout()
        {
            foreach (string s in _caves.Keys)
            {
                Console.WriteLine($"Cave: {s} Flow: {_caves[s].FlowRate}");
                foreach (string s2 in _caves[s].ExitList())
                {
                    Console.WriteLine($"--{s2} {_caves[s].ExitDistance(s2)}");
                }
            }
            Console.WriteLine(" ");
        }
        private void Dijkstra(string startNode, out Dictionary<string, (string prev, int dist)> paths)  
        // Return a list of node/dist pairs we can feed into a cave exit dictionary
        {
            // dist is from start to key, coming from parent.
            // dist[source] ← 0                           // Initialization/
            paths = new();
            
            // create vertex queue
            List<string> searchQueue = new();

            // Load our queue and intialzie all of our result values. 
            foreach (KeyValuePair<string, Cave> kvpCave in _caves)
            {
                paths.Add(kvpCave.Key, (string.Empty, int.MaxValue));
                searchQueue.Add(kvpCave.Key);
            }
            paths[startNode] = (string.Empty, 0);

            // while Q is not empty:
            // u ← vertex in Q with min dist[u]
            // remove u from Q
            while (searchQueue.Count > 0)
            {
                string u = paths.OrderBy(d => d.Value.dist).ToList().Select(x => x.Key).Where(searchQueue.Contains).First();
                searchQueue.Remove(u);

                // for each neighbor v of u still in Q:
                foreach (string v in _caves[u].ExitList().Intersect(searchQueue))
                {
                    int alt = paths[u].dist + _caves[u].ExitDistance(v);
                    
                    if (alt < paths[v].dist) paths[v] = (u, alt);
                }
            }
        }
        public int PathCost(List<string> nodes, int startTime, out int timeRemaining)
        {
            const int ValveOpenTime = 1; //figure out how to best build this into our pathing.
            int returnValue = 0;

            timeRemaining = startTime; 

            for (int i = 0; i < nodes.Count - 1; i++)
            {
                timeRemaining -= _caves[nodes[i]].ExitDistance(nodes[i + 1]) + ValveOpenTime;
                if (timeRemaining <= 0) break;
                returnValue += _caves[nodes[i + 1]].FlowFromTime(timeRemaining);
            }

            return -returnValue;
        }
        public static string ListToString(List<string> s) => s.Aggregate((x, y) => x + y);
        public List<string> BnBSolve(string startNode, List<string> searchSpace, int totalTime, int totalDepth = -1) 
        {
            //the problem is a set of nodes that we have to visit within the scope of timeRemaining. 

            // for each node. 
            // - generate a list of subnodes.  Priortize by distance( lowest) and flow rate (highest) 
            // - AA -> DD (1 step, flow 20) comes before AA -> JJ (2 steps, flow 21) 
            // enqueue a set of proposed solutions for each node off of our current node, that does not include a node already visited.
            // and then repeat... 

            int current_optimum_pressure = int.MaxValue;
            List<string> current_optimum_path = new()
            {
                startNode
            };

            PriorityQueue<List<string>, int> candidate_queue = new();

            candidate_queue.Enqueue(current_optimum_path, current_optimum_pressure);

            while (candidate_queue.Count > 0)
            { 
                List<string> candidate_solution = candidate_queue.Dequeue();
                List<string> nextSteps = searchSpace.Except(candidate_solution).ToList();

                if (nextSteps.Count < 1) // no more caves to explore, we have potential solution. 
                {
                    // test for potential problem solution. 
                    int testPressure = PathCost(candidate_solution, totalTime, out _);
                    if (testPressure < current_optimum_pressure)
                    {
                        current_optimum_path = candidate_solution;
                        current_optimum_pressure = testPressure;
                    }
                    if (_verbose) Console.WriteLine($"New potential solution : {ListToString(current_optimum_path)} {current_optimum_pressure}");
                }
                else
                {
                    foreach(string next in nextSteps)
                    {
                        List<string> newStep = candidate_solution.Append(next).ToList();

                        int newPressure = PathCost(newStep, totalTime, out int timeRemaining);

                        if (_verbose) Console.Write($"considering {ListToString(newStep)} : {newPressure} >= {current_optimum_pressure} TR: {timeRemaining}");
                        if (newPressure <= current_optimum_pressure && timeRemaining > 0)
                        {
                            if (_verbose) Console.Write(" *ADDED");
                            candidate_queue.Enqueue(newStep, newPressure); //prefer stuff with more pressure. 
                        }
                        if (_verbose) Console.WriteLine();
                    }
                }
            }

            return current_optimum_path;
        }
    }
}