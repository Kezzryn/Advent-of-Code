namespace AoC_2015_Day_07
{
	internal record Node
	{
		public string SourceA;
		public string SourceB;
		public Ops Op;
		public ushort? Value;

		public Node(string input)
		{
			string[] temp = input.Split(' ');

            SourceA = "";
            SourceB = "";

            // Four patterns. 
            //12345 -> aa
            //aa -> bb
            //NOT aa -> bb
            //aa [AND|OR|LSHIFT|RSHIFT] bb -> cc
            if (ushort.TryParse(input, out ushort value))
			{
				Value = value;
				Op = Ops.Wire;
            } 
            else if (temp.Length == 1)
            {
                SourceA = temp[0];
                Op = Ops.Wire;
            }
			else if ( temp[0] == "NOT")
			{
				SourceA = temp[1];
                Op = Ops.Not;
            } 
			else
			{
                SourceA = temp[0];
                SourceB = temp[2];
                Op = temp[1] switch
                {
                    "OR" => Ops.Or,
                    "AND" => Ops.And,
                    "LSHIFT" => Ops.LShift,
                    "RSHIFT" => Ops.RShift,
                    _ => throw new Exception($"Unknown op {temp[1]}")
                };
            }
		}

        public enum Ops
        {
            And,
            Or,
            LShift,
            RShift,
            Not,
            Wire
        }
    }
}
