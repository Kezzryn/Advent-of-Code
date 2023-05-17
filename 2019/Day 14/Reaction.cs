namespace AoC_2019_Day_14
{
    internal class Reaction
    {
        public static readonly string ORE = "ORE";
        public static readonly string FUEL = "FUEL";

        public string ID { get; set; }
        public long Output { get; set; }
        public List<(string ID, long Amt)> Inputs { get; set; }
        public Reaction(string formula)
        {
            Inputs = new ();

            string[] split = formula.Split("=>", StringSplitOptions.TrimEntries);

            string[] temp = split[1].Split(' ', StringSplitOptions.TrimEntries);
            ID = temp[1];
            Output = long.Parse(temp[0]);

            foreach(string ingredient in split[0].Split(',', StringSplitOptions.TrimEntries))
            {
                temp = ingredient.Split(' ', StringSplitOptions.TrimEntries);
                Inputs.Add((temp[1], long.Parse(temp[0])));
            }
        }
    }

}
