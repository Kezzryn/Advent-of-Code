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

        private string? _inputBuffer = string.Empty;
        private bool _stopExecution; 

        public Synacor9000() {}
        public bool Load(string fileName, out string? errorMessage)
        {
            try
            {
                if (fileName == string.Empty) fileName = DefaultLoadFile;
                using BinaryReader reader = new(new FileStream(fileName, FileMode.Open, FileAccess.Read));

                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    ushort address = (ushort)(reader.BaseStream.Position / 2);
                    ushort value = reader.ReadUInt16();

                    if (address < INVALID_MEMORY)
                    {
                        Mem_Write(address, value);
                    }
                    else
                    {
                        break;
                    }
                } // done main memory 

                //try the stack. 
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    ushort value = reader.ReadUInt16();
                    if (value == ushort.MaxValue) break;
                    _stack.Push(value);
                }
                
                if (reader.BaseStream.Position < reader.BaseStream.Length) _instPtr = reader.ReadUInt16();

                errorMessage = null;
                return true;
                // TODO LOAD SCREEN STATE? 

                // need to add the capacity to load registers and the stack.
                // registers are easy, they're just higher things.. 
                // need to restore the state of the _instrPtr as well. 
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }
        }
        public bool Save(string fileName, out string? errorMessage)
        {
            try
            {
                if (fileName == string.Empty) fileName = DefaultSaveFile;
                using BinaryWriter bw = new(new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write));

                for (ushort i = 0; i < INVALID_MEMORY; i++)
                {
                    bw.Write(Mem_Read(i));
                }
                foreach (ushort stackValue in _stack.Reverse())
                {
                    bw.Write(Mem_Read(stackValue));
                }
                bw.Write(ushort.MaxValue); // end of stack marker
                bw.Write(_instPtr);

                errorMessage = null;
                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }
            
            // TODO SAVE SCREEN STATE? 

            // A stack can be enumerated without disturbing its contents.
           // foreach (string number in numbers)
            //{
            //    Console.WriteLine(number);
           // }
           //NOTE This return LAST PUSHED ITEM FIRST 
           // push 1, push 2, push 3, returns 3,2,1 

            // need to add the capacity to save registers and the stack.
            // registers are easy, they're just higher memory things. 
            // need to restore the state of the _instrPtr as well. 
        }
        public void Run()
        {
            _stopExecution = false;
            while (!_stopExecution)
            {
                Dispatcher(Ptr_ReadValue());
            }
        }
        private void Dispatcher(ushort instruction)
        {
            switch (instruction)
            {
                case 0:
                    _stopExecution = true;
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
            const char NEWLINE = '\n';

            switch (instruction)
            {
                case 19:         // out: 19 a    write the character represented by ascii code <a> to the terminal
                    Console.Write((char)Ptr_ReadValue());
                    break;
                case 20:        // in: 20 a     read a character from the terminal and write its ascii code to <a>;
                    // it can be assumed that once input starts, it will continue until a newline is encountered;
                    // this means that you can safely read whole lines from the keyboard and trust that they will be fully read
                    const string CMD_EXIT = "exit";

                    StringBuilder sb = new();
                    if (string.IsNullOrEmpty(_inputBuffer))
                    {
                        ConsoleKeyInfo cki;
                        bool doneInput = false;
                        do
                        {
                            cki = Console.ReadKey();
                            switch (cki.Key)
                            {
                                //case ConsoleKey.F5:
                                //    Console.WriteLine("Quicksave...");
                                //   Save("",out _);
                                //   break;
                                case ConsoleKey.F7:
                                    //Console.WriteLine("Quickload...");
                                    //Load("", out _);
                                //    break;
                                case ConsoleKey.Escape:
                                    sb.Clear();
                                    sb.Append(CMD_EXIT);
                                    _stopExecution = true;
                                    doneInput = true;
                                    break;
                                case ConsoleKey.Backspace:
                                    sb = sb.Remove(sb.Length - 1, 1);
                                    // backspace moves the cursor back, so whitespace, then \b to back the cursor up again. 
                                    Console.Write(" \b");
                                    break;
                                case ConsoleKey.Enter:
                                    Console.WriteLine();
                                    sb.Append(NEWLINE);
                                    doneInput = true;
                                    break;
                                default:
                                    if ((cki.Modifiers & ConsoleModifiers.Control) != 0 && cki.Key == ConsoleKey.C)
                                    {
                                        sb.Clear();
                                        sb.Append(CMD_EXIT);
                                        doneInput = true;
                                        break;
                                    }
                                    sb.Append(cki.KeyChar);
                                    break;
                            }
                        } while (!doneInput);

                        // don't pass back ENTER only.
                        // capture other stuff. 
                        if (string.IsNullOrEmpty(sb.ToString()) || sb.ToString()[0] == NEWLINE)
                        {
                            _inputBuffer = string.Empty;
                        } 
                        else
                        {
                            _inputBuffer = sb.ToString();
                            switch (_inputBuffer[..^1].ToLower()) // strip newline
                            {
                                case CMD_EXIT:
                                    _stopExecution = true;
                                    break;
                                default:
                                    break;
                            }
                        }
                    } // end reading from the console. 

                    if (_stopExecution || string.IsNullOrEmpty(_inputBuffer))
                    {
                        _instPtr--; // back this up to what should be the instruction, so if/when we resume we don't freak out.
                        break;
                    }
                    else
                    {
                        ushort value = _inputBuffer[0];
                        ushort address = Ptr_ReadRaw();

                        _inputBuffer = _inputBuffer.Length > 1 ? _inputBuffer[1..] : string.Empty;
                       
                        Mem_Write(address, value);
                    }

                    break;
                default:
                    throw new NotImplementedException();    // PEBCAK 
            }
        }
    }
}
