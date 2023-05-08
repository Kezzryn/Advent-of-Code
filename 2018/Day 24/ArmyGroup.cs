using System.Text.RegularExpressions;

namespace AoC_2018_Day_24
{
    enum DamageTypes
    {
        Normal,
        Bludgeoning,
        Cold,
        Fire,
        Radiation,
        Slashing
    }

    internal class ArmyGroup
    {
        private readonly int _unitHP = 0;
        private readonly int _attackDamage = 0;

        private readonly List<DamageTypes> _immunities = new();
        private readonly List<DamageTypes> _weaknessess = new();

        public int Units { get; set; } = 0;
        public DamageTypes DamageType { get; } = DamageTypes.Normal;
        public int Initative { get; } = 0;
        public int EffectivePower => Units * _attackDamage;
        public bool IsTargetted { get; set; } = false; 
        public int TargetIndex { get; set; } = 0;

        public ArmyGroup(string input, int boostDamage = 0)
        {
            static DamageTypes StringToDamage(string s)
            {
                return s switch
                {
                    "bludgeoning" => DamageTypes.Bludgeoning,
                    "cold" => DamageTypes.Cold,
                    "fire" => DamageTypes.Fire,
                    "radiation" => DamageTypes.Radiation,
                    "slashing" => DamageTypes.Slashing,
                    _ => DamageTypes.Normal
                };
            }

            //18 units each with 729 hit points (weak to fire; immune to cold, slashing) with an attack that does 8 radiation damage at initiative 10
            //bracket content is optional. 

            var regMatch = Regex.Match(input, @"(\d+) units each with (\d+) hit points ?(.+)? with an attack that does (\d+) (.+) damage at initiative (\d+)");

            Units = int.Parse(regMatch.Groups[1].Value);
            _unitHP = int.Parse(regMatch.Groups[2].Value);
            Initative = int.Parse(regMatch.Groups[^1].Value);
            
            _attackDamage = 
                    boostDamage
                    + (int.TryParse(regMatch.Groups[3].Value, out int result) 
                        ? result 
                        : int.Parse(regMatch.Groups[4].Value));
            
            DamageType = StringToDamage(regMatch.Groups[^2].Value);

            if (input.Contains('('))
            {
                foreach (string s in regMatch.Groups[3].Value[1..^1].Split("; "))
                {
                    if (s.StartsWith("weak to"))
                    {
                        _weaknessess = s[8..].Split(", ", StringSplitOptions.TrimEntries).Select(StringToDamage).ToList();
                    }

                    if (s.StartsWith("immune to"))
                    {
                        _immunities = s[10..].Split(", ", StringSplitOptions.TrimEntries).Select(StringToDamage).ToList();
                    }
                }
            }
        }

        public int DamageTest(ArmyGroup attacker)
        {
            if (_immunities.Contains(attacker.DamageType)) return 0;

            return attacker.EffectivePower * (_weaknessess.Contains(attacker.DamageType) ? 2 : 1);
        }

        public int TakeDamage(ArmyGroup attacker)
        {
            int unitsLost = DamageTest(attacker) / _unitHP;
            int returnValue = int.Min(Units, unitsLost);

            Units -= unitsLost;
            return returnValue;
        }
    }
}
