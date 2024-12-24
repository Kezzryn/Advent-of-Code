namespace AoC_2024_Day_24;

//Shamelessly reused from 2015 Day 7
internal record Node
{
    public string SourceA;
    public string SourceB;
    public Ops Op;
    public long? Value = null;

    public Node(long value)
    {
        SourceA = "";
        SourceB = "";
        Op = Ops.VALUE;
        Value = value;
    }
    public Node(string input)
    {
        string[] temp = input.Split(' ');

        SourceA = "";
        SourceB = "";

        //mrg XOR wrd -> z32

        SourceA = temp[0];
        SourceB = temp[2];
        Op = temp[1] switch
        {
                "XOR" => Ops.XOR,
                "AND" => Ops.AND,
                "OR" => Ops.OR,
                _ => throw new Exception($"Unknown op {temp[1]}")
        };
    }

    public enum Ops
    {
        AND,
        OR,
        XOR, 
        VALUE
    }
}