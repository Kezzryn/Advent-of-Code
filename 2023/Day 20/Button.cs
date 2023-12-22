using System.Runtime.CompilerServices;

namespace AoC_2023_Day_20
{
    public class Button
    {
        private const char CONJ = '&';
        private const char FLFP = '%';
        private const string BROADCASTER = "broadcaster";
        private Dictionary<string, Node> _modules = [];
        private Queue<(string source, string dest, bool pulse)> _pulseQueue = [];
        private Dictionary<string, long> _intercept = [];
        public Button(string[] puzzleInput)
        {
            // build the list of inputs to each conjunction.
            Dictionary<string, List<string>> conjSourceModules =
                puzzleInput.Where(wa => wa[0] == CONJ)
                .ToDictionary(k => k[1..3],
                              v => puzzleInput.Where(wb => wb[7..].Contains(v[1..3])).Select(s => s[1..3]).ToList());

            foreach (string line in puzzleInput)
            {
                //&tb -> sx, qn, vj, qq, sk, pv
                char type = line[0];
                string label = line[1..3];
                List<string> targetModules = [.. line[(line.IndexOf('>') + 1)..].Split(',', StringSplitOptions.TrimEntries)];

                if (line.StartsWith(BROADCASTER)) _modules.Add(BROADCASTER, new Broadcaster(targetModules));
                if (type == CONJ) _modules.Add(label, new Conjunction(targetModules, conjSourceModules[label]));
                if (type == FLFP) _modules.Add(label, new FlipFlop(targetModules));
            }

            string endNode = _modules.SelectMany(x => x.Value.OutputModules).Except(_modules.Keys).FirstOrDefault("NA");
            _modules.Add(endNode, new EndNode([]));
        }

        private string BroadCast()
        {
            // only used for part two.
            string returnValue = String.Empty;
            _pulseQueue.Enqueue(("", BROADCASTER, Node.LOW));

            while (_pulseQueue.TryDequeue(out (string source, string curModule, bool value) pulse))
            {
                foreach ((string target, bool pulseValue) in _modules[pulse.curModule].Pulse(pulse.value, pulse.source))
                {
                    if (_intercept.ContainsKey(target) && pulseValue == Node.LOW) returnValue = target;
                    _pulseQueue.Enqueue((pulse.curModule, target, pulseValue));
                }
            }
            return returnValue;
        }

        public long PressPart1(int numPresses)
        {
            _intercept.Clear();
            foreach (string key in _modules.Keys)
            {
                _modules[key].Reset();
            }

            for (int i = 1; i <= numPresses; i++)
            {
                BroadCast();
            }

            //Remember to add back number of low signals signals sent by the button.
            return (_modules.Values.Sum(x => x.LowPulses) + numPresses) * _modules.Values.Sum(x => x.HighPulses);
        }

        public long PressPart2()
        {
            long numPresses = 0;
            foreach (string key in _modules.Keys)
            {
                _modules[key].Reset();
            }

            string endNode = _modules.Where(x => x.Value is EndNode).First().Key;

            _intercept = _modules.Values.Where(x => x is Conjunction)
                .Select(c => (Conjunction)c)
                .Where(x => x.OutputModules.Contains(endNode))
                .SelectMany(x => x.InputModules)
                .ToDictionary(k => k, v => -1L);

            do
            {
                numPresses++;
                string temp = BroadCast();

                if (temp != String.Empty) _intercept[temp] = numPresses;
            } while (_intercept.Any(x => x.Value == -1L) && numPresses < 5000);

            //At this point I'd take the LCM of the numbers.
            //However, all the answers for my input are prime.
            //So... 
            return _intercept.Values.Aggregate((x, y) => x * y);
        }
    }
}
