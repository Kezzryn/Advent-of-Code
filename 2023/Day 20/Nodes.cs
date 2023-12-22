namespace AoC_2023_Day_20
{
    internal abstract class Node(List<string> targetNodes)
    {
        static public bool HIGH = true;
        static public bool LOW = false;
        public List<string> OutputModules { get => _targetNodes; }
        public long LowPulses { get; protected set; }
        public long HighPulses { get; protected set; }

        protected List<string> _targetNodes = targetNodes;
        protected bool _value = false;
        public void Reset()
        {
            _value = LOW;
            LowPulses = 0;
            HighPulses = 0;
        }

        protected List<(string, bool)> SendPulse()
        {
            if (_value == HIGH)
                HighPulses += _targetNodes.Count;
            else
                LowPulses += _targetNodes.Count;

            return _targetNodes.Select(s => (s, _value)).ToList();
        }

        public abstract List<(string, bool)> Pulse(bool inPulseValue, string source);
    }

    internal class Broadcaster(List<string> targetNodes) : Node(targetNodes)
    {
        public override List<(string, bool)> Pulse(bool inPulseValue, string source)
        {
            return SendPulse();
        }
    }

    internal class EndNode(List<string> targetNodes) : Node(targetNodes)
    {
        public override List<(string, bool)> Pulse(bool inPulseValue, string source)
        {
            return [];
        }
    }

    internal class FlipFlop(List<string> targetNodes) : Node(targetNodes)
    {
        public override List<(string, bool)> Pulse(bool inPulseValue, string source)
        {
            if (inPulseValue == HIGH) return [];
            
            _value = !_value;
            return SendPulse();
        }
    }
    internal class Conjunction : Node
    {
        public List<string> InputModules => _linkedModules.Keys.ToList();

        readonly Dictionary<string, bool> _linkedModules = [];
        public Conjunction(List<string> targetNodes, List<string> linkedModules)
           : base(targetNodes)
        {
            foreach(string module in linkedModules)
            {
                _linkedModules.Add(module, LOW);
            }
        }

        public new void Reset()
        {
            base.Reset();
            foreach (string key in _linkedModules.Keys)
            {
                _linkedModules[key] = false;
            }
        }

        public override List<(string, bool)> Pulse(bool inPulseValue, string source = "")
        {
            _linkedModules[source] = inPulseValue;

            _value = HIGH;
            if(_linkedModules.All(x => x.Value == HIGH)) _value = LOW;

            return SendPulse();
        }
    }
}
