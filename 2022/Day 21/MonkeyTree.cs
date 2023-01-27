namespace AoC_2022_Day_21
{
    internal record MTNode
    {
        public long? Value = null;
        public string? Left = null;
        public string? Right = null;
        public string? Operand = null;
        public string? ParentKey = null;

        public MTNode()
        {

        }
        public MTNode(long value)
        {
            Value = value;
        }

        public MTNode(string value)
        {
            var temp = value.Split(" ");

            Left = temp[0];
            Operand = temp[1];
            Right = temp[2];
        }

        public void UpdateNode(MTNode newNode, bool overwrite = false)
        {
            if (newNode.Value != null || overwrite) Value = newNode.Value;
            if (newNode.Left != null || overwrite) Left = newNode.Left;
            if (newNode.Right != null || overwrite) Right = newNode.Right;
            if (newNode.Operand != null || overwrite) Operand = newNode.Operand;
            if (newNode.ParentKey != null || overwrite) ParentKey = newNode.ParentKey;
        }

        public void UpdateNodeValue(long? value) => Value = value;

        public override string ToString()
        {
            return $"{Value} = {Left} {Operand} {Right}  parent {ParentKey}";
        }
    }

    internal class MonkeyTree
    {
        private readonly Dictionary<string, MTNode> _monkeys = new();

        bool _verbose = false;

        public MonkeyTree(string[] puzzleData, bool verbose = false)
        {
            _verbose = verbose;

            foreach (string line in puzzleData)
            {
                string[] keyValue = line.Split(": ");

                MTNode newMonkeyNode = long.TryParse(keyValue[1], out long value) ? new MTNode(value) : new MTNode(keyValue[1]);

                if (_monkeys.TryGetValue(keyValue[0], out MTNode? existingMonkeyNode))
                {
                    if (_verbose) Console.WriteLine($"Updating existing node {keyValue[0]} with {newMonkeyNode}");
                    existingMonkeyNode.UpdateNode(newMonkeyNode);
                    if (_verbose) Console.WriteLine($"After udate: {existingMonkeyNode}");
                }
                else
                {
                    if (_verbose) Console.WriteLine($"Adding node {keyValue[0]} with {newMonkeyNode}");
                    _monkeys.Add(keyValue[0], newMonkeyNode);
                }

                void UpdateChild(string? childKey)
                {
                    if (childKey is null) return;
                    if (_verbose) Console.WriteLine($"Update child node {childKey} with new parent {keyValue[0]}");

                    if (_monkeys.TryGetValue(childKey, out MTNode? existingMonkeyParent))
                    {
                        if (_verbose) Console.WriteLine($"node {childKey} found");
                        existingMonkeyParent.ParentKey = keyValue[0];
                    }
                    else
                    {
                        if (_verbose) Console.WriteLine($"New node {childKey} created");
                        MTNode newMonkyChild = new()
                        {
                            ParentKey = keyValue[0]
                        };
                        _monkeys.Add(childKey, newMonkyChild);
                    }
                }

                //check for and add a new node with the key of the operand, and the parent of the current key. 
                //if it exists, then just update the parent value then do the same with the right. 
                UpdateChild(newMonkeyNode.Left);
                UpdateChild(newMonkeyNode.Right);

            }
        }

        public void ToggleVerbose() => _verbose = !_verbose;

        public void UpdateMonkey(string key, MTNode newMonkeyData, bool overwrite = false)
        {
            if (_monkeys.TryGetValue(key, out MTNode? monkey)) monkey.UpdateNode(newMonkeyData, overwrite);
        }

        public void UpdateMonkeyValue(string key, long? value)
        {
            if (_monkeys.TryGetValue(key, out MTNode? monkey)) monkey.UpdateNodeValue(value);
        }

        public long GetValue(string? key)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));

            //this walks down the chain, doing the operations at each node on each set of children values. 
            if (_monkeys.TryGetValue(key, out MTNode? monkey))
            {
                return monkey.Value ?? (monkey.Operand) switch
                {
                    "+" => GetValue(monkey.Left) + GetValue(monkey.Right),
                    "-" => GetValue(monkey.Left) - GetValue(monkey.Right),
                    "*" => GetValue(monkey.Left) * GetValue(monkey.Right),
                    "/" => GetValue(monkey.Left) / GetValue(monkey.Right),
                    _ => 0
                };
            }
            else
            {
                throw new Exception($"No monkey at key {key}");
            }
        }

        public long GetNodeValue(string? monkeyKey)
        {
            if (monkeyKey is null) throw new ArgumentNullException(nameof(monkeyKey));

            if (!_monkeys.TryGetValue(monkeyKey, out MTNode? monkey)) throw new Exception($"Monkey not found {monkeyKey}");

            //note that the root node will have a null parent, so when we get there, just return. 
            if (monkey.ParentKey is null) return 0;

            if (!_monkeys.TryGetValue(monkey.ParentKey, out MTNode? monkeyParent)) throw new Exception($"Monkey not found {monkey.ParentKey}");

            long rv2 = (monkeyParent.Left == monkeyKey) ? GetValue(monkeyParent.Right) : GetValue(monkeyParent.Left);

            return monkeyParent.Operand switch
            {
                "+" => GetNodeValue(monkey.ParentKey) - rv2,
                "-" => (monkeyParent.Left == monkeyKey) ? GetNodeValue(monkey.ParentKey) + rv2 : rv2 - GetNodeValue(monkey.ParentKey),
                "*" => GetNodeValue(monkey.ParentKey) / rv2,
                "/" => (monkeyParent.Left == monkeyKey) ? GetNodeValue(monkey.ParentKey) * rv2 : rv2 / GetNodeValue(monkey.ParentKey),
                "=" => rv2,
                _ => throw new NotImplementedException($"Unknown operand. {monkey.Operand}"),
            };
        }
    }
}
