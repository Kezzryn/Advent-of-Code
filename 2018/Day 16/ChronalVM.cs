namespace AoC_2018_Day_16
{
    internal class ChronalVM
    {
        private readonly int[] _registers = new int[4];
        
        public void Dispatcher(int opCode, int inA, int inB, int output)
        {
            _registers[output] = opCode switch
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
                _ => throw new NotImplementedException()
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

        public void SetRegisters(int[] newRegisters)
        {
            if (newRegisters.Length != _registers.Length) throw new Exception();
            newRegisters.CopyTo(_registers, 0);
        }
    }
}
