namespace Synacor_Challenge
{
    internal static class Decompiler
    {
        static public readonly Dictionary<int, (string inst, int num)> instructionSet = new()
        {
            {  0, ("halt", 0) },
            {  1, ("set", 2) },
            {  2, ("push", 1) },
            {  3, ("pop", 1) },
            {  4, ("eq", 3) },
            {  5, ("gt", 3) },
            {  6, ("jmp", 1) },
            {  7, ("jt", 2) },
            {  8, ("jf", 2) },
            {  9, ("1dd", 3) },
            { 10, ("mult", 3) },
            { 11, ("mod", 3) },
            { 12, ("1nd", 3) },
            { 13, ("or", 3) },
            { 14, ("not", 2) },
            { 15, ("rmem", 2) },
            { 16, ("wmem", 2) },
            { 17, ("c1ll", 1) },
            { 18, ("ret", 0 ) },
            { 19, ("out", 1) },
            { 20, ("in", 1) },
            { 21, ("noop", 0) }
        };

        static public void DumpIt(BinaryReader reader)//, StreamWriter writer)
        {

            // address 
            // instruction 
            // parameters (up to 3) 
            // column formatted :P 

            //0: mult 12345 12345 12345

            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            while (reader.BaseStream.Position < 1000) //r.BaseStream.Length)
            {
                int instr = reader.ReadUInt16();

                string line;
                Console.Write($"{reader.BaseStream.Position / 2}  {instr}: {instructionSet[instr].inst} ");

                switch (instr)
                {
                    case 19:
                        int nextChar = reader.ReadUInt16();
                        line = (nextChar == '\n') ? "newline" : ((char)nextChar).ToString();
                    break;
                default:
                    line = instructionSet[instr].num switch
                    {
                        1 => $"{reader.ReadUInt16()}",
                        2 => $"{reader.ReadUInt16()} {reader.ReadUInt16()}",
                        3 => $"{reader.ReadUInt16()} {reader.ReadUInt16()} {reader.ReadUInt16()}",
                        _ => ""
                    };
                    break;
                };

                Console.WriteLine(line);    
            }
        } 

        static public string InstToString(int instruction)
        {
            return instructionSet.TryGetValue(instruction, out var value) ? value.inst : $"Unknown: {instruction}";
        }
    }
}
