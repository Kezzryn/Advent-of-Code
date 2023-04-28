namespace AoC_2018_Day_19
{
    internal class ChronalVM
    {
        private readonly int[] _registers = new int[6];

        private readonly int _boundRegister = 0;
        private readonly List<int[]> _instructions = new();

        public ChronalVM(string[] instructions)
        {
            Dictionary<string, int> opCodes = new()
            {
                { "addr", 0 },
                { "addi", 1 },
                { "mulr", 2 },
                { "muli", 3 },
                { "banr", 4 },
                { "bani", 5 },
                { "borr", 6 },
                { "bori", 7 },
                { "setr", 8 },
                { "seti", 9 },
                { "gtir", 10 },
                { "gtri", 11 },
                { "gtrr", 12 },
                { "eqir", 13 },
                { "eqri", 14 },
                { "eqrr", 15 }
            };

            //big assumption that the first instruction line is a bind command.
            _boundRegister = int.Parse(instructions[0].Split(' ').Last());

            foreach (string[] splitLine in instructions[1..].Select(x => x.Split(' ').ToArray()))
            {
                _instructions.Add(new int[] { opCodes[splitLine[0]], int.Parse(splitLine[1]), int.Parse(splitLine[2]), int.Parse(splitLine[3]) } );
            }
        }

        public void Run()
        {
            bool isDone = false;
            //_instPtr = 0;
            _registers[_boundRegister] = 0;
            while (!isDone)
            {
                Dispatcher(_instructions[_registers[_boundRegister]]);
                _registers[_boundRegister]++;
                if (_registers[_boundRegister] >= _instructions.Count) isDone = true;
            }
        }

        public void Dispatcher(int[] instruction)
        {
            const int OPCODE = 0;
            const int IN_A = 1;
            const int IN_B = 2;
            const int OUTPUT = 3;

            int inA = instruction[IN_A];
            int inB = instruction[IN_B];

            _registers[instruction[OUTPUT]] = instruction[OPCODE] switch
            {
                0 => _registers[inA] + _registers[inB],          // addr
                1 => _registers[inA] + inB,                      // addi

                2 => _registers[inA] * _registers[inB],          // mulr
                3 => _registers[inA] * inB,                      // muli

                4 => _registers[inA] & _registers[inB],          // banr                
                5 => _registers[inA] & inB,                      // bani

                6 => _registers[inA] | _registers[inB],          // borr
                7 => _registers[inA] | inB,                      // bori

                8 => _registers[inA],                            // setr
                9 => inA,                                        // seti

                10 => inA > _registers[inB] ? 1 : 0,             // gtir
                11 => _registers[inA] > inB ? 1 : 0,             // gtri
                12 => _registers[inA] > _registers[inB] ? 1 : 0, // gtrr
                
                13 => inA == _registers[inB] ? 1 : 0,            // eqir
                14 => _registers[inA] == inB ? 1 : 0,            // eqri
                15 => _registers[inA] == _registers[inB] ? 1 : 0,// eqrr
                _ => throw new NotImplementedException($"{instruction[OPCODE]} not implemented")
            };
        }

        public void ClearRegisters() => Array.Clear(_registers);

        public bool CompareToRegisters(int[] a)
        {
            if (a.Length != _registers.Length) return false;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != _registers[i]) return false;
            }
            return true;
        }

        public int GetRegisterValue(int regNum) => _registers[regNum];

        public void SetRegister(int register, int value)  => _registers[register] = value;

        public void SetRegisters(int[] newRegisters)
        {
            if (newRegisters.Length != _registers.Length) throw new Exception();
            newRegisters.CopyTo(_registers, 0);
        }
    }
}
