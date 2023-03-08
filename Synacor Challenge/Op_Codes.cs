namespace Synacor_Challenge
{
    internal partial class Synacor9000
    {
        /*
         * This file contains the implementation for:
         * 1) Instruction dispatcher 
         * 2) Opcodes for the VM machine
         * 3) Memory read/write helper functions. 
         * 4) Instruction pointer and helper functions.
         */

        // got tired of doing (ushort)0 (ushort)1 in places.
        private const ushort USHORT_0 = 0;
        private const ushort USHORT_1 = 1;

        private ushort _instPtr = 0;    // instruction pointer. 
        private State Dispatcher(ushort instruction)
        {
            switch (instruction)
            {
                case 0:
                    return Synacor9000.State.Halted;
                case 1 or 15 or 16:
                    Op_Memory(instruction);
                    break;
                case 2 or 3:
                    Op_Stack(instruction);
                    break;
                case 4 or 5:
                    Op_Comparison(instruction);
                    break;
                case 6 or 7 or 8 or 17 or 18:
                    _instPtr = Op_Jump(instruction);
                    break;
                case >= 9 and <= 14:
                    Op_Math(instruction);
                    break;
                case 19 or 20:
                    return Op_Terminal(instruction);
                case 21:
                    // noop: 21  no operation 
                    break;
                default:
                    throw new NotImplementedException($"{instruction}");
            }
            return State.Running;
        }
        private void Op_Comparison(ushort instruction)
        {
            ushort address = Ptr_ReadRaw();
            ushort b = Ptr_ReadValue();
            ushort c = Ptr_ReadValue();

            ushort value = instruction switch
            {
                4 => (b == c) ? USHORT_1 : USHORT_0,        // eq: 4 a b c  set <a> to 1 if <b> is equal to <c>; set it to 0 otherwise    
                5 => (b > c) ? USHORT_1 : USHORT_0,        // gt: 5 a b c  set <a> to 1 if <b> is greater than <c>; set it to 0 otherwise
                _ => throw new NotImplementedException()    // Comparison shmarison.
            };
            Mem_Write(address, value);
        }
        private ushort Op_Jump(ushort instruction)
        {
            ushort a; // do not pre-assign. ret could break things 
            ushort b;

            switch (instruction)
            {
                case 6:                             // jmp: 6 a     jump to <a>
                    a = Ptr_ReadRaw();
                    return a;
                case 7:                             // jt: 7 a b	if <a> is nonzero, jump to <b>
                    a = Ptr_ReadValue();
                    b = Ptr_ReadValue();
                    return (a != 0) ? b : _instPtr;
                case 8:                             // jf: 8 a b    if <a> is zero, jump to <b>
                    a = Ptr_ReadValue();
                    b = Ptr_ReadValue();
                    return (a == 0) ? b : _instPtr;
                case 17:                            // call: 17 a   write the address of the next instruction to the stack and jump to <a> 
                    a = Ptr_ReadValue();
                    _stack.Push(_instPtr);
                    return a;
                case 18:                            // ret: 18    remove the top element from the stack and jump to it; empty stack = halt
                    if (_stack.Count == 0) throw new Exception("Stack is empty");
                    return _stack.Pop();
                default:                            // Jump around, jump around, get up, get up and go thump.
                    throw new NotImplementedException();
            }
        }
        private void Op_Math(ushort instruction)
        {
            ushort address = Ptr_ReadRaw();
            ushort b = Ptr_ReadValue();
            ushort c = (instruction != 14) ? Ptr_ReadValue() : USHORT_1;
            ushort value = instruction switch
            {
                9 => (ushort)((b + c) % MODULO),           //  add:  9 a b c   assign into <a> the sum of <b> and <c> (modulo 32768)
                10 => (ushort)(b * c % MODULO),             // mult: 10 a b c	store into <a> the product of <b> and <c> (modulo 32768)
                11 => (ushort)(b % c),                      //  mod: 11 a b c	store into <a> the remainder of <b> divided by <c>
                12 => (ushort)(b & c),                      //  and: 12 a b c   stores into <a> the bitwise and of <b> and <c>
                13 => (ushort)(b | c),                      //   or: 13 a b c	stores into <a> the bitwise or of <b> and <c>
                14 => (ushort)((ushort)(~b << c) >> c),     //  not: 14 a b	    stores 15-bit bitwise inverse of <b> in <a>
                _ => throw new NotImplementedException()    // Math is MATH! 
            };
            Mem_Write(address, value);
        }
        private void Op_Stack(ushort instruction)
        {
            ushort a;
            switch (instruction)
            {
                case 2:                                     // push: 2 a    push <a> onto the stack
                    a = Ptr_ReadValue();
                    _stack.Push(a);
                    break;
                case 3:                                     //  pop: 3 a    remove the top element from the stack and write it into <a>; empty stack = error 
                    if (_stack.Count == 0) throw new Exception("Stack is empty");
                    a = Ptr_ReadRaw();
                    Mem_Write(a, _stack.Pop());
                    break;
                default:                                    // We suck at Jenga
                    throw new NotImplementedException();
            }
        }
        private void Op_Memory(ushort instruction)
        {/*
          * Think of them as pointers. In C code:

             rmem a b   a = *b;
             wmem a b   *a = b
One of the two parameters is indirect: it's not the destination/source of the move, but it contains the destination/source address of the move

            Oversimplified answer is that rmem is reading memory to a register, wmem is writing a register to memory

        */
            ushort address;
            ushort value;

            switch (instruction)
            {
                case 1:                         // set: 1 a b       set register <a> to the value of <b>
                    address = Ptr_ReadRaw();
                    value = Ptr_ReadValue();
                    break;
                case 15:                        // rmem: 15 a b	    read memory at address <b> and write it to <a>
                    address = Ptr_ReadRaw();
                    value = Mem_Read(Ptr_ReadValue());
                    break;
                case 16:                        // wmem: 16 a b	    write the value from <b> into memory at address <a>
                    address = Ptr_ReadValue();
                    value = Ptr_ReadValue();
                    break;
                default:                                    // I don't remember where I came from. 
                    throw new NotImplementedException();
            }
            Mem_Write(address, value);
        }
        private State Op_Terminal(ushort instruction)
        {
            switch (instruction)
            {
                case 19:         // out: 19 a    write the character represented by ascii code <a> to the terminal
                    char c = (char)Ptr_ReadValue();

                    _sbOutput.Append(c);
                    if (c == '\n')
                    {
                        _outputBuffer.Enqueue(_sbOutput.ToString()); 
                        _sbOutput.Clear();
                    } 
                    break;
                case 20:        // in: 20 a     read a character from the terminal and write its ascii code to <a>;
                    // it can be assumed that once input starts, it will continue until a newline is encountered;
                    // this means that you can safely read whole lines from the keyboard and trust that they will be fully read

                    if (string.IsNullOrEmpty(_inputBuffer))
                    {
                        _instPtr--; // back this up to what should be the instruction, so if/when we resume we don't freak out.
                        return State.Paused_For_Input;
                    }

                    ushort value = _inputBuffer[0];
                    ushort address = Ptr_ReadRaw();

                    _inputBuffer = _inputBuffer.Length > 1 ? _inputBuffer[1..] : string.Empty;

                    Mem_Write(address, value);

                    break;
                default:
                    throw new NotImplementedException();    // PEBCAK 
            }
            return State.Running;
        }
        private ushort Mem_Read(ushort address)
        {
            return address switch
            {
                <= MEMORY_MAX => _mainMemory[address],
                > MEMORY_MAX and < INVALID_MEMORY => _registers[address % MODULO],
                _ => throw new Exception($"{address} is out of bounds")
            };
        }
        private void Mem_Write(ushort address, ushort value)
        {
            switch (address)
            {
                case <= MEMORY_MAX:
                    _mainMemory[address] = value;
                    break;
                case > MEMORY_MAX and < INVALID_MEMORY:
                    _registers[address % MODULO] = value;
                    break;
                default:
                    throw new Exception($"{address} is out of bounds");
            };
        }
        private ushort Ptr_ReadValue()
        {

            ushort value = _mainMemory[_instPtr++];

            return value switch
            {
                <= MEMORY_MAX => value,
                > MEMORY_MAX and < INVALID_MEMORY => _registers[value % MODULO],
                _ => throw new Exception($"{value} is out of bounds")
            };
        }
        private ushort Ptr_ReadRaw() => Mem_Read(_instPtr++);   //Raw value at the pointer location. Used mainly for addressing. 
    }
}
