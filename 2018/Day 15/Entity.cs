using System.Drawing;

namespace AoC_2018_Day_15
{
    internal class Entity
    {
        public static int Elf = 0;
        public static int Goblin = 1;

        public int Type { get; }

//        public Point Position { get; set; }

        public int HP { get; set; } = 200;

        public int Attack { get; set; } = 3;

        public Entity(int type)//, Point startPosition)
        {
            Type = type;
  //          Position = startPosition;
        }

        public Entity(int type, int hp, int attack)
        {
            Type = type;
            //Position = startPosition;
            HP = hp;
            Attack = attack;
        }
    }
}
