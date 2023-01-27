namespace AoC_2022_Day_11
{
    internal class MonkeyTroupe
    {
        private Dictionary<int, Monkey> _monkeys = new();
        private readonly string _worryReductionType;

        public long WorryFactor { get; set; }
        public long WorryReductionType { get; }

        public MonkeyTroupe(string inputFileName, string worryReductionType = "PART1")
        {
            string inputData = File.ReadAllText(inputFileName);

            foreach (string line in inputData.Split("Monkey ", StringSplitOptions.RemoveEmptyEntries))
            {
                string[] parts = line.Split(":"); //peel off the monkey ID number 
                _monkeys.Add(int.Parse(parts[0]), new Monkey(line));
            }

            _worryReductionType = worryReductionType;
        }

        public void DoRound(int numRounds = 1)
        {
            //this is some Chinese Remainder Theorem voodoo here. This only works since the WorryTest divisor is prime. 
            //https://brilliant.org/wiki/chinese-remainder-theorem/
            long worryFactor = _monkeys.Values.Select(x => x.WorryTest).Aggregate((x, next) => x * next);

            long tempWorry = 0;

            for (long round = 1; round <= numRounds; round++)
            {
                for (int m = 0; m < _monkeys.Count; m++)
                {
                    while (true)
                    {
                        tempWorry = _monkeys[m].BackpackRemove();
                        if (tempWorry == -1) break; //out of backpack items.

                        tempWorry = _monkeys[m].CalcWorry(tempWorry);

                        tempWorry = (_worryReductionType == "PART1") ? tempWorry / 3 : tempWorry % worryFactor;

                        _monkeys[_monkeys[m].TestWorry(tempWorry)].BackpackAdd(tempWorry);
                    }
                }
            }
        }
        public long GetMonkeyBusiness()
        {
            return _monkeys.Values
                 .Select(x => x.MonkeyBusiness())
                 .OrderByDescending(x => x)
                 .Take(2)
                 .Aggregate((x, next) => x * next);
        }

        public void DumpBackpacks()
        {
            foreach (Monkey m in _monkeys.Values)
            {
                m.ShowBackpack();
            }
        }
    }
}
