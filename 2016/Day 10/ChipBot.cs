namespace AoC_2016_Day_10
{
    internal class ChipBot
    {
        public int GiveHighTo { get; } = 0;
        public bool HighIsOutput { get; } = false;
        public int GiveLowTo { get; } = 0;
        public bool LowIsOutput { get; } = false;
        public List<int> Chips { get; set; }

        public ChipBot()
        {
            Chips = new();
        }
        public ChipBot(int chip)
        {
            Chips = new()
            {
                chip
            };
        }
        public ChipBot(int givehigh, bool isHighOutput, int givelow, bool isLowOutput)
        {
            Chips = new();

            GiveHighTo = givehigh;
            HighIsOutput = isHighOutput;

            GiveLowTo = givelow;
            LowIsOutput = isLowOutput;
        }
    }
}
