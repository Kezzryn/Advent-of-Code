namespace AoC_2022_Day_23
{
    internal record Elf
    {
        public Elf(string ID, int x, int y)
        {
            this.ID = ID;
            X = x;
            Y = y;
        }

        public string ID { get; }

        public int X { get; set; }

        public int Y { get; set; }

        public override string ToString() => $"{ID} : ({X}, {Y})";
    }
}
