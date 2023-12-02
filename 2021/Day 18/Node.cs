using System.Diagnostics;

namespace AoC_2021_Day_18
{
    internal class Node
    {
        //idea to borrow:  set depth as parent depth + 1
        //move is splittable and explodable tests to the node itself. 
        //Split can be done in-node, splitting on the "midpoint" recursivly. (f'kin, brilliant)
        //Explode can (probably) be done in-node
        //values as node "leafs?" (could simplify the entire Left vs Right mess... ) 
        // nodes that are leafs have no children, only a value.  This pushes the level down one 

        private const bool GO_DOWN = false;
        private const bool GO_UP = true;

        public int? Value { get; set; }

        public Node? ParentNode = null;
        public Node? LeftChild = null;
        public Node? RightChild = null;

        public int Depth
        {
            get
            {
                return ParentNode is null ? 1 : ParentNode.Depth + 1;
            }
        }

        public bool IsLeaf
        {
            get
            {
                return Value is not null;
            }
        }

        public Node(Node? parent, int value)
        {
            ParentNode = parent;
            Value = value;
        }

        public Node(string snailFishNumber, Node? parent = null)
        {
            ParentNode = parent;
            if (snailFishNumber.Contains(','))
            {
                snailFishNumber = snailFishNumber[1..^1]; // strip outer brackets.
                var mid_point = FindMiddle(snailFishNumber);
                LeftChild = new(snailFishNumber[..mid_point], this);
                RightChild = new(snailFishNumber[(mid_point + 1)..], this);
            }
            else
            {
                Value = int.Parse(snailFishNumber);
            }
        }

        public static Node Add(Node left, Node right)
        {
            Node newNode = new($"[{left},{right}]");
            newNode.ReduceSnailfish();
            return newNode;
        }

        public static Node Add(string left, string right)
        {
            return Add(new Node(left), new Node(right));
        }

        private static bool ExplodeLeft(Node? cursor, Node prevNode, int value, bool goUpOrDown = true)
        {
            if (cursor is null) return false;

            if (goUpOrDown == GO_UP)
            {
                if (cursor.LeftChild != prevNode)
                {
                    return ExplodeLeft(cursor.LeftChild, cursor, value, GO_DOWN);
                }

                if (cursor.ParentNode is not null)
                {
                    return ExplodeLeft(cursor.ParentNode, cursor, value, GO_UP);
                }
            }

            if (goUpOrDown == GO_DOWN)
            {
                // once we're going down, we're deep diving for the bottom. No parent, no forks.
                if (cursor.Value is not null)
                {
                    cursor.Value += value;
                    return true;
                }

                return ExplodeLeft(cursor.RightChild, cursor, value, GO_DOWN);
            }

            return false; // should never get here
        }

        private static bool ExplodeRight(Node? cursor, Node prevNode, int value, bool goUpOrDown = true)
        {
           // if (cursor is null) return false;

            if (goUpOrDown == GO_UP)
            {
                if (cursor.RightChild != prevNode)
                {
                    return ExplodeRight(cursor.RightChild, cursor, value, GO_DOWN);
                }

                if (cursor.ParentNode is not null)
                {
                    return ExplodeRight(cursor.ParentNode, cursor, value, GO_UP);
                }
            }

            if (goUpOrDown == GO_DOWN)
            {
                // once we're going down, we're deep diving for the bottom. No parent, no forks.
                if (cursor.Value is not null)
                {
                    cursor.Value += value;
                    return true;
                }

                return ExplodeRight(cursor.LeftChild, cursor, value, GO_DOWN);
            }

            return false; // should never get here
        }

        private bool FindExplodeNode()
        {
            //are we a node with a depth of 4 and two leaf-children?
            if (Depth > 4 && !IsLeaf)
            {
                if(LeftChild is not null && RightChild is not null)
                {
                    if(LeftChild.IsLeaf &&  RightChild.IsLeaf)
                    {
                        ExplodeLeft(ParentNode, this, (int)LeftChild.Value!, GO_UP);
                        ExplodeRight(ParentNode, this, (int)RightChild.Value!, GO_UP);

                        LeftChild = null;
                        RightChild = null;
                        Value = 0;
                        return true;
                    }
                }
            }

            if (LeftChild is not null)
            {
                bool result = LeftChild.FindExplodeNode();
                if (result) return result;
            }

            if (RightChild is not null)
            {
                bool result = RightChild.FindExplodeNode();
                if (result) return result;
            }

            return false;
        }

        public int FindMagnitude()
        {
            if (Value is not null)  return (int)Value;

            int leftValue = LeftChild is not null ? LeftChild.FindMagnitude() : -1;
            int rightValue = RightChild is not null ? RightChild.FindMagnitude() : -1;

            return (leftValue * 3) + (rightValue * 2);
        }

        private static int FindMiddle(string inputPacket)
        {
            int currentDepth = 0;
            int lowestDepth = int.MaxValue;
            int midPoint = -1;
            for (int i = 0; i < inputPacket.Length; i++)
            {
                if (Char.IsDigit(inputPacket[i])) continue;

                if (inputPacket[i] == '[')
                    currentDepth++;
                else if (inputPacket[i] == ']')
                    currentDepth--;
                else if (inputPacket[i] == ',')
                {
                    if (currentDepth < lowestDepth)
                    {
                        midPoint = i;
                        lowestDepth = currentDepth;
                    }
                }
            }

            return midPoint;
        }

        private bool FindSplitNode()
        {
            if (LeftChild is not null)
            {
                bool result = LeftChild.FindSplitNode();
                if (result) return result;
            }

            if (IsLeaf && Value >= 10)
            {
                int newLeft = (int)(Value / 2);
                int newRight = (int)((Value + 1) / 2);
                LeftChild = new Node(this, newLeft);
                RightChild = new Node(this, newRight);
                Value = null;

                return true;
            }

            if (RightChild is not null)
            {
                bool result = RightChild.FindSplitNode();
                if (result) return result;
            }

            return false;
        }

        public void ReduceSnailfish()
        {
            // Check entire thing for explosions.
            // If there are no explosions THEN check entire thing for splits

            bool isDone = false;
            do
            {
                if (FindExplodeNode()) continue;
                if (FindSplitNode()) continue;

                isDone = true;

            } while (!isDone);
        }

        public override string ToString()
        {
            if (Value is not null)
            { 
                return Value.ToString()!;
            }
            else
            {
                return $"[{LeftChild},{RightChild}]";
            }
        }
    }
}
