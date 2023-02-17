using System.Text.RegularExpressions;

namespace AoC_2022_Day_19
{
    internal enum ResourceTypes
    {
        // NB: the order is used as a priorty in our search.  Geodes are tested for first, then Obsidian, and so on. 
        Ore = 0,
        Clay = 1,
        Obsidian = 2,
        Geode = 3
    }

    internal class RoboBlueprint
    {
        private readonly int _ID = -1;
        private readonly Dictionary<ResourceTypes, int[]> _robots = new();
        private readonly int[] _maxOreTypeNeeded;

        private int _maxGeodes = 0;

        public RoboBlueprint(string puzzleData, int time)
        {
            //there has got to be a better way to do this than the Skip/Take combo. 
            int[] numbers = Regex.Split(puzzleData, @"\D+").Skip(1).Take(7).Select(int.Parse).ToArray();

            _ID = numbers[0];

            _robots.Add(ResourceTypes.Ore,      new[] {numbers[1], 0,           0 });
            _robots.Add(ResourceTypes.Clay,     new[] {numbers[2], 0,           0 });
            _robots.Add(ResourceTypes.Obsidian, new[] {numbers[3], numbers[4],  0 });
            _robots.Add(ResourceTypes.Geode,    new[] {numbers[5], 0,           numbers[6] });

            _maxOreTypeNeeded = new[] {
                new[] { numbers[2], numbers[3], numbers[5] }.Max(),
                numbers[4],
                numbers[6]
                };

            DepthFirstSearch(new RoboState(time));
        }

        public int GetQualityLevel() => _ID * _maxGeodes;
        
        public int GetNumGeodes() => _maxGeodes;

        private void DepthFirstSearch(RoboState currentState)
        {
            // Can we theoretically beat the maxGeodes?
            if (currentState.MaxGeodes() <= _maxGeodes) return;

            _maxGeodes = int.Max(_maxGeodes, currentState.CurrentGeodes());

            // Work down the enumerated resources types in decending order. If we can/should BuildBot() then we create a new RoboState and make that our new depth first search node. 
            foreach (ResourceTypes resourceType in Enum.GetValues(typeof(ResourceTypes)).Cast<int>().ToArray().Reverse().Select(v => (ResourceTypes)v))
            {
                if (currentState.BuildBot(resourceType, _maxOreTypeNeeded))
                {
                    RoboState nextState = new(currentState);
                    nextState.AdvanceTime(resourceType, _robots[resourceType]);

                    if (nextState.TimeRemaining > 0) DepthFirstSearch(nextState); // don't call for out of time items. 
                }
                if (currentState.MaxGeodes() <= _maxGeodes) return; // we may have updated our global max count, so retest as an early cut. 
            }
        }
    }
}
