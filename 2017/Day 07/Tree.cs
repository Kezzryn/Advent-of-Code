using Microsoft.Win32;

namespace AoC_2017_Day_07
{
    internal class Node
    {
        public readonly string ID = string.Empty;
        public string ParentID { get; set; }
        public int Weight { get; set; }
        public int SumChildren { get; set; }

        public readonly HashSet<string> Children = [];

        public Node(string id, int weight = 0)
        {
            ID = id;
            ParentID = string.Empty;
            Weight = weight;
            SumChildren = -1;
        }

        public Node(string id, string parentID, int weight = 0) 
        { 
            ID = id;
            ParentID = parentID;
            Weight = weight;
            SumChildren = -1;
        }

        public void AddChild(string childID)
        {
            Children.Add(childID);
        }

    }

    internal class Tree
    {
        private readonly Dictionary<string, Node> _theTree = [];

        public string RootNode { get; set; }

        public Tree(string[] puzzleInput)
        {
            foreach (string node in puzzleInput)
            {
                string[] supportNode = node.Split("->");

                string[] parent = [.. supportNode[0].Split(' ', StringSplitOptions.TrimEntries)];

                AddNode(parent[0], int.Parse(parent[1].Trim("()".ToCharArray())));

                if (supportNode.GetUpperBound(0) > 0)
                {
                    string[] children = supportNode[1].Split(',', StringSplitOptions.TrimEntries);

                    foreach (string child in children)
                    {
                        AddNode(child);
                        LinkNode(parent[0], child);
                    }
                }
            }

            RootNode = FindRoot();
            SumChildren(RootNode);
        }

        public Node GetNode(string id) => _theTree[id];

        public string FindRoot()
        {
            Node rootNode = _theTree.ElementAt(0).Value;

            while (rootNode.ParentID != String.Empty)
            {
                if (_theTree[rootNode.ID].ParentID == String.Empty) break;
                rootNode = _theTree[rootNode.ParentID];
            }
            return rootNode.ID;
        }

        public void AddNode(string id, int weight = -1)
        {
            if (!_theTree.TryAdd(id, new(id, weight)))
            {
                if (weight != -1 && _theTree.TryGetValue(id, out Node? value))
                {
                    value.Weight = weight;
                }
            }
        }

        public void LinkNode(string parentID, string childID)
        {
            if (_theTree.TryGetValue(parentID, out Node? parentNode))
            {
                parentNode.AddChild(childID);
            }
            else
            {
                AddNode(parentID);
                _theTree[parentID].AddChild(childID);
            }

            if (_theTree.TryGetValue(childID, out Node? childNode))
            {
                childNode.ParentID = parentID;
            }
            else
            {
                AddNode(childID);
                _theTree[childID].ParentID = parentID;
            }
        }
        private int SumChildren(string id)
        {
            int rv;
            if (_theTree[id].SumChildren == -1)
            {
                rv = _theTree[id].Weight;
                foreach (string childID in _theTree[id].Children)
                {
                    rv += SumChildren(childID);
                }
                _theTree[id].SumChildren = rv;
            }

            return _theTree[id].SumChildren;
        }

        public HashSet<string> GetChildren(string parentid)
        {
            return GetNode(parentid).Children;
        }
    }
}
