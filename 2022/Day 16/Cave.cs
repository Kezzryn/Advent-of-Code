namespace AoC_2022_Day_16
{
    class Cave
    {
        //Key = caveName, Value = travel cost.
        private readonly Dictionary<string, int> _exits = new();

        private readonly int _flowRate = 0;
        private bool _valveOpen = false;

        public int FlowRate
        {
            get { return _flowRate; }
        }
        public bool IsOpen
        {
            get { return _valveOpen; }
        }
        public Cave(int flowRate)
        {
            _exits = new Dictionary<string, int>();
            _flowRate = flowRate;
        }
        public void ToggleValve() => _valveOpen = !_valveOpen;
        public void AddExit(string exitName, int distance) => _exits.TryAdd(exitName, distance);
        public void RemoveExit(string exitName) => _exits.Remove(exitName);
        public List<string> ExitList() => _exits.Select(x => x.Key).ToList();
        public int ExitDistance(string exitName) => _exits.TryGetValue(exitName, out int distance) ? distance : -1;
    }
}
