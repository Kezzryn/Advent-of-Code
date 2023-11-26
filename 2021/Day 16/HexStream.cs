namespace AoC_2021_Day_16
{
    internal class Packet
    {
        public int Version { get; private set; }
        public int TypeID { get; private set; }
        public long Value { get; private set; }
        public List<Packet> SubPackets { get; set; }
        public Packet(int version, int typeID, long value = -1)
        {
            Version = version;
            TypeID = typeID;
            Value = value;
            SubPackets = new();
        }
    }

    internal class HexStream
    {
        private int _cursor = 0;
        private readonly string _hexString;
        private string _buffer = String.Empty;

        public HexStream(string baseData, bool isBinary = false)
        {
            if (isBinary)
            {
                // used for processing fixed sized buffers
                _hexString = String.Empty;
                _buffer = baseData;
            }
            else
            {
                _hexString = baseData;
            }
        }

        private void FillBuffer(int length)
        {
            while (_buffer.Length < length && _cursor < _hexString.Length)
            {
                _buffer += Convert.ToString(
                    Convert.ToInt32(_hexString[_cursor++].ToString(), 16), 2)
                    .PadLeft(4, '0');
            }
        }

        private int ReadBuffer(int length) => Convert.ToInt32(ReadRawBuffer(length), 2); 

        private string ReadRawBuffer(int length)
        {
            if (_buffer.Length < length) FillBuffer(length);
            string _returnValue = _buffer[0..length];
            _buffer = _buffer[length..];

            return _returnValue;
        }
        private long GetLiteralValue()
        {
            bool isDone;
            string value = String.Empty;

            do
            {
                isDone = ReadBuffer(1) == 0;
                value += ReadRawBuffer(4);
            } while (!isDone);

            return Convert.ToInt64(value, 2);
        }

        public bool TryGetPacket(out Packet? returnPacket)
        {
            returnPacket = null;
            if (_cursor >= _hexString.Length && String.IsNullOrEmpty(_buffer)) return false;

            int version = ReadBuffer(3);
            int typeID = ReadBuffer(3);

            if (typeID == 4)
            {
                returnPacket = new(version, typeID, GetLiteralValue());
                return true;
            }

            // now the "fun" begins. 
            int lengthID = ReadBuffer(1);
            switch (lengthID)
            {
                case 0:
                    // fixed buffer size thingy
                    int bufferLength = ReadBuffer(15);
                    HexStream tempStream = new(ReadRawBuffer(bufferLength), true);

                    returnPacket = new(version, typeID);
                    while (tempStream.TryGetPacket(out Packet? p))
                    {
                        returnPacket.SubPackets.Add(p!);
                    }
                    break;
                case 1:
                    // number of sub-packets 
                    int numPackets = ReadBuffer(11);
                    returnPacket = new(version, typeID);
                    for (int i = 0; i < numPackets; i++)
                    {
                        if (TryGetPacket(out Packet? p))
                        {   
                            returnPacket.SubPackets.Add(p!);
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException($"length ID = {lengthID}");
            }
            
            return true;
        }
    }
}

