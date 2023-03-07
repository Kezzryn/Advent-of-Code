namespace Synacor_Challenge
{
    internal partial class Synacor9000
    {
        /*
         * This file contains the save/load functionality.
         * This is implemented as a simple stream dump of each memory location, main, registers, stack and instPtr in order and seperated by FFFF.
         */
        private const ushort MARKER = ushort.MaxValue;

        public bool Load(string fileName, out string? errorMessage)
        {
            try
            {
                string[] stageText = { "main memory", "registers", "stack", "pointer", "end of file" };

                if (fileName == string.Empty) fileName = DefaultLoadFile;
                using BinaryReader reader = new(new FileStream(fileName, FileMode.Open, FileAccess.Read));

                int stage = 0;
                int memPos = 0;
                Console.WriteLine($"Loading stage {stage} {stageText[stage]}");
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    ushort value = reader.ReadUInt16();
                    if (value == MARKER)
                    {
                        stage++;
                        memPos = 0;
                        Console.WriteLine($"Loading stage {stage} {stageText[stage]}");
                    }
                    else
                    {
                        switch (stage)
                        {
                            case 0: // main memory 
                                _mainMemory[memPos++] = value;
                                break;
                            case 1: // registers
                                _registers[memPos++] = value;
                                break;
                            case 2: // stack
                                _stack.Push(value);
                                break;
                            case 3: // pointer. 
                                _instPtr = value;
                                break;
                            //case 4:
                            default:
                                throw new NotImplementedException($"Too many stages. Stage = {stage}");
                        }
                    }
                }

                // Any other variables that need resetting?
                _inputBuffer = string.Empty;

                // We had a basic bin. Clear out the non-main mem stuff.
                if (stage == 0) 
                {
                    _stack.Clear();
                    _instPtr = USHORT_0;
                    for (int i = 0 ; i < _registers.Length; i++)
                    {
                        _registers[i] = USHORT_0;  
                    }
                }
                Console.WriteLine($"Load of {fileName} done.");
                errorMessage = null;
                return true;
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

                for (int i = 0; i <= _mainMemory.GetUpperBound(0); i++)
                {
                    bw.Write(_mainMemory[i]);
                }
                bw.Write(MARKER); // memory done

                for (int i = 0; i <= _registers.GetUpperBound(0); i++)
                {
                    bw.Write(_registers[i]);
                }
                bw.Write(MARKER); // registers done
                
                //reverse as the enumerator returns LIFO.
                foreach (ushort stackValue in _stack.Reverse())
                {
                    bw.Write(stackValue);
                }
                bw.Write(MARKER); // stack done

                bw.Write(_instPtr);
                bw.Write(MARKER); // ptr done

                Console.WriteLine($"Save of {fileName} done.");
                errorMessage = null;
                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }
        }
    }
}