namespace AoC_2022_Day_23
{
    internal record Elf
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string ID { get; }

        public bool bounce { get; set; }

        public int proposed_X { get; set; }
        public int proposed_Y { get; set; }

        public Elf(string ID, int x, int y)
        {
            this.ID = ID;
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"{ID} : ({X.ToString() + ", " + Y.ToString()})";
        }
    }

}
