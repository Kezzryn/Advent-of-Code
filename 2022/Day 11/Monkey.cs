namespace AoC_2022_Day_11
{
    internal class Monkey
    {
        const string CRLF = "\r\n";

        private Queue<long> _backpack = new();
        private long _monkeyBusiness;

        private readonly int _throwMonkeyIfTrue;
        private readonly int _throwMonkeyIfFalse;
        private readonly string _worryCalc = "";

        public long WorryTest { get; }

        public int TestWorry(long worryLevel)
        {
            return (worryLevel % WorryTest == 0) ? _throwMonkeyIfTrue : _throwMonkeyIfFalse;
        }

        public long CalcWorry(long val)
        {
            //Tried to use DataTable, but it was limited to Int32, so, we'll do this the hard way.
            string[] worryCalcParts = string.Format(_worryCalc, val).Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (worryCalcParts.Length == 1) return long.Parse(worryCalcParts[0]);

            return worryCalcParts[1] switch
            {
                "+" => long.Parse(worryCalcParts[0]) + long.Parse(worryCalcParts[2]),
                "*" => long.Parse(worryCalcParts[0]) * long.Parse(worryCalcParts[2]),
                "-" => long.Parse(worryCalcParts[0]) - long.Parse(worryCalcParts[2]),
                "/" => long.Parse(worryCalcParts[0]) / long.Parse(worryCalcParts[2]),
                _ => throw new NotImplementedException(worryCalcParts[1])
            };
        }

        public void BackpackAdd(long item)
        {
            _backpack.Enqueue(item);
        }

        public long BackpackRemove()
        {

            if (_backpack.TryDequeue(out long returnValue))
            {
                _monkeyBusiness++;
            }
            else
            {
                returnValue = -1;
            }
            return returnValue;
        }

        public long MonkeyBusiness()
        {
            return _monkeyBusiness;
        }

        public Monkey(string monkeyData)
        {
            /* example monkeyData
             *  0: 
                Starting items: 99, 63, 76, 93, 54, 73
                Operation: new = old * 11
                Test: divisible by 2
                If true: throw to monkey 7
                If false: throw to monkey 1
            */

            foreach (string line in monkeyData.Split(CRLF))
            {
                string[] parts = line.Split(':');
                switch (parts[0])
                {
                    case "Starting items":
                        foreach (long item in parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse))
                        {
                            BackpackAdd(item);
                        }
                        break;

                    case "Operation":
                        _worryCalc = parts[1]
                            .Split('=', StringSplitOptions.RemoveEmptyEntries).Last()
                            .Replace("old", "{0}");
                        break;

                    case "Test":
                        WorryTest = int.Parse(parts[1].Split(' ').Last());
                        break;

                    case "If true":
                        _throwMonkeyIfTrue = int.Parse(parts[1].Split(' ').Last());
                        break;

                    case "If false":
                        _throwMonkeyIfFalse = int.Parse(parts[1].Split(' ').Last());
                        break;
                };
            }
        }

        public void ShowBackpack()
        {
            foreach (long itm in _backpack)
            {
                Console.Write($"{itm} ");
            }
            Console.WriteLine("");
        }
    }
}

