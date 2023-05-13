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
        private int _outAddr = 0;
        private enum Mode
        {
            Position = 0,
            Immediate = 1
        }

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
        private State Dispatcher(int opCode) 
        {
            Mode modeFirst = (Mode)(opCode % 1000 / 100);
            Mode modeSecond = (Mode)(opCode % 10000 / 1000);
            Mode modeThird = (Mode)(opCode / 10000);

            int instruction = opCode % 100;

            switch (instruction)
            {
                case 1: //ADD
                    _inputA = Ptr_ReadValue(modeFirst);
                    _inputB = Ptr_ReadValue(modeSecond);
                    _outAddr = Ptr_ReadValue();
                    _output = _inputA + _inputB;
                    Mem_Write(_outAddr, _output);
                    break;
                case 2: //MULTIPLY
                    _inputA = Ptr_ReadValue(modeFirst);
                    _inputB = Ptr_ReadValue(modeSecond);
                    _outAddr = Ptr_ReadValue();
                    _output = _inputA * _inputB;
                    Mem_Write(_outAddr, _output);
                    break;
                case 3: // Opcode 3 takes a single integer as input and saves it to the position given by its only parameter.
                        // For example, the instruction 3,50 would take an input value and store it at address 50.

                    _outAddr = Ptr_ReadValue();

                    bool isInvalidInput = true;
                    do
                    {
                        Console.Write("_> ");
                        string? readLine = Console.ReadLine();

                        if (int.TryParse(readLine, out _output))
                        {
                            isInvalidInput = false;
                        }
                        else
                        {
                            Console.WriteLine("Unable to parse input.");
                        }
                    } while (isInvalidInput);

                    Mem_Write(_outAddr, _output);
                    break;
                case 4: //output
                    _output = Ptr_ReadValue(modeFirst);
                    Console.WriteLine(_output);
                    break;
                case 5: //Opcode 5 is jump -if-true: if the first parameter is non - zero, it sets the instruction pointer to the value from the second parameter.Otherwise, it does nothing.
                    _inputA = Ptr_ReadValue(modeFirst);
                    _outAddr = Ptr_ReadValue(modeSecond);
                    if (_inputA != 0) _instPtr = _outAddr;
                    break;
                case 6: //Opcode 6 is jump -if-false: if the first parameter is zero, it sets the instruction pointer to the value from the second parameter.Otherwise, it does nothing.
                    _inputA = Ptr_ReadValue(modeFirst);
                    _outAddr = Ptr_ReadValue(modeSecond);
                    if (_inputA == 0) _instPtr = _outAddr;
                    break;
                case 7: //Opcode 7 is less than: if the first parameter is less than the second parameter, it stores 1 in the position given by the third parameter.Otherwise, it stores 0.
                    _inputA = Ptr_ReadValue(modeFirst);
                    _inputB = Ptr_ReadValue(modeSecond);
                    _outAddr = Ptr_ReadValue();
                    _output = _inputA < _inputB ? 1 : 0;
                    Mem_Write(_outAddr, _output);
                    break;
                case 8: //Opcode 8 is equals: if the first parameter is equal to the second parameter, it stores 1 in the position given by the third parameter.Otherwise, it stores 0.
                    _inputA = Ptr_ReadValue(modeFirst);
                    _inputB = Ptr_ReadValue(modeSecond);
                    _outAddr = Ptr_ReadValue();
                    _output = _inputA == _inputB ? 1 : 0;
                    Mem_Write(_outAddr, _output);
                    break;
                case 99: //HALT
                    return State.Halted;
                default:
                    throw new NotImplementedException($"unknown instruction: {instruction}");
            }
            return State.Running;
        }

        private int Ptr_ReadValue(Mode mode = Mode.Immediate) 
        {
            int addressOrValue = Mem_Read(_instPtr++);
            return mode switch
            {
                Mode.Position => Mem_Read(addressOrValue),
                Mode.Immediate => addressOrValue,
                _ => throw new NotImplementedException($"unknown Mode {mode}")
            };
        } 

        public void Mem_Write(int address, int value) => _mainMemory[address] = value;
        public int Mem_Read(int address) => _mainMemory[address];
        public void Reset(int[] instructions)
        {
            _instPtr = 0;
            Array.Copy(instructions, _mainMemory, instructions.Length);
        }
    }
}
