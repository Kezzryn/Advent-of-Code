namespace Synacor_Challenge
{
    internal partial class Synacor9000
    {
        /*
         * This file contains the save/load functionality.
         * This is implemented as a simple stream dump of each memory location, main, registers, stack and instPtr in order and separated by FFFF. The load will read back in the same order.
         * The only partial load supported is one of main memory only.
         */
        private const ushort MARKER = ushort.MaxValue;

        public bool Load(string fileName, out string resultMessage)
        {
            try
            {
                string[] stageText = { "main memory", "registers", "stack", "pointer", "end of file" };
                resultMessage = "";

                if (fileName == string.Empty) fileName = DefaultLoadFile;
                using BinaryReader reader = new(new FileStream(fileName, FileMode.Open, FileAccess.Read));
                
                int stage = 0;
                int memPos = 0;
                resultMessage += $"Loading stage {stage} {stageText[stage]}\n";
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    ushort value = reader.ReadUInt16();
                    if (value == MARKER)
                    {
                        stage++;
                        memPos = 0;
                        resultMessage += $"Loading stage {stage} {stageText[stage]}\n";
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
                            default:
                                throw new NotImplementedException($"Too many stages. Stage = {stage}");
                        }
                    }
                }

                // Any other variables that need resetting?
                _inputBuffer = string.Empty;

                // We have a basic bin. Clear out the non-main mem stuff.
                if (stage == 0) 
                {
                    _stack.Clear();
                    _instPtr = USHORT_0;
                    for (int i = 0 ; i < _registers.Length; i++)
                    {
                        _registers[i] = USHORT_0;  
                    }
                }

                resultMessage = $"Load of {fileName} done.";
                return true;
            }
            catch (Exception e)
            {
                resultMessage = e.Message;
                return false;
            }
        }

        public bool Save(string fileName, out string resultMessage)
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

                resultMessage = $"Save of {fileName} done.";
                return true;
            }
            catch (Exception e)
            {
                resultMessage = e.Message;
                return false;
            }
        }
    }
}