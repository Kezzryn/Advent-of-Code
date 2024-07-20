namespace AoC_2018_Day_08
{
    internal record Node()
    {
        public int NodeValue { get; set; }
        public readonly List<int> Children = [];
        public readonly List<int> MetaData = [];
    }
}