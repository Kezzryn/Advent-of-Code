namespace Synacor_Challenge
{
    internal static class Decompiler
    {
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

        static public void DumpIt(BinaryReader reader, StreamWriter writer)
        {
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                ushort value = reader.ReadUInt16();

                ushort address = (ushort)(reader.BaseStream.Position / 2);
                ushort param1 = 0;
                ushort param2 = 0;
                ushort param3 = 0;
                string value1 = "";
                string value2 = "";
                string value3 = "";

                if (instructionSet.TryGetValue(value, out var instKey))
                {
                    if (instKey.numParam >= 0)
                    {
                        //value1 = "";
                    }
                    if (instKey.numParam >= 1)
                    {
                        param1 = reader.ReadUInt16();
                        value1 = param1.ToString();
                        if (value == 19)
                        {
                            if (param1 >= 32768)
                            {
                                value2 = $"reg[{param1 % 32768}]";
                            } 
                            else
                            {
                                value2 = (param1 == '\n') ? "\\n" : ((char)param1).ToString();
                            }
                        }
                    }
                    if (instKey.numParam >= 2)
                    {
                        param2 = reader.ReadUInt16();
                        value2 = param2.ToString();
                    }
                    if (instKey.numParam >= 3)
                    {
                        param3 = reader.ReadUInt16();
                        value3 = param3.ToString();
                    }
                    if (instKey.numParam >= 4 || instKey.numParam < 0)
                    {
                        throw new NotImplementedException($"Unknown instkey {instKey.numParam}");
                    }
                } 
                else
                {
                    value1 = value.ToString();
                }

                writer.Write($"{Convert.ToString(address,16),8}{address,8}{((instKey.instString is null) ? "" : instKey.instString),8}{value1,8}{value2,8}{value3,8}");
                writer.WriteLine($"{Convert.ToString(param1,16),20}{Convert.ToString(param2,16),20}{Convert.ToString(param3, 16),20}");
            }
        } 
    }
}
