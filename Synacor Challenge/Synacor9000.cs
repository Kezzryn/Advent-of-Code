namespace Synacor_Challenge
{
    internal partial class Synacor9000
    {
        const ushort MEMORY_MAX = 32767; // 0 bound address max
        const ushort MODULO = 32768;
        const ushort INVALID_MEMORY = 32776; // start of invalid memory numbers. 

        // memory regions. 
        private readonly ushort[] _mainMemory = new ushort[32768];
        private readonly ushort[] _registers = new ushort[8];
        private readonly Stack<ushort> _stack = new();

        public Synacor9000() {}
        public void Load(BinaryReader reader)
        {
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                _mainMemory[reader.BaseStream.Position / 2] = reader.ReadUInt16();
            }
        }
        public void Run()
        {
            while (true)
            {
                Dispatcher(Ptr_ReadValue());
            }
        }
        private void Dispatcher(ushort instruction)
        {
            switch (instruction)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1 or 15 or 16:
                    Mem_Manipulation(instruction);
                    break;
                case 2 or 3:
                    Mem_Stack(instruction);
                    break;
                case 4 or 5:
                    Comparison(instruction);
                    break;
                case 6 or 7 or 8 or 17 or 18:
                    _instPtr = Jump(instruction);
                    break;
                case >= 9 and <= 14:
                    Math_Synacor(instruction);
                    break;
                case 19 or 20:
                    Terminal(instruction);
                    break;
                case 21:
                    // noop: 21  no operation 
                    break;
                default:
                    throw new NotImplementedException($"{instruction}");
            }
        }
        private void Terminal(ushort instruction)
        {
            switch (instruction)
            {
                case 19:                                    // out: 19 a    write the character represented by ascii code <a> to the terminal
                    Console.Write((char)Ptr_ReadValue());
                    break;
                case 20:                                    // in: 20 a     read a character from the terminal and write its ascii code to <a>;
                    // it can be assumed that once input starts, it will continue until a newline is encountered;
                    // this means that you can safely read whole lines from the keyboard and trust that they will be fully read
                    ushort address = Ptr_ReadRaw();
                    ushort value = (ushort)Console.Read();
                    Mem_Write(address, value);
                    break;
                default:
                    throw new NotImplementedException();    // PEBCAK 
            }
        }
    }
}
