namespace AoC_2017_TabletVM;

public enum State
{
    Running,
    Halted,
    Paused_For_Input
};

public class TabletVM
{
    private readonly struct Instruction
    {
        public readonly string OpCode;
        public readonly char Param1;
        public readonly long Param2 = 0;
        public readonly bool IsParam2Register = false;
            
        public Instruction(string inst)
        {
            string[] cmds = inst.Split(' ');
            OpCode = cmds[0];
            Param1 = cmds[1][0];
            
            if (cmds.GetUpperBound(0) >= 2)
            {
                if(int.TryParse(cmds[2], out int parseValue))
                {
                    Param2 = parseValue;
                }
                else
                {
                    IsParam2Register = true;
                    Param2 = cmds[2][0];
                }
            }
        }
    }


    // populated from studying input.
    private readonly Dictionary<char, long> _registers = [];
    private readonly List<Instruction> _instructionSet = [];

    private long _instPtr = 0;
    private long _lastSound = 0;

    public long FirstRCVValue { get; private set; } = -1; //2017 Day 18 Part 1
    public long NumSentMessages { get; private set; } = 0; //2017 Day 18 Part 2
    public long NumMultOps { get; private set; } = 0; //2017 Day 23 Part 1
    public State CurrentState { get; private set; }

    public Queue<long> OutputBuffer = new();
    public Queue<long> InputBuffer = new();

    public TabletVM(string[] instructionSet)
    {
        foreach(var instruction in instructionSet)
        {
            Instruction inst = new(instruction);
            
            // build the registers ranges dynamically, since they change between puzzles. 
            if(inst.Param1 != '1') _registers.TryAdd(inst.Param1, 0);
            if(inst.IsParam2Register) _registers.TryAdd((char)inst.Param2!, 0);

            _instructionSet.Add(inst);
        }
    }

    public void SetRegister(char reg, long value)  => _registers[reg] = value;
    public long GetRegister(char reg) => _registers[reg];

    public void Run()
    {
        do
        {
            Step();
        } while (CurrentState == State.Running);
    }

    public void Step()
    {
        if (CurrentState != State.Halted)
            CurrentState = Dispatcher();
    }

    private State Dispatcher()
    {
        if (_instPtr >= _instructionSet.Count) return State.Halted;

        Instruction inst = _instructionSet[(int)_instPtr++];
        long Param2 = inst.IsParam2Register 
                    ? _registers[(char)inst.Param2]
                    : inst.Param2;

        switch (inst.OpCode)
        {
            case "snd":
                _lastSound = _registers[inst.Param1];
                OutputBuffer.Enqueue(_registers[inst.Param1]);
                NumSentMessages++;
                break;
            case "set":
                //set X Y sets register X to the value of Y.
                _registers[inst.Param1] = Param2;
                break;
            case "add":
                //add X Y increases register X by the value of Y.
                _registers[inst.Param1] += Param2;
                break;
            case "sub":
                //sub X Y decreases register X by the value of Y.
                _registers[inst.Param1] -= Param2;
                break;
            case "mul":
                //mul X Y sets register X to the result of multiplying the value contained in register X by the value of Y.
                _registers[inst.Param1] *= Param2;
                NumMultOps++;
                break;
            case "mod":
                //mod X Y sets register X to the remainder of dividing the value contained in register X by the value of Y (that is, it sets X to the result of X modulo Y).
                _registers[inst.Param1] = _registers[inst.Param1] % Param2;
                break;
            case "rcv":
                //rcv X recovers the frequency of the last sound played, but only when the value of X is not zero. (If it is zero, the command does nothing.)
                if (FirstRCVValue == -1 && _registers[inst.Param1] > 0) FirstRCVValue = _lastSound;

                if (InputBuffer.Count == 0)
                {
                    _instPtr--;  // back up so we can resume correctly. 
                    return State.Paused_For_Input;
                }

                _registers[inst.Param1] = InputBuffer.Dequeue();

                break;
            case "jgz":
                //jgz X Y jumps with an offset of the value of Y, but only if the value of X is greater than zero. (An offset of 2 skips the next instruction, an offset of -1 jumps to the previous instruction, and so on.)

                // cheat to avoid a pile of dictionary key BS. 
                if (inst.Param1 == '1' || _registers[inst.Param1] > 0)
                {
                    _instPtr--;
                    _instPtr += Param2;
                }
                break;
            case "jnz":
                //jnz X Y jumps with an offset of the value of Y, but only if the value of X is not zero. (An offset of 2 skips the next instruction, an offset of - 1 jumps to the previous instruction, and so on.)

                // cheat to avoid a pile of dictionary key BS. 
                if (inst.Param1 == '1' || _registers[inst.Param1] != 0)
                {
                    _instPtr--;
                    _instPtr += Param2;
                }
                break;
            default:
                throw new NotImplementedException(inst.OpCode);
        }

        return State.Running;
    }
}