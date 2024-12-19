namespace AoC_2024_Day_17;

internal class ThreeBitVM
{
    internal enum OpCode
    {
        ADV, BXL, BST, JNZ, BXC, OUT, BDV, CDV
    }

    public enum State
    {
        Running,
        Halted,
        Paused_For_Input,
        Paused_For_Output
    };
    private readonly Dictionary<char, long> _registers = new()
        {
            { 'a', 0 },
            { 'b', 0 },
            { 'c', 0 },
        };

    private readonly List<long> _instructionSet = [];
    private int _instPtr = 0;

    public readonly Queue<long> OutputQueue = new();

    public ThreeBitVM(List<long> program, long registerA, long registerB, long registerC)
    {
        _instructionSet = new(program);
        SetRegister('a', registerA);
        SetRegister('b', registerB);
        SetRegister('c', registerC);
    }

    public bool SetRegister(char name, long value)
    {
        if (_registers.ContainsKey(name))
        {
            _registers[name] = value;
            return true;
        }
        return false;
    }

    public long GetRegister(char name)
    {
        if (_registers.TryGetValue(name, out long value)) return value;
        throw new Exception($"Unknown register. {name}");
    }

    public State Run()
    {
        State currentState;
        do
        {
            if (_instPtr >= _instructionSet.Count || _instPtr < 0) return State.Halted;
            currentState = Dispatcher((OpCode)_instructionSet[_instPtr++], _instructionSet[_instPtr++]);
        } while (currentState == State.Running);

        return currentState;
    }

    public void Reset()
    {
        _instPtr = 0;
        _registers['a'] = 0;
        _registers['b'] = 0;
        _registers['c'] = 0;
    }

    private long GetValueOrRegister(long value)
    {
        return value switch
        {
            0 => 0,
            1 => 1,
            2 => 2,
            3 => 3,
            4 => GetRegister('a'),
            5 => GetRegister('b'),
            6 => GetRegister('c'),
            _ => throw new NotImplementedException()
        };
    }

    private State Dispatcher(OpCode op, long comboOp)
    {
        long a, b, c, v;

        switch (op)
        {
            case OpCode.ADV:
                //The numerator is the value in the A register.
                a = GetRegister('a');
                //The denominator is found by raising 2 to the power of the instruction's combo operand.
                long d = (long)Math.Pow(2, GetValueOrRegister(comboOp));
                //The result of the division operation is truncated to an integer and then written to the A register.
                a /= d;

                SetRegister('a', a);
                break;
            case OpCode.BXL:
                //The bxl instruction(opcode 1) calculates the bitwise XOR of register B and the instruction's literal operand, then stores the result in register B.
                b = GetRegister('b');
                b ^= comboOp;
                SetRegister('b', b);
                break;
            case OpCode.BST:
                //The bst instruction (opcode 2) calculates the value of its combo operand modulo 8 (thereby keeping only its lowest 3 bits), then writes that value to the B register.
                v = GetValueOrRegister(comboOp);
                v %= 8;
                SetRegister('b', v);
                break;
            case OpCode.JNZ:
                //The jnz instruction(opcode 3) does nothing if the A register is 0.
                //If the A register is not zero, it jumps by setting the instruction pointer to the value of its literal operand;
                if (GetRegister('a') != 0) _instPtr = (int)comboOp;
                break;
            case OpCode.BXC:
                //The bxc instruction (opcode 4) calculates the bitwise XOR of register B and register C, then stores the result in register B.
                //(For legacy reasons, this instruction reads an operand but ignores it.)
                b = GetRegister('b');
                c = GetRegister('c');
                SetRegister('b', b ^ c);
                break;
            case OpCode.OUT:
                //    The out instruction(opcode 5) calculates the value of its combo operand modulo 8, then outputs that value. (If a program outputs multiple values, they are separated by commas.)
                v = GetValueOrRegister(comboOp) % 8;
                OutputQueue.Enqueue(v);
                //return State.Paused_For_Output;
                break;
            case OpCode.BDV:
                //The bdv instruction(opcode 6) works exactly like the adv instruction except that the result is stored in the B register. (The numerator is still read from the A register.)
                //The numerator is the value in the A register.
                a = GetRegister('a');
                //The denominator is found by raising 2 to the power of the instruction's combo operand.
                v = (int)Math.Pow(2, GetValueOrRegister(comboOp));
                //The result of the division operation is truncated to an integer and then written to the B register.
                a /= v;
                SetRegister('b', a);
                break;
            case OpCode.CDV:
                //The cdv instruction(opcode 7) works exactly like the adv instruction except that the result is stored in the C register. (The numerator is still read from the A register.)
                //The numerator is the value in the A register.
                a = GetRegister('a');
                //The denominator is found by raising 2 to the power of the instruction's combo operand.
                v = (int)Math.Pow(2, GetValueOrRegister(comboOp));
                //The result of the division operation is truncated to an integer and then written to the C register.
                a /= v;
                SetRegister('c', a);
                break;
            default:
                throw new NotImplementedException();
        }
        return State.Running;
    }
}
