namespace Synacor_Challenge
{
    internal partial class Synacor9000
    {
        private const ushort MEMORY_MAX = 32767; // 0 bound address max
        private const ushort MODULO = 32768;
        private const ushort INVALID_MEMORY = 32776; // start of invalid memory numbers. 

        public const string DefaultSaveFile = "quicksave.bin";
        public const string DefaultLoadFile = "challenge.bin";

        // memory regions. 
        private readonly ushort[] _mainMemory = new ushort[32768];
        private readonly ushort[] _registers = new ushort[8];
        private readonly Stack<ushort> _stack = new();

        private bool _stopExecution; 

        public Synacor9000() {}
   
        public void Run()
        {
            _stopExecution = false;
            _inputBuffer = string.Empty;

            while (!_stopExecution)
            {
                Dispatcher(Ptr_ReadValue());
            }
        }
    }
}
