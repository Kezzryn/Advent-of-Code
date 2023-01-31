using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AoC_2022_Day_16
{
    class CaveComplex
    {
        private const string rootNode = "AA";
        private readonly Dictionary<string, Cave> _caves = new();
        
        public CaveComplex(string[] caveData)
        {
            _caves = new Dictionary<string, Cave>();

            foreach (string cave in caveData) AddCave(cave);

            Simplify();
          //  FullLinkage();
        }

        public void AddCave(string caveName, int flowRate)
        {
            if (!_caves.ContainsKey(caveName)) _caves.Add(caveName, new Cave(flowRate));
        }

        public string AddCave(string caveData)
        {
            //Valve VS has flow rate=0; tunnels lead to valves FF, GC
            //Valve OI has flow rate=20; tunnel leads to valve SY
            //012345678901234567890123456789012345678901234567890123456789

            string caveName = caveData[6..8];
            int flowRate = int.Parse(caveData[(caveData.IndexOf('=')+1)..caveData.IndexOf(';')]);

            string[] tunnels = caveData[
                (caveData.IndexOf(' ', caveData.IndexOf("valve"))+1)..].Split(", ");

            AddCave(caveName, flowRate);

            foreach (string link in tunnels) LinkCave(caveName, link);
            
            return caveName;
        }

        public void LinkCave(string caveOne, string caveTwo, int distance = 1)
        {
            if (caveOne == caveTwo) return; //never link to ourselves.

            if (_caves.TryGetValue(caveOne, out Cave? caveA)) caveA.AddExit(caveTwo, distance);
            if (_caves.TryGetValue(caveTwo, out Cave? caveB)) caveB.AddExit(caveOne, distance);
        }

        public void RemoveLink(string caveOne, string caveTwo)
        {
            if (_caves.TryGetValue(caveOne, out Cave? caveA)) caveA.RemoveExit(caveTwo);
            if (_caves.TryGetValue(caveTwo, out Cave? caveB)) caveB.RemoveExit(caveOne);
        }

        public void RemoveCave(string name)
        {
            if (!_caves.TryGetValue(name, out Cave? value)) return;

            foreach (string cave in value.ExitList())
            {
                RemoveLink(name, cave);
            }
            _caves.Remove(name);
        }

        public void Simplify()
        {
            // Remove all non-root nodes with a flow rate of 0 We don't care about them, they're just extra distance on the graph.
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

        private void FullLinkage()
        {
            Dictionary<string, string> prev;
            Dictionary<string, int> dist;

            foreach (string node in _caves.Keys)
            {
    //            Dijkstra(node, out prev, out dist);
            }

    
            /*
             * Compute dist(u), the shortest-path distance from root v to vertex u in G using Dijkstra's algorithm or Bellman–Ford algorithm.
             * 
             * For all non-root vertices u, we can assign to u a parent vertex pu such that pu is connected to u, 
             * and that dist(pu) + edge_dist(pu,u) = dist(u).
             * 
             * In case multiple choices for pu exist, choose pu for which there exists a shortest path from v to pu with as few edges as possible; 
             * this tie-breaking rule is needed to prevent loops when there exist zero-length cycles.
             * Construct the shortest-path tree using the edges between each node and its parent.
             */
        }

        public void Dijkstra(string startNode, out Dictionary<string, string> prev, out Dictionary<string, int> dist)  
        // Return a list of node/dist pairs we can feed into a cave exit dictionary
        {
            // dist is from start to key, coming from parent.
            // dist[source] ← 0                           // Initialization/
            prev = new();
            dist = new();

            // create vertex queue
            List<string> searchQueue = new();

            // Load our queue and intialzie all of our result values. 
            foreach (KeyValuePair<string, Cave> kvpCave in _caves)
            {
                dist.Add(kvpCave.Key, int.MaxValue);
                prev.Add(kvpCave.Key, string.Empty);

                searchQueue.Add(kvpCave.Key);
            }

            // while Q is not empty:
            // u ← vertex in Q with min dist[u]
            // remove u from Q
            while (searchQueue.Count > 0)
            {
                string u = dist.OrderBy(d => d.Value).ToList().Select(x => x.Key).First();
                searchQueue.Remove(u);

                // for each neighbor v of u still in Q:
                foreach (string v in _caves[u].ExitList().Intersect(searchQueue))
                {
                    int alt = dist[u] + _caves[u].ExitDistance(v);
                    if (alt < dist[v])
                    {
                        dist[v] = alt;
                        prev[v] = u;
                    }
                }
            }
        }

        // public int DontBlowUp(int timeRemaing)
        // {

        //    return 0;
        // }

    }
}