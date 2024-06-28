using Microsoft.Win32;

namespace BKH.AoC_AssemBunny
{
    internal enum OpCode
    {
        CPY, INC, DEC, JNZ, TGL, OUT, HALT
    }

    internal class InstructionNode()
    {
        public OpCode Op;
        public int ParamA = 0;
        public bool ParamAIsReg = false;
        public int ParamB = 0;
        public bool ParamBIsReg = false;
    }

    public enum State
    {
        Running,
        Halted,
        Paused_For_Input,
        Paused_For_Output
    };

    public class AssemBunny
    {
       
        private readonly Dictionary<int, int> _registers = new()
        {
            { 'a', 0 },
            { 'b', 0 },
            { 'c', 0 },
            { 'd', 0 }
        };

        private readonly List<InstructionNode> _instructionSet = [];
        private int _instPtr = 0;

        public readonly Queue<int> OutputQueue = new();

        public AssemBunny(string[] program)
        {
            foreach (string instruction in program)
            {
                string[] instr = instruction.Split(' ');

                InstructionNode node = new();
                node.Op = instr[0] switch
                {
                    "cpy" => OpCode.CPY,
                    "inc" => OpCode.INC,
                    "dec" => OpCode.DEC,
                    "jnz" => OpCode.JNZ,
                    "tgl" => OpCode.TGL,
                    "out" => OpCode.OUT,
                    _ => throw new NotImplementedException(instr[0])
                };
                node.ParamAIsReg = !int.TryParse(instr[1], out int paramA);
                node.ParamA = node.ParamAIsReg ? instr[1][0] : paramA;

                if (instr.GetUpperBound(0) == 2)
                {
                    node.ParamBIsReg = !int.TryParse(instr[2], out int paramB);
                    node.ParamB = node.ParamBIsReg ? instr[2][0] : paramB;
                }

                _instructionSet.Add(node);
            }
        }
        public bool SetRegister(char name, int value)
        {
            if (!_registers.ContainsKey(name)) return false;
            _registers[name] = value;
            return true;
        }

        public int GetRegister(char name)
        {
            if (_registers.TryGetValue(name, out int value)) return value;
            throw new Exception($"Unknown register. {name}");
        }

        public State Run()
        {
            State currentState;
            do
            {
                if (_instPtr >= _instructionSet.Count || _instPtr < 0) return State.Halted;
                currentState = Dispatcher(_instructionSet[_instPtr++]);
            } while (currentState == State.Running);

            return currentState;
        }

        public void Reset()
        {
            if (_instructionSet.Any(x => x.Op == OpCode.TGL)) throw new Exception("Unable to reset self modify code.");

            _instPtr = 0;
            _registers['a'] = 0;
            _registers['b'] = 0;
            _registers['c'] = 0;
            _registers['d'] = 0;
        }

        private State Dispatcher(InstructionNode currentNode)
        {
            switch (currentNode.Op)
            {
                case OpCode.CPY:
                    if (_registers.ContainsKey(currentNode.ParamB))
                    {
                        _registers[currentNode.ParamB] = currentNode.ParamAIsReg ? _registers[currentNode.ParamA] : currentNode.ParamA;
                    }
                    break;
                case OpCode.INC:
                    _registers[currentNode.ParamA]++;
                    break;
                case OpCode.DEC:
                    _registers[currentNode.ParamA]--;
                    break;
                case OpCode.JNZ:
                    bool isNotZero = (currentNode.ParamAIsReg ? _registers[currentNode.ParamA] : currentNode.ParamA) != 0;

                    int target = currentNode.ParamBIsReg ? _registers[currentNode.ParamB] : currentNode.ParamB;

                    //drop one from the target to adjust for auto increment of _instPtr 
                    if (isNotZero) _instPtr += --target;
                    break;
                case OpCode.TGL:
                    int tgl_offset = _instPtr - 1 + (currentNode.ParamAIsReg ? _registers[currentNode.ParamA] : currentNode.ParamA);

                    // target outside program, do nothing. 
                    if (0 > tgl_offset || tgl_offset >= _instructionSet.Count) return State.Running;
                    
                    _instructionSet[tgl_offset].Op = _instructionSet[tgl_offset].Op switch
                    {
                        //For one-argument instructions, inc becomes dec, and all other one-argument instructions become inc.
                        OpCode.INC => OpCode.DEC,
                        OpCode.DEC => OpCode.INC,
                        OpCode.TGL => OpCode.INC,

                        //For two-argument instructions, jnz becomes cpy, and all other two-instructions become jnz.
                        OpCode.JNZ => OpCode.CPY,
                        OpCode.CPY => OpCode.JNZ,
                        _ => throw new NotImplementedException(),
                    };
                    break;
                case OpCode.OUT:
                    OutputQueue.Enqueue(_registers[currentNode.ParamA]);
                    return State.Paused_For_Output;
                default:
                    throw new NotImplementedException();
            }
            return State.Running;
        }
    }
}