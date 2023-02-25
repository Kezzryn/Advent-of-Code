namespace AoC_2015_Day_21
{
    internal static class Equipment
    {
        public static bool HasArmor(int equipment) => Armor.Keys.Select(x => x & equipment).Where(x => x == 0).Count() != Armor.Count;
        public static bool HasRings(int equipment) => Rings.Keys.Select(x => x & equipment).Where(x => x == 0).Count() != Rings.Count;
        public static (int GoldCost, int Damage, int Armor) GetEquipment(int id)
        {
            if (Rings.TryGetValue(id, out var equipment)) return equipment;
            if (Armor.TryGetValue(id, out equipment)) return equipment;
            if (Weapons.TryGetValue(id, out equipment)) return equipment;
            throw new NotImplementedException($"Unknown equipment ID: {id}");
        }
        // tag (gold, damage, armor) 
        public static Dictionary<int, (int GoldCost, int Damage, int Armor)> Weapons = new()
        {
            { 1 << 1, ( 8, 4, 0) }, // Dagger
            { 1 << 2, (10, 5, 0) }, // Shortsword
            { 1 << 3, (25, 6, 0) }, // Warhammer
            { 1 << 4, (40, 7, 0) }, // Longsword
            { 1 << 5, (74, 8, 0) }  // Greataxe 
        };
        // tag (gold, damage, armor) 
        public static Dictionary<int, (int GoldCost, int Damage, int Armor)> Armor = new()
        {
            { 1 << 6,  ( 13, 0, 1) }, // Leather
            { 1 << 7,  ( 31, 0, 2) }, // Chainmail
            { 1 << 8,  ( 53, 0, 3) }, // Splintmail
            { 1 << 9,  ( 75, 0, 4) }, // Bandedmail
            { 1 << 10, (102, 0, 5) }  // Platemail
        };
        // tag (gold, damage, armor) 
        public static Dictionary<int, (int GoldCost, int Damage, int Armor)> Rings = new()
        {
            // One handed
            { 1 << 11, ( 25, 1, 0) },
            { 1 << 12, ( 50, 2, 0) },
            { 1 << 13, (100, 3, 0) },
            { 1 << 14, ( 20, 0, 1) },
            { 1 << 15, ( 40, 0, 2) },
            { 1 << 16, ( 80, 0, 3) },

            // Two handed combos 
            // Damage + Damage 
            { 1 << 17, ( 75, 3, 0) }, // +1 +2
            { 1 << 18, (125, 4, 0) }, // +1 +3
            { 1 << 19, (150, 5, 0) }, // +2 +3

            // Armor + Armor
            { 1 << 20, ( 60, 0, 3) }, // +1 +2
            { 1 << 21, (100, 0, 4) }, // +1 +3
            { 1 << 22, (140, 0, 5) }, // +2 +3
                
            // +1 damage +x Armor 
            { 1 << 23, ( 45, 1, 1) },
            { 1 << 24, ( 65, 1, 2) },
            { 1 << 25, (105, 1, 3) },

            // +2 damage +x Armor 
            { 1 << 26, ( 70, 2, 1) },
            { 1 << 27, ( 90, 2, 2) },
            { 1 << 28, (130, 2, 3) },

            // +3 damage +x Armor 
            { 1 << 29, (120, 3, 1) },
            { 1 << 30, (140, 3, 2) },
            { 1 << 31, (180, 3, 3) }
        };  
    }
}
