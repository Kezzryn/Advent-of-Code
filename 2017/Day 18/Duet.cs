namespace AoC_2017_Day_18
{
    public enum State
    {
        Running,
        Halted,
        Paused_For_Input
    };

    internal class Duet
    {
        // populated from studying input.
        private readonly Dictionary<string, long> _registers;
        private readonly string[] _instructionSet;

        private long _instPtr = 0;
        private long _lastSound = 0;
        private long _part1Answer = -1;
        private long _part2Answer = 0;

        public State CurrentState { get; private set; }

        private readonly Queue<long> _outputBuffer = new();
        private readonly Queue<long> _inputBuffer = new();

        public Duet(string[] instructionSet, int progID)
        {
            _instructionSet = instructionSet;

            _registers = new()
            {
                { "a", 0 },
                { "b", 0 },
                { "f", 0 },
                { "i", 0 },
                { "p", progID }
            };
        }

        public void Run()
        {
            int numSteps = 10;

            do
            {
                CurrentState = Dispatcher();
                numSteps--;
            } while (CurrentState == State.Running && numSteps > 0);
        }

        State Dispatcher()
        {
            if (_instPtr >= _instructionSet.Length ) return State.Halted;
            string[] cmds = _instructionSet[_instPtr].Split(' ');
            long instValue = 0;
            _instPtr++;

            if (cmds.GetUpperBound(0) >= 2)
            {
                instValue =
                    int.TryParse(cmds[2], out int parseValue)
                        ? parseValue
                        : _registers.TryGetValue(cmds[2], out long regValue)
                            ? regValue
                            : 0;
            }

            switch (cmds[0])
            {
                case "snd":
                    _lastSound = _registers[cmds[1]];
                    _outputBuffer.Enqueue(_registers[cmds[1]]);
                    _part2Answer++;
                    break;
                case "set":
                    //set X Y sets register X to the value of Y.
                    _registers[cmds[1]] = instValue;
                    break;
                case "add":
                    //add X Y increases register X by the value of Y.
                    _registers[cmds[1]] += instValue;
                    break;
                case "mul":
                    //mul X Y sets register X to the result of multiplying the value contained in register X by the value of Y.
                    _registers[cmds[1]] *= instValue;
                    break;
                case "mod":
                    //mod X Y sets register X to the remainder of dividing the value contained in register X by the value of Y (that is, it sets X to the result of X modulo Y).
                    _registers[cmds[1]] = _registers[cmds[1]] % instValue;
                    break;
                case "rcv":
                    //rcv X recovers the frequency of the last sound played, but only when the value of X is not zero. (If it is zero, the command does nothing.)
                    if (_part1Answer == -1 && _registers[cmds[1]] > 0)
                    {
                        _part1Answer = _lastSound;
                    }

                    if (_inputBuffer.Count == 0)
                    {
                        _instPtr--;  // back up so we can resume correctly. 
                        return State.Paused_For_Input;
                    }

                    _registers[cmds[1]] = _inputBuffer.Dequeue();

                    break;
                case "jgz":
                    //jgz X Y jumps with an offset of the value of Y, but only if the value of X is greater than zero. (An offset of 2 skips the next instruction, an offset of -1 jumps to the previous instruction, and so on.)

                    
                    // cheat to avoid a pile of dictionary key BS. 
                    if (cmds[1] == "1" || _registers[cmds[1]] > 0)
                    {
                        _instPtr--;
                        _instPtr += instValue;
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }


            return State.Running;
        }

        public long FirstRCVValue() => _part1Answer;
        public long NumSentMessages() => _part2Answer;

        public bool GetProgramOutput(out long output)
        {
            output = 0;
            if (_outputBuffer.Count == 0) return false;

            output = _outputBuffer.Dequeue();
            return true;
        }

        public bool HasOutput()
        {
            if (_outputBuffer.Count == 0) return false;
            return true;
        }

        public void SetProgramInput(long input) => _inputBuffer.Enqueue(input);
    }
}
