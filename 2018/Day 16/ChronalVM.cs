namespace AoC_2018_Day_16
{
    internal class ChronalVM
    {
        private readonly int[] _registers = new int[4];
        
        public void Dispatcher(int opCode, int inA, int inB, int output)
        {
            _registers[output] = opCode switch
            {
                0 => _registers[inA] * inB,                       // muli
                1 => inA,                                         // seti
                2 => _registers[inA] & inB,                       // bani
                3 => _registers[inA] > inB ? 1 : 0,               // gtri
                4 => _registers[inA] > _registers[inB] ? 1 : 0,   // gtrr
                5 => _registers[inA] == _registers[inB] ? 1 : 0,  // eqrr
                6 => _registers[inA] + inB,                       // addi
                7 => inA > _registers[inB] ? 1 : 0,               // gtir
                8 => inA == _registers[inB] ? 1 : 0,              // eqir
                9 => _registers[inA] * _registers[inB],           // mulr
                10 => _registers[inA] + _registers[inB],          // addr
                11 => _registers[inA] | _registers[inB],          // borr
                12 => _registers[inA] | inB,                      // bori
                13 => _registers[inA] == inB ? 1 : 0,             // eqri
                14 => _registers[inA] & _registers[inB],          // banr
                15 => _registers[inA],                            // setr
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
