using System.Text;

namespace Synacor_Challenge
{
    internal partial class Synacor9000
    {
        // trace tools.
        private readonly Queue<string> _commandTrace = new();
        private bool _doTrace = false;
        private bool _doTraceEcho = false;
        private bool _doTraceStack = false;
        private int _traceDepth = 1500;

        // break tools. 
        private readonly HashSet<ushort> _breakAddr = new();
        private readonly HashSet<int> _breakInst = new();

        static private readonly Dictionary<int, (string instString, int numParam)> instructionSet = new()
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
        static private readonly List<string> commandList = new()
        {
            "set register [0-7] [value]",
            "set instptr [value]",
            "set memory [value]",
            "set input [value]",
            "dump binary [load file] [save file]",
            "dump trace [save file]",
            "dump memory [address]",
            "dump state",
            "trace toggle",
            "trace depth [value]",
            "step [value]",
            "break addy [value]",
            "break instr [0-21]",
            "break clear",
            "break show",
            "break run"
        };
        public bool DebugDispatcher(string instruction, out List<string> returnValue)
        {
            bool success = true;
            var split = instruction.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            returnValue = new();

            // command
            // sub command 
            // args - each command/sub command is responsable for splitting, all the top guarentees is the variables are not null. (but they might be Empty)
            string command = split[0];
            string sub_cmd = (split.GetUpperBound(0) >= 1) ? split[1] : string.Empty;
            string args = (split.GetUpperBound(0) >= 2) ? instruction[(split[0].Length + split[1].Length + 1)..].Trim() : string.Empty;

            switch (command)
            {
                case "help":
                    returnValue = commandList;
                    break;
                case "set":
                    var argsparse = args.Split(' ');
                    switch (sub_cmd)
                    {
                        case "register":
                            _registers[ushort.Parse(argsparse[0])] = ushort.Parse(argsparse[1]);
                            returnValue.Add($"registr: {argsparse[0]} set to: {argsparse[1]}");
                            break;
                        case "instptr":
                            _instPtr = ushort.Parse(argsparse[0]);
                            returnValue.Add($"instPtr: set to: {argsparse[0]}");
                            break;
                        case "input":
                            SetProgramInput(args);
                            returnValue.Add($"inputBuffer: set to: {args}");
                            break;
                        case "memory":
                            _mainMemory[ushort.Parse(argsparse[0])] = ushort.Parse(argsparse[1]);
                            returnValue.Add($"address: {argsparse[0]} set to: {argsparse[1]}");
                            break;
                        default:
                            success = false;
                            returnValue.Add($"Unknown instruction {instruction}");
                            break;
                    };
                    break;
                case "dump":
                    // todo better filenames
                    var dumpArgs = args.Split(' ');
                    string loadFile = (dumpArgs.GetUpperBound(0) >= 0) ? dumpArgs[0] : string.Empty;
                    string saveFile = (dumpArgs.GetUpperBound(0) >= 1) ? dumpArgs[1] : string.Empty;
                    switch (sub_cmd)
                    {
                        case "binary":
                            success = DumpBinary(loadFile, saveFile, out string resultDumpBin);
                            returnValue.Add(resultDumpBin);
                            break;
                        case "trace":
                            success = DumpCommandTrace(saveFile, out string resultDumpTrace);
                            returnValue.Add(resultDumpTrace);
                            break;
                        case "state":
                            returnValue.Add(GetCurrentState());
                            break;
                        case "memory":
                            if (ushort.TryParse(dumpArgs[0], out ushort address))
                                returnValue.Add(Mem_Read(address).ToString());
                            else
                                returnValue.Add($"Unable to convert {dumpArgs[0]} to ushort.");
                                success = false;
                            break;
                        default:
                            success = false;
                            returnValue.Add($"Unknown instruction {instruction}");
                            break;
                    }
                    break;
                case "trace":
                    int traceDepth = int.TryParse(args, out int trace_parse) ? trace_parse : 200;
                    switch (sub_cmd)
                    {
                        case "cmd":
                            _doTrace = !_doTrace;
                            returnValue.Add($"Command trace is now {(_doTrace ? "on" : "off")}.");
                            break;
                        case "depth":
                            _traceDepth = traceDepth;
                            returnValue.Add($"Trace depth is now the last {_traceDepth} commands.");
                            break;
                        case "stack":
                            _doTraceStack = !_doTraceStack;
                            returnValue.Add($"Stack trace is now {(_doTraceStack ? "on" : "off")}.");
                            break;
                        case "echo":
                            _doTraceEcho = !_doTraceEcho;
                            returnValue.Add($"Command trace console echo is now {(_doTraceEcho ? "on" : "off")}.");
                            break;
                        default:
                            success = false;
                            returnValue.Add($"Unknown instruction {instruction}");
                            break;
                    }
                    break;
                case "step":
                    // Oh look the only thing without subcommands, just to fuck us up on the parsing.
                    int numSteps = int.TryParse(sub_cmd, out int step_parse) ? step_parse : 1;
                    State stepState = Step(numSteps);
                    if (!_doTraceEcho) returnValue.Add($"Stepped {numSteps} state {stepState}");
                    break;
                case "break":
                    ushort value = ushort.TryParse(args, out ushort break_parse) ? break_parse : USHORT_0;
                    switch (sub_cmd)
                    {
                        case "inst":
                            _breakInst.Add(value);
                            returnValue.Add($"Added instruction breakpoint {value}");
                            break;
                        case "addy":
                            _breakAddr.Add(value);
                            returnValue.Add($"Added address breakpoint {value}");
                            break;
                        case "clear":
                            _breakAddr.Clear();
                            _breakInst.Clear();
                            returnValue.Add($"Cleared all breakpoints.");
                            break;
                        case "show":
                            returnValue.Add("Current breakpoints:");
                            if (_breakInst.Count > 0) foreach(var s in _breakInst) returnValue.Add($"inst: {s}");
                            if (_breakAddr.Count > 0) foreach(var s in _breakAddr) returnValue.Add($"addr: {s}");
                            break;
                        case "run":
                            ushort instr;
                            bool isDone = false;
                            // State stepState; declared in "step" 
                            do
                            {
                                stepState = Step();
                                if (stepState != State.Running)
                                {
                                    returnValue.Add($"Program entered {stepState}");
                                    isDone = true;
                                }

                                instr = Mem_Read(_instPtr);
                                if (_breakInst.Contains(instr))
                                {
                                    returnValue.Add($"Break on instruction {instructionSet[instr].instString}");
                                    isDone = true;
                                }

                                if (_breakAddr.Contains(_instPtr))
                                {
                                    returnValue.Add($"Break on address {_instPtr}");
                                    isDone = true;
                                }
                            } while (!isDone);
                            while (GetProgramOutput(out string output))
                            {
                                returnValue.Add($"PROGRAM OUT: {output}"); ;
                            }
                            break;
                        default:
                            success = false;
                            returnValue.Add($"Unknown instruction {instruction}");
                            break;
                    }
                    break;
                default:
                    success = false;
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
                    sb.Append($"{address,8}");

                    if (instructionSet.TryGetValue(value, out var instKey))
                    {
                        sb.Append($"{instructionSet[value].instString,5}");

                        if (instKey.instString == "out")
                        {
                            value = reader.ReadUInt16();
                            sb.Append((value >= MODULO) ? $"reg[{value % MODULO}]" : (value == '\n' ? "\\n" : (char)value));
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
                                sb.Append($"{lineValue,8}");
                            }
                        }
                    }
                    else
                    {
                        sb.Append($"{value,8}"); // instr & param spacer
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
        private State Step(int numSteps = 1)
        {
            State returnValue = State.Running;
            for (int i = 1; i <= numSteps; i++)
            {
                if (_doTrace)
                {
                    _commandTrace.Enqueue(GetCurrentState());
                    if (_commandTrace.Count > _traceDepth) _commandTrace.Dequeue();
                }
                if (_doTraceEcho) Console.WriteLine(GetCurrentState());
                returnValue = Dispatcher(Ptr_ReadValue());
                
                if (returnValue != State.Running) break;
            }
            return returnValue;
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
            catch (Exception e)
            {
                resultsMessage = e.Message;
                return false;
            }
        }
        private string GetCurrentState()
        {
            // change header in DumpCommandTrace if format changes. 
            StringBuilder sb = new();
            int numParam = instructionSet[Mem_Read(_instPtr)].numParam;

            sb.Append($"{_instPtr,8}");
            sb.Append($"{instructionSet[Mem_Read(_instPtr)].instString,5}");

            sb.Append($"{((numParam >= 1) ? Mem_Read((ushort)(_instPtr + 1)) : ""),6}");
            sb.Append($"{((numParam >= 2) ? Mem_Read((ushort)(_instPtr + 2)) : ""),6}");
            sb.Append($"{((numParam >= 3) ? Mem_Read((ushort)(_instPtr + 3)) : ""),6}");
            sb.Append($" : ");

            for (int i = 0; i < 8; i++)
            {
                sb.Append($"{_registers[i],6}");
            }

            if (_doTraceStack)
            {
                sb.Append($" : ");
                foreach (ushort value in _stack.Reverse())
                {
                    sb.Append($"{value,6}");
                }
            }

            return sb.ToString();
        }
    }
}