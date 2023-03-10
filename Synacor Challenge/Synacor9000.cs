using System.Text;

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

        // IO buffers and builders
        private string _inputBuffer = string.Empty;
        private readonly Queue<string> _outputBuffer = new();
        private readonly StringBuilder _sbOutput = new();

        public enum State
        {
            Running,
            Halted,
            Paused_For_Input
        };

        public Synacor9000() {}

        public State Run()
        {
            State currentState;
            do
            {
                currentState = Dispatcher(Ptr_ReadValue());
            } while (currentState == State.Running);

            return currentState;
        }

        public bool GetProgramOutput(out string output)
        {
            output = string.Empty;
            if (_outputBuffer.Count == 0) return false;

            output = _outputBuffer.Dequeue();
            return true;
        }

        public void SetProgramInput(string input)
        {
            _inputBuffer = input;
            if (_inputBuffer != string.Empty && _inputBuffer[^1] != '\n') _inputBuffer += '\n';
        }
    }
}