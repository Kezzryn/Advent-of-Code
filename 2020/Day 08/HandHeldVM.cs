using System.Runtime.CompilerServices;

namespace AoC_2020_HandHeldVM
{
    public enum State
    {
        Running,
        Halted,
        Stopped,
        Not_Run
    };


    internal class HandHeldVM
    {
        Dictionary<string, int> _opcodes = new()
        {
            { "nop", 0},
            { "acc", 1},
            { "jmp", 2}
        };

        private int _accumulator = 0;
        private int _instPtr = 0;

        private List<(int opcode, int value)> _instruction = new();

        private State _currentState = State.Not_Run;
        public State CurrentState => _currentState;

        public HandHeldVM(string[] instructions)
        {
            foreach(string line in instructions)
            {
                _instruction.Add((_opcodes[line[..3]], int.Parse(line[4..])));
            }
        }

        public int GetAccumulator => _accumulator; 

        public State Run()
        {
            (int opcode, int value) current;
            HashSet<int> history = new();

            do
            {
                if (_instPtr >= _instruction.Count) return State.Halted;

                if (!history.Add(_instPtr)) return State.Stopped;

                current = _instruction[_instPtr++];
                
                _currentState = Dispatcher(current.opcode, current.value);
            } while (_currentState == State.Running);

            return _currentState;
        }
        private State Dispatcher(int opCode, int value)
        {
            switch (opCode)
            {
                case 0: //NOOP
                    break;
                case 1: //Add
                    _accumulator += value;
                    break;
                case 2: //JMP
                    _instPtr--; // reposition for the jump. 
                    _instPtr += value;
                    break;
                default:
                    throw new NotImplementedException($"unknown instruction: {opCode}");
            }
            return State.Running;
        }

        private int lastIndexChecked = 0;
        private (int opcode, int value) prevInst = (0, 0);

        public void TryFixCode()
        {
            _instPtr = 0;
            _accumulator = 0;
            if(lastIndexChecked > 0 )
            {
                _instruction[lastIndexChecked] = prevInst;
            }

            lastIndexChecked = _instruction.FindIndex(lastIndexChecked + 1, x => x.opcode != 1);
            prevInst = _instruction[lastIndexChecked];
            _instruction[lastIndexChecked] = (prevInst.opcode == 0 ? 2 : 0, prevInst.value);
        }
    }
}
