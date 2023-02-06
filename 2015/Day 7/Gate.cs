namespace AoC_2015_Day_7
{
	static class Ops
	{
		public const int And = 0;
		public const int Or = 1;
		public const int LShift = 2;
		public const int RShift = 3;
		public const int Not = 4;
		public const int Wire = 5;
	}

	internal record Node
	{
		public string SourceA;
		public string SourceB;
		public int Op;
		public UInt16? Value;
		

		public Node(string a, string b, int op)
		{
			SourceA = a;
			SourceB = b;
			Op = op;
			Value = null;
		}

		public Node(UInt16 value)
		{
			SourceA = "";
            SourceB = "";
			Op = Ops.Wire;
            Value = value;
		}
	}
}
