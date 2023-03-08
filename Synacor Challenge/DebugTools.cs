using System.Text;

namespace Synacor_Challenge
{
    internal partial class Synacor9000
    {
        private readonly Queue<string> _commandTrace = new();
        private bool _doTrace = false;
        static public readonly Dictionary<int, (string instString, int numParam)> instructionSet = new()
        {
            {  0, ("halt",  0) },
            {  1, ("set",   2) },
            {  2, ("push",  1) },
            {  3, ("pop",   1) },
            {  4, ("eq",    3) },
            {  5, ("gt",    3) },
            {  6, ("jmp",   1) },
            {  7, ("jt",    2) },
            {  8, ("jf",    2) },
            {  9, ("add",   3) },
            { 10, ("mult",  3) },
            { 11, ("mod",   3) },
            { 12, ("and",   3) },
            { 13, ("or",    3) },
            { 14, ("not",   2) },
            { 15, ("rmem",  2) },
            { 16, ("wmem",  2) },
            { 17, ("call",  1) },
            { 18, ("ret",   0) },
            { 19, ("out",   1) },
            { 20, ("in",    1) },
            { 21, ("noop",  0) }
        };

        public bool DebugDispatcher(string instruction, out List<string> returnValue)
        {
            bool success = true;
            var split = instruction.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            returnValue = new();

            // command
            // sub command 
            // args - each command/sub command is responsable for splitting, all the top guarentees is the values are there.
            string command = split[0];
            string sub_cmd = (split.GetUpperBound(0) >= 1) ? split[1] : "nosub";
            string args = instruction[(split[0].Length + split[1].Length + 1)..].Trim();
            Console.WriteLine($"{instruction} => |{command}| |{sub_cmd}| |{args}|");

            switch (command)
            {
                case "set":
                    var argsparse = args.Split(' ').Select(ushort.Parse).ToArray();
                    switch (sub_cmd)
                    {
                        case "register":
                            _registers[argsparse[0]] = argsparse[1];
                            break;
                        case "instptr":
                            _instPtr = argsparse[0];
                            break;
                        case "input":
                            TakeCommand(args);
                            break;
                        case "memory":
                            _mainMemory[argsparse[0]] = argsparse[1];
                            break;
                        default:
                            returnValue.Add($"Unknown instruction {instruction}");
                            break;
                    };
                    break;
                case "dump":
                    switch (sub_cmd)
                    {
                        case "binary":
                            success = DumpBinary("", "", out string resultDumpBin);
                            returnValue.Add(resultDumpBin);
                            break;
                        case "trace":
                            success = DumpCommandTrace("", out string resultDumpTrace);
                            returnValue.Add(resultDumpTrace);
                            break;
                        case "state":
                            returnValue.Add(GetCurrentState());
                            break;
                        default:
                            returnValue.Add($"Unknown instruction {instruction}");
                            break;
                    }
                    break;
                case "toggle":
                    _doTrace = !_doTrace;
                    returnValue.Add($"Command trace is now {(_doTrace ? "on" : "off")}.");
                    break;
                case "step":
                    // read param
                    Step();
                    break;
                case "break":
                /*
                 * add [address]
                 * remove [address]
                 * clear 
                 * show/print
                 */
                default:
                    returnValue.Add($"Unknown instruction {instruction}");
                    break;
            }

            return success;
        }
        static private bool DumpBinary(string inFile, string outFile, out string resultMessage)
        {
            try
            {
                if (inFile == string.Empty) inFile = DefaultLoadFile;
                if (outFile == string.Empty) outFile = $"Full_Dump_{DateTime.Now:yyyyMMddHHmmss}.txt";

                using BinaryReader reader = new(new FileStream(inFile, FileMode.Open, FileAccess.Read));
                using StreamWriter writer = new(new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.Write));

                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    ushort value = reader.ReadUInt16();
                    ushort address = (ushort)((reader.BaseStream.Position / 2) - 1); // zero bound

                    StringBuilder sb = new();

                    if (instructionSet.TryGetValue(value, out var instKey))
                    {
                        sb.Append($"{address,8}");
                        sb.Append($"{instructionSet[value].instString,5}");

                        if (instKey.instString == "out")
                        {
                            do
                            {
                                value = reader.ReadUInt16();
                                sb.Append((value >= MODULO) ? $"reg[{value % MODULO}]" : ((char)value == '\n') ? "\\n" : (char)value);
                            }
                            while (reader.PeekChar() == 19);
                        }
                        else
                        {
                            for (int i = 1; i <= 3; i++)
                            {
                                string lineValue = "";
                                if (instKey.numParam >= i)
                                {
                                    value = reader.ReadUInt16();
                                    lineValue = (value >= MODULO) ? $"reg[{value % MODULO}]" : $"{value}";
                                }
                                sb.Append($"{lineValue, 8}");
                            }
                        }
                    }
                    else
                    {
                        sb.Append(value);
                    }

                    writer.WriteLine(sb);
                }

                resultMessage = $"Dump of {inFile} to {outFile} done.";
                return true;
            }
            catch (Exception e)
            {
                resultMessage = e.Message;
                return false;
            }
        }
        public void BreakOnInstr(ushort targetInst)
        {
            ushort instr;// = Mem_Read(_instPtr);
            do
            {
                Step();
                instr = Mem_Read(_instPtr);
            } while (instr != targetInst);
        }
        public void BreakOnAddress(ushort targetInst)
        {
            do
            {
                Step();
            } while (_instPtr != targetInst);
        }
        public void Step(int numSteps = 1)
        {   
            for (int i = 1; i <= numSteps; i++)
            {
                if (_doTrace) _commandTrace.Enqueue(GetCurrentState());
                Dispatcher(Ptr_ReadValue());
            }
        }
        private bool DumpCommandTrace(string outFile, out string resultsMessage)
        {
            try
            {
                if (outFile == string.Empty) outFile = $"commandDump_{DateTime.Now:yyyyMMddHHmmss}.txt";

                using StreamWriter sw = new(new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.Write));
                sw.WriteLine("    addr inst val_1 val_2 val_3 :  32768 32769 32770 32771 32772 32773 32774 32775");
                while (_commandTrace.Count > 0)
                {
                    sw.WriteLine(_commandTrace.Dequeue());  
                }
                sw.Close();
                    resultsMessage = $"Dumped commands to {outFile}";
                    return true;
            } 
            catch  (Exception e)
            {
                resultsMessage = e.Message;
                return false;
            }
        }

        private string GetCurrentState()
        {
            // change header in DumpCommandTrace if changed. 
            StringBuilder sb = new();
            int numParam = instructionSet[Mem_Read(_instPtr)].numParam;

            sb.Append($"{_instPtr,8}");

            sb.Append($"{instructionSet[Mem_Read(_instPtr)].instString, 5}");
            sb.Append($"{((numParam >= 1) ? Mem_Read((ushort)(_instPtr + 1)) : ""),6}");
            sb.Append($"{((numParam >= 2) ? Mem_Read((ushort)(_instPtr + 2)) : ""),6}");
            sb.Append($"{((numParam >= 3) ? Mem_Read((ushort)(_instPtr + 3)) : ""),6}");
            sb.Append($" : ");
            for (int i = 0; i < 8; i++)
            {
                sb.Append($"{_registers[i],6}");
            }
            return sb.ToString();
        }
    }
}

