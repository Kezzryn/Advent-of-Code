using System.Drawing;

namespace AoC_2018_Day_15
{
    internal class Entity
    {
        public static int Elf = 0;
        public static int Goblin = 1;

        public int Type { get; }

        public string TypeName => Type == Entity.Goblin ? "G" : "E";

        public int HP { get; set; } = 200;

        public int Attack { get; set; } = 3;

        public Entity(int type, int attackBoost = 0)
        {
            Type = type;
            Attack = 3 + attackBoost;
        }
    }
}
