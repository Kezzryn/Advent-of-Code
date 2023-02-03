namespace AoC_2022_Day_16
{
    //This is not used in the Day 16 solution and is here for historical reasons. 

    class Cave
    {
        //Key = caveName, Value = travel cost.
        private readonly Dictionary<string, int> _exits = new();
        private readonly int _flowRate = 0;

        public int FlowRate
        {
            get { return _flowRate; }
        }

        public Cave(int flowRate)
        {
            _exits = new Dictionary<string, int>();
            _flowRate = flowRate;
        }
        public void AddExit(string exitName, int distance) => _exits.TryAdd(exitName, distance);
        public void RemoveExit(string exitName) => _exits.Remove(exitName);
        public List<string> ExitList() => _exits.OrderBy(d => d.Value).ToList().Select(x => x.Key).ToList();  // returns "closer" exits first.
        public int ExitDistance(string exitName) => _exits.TryGetValue(exitName, out int distance) ? distance : -1;
        public int FlowFromTime(int time) => _flowRate * time;
    }
}
