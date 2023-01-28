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
            /*
             * 
Compute dist(u), the shortest-path distance from root v to vertex u in G using Dijkstra's algorithm or Bellman–Ford algorithm.

For all non-root vertices u, we can assign to u a parent vertex pu such that pu is connected to u, and that dist(pu) + edge_dist(pu,u) = dist(u). In case multiple choices for pu exist, choose pu for which there exists a shortest path from v to pu with as few edges as possible; this tie-breaking rule is needed to prevent loops when there exist zero-length cycles.

Construct the shortest-path tree using the edges between each node and its parent.
             */


        }

        private void Dijkstra(string startNode, string[] endNode) //return what? list of distnaces for a start of X?
        {
            // dist[source] ← 0                           // Initialization/
            // create vertex priority queue Q

            PriorityQueue<string, int> searchQueue = new(); //we enque based on fScore + h, the distance travelled, plus taxi distance guess to destination.
//            HashSet<string> inSearchQueue = new(); //we add this because we don't have a way to query the queue to see if a specific item is in it.

            //dist is from start to Key, coming from parent.
            Dictionary<string, (int dist, string? parent)> stepCounter = new();

            foreach (KeyValuePair<string, Cave> kvp in _caves)
            {
                //if v ≠ source
                //   dist[v] ← INFINITY                 // Unknown distance from source to v
                //   prev[v] ← UNDEFINED                // Predecessor of v
                //Q.add_with_priority(v, dist[v])
                if (kvp.Key != startNode)
                {

                }
            }
          
            while (searchQueue.Count > 0)
            {
                // u ← Q.extract_min()                    // Remove and return best vertex

                //  for each neighbor v of u:              // Go through all v neighbors of u
                    // alt ← dist[u] + Graph.Edges(u, v)
                    // if alt < dist[v]:
                    // dist[v] ← alt
                    // prev[v] ← u
                    // Q.decrease_priority(v, alt)
            }


        //return dist, prev

        }

        // public int DontBlowUp(int timeRemaing)
        // {

        //    return 0;
        // }

    }
}