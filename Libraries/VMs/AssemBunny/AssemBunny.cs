﻿namespace BKH.AoC_AssemBunny
{
    internal enum OpCode
    {
        CPY, INC, DEC, JNZ, HALT
    }

    internal class InstructionNode()
    {
        public OpCode Op;
        public int ParamA = 0;
        public bool ParamAIsReg = false;
        public int ParamB = 0;
        public bool ParamBIsReg = false;
    }

    public class AssemBunny
    {
        public enum State
        {
            Running,
            Halted,
            Paused_For_Input
        };
        
        private readonly Dictionary<int, int> _registers = new()
        {
            { 'a', 0 },
            { 'b', 0 },
            { 'c', 0 },
            { 'd', 0 }
        };

        private readonly List<InstructionNode> _instructionSet = [];
        private int _instPtr = 0;

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

        public void Reset()
        {
            _instPtr = 0;
            foreach (int key in _registers.Keys)
            {
                _registers[key] = 0;
            }
        }

        public State Run()
        {
            State currentState;
            do
            {
                if (_instPtr >= _instructionSet.Count || _instPtr < 0)
                {
                    currentState = State.Halted;
                }
                else
                {
                    currentState = Dispatcher(_instructionSet[_instPtr++]);
                }
            } while (currentState == State.Running);

            return currentState;
        }

        private State Dispatcher(InstructionNode currentNode)
        {
            switch (currentNode.Op)
            {
                case OpCode.CPY:
                    _registers[currentNode.ParamB] = currentNode.ParamAIsReg ? _registers[currentNode.ParamA] : currentNode.ParamA;
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
                default:
                    throw new NotImplementedException();
            }
            return State.Running;
        }
    }
}