namespace AoC_2018_Day_08
{
    internal class Node
    {
        public int Value { get; set; }
        public List<int> Children = new();
        public List<int> MetaData = new();

        public Node()
        {
            Value = -1;
        }
    }
}
