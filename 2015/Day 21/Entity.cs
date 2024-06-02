namespace AoC_2015_Day_21
{
    internal class Entity
    {
        public int HP { get; set; }
        public int Damage { get; set; }
        public int Armor { get; set; }
        public int GoldCost { get; set; }
        public int MyEquipment { get; set; }

        // Boss load. 
        public Entity(string[] stats)
        { 
            foreach(string stat in stats)
            {
                string[] split = stat.Split(':');
                switch (split[0])
                {
                    case "Hit Points":
                        HP = int.Parse(split[1]);
                        break;
                    case "Damage":
                        Damage = int.Parse(split[1]);
                        break;
                    case "Armor":
                        Armor = int.Parse(split[1]);
                        break;
                }
            }
            GoldCost = 0;
        }

        // Initial player setup, and debug handles.
        public Entity(int hp, int damage, int armor, int gold)
        {
            HP = hp;
            Damage = damage;
            Armor = armor;
            GoldCost = gold;
        }

        // Clone
        public Entity(Entity clone)
        {
            HP = clone.HP;
            Damage = clone.Damage;
            Armor = clone.Armor;
            GoldCost = clone.GoldCost;
            MyEquipment = clone.MyEquipment;
        }

        public void AddEquipment(int equipmentID)
        {
            // Make sure we don't double equip things. 
            if ((MyEquipment & equipmentID) == 0)
            {
                (int g, int d, int a) = Equipment.GetEquipment(equipmentID);
                MyEquipment |= equipmentID;
                Damage += d;
                Armor += a;
                GoldCost += g;
            }
        }
        public bool BeatBoss(Entity opponent)
        {
            // Magic formula to round up integer division. (x + y - 1) ÷ y 
            // Return true if we win, false if we fail.
            int timeToKill = (opponent.HP + (int.Max(Damage - opponent.Armor, 1) - 1)) / int.Max(Damage - opponent.Armor, 1);
            int timeToDie = (HP + (int.Max(opponent.Damage - Armor, 1) - 1)) / int.Max(opponent.Damage - Armor, 1);

            return timeToKill <= timeToDie;
        } 
    }
}
