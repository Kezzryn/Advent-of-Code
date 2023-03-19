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

        static private readonly Dictionary<int, (string instString, int numParam)> _instructionSet = new()
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
        static private readonly List<string> _commandList = new()
        {
            "break addy [value]",
            "break clear",
            "break instr [0-21]",
            "break run",
            "break show",
            "dump binary [load file] [save file]",
            "dump memory [address]",
            "dump state",
            "dump trace [save file]",
            "set input [value]",
            "set instptr [value]",
            "set memory [value]",
            "set register [0-7] [value]",
            "step [value]",
            "trace cmd",
            "trace depth [value]",
            "trace echo",
            "trace stack"
        };

        private readonly Queue<string> _debugOutputBuffer = new();

        public bool DebugDispatcher(string instruction)
        {
            // monster case statement to parse and execute Debug commands against the currently loaded binary. 
            // instruction format is expected to be:
            // command sub_cmd args
            // each command/sub command is responsable for splitting out its own args.
            // All the top level guarantees is the variables are not null.

            bool success = true;
            var split = instruction.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string command = split[0];
            string sub_cmd = (split.GetUpperBound(0) >= 1) ? split[1] : string.Empty;
            string args = (split.GetUpperBound(0) >= 2) ? instruction[(split[0].Length + split[1].Length + 1)..].Trim() : string.Empty;

            State stepState; // used in "break run" and "step" commands

            switch (command)
            {
                case "break":
                    ushort value = ushort.TryParse(args, out ushort break_parse) ? break_parse : USHORT_0;
                    switch (sub_cmd)
                    {
                        case "addy":
                            _breakAddr.Add(value);
                            _debugOutputBuffer.Enqueue($"Added address breakpoint {value}");
                            break;
                        case "clear":
                            _breakAddr.Clear();
                            _breakInst.Clear();
                            _debugOutputBuffer.Enqueue($"Cleared all breakpoints.");
                            break;
                        case "inst":
                            _breakInst.Add(value);
                            _debugOutputBuffer.Enqueue($"Added instruction breakpoint {value}");
                            break;
                        case "run":
                            ushort instr;
                            bool isDone = false;
                            do
                            {
                                stepState = Step();
                                if (stepState != State.Running)
                                {
                                    _debugOutputBuffer.Enqueue($"Program entered {stepState}");
                                    isDone = true;
                                }

                                instr = Mem_Read(_instPtr);
                                if (_breakInst.Contains(instr))
                                {
                                    _debugOutputBuffer.Enqueue($"Break on instruction {_instructionSet[instr].instString}");
                                    isDone = true;
                                }

                                if (_breakAddr.Contains(_instPtr))
                                {
                                    _debugOutputBuffer.Enqueue($"Break on address {_instPtr}");
                                    isDone = true;
                                }
                            } while (!isDone);

                            while (GetProgramOutput(out string output))
                            {
                                _debugOutputBuffer.Enqueue($"PROGRAM OUT: {output}");
                            }

                            break;
                        case "show":
                            _debugOutputBuffer.Enqueue("Current breakpoints:");
                            if (_breakInst.Count > 0) foreach (var s in _breakInst) _debugOutputBuffer.Enqueue($"inst: {s}");
                            if (_breakAddr.Count > 0) foreach (var s in _breakAddr) _debugOutputBuffer.Enqueue($"addr: {s}");
                            break;
                        default:
                            success = false;
                            _debugOutputBuffer.Enqueue($"Unknown instruction {instruction}");
                            break;
                    }
                    break;
                case "dump":
                    // TODO better filename parsing
                    var dumpArgs = args.Split(' ');
                    string loadFile = (dumpArgs.GetUpperBound(0) >= 0) ? dumpArgs[0] : string.Empty;
                    string saveFile = (dumpArgs.GetUpperBound(0) >= 1) ? dumpArgs[1] : string.Empty;
                    switch (sub_cmd)
                    {
                        case "binary":
                            success = DumpBinary(loadFile, saveFile);
                            break;
                        case "memory":
                            if (ushort.TryParse(dumpArgs[0], out ushort address))
                                _debugOutputBuffer.Enqueue(Mem_Read(address).ToString());
                            else
                                _debugOutputBuffer.Enqueue($"Unable to convert {dumpArgs[0]} to ushort.");
                            success = false;
                            break;
                        case "state":
                            _debugOutputBuffer.Enqueue(GetCurrentState());
                            break;
                        case "trace":
                            success = DumpCommandTrace(saveFile);
                            break;
                        default:
                            success = false;
                            _debugOutputBuffer.Enqueue($"Unknown instruction {instruction}");
                            break;
                    }
                    break;
                case "help":
                    foreach (string s in _commandList) _debugOutputBuffer.Enqueue(s);
                    break;
                case "set":
                    var argsparse = args.Split(' ');
                    switch (sub_cmd)
                    {
                        case "input":
                            SetProgramInput(args);
                            _debugOutputBuffer.Enqueue($"inputBuffer: set to: {args}");
                            break;
                        case "instptr":
                            _instPtr = ushort.Parse(argsparse[0]);
                            _debugOutputBuffer.Enqueue($"instPtr: set to: {argsparse[0]}");
                            break;
                        case "memory":
                            _mainMemory[ushort.Parse(argsparse[0])] = ushort.Parse(argsparse[1]);
                            _debugOutputBuffer.Enqueue($"address: {argsparse[0]} set to: {argsparse[1]}");
                            break;
                        case "register":
                            _registers[ushort.Parse(argsparse[0])] = ushort.Parse(argsparse[1]);
                            _debugOutputBuffer.Enqueue($"register: {argsparse[0]} set to: {argsparse[1]}");
                            break;
                        default:
                            success = false;
                            _debugOutputBuffer.Enqueue($"Unknown instruction {instruction}");
                            break;
                    };
                    break;
                case "step":
                    // Oh look the only thing without subcommands, just to mess us our nice scheme.
                    int numSteps = int.TryParse(sub_cmd, out int step_parse) ? step_parse : 1;
                    stepState = Step(numSteps);
                    if (!_doTraceEcho) _debugOutputBuffer.Enqueue($"Stepped {numSteps} state {stepState}");
                    break;

                case "trace":
                    int traceDepth = int.TryParse(args, out int trace_parse) ? trace_parse : 200;
                    switch (sub_cmd)
                    {
                        case "cmd":
                            _doTrace = !_doTrace;
                            _debugOutputBuffer.Enqueue($"Command trace is now {(_doTrace ? "on" : "off")}.");
                            break;
                        case "depth":
                            _traceDepth = traceDepth;
                            _debugOutputBuffer.Enqueue($"Trace depth is now the last {_traceDepth} commands.");
                            break;
                        case "echo":
                            _doTraceEcho = !_doTraceEcho;
                            _debugOutputBuffer.Enqueue($"Command trace console echo is now {(_doTraceEcho ? "on" : "off")}.");
                            break;
                        case "stack":
                            _doTraceStack = !_doTraceStack;
                            _debugOutputBuffer.Enqueue($"Stack trace is now {(_doTraceStack ? "on" : "off")}.");
                            break;
                        default:
                            success = false;
                            _debugOutputBuffer.Enqueue($"Unknown instruction {instruction}");
                            break;
                    }
                    break;
                default:
                    success = false;
                    _debugOutputBuffer.Enqueue($"Unknown instruction {instruction}");
                    break;
            }
            return success;
        }

        private bool DumpBinary(string inFile, string outFile)
        {
            // renders a binary Synacor Challenge file into human readable text.
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

                    if (_instructionSet.TryGetValue(value, out var instKey))
                    {
                        sb.Append($"{_instructionSet[value].instString,5}");

                        if (instKey.instString == "out")
                        {
                            value = reader.ReadUInt16();
                            sb.Append($"{((value >= MODULO) ? $"reg[{value % MODULO}]" : (value == '\n' ? "\\n" : (char)value)),8}");
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

                _debugOutputBuffer.Enqueue($"Dump of {inFile} to {outFile} done.");
                return true;
            }
            catch (Exception e)
            {
                _debugOutputBuffer.Enqueue(e.Message);
                return false;
            }
        }

        private State Step(int numSteps = 1)
        {
            // performs [numSteps] instructions. 
            State returnValue = State.Running;
            for (int i = 1; i <= numSteps; i++)
            {
                if (_doTrace)
                {
                    _commandTrace.Enqueue(GetCurrentState());
                    if (_commandTrace.Count > _traceDepth) _commandTrace.Dequeue();
                }
                if (_doTraceEcho) _debugOutputBuffer.Enqueue(GetCurrentState());
                returnValue = Dispatcher(Ptr_ReadValue());

                if (returnValue != State.Running) break;
            }
            return returnValue;
        }

        private bool DumpCommandTrace(string outFile)
        {
            // Writes contents of the command trace queue to a file, emptying it in the process. 
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
                _debugOutputBuffer.Enqueue($"Dumped commands to {outFile}");
                return true;
            }
            catch (Exception e)
            {
                _debugOutputBuffer.Enqueue(e.Message);
                return false;
            }
        }

        private string GetCurrentState()
        {
            // Returns a string that contains the current pointer, the currnet instruction, the registers,
            // and optionally, the contents of the stack. 

            // NB! Change header in DumpCommandTrace if format changes.
            StringBuilder sb = new();
            int numParam = _instructionSet[Mem_Read(_instPtr)].numParam;

            sb.Append($"{_instPtr,8}");
            sb.Append($"{_instructionSet[Mem_Read(_instPtr)].instString,5}");

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

        public bool GetDebugOutput(out string output)
        {
            output = string.Empty;
            if (_debugOutputBuffer.Count == 0) return false;

            output = _debugOutputBuffer.Dequeue();
            return true;
        }
    }
}