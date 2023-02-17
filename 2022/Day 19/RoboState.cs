namespace AoC_2022_Day_19
{
    internal record RoboState
    {
        // Array size needs to match the number of entries in RockTypes enum, however, we don't track Geode bots. 
        private readonly int[] _stockPile;
        private readonly int[] _numBots;

        public RoboState(int time)
        {
            _stockPile = new int[4];
            _numBots = new int[3]; //We don't track Geode bots. 

            OreStock(ResourceTypes.Ore, 0);
            OreStock(ResourceTypes.Clay, 0);
            OreStock(ResourceTypes.Obsidian, 0);
            OreStock(ResourceTypes.Geode, 0);

            BotStock(ResourceTypes.Ore, 1);
            BotStock(ResourceTypes.Clay, 0);
            BotStock(ResourceTypes.Obsidian, 0);

            TimeRemaining = time;
        }

        public RoboState(RoboState other)
        {
            _stockPile = new int[4];
            _numBots = new int[3];  //We don't track Geode bots. 

            OreStock(ResourceTypes.Ore, other.OreStock(ResourceTypes.Ore));
            OreStock(ResourceTypes.Clay, other.OreStock(ResourceTypes.Clay));
            OreStock(ResourceTypes.Obsidian, other.OreStock(ResourceTypes.Obsidian));
            OreStock(ResourceTypes.Geode, other.OreStock(ResourceTypes.Geode));

            BotStock(ResourceTypes.Ore, other.BotStock(ResourceTypes.Ore));
            BotStock(ResourceTypes.Clay, other.BotStock(ResourceTypes.Clay));
            BotStock(ResourceTypes.Obsidian, other.BotStock(ResourceTypes.Obsidian));

            TimeRemaining = other.TimeRemaining;
        }

        public override string ToString()
        {
            return $"Time: {TimeRemaining} O:{_stockPile[0]} C:{_stockPile[1]} Ob:{_stockPile[2]} G:{_stockPile[3]} BO:{_numBots[0]} BC:{_numBots[1]} BOb:{_numBots[2]}  ";
        }

        //Theoretical Max Geodes if we build a geode bot each tick for the remaining time. 
        public int MaxGeodes() => OreStock(ResourceTypes.Geode) + (TimeRemaining * (TimeRemaining - 1));
        public int CurrentGeodes() => OreStock(ResourceTypes.Geode);

        public int TimeRemaining { get; set; }

        // Get/Set Ore Stock numbers. 
        private int OreStock(ResourceTypes type, int? value = null)
        {
            if (value is not null) _stockPile[(int)type] += (int)value;
            return _stockPile[(int)type];
        }

        // Get/Set Bot Stock numbers. 
        private int BotStock(ResourceTypes type, int? value = null)
        {
            if (value is not null) _numBots[(int)type] += (int)value;
            return _numBots[(int)type];
        }

        // Decision tree, to test if we can or should build a bot. 
        public bool BuildBot(ResourceTypes type, int[] maxOreNeeded)
        {
            return type switch
            {
                ResourceTypes.Geode => BotStock(ResourceTypes.Obsidian) > 0,
                ResourceTypes.Obsidian => BotStock(ResourceTypes.Clay) > 0 && OreStock(type) + (BotStock(type) * TimeRemaining) < (maxOreNeeded[(int)type] * TimeRemaining),
                _ => OreStock(type) + (BotStock(type) * TimeRemaining) < (maxOreNeeded[(int)type] * TimeRemaining)
            }; 
        }

        // How long until we can build this bot? 
        private int TimeToOre(int[] newBotCost)
        {
            int returnValue = 0;
            for(int i = newBotCost.GetLowerBound(0); i <= newBotCost.GetUpperBound(0); i++)
            {
                if (newBotCost[i] > 0)
                {
                    if (_numBots[i] == 0) return int.MaxValue;  // we don't have one of these bots. 

                    // To round UP with integer math, use the forumula: (x + y - 1) / y
                    // the math gives us the tick to gather the ore, but we add one, since we can't build till the next tick. 
                    returnValue = int.Max(returnValue, (int.Max(newBotCost[i] - _stockPile[i] + (_numBots[i] - 1), 0) / _numBots[i]) + 1);
                }
            }

            return returnValue;
        }

        public void AdvanceTime(ResourceTypes newBotType, int[] newBotCost)
        {
            // We've gotten an order to build a newBotType with a newBotCost. Move forward until we can build it, and adjust ourstockpiles. 
            int time = TimeToOre(newBotCost);

            OreStock(ResourceTypes.Ore,      (time * BotStock(ResourceTypes.Ore))       - newBotCost[(int)ResourceTypes.Ore]);
            OreStock(ResourceTypes.Clay,     (time * BotStock(ResourceTypes.Clay))      - newBotCost[(int)ResourceTypes.Clay]);
            OreStock(ResourceTypes.Obsidian, (time * BotStock(ResourceTypes.Obsidian))  - newBotCost[(int)ResourceTypes.Obsidian]);

            TimeRemaining -= time;

            // For Geodes we precalculate the total output
            if (newBotType == ResourceTypes.Geode) 
                OreStock(ResourceTypes.Geode, TimeRemaining); 
            else 
                BotStock(newBotType, 1);
        }
    }
}
