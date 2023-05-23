namespace AoC_2019_IntcodeVM
{
    public enum State
    {
        Running,
        Halted,
        Paused_For_Input,
        Paused_For_Output
    };

    internal class IntcodeVM
    {
        private const bool AS_LITERAL = true;

        private readonly long[] _mainMemory = new long[65536];
        private long _instPtr = 0;

        private long _inputA = 0;
        private long _inputB = 0;
        private long _output = 0;
        private long _outAddr = 0;
        private long _relativeBase = 0;

        private readonly Queue<long> _inputBuffer = new ();
        private readonly Queue<long> _outputBuffer = new();

        private enum Mode
        {
            Position = 0,
            Immediate = 1,
            Relative = 2
        }

        public IntcodeVM(long[] instructions)
        {
            Array.Copy(instructions, _mainMemory, instructions.Length);
        }

        public State Run()
        {
            State currentState;
            do
            {
                currentState = Dispatcher(Ptr_ReadValue(Mode.Immediate));
            } while (currentState == State.Running);

            return currentState;
        }
        private State Dispatcher(long opCode) 
        {
            Mode modeFirst = (Mode)(opCode % 1000 / 100);
            Mode modeSecond = (Mode)(opCode % 10000 / 1000);
            Mode modeThird = (Mode)(opCode / 10000);

            long instruction = opCode % 100;

            switch (instruction)
            {
                case 1: //ADD
                    _inputA = Ptr_ReadValue(modeFirst);
                    _inputB = Ptr_ReadValue(modeSecond);
                    _outAddr = Ptr_ReadValue(modeThird, AS_LITERAL);
                    _output = _inputA + _inputB;
                    Mem_Write(_outAddr, _output);
                    break;
                case 2: //MULTIPLY
                    _inputA = Ptr_ReadValue(modeFirst);
                    _inputB = Ptr_ReadValue(modeSecond);
                    _outAddr = Ptr_ReadValue(modeThird, AS_LITERAL);
                    _output = _inputA * _inputB;
                    Mem_Write(_outAddr, _output);
                    break;
                case 3: // Opcode 3 takes a single long as input and saves it to the position given by its only parameter.
                        // For example, the instruction 3,50 would take an input value and store it at address 50.

                    if (_inputBuffer.Count == 0 )
                    {
                        _instPtr--; // back up so we don't freak out when we resume.
                        return State.Paused_For_Input;
                    }
                    _output = _inputBuffer.Dequeue();
                    _outAddr = Ptr_ReadValue(modeFirst, AS_LITERAL);
                    Mem_Write(_outAddr, _output);
                    break;
                case 4: //output
                    _output = Ptr_ReadValue(modeFirst);
                    _outputBuffer.Enqueue(_output);
                    if (_output == '\n') return State.Paused_For_Output;
                    break;
                case 5: //Opcode 5 is jump -if-true: if the first parameter is non - zero, it sets the instruction polonger to the value from the second parameter.Otherwise, it does nothing.
                    _inputA = Ptr_ReadValue(modeFirst);
                    _outAddr = Ptr_ReadValue(modeSecond);
                    if (_inputA != 0) _instPtr = _outAddr;
                    break;
                case 6: //Opcode 6 is jump -if-false: if the first parameter is zero, it sets the instruction polonger to the value from the second parameter.Otherwise, it does nothing.
                    _inputA = Ptr_ReadValue(modeFirst);
                    _outAddr = Ptr_ReadValue(modeSecond);
                    if (_inputA == 0) _instPtr = _outAddr;
                    break;
                case 7: //Opcode 7 is less than: if the first parameter is less than the second parameter, it stores 1 in the position given by the third parameter.Otherwise, it stores 0.
                    _inputA = Ptr_ReadValue(modeFirst);
                    _inputB = Ptr_ReadValue(modeSecond);
                    _outAddr = Ptr_ReadValue(modeThird, AS_LITERAL);
                    _output = _inputA < _inputB ? 1 : 0;
                    Mem_Write(_outAddr, _output);
                    break;
                case 8: //Opcode 8 is equals: if the first parameter is equal to the second parameter, it stores 1 in the position given by the third parameter.Otherwise, it stores 0.
                    _inputA = Ptr_ReadValue(modeFirst);
                    _inputB = Ptr_ReadValue(modeSecond);
                    _outAddr = Ptr_ReadValue(modeThird, AS_LITERAL);
                    _output = _inputA == _inputB ? 1 : 0;
                    Mem_Write(_outAddr, _output);
                    break;
                case 9: //Opcode 9 adjusts the relative base by the value of its only parameter.
                    _inputA = Ptr_ReadValue(modeFirst);
                    _relativeBase += _inputA;
                    break;
                case 99: //HALT
                    return State.Halted;
                default:
                    throw new NotImplementedException($"unknown instruction: {instruction}");
            }
            return State.Running;
        }

        public bool GetOutput(out long output)
        {
            output = 0;
            if (_outputBuffer.Count == 0) return false;

            output = _outputBuffer.Dequeue();
            return true;
        }

        public bool GetOutput(out char output)
        {
            bool returnValue = GetOutput(out long outputLong);
            output = (char)outputLong;
            return returnValue;
        }

        public void SetInput(long input) => _inputBuffer.Enqueue(input);

        public void SetInput(string input)
        {
            foreach (char c in input) SetInput(c);
            if (input[^1] != '\n') SetInput('\n');
        }

        private long Ptr_ReadValue(Mode mode, bool asLiteral = false) 
        {
            long addressOrValue = Mem_Read(_instPtr++);

            return mode switch
            {
                Mode.Position => asLiteral ? addressOrValue : Mem_Read(addressOrValue),
                Mode.Immediate => addressOrValue,
                Mode.Relative => asLiteral ? (addressOrValue + _relativeBase) : Mem_Read(addressOrValue + _relativeBase),
                _ => throw new NotImplementedException($"unknown Mode {mode}")
            };
        } 
        public void SetMemory(long address, long value) => Mem_Write(address, value);
        private void Mem_Write(long address, long value) => _mainMemory[address] = value;
        private long Mem_Read(long address) => _mainMemory[address];
        public void Reset(long[] instructions)
        {
            _relativeBase = 0;
            _instPtr = 0;
            _inputBuffer.Clear();
            _outputBuffer.Clear();
            Array.Clear(_mainMemory);
            Array.Copy(instructions, _mainMemory, instructions.Length);
        }

        public void Reboot()
        {
            // program specific reinitalize to avoid reloading array in Reset() command.
            _relativeBase = 0;
            _instPtr = 0;

            _mainMemory[132] = 303;
            _mainMemory[249] = 225;
            
            _mainMemory[221] = 0;
            _mainMemory[222] = 0;
            _mainMemory[223] = 0;
            _mainMemory[224] = 0;
        }

        public void Diffs(long[] instructions)
        {
            //used to find the commands for the reboot function.
            for(int i = 0; i <  instructions.Length; i++)
            {
                Console.ForegroundColor = instructions[i] == _mainMemory[i] ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"{i,-4} {instructions[i],-8} {_mainMemory[i],8}");
            }
            Console.ResetColor();
        }
    }
}
