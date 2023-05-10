namespace AoC_2019_IntcodeVM
{
    public enum State
    {
        Running,
        Halted,
        Paused_For_Input
    };

    internal class IntcodeVM
    {
        private int[] _mainMemory;
        private int _instPtr = 0;

        private int _inputA = 0;
        private int _inputB = 0;
        private int _output = 0;

        public IntcodeVM(int[] instructions)
        {
            _mainMemory = new int[instructions.Length];
            Array.Copy(instructions, _mainMemory, instructions.Length);
        }

        public State Run()
        {
            State currentState;
            do
            {
                currentState = Dispatcher(Ptr_ReadValue());
            } while (currentState == State.Running);

            return currentState;
        }

        private State Dispatcher(int instruction) 
        {
            switch (instruction)
            {
                case 1: //ADD
                    _inputA = Ptr_ReadValueAt();
                    _inputB = Ptr_ReadValueAt();
                    _output = _inputA + _inputB;
                    Mem_Write(Ptr_ReadValue(), _output);
                    break;
                case 2: //MULTIPLY
                    _inputA = Ptr_ReadValueAt();
                    _inputB = Ptr_ReadValueAt();
                    _output = _inputA * _inputB;
                    Mem_Write(Ptr_ReadValue(), _output);
                    break;
                case 99: //HALT
                    return State.Halted;
                default:
                    throw new NotImplementedException($"unknown instruction: {instruction}");
            }
            return State.Running;
        }

        private int Ptr_ReadValue() => Mem_Read(_instPtr++);
        private int Ptr_ReadValueAt() => Mem_Read(Ptr_ReadValue());
        public void Mem_Write(int address, int value) => _mainMemory[address] = value;
        public int Mem_Read(int address) => _mainMemory[address];
        public void Reset(int[] instructions)
        {
            _instPtr = 0;
            Array.Copy(instructions, _mainMemory, instructions.Length);
        }
    }
}
