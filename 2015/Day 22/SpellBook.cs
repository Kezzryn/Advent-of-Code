namespace AoC_2015_Day_22
{
    enum SpellNames
    {
        MagicMissile,
        Drain,
        Shield,
        Poison,
        Recharge
    }
    
    internal abstract class Spell
    {
        public int ManaCost { get; protected set; }
        public SpellNames SpellName { get; protected set; }
        public abstract void Cast(Wizard source, Entity target);       
    }

    internal abstract class Effect : Spell
    {
        public int Duration { get; protected set; }
        public abstract int DoUpkeep(Entity source);
        public abstract Effect Clone();
    }

    internal class MagicMissile : Spell
    {
        private const int _damage = 4;
        public MagicMissile()
        {
            SpellName = SpellNames.MagicMissile;
            ManaCost = 53;
        }
     
        public override void Cast(Wizard source, Entity target)
        {
            source.LoseMana(ManaCost);
            target.LoseHP(_damage);
        }
    }

    internal class Drain : Spell
    {
        private const int _leech = 2;
        public Drain()
        {
            SpellName = SpellNames.Drain;
            ManaCost = 73;
        }

        public override void Cast(Wizard source, Entity target)
        {
                source.LoseMana(ManaCost);
                source.GainHP(_leech);
                target.LoseHP(_leech);
        }
    }

    internal class Shield : Effect
    {
        private const int _armorAmount = 7;

        public Shield(int duration = 6)
        {
            SpellName = SpellNames.Shield;
            ManaCost = 113;
            Duration = duration;
        }

        public override void Cast(Wizard source, Entity _)
        {
            source.LoseMana(ManaCost);
            source.GainArmor(_armorAmount);
            source.GainEffect(new Shield());
        }

        public override int DoUpkeep(Entity source)
        {
            Duration--;
            if (Duration <= 0)
            {
                source.LoseArmor(_armorAmount);
            }
            return Duration;
        }

        public override Effect Clone() =>  new Shield(Duration);
    }

    internal class Poison : Effect
    {
        private const int _damage = 3;

        public Poison(int duration = 6)
        {
            SpellName = SpellNames.Poison;
            ManaCost = 173;
            Duration = duration;
        }
        
        public override void Cast(Wizard source, Entity target)
        {
            source.LoseMana(ManaCost);
            target.GainEffect(new Poison());
        }

        public override int DoUpkeep(Entity source)
        {
            source.LoseHP(_damage);
            Duration--;
            return Duration;
        }

        public override Effect Clone() => new Poison(Duration);
    }

    internal class Recharge : Effect
    {
        private const int _recharge = 101;

        public Recharge(int duration = 5)
        {
            SpellName = SpellNames.Recharge;
            ManaCost = 229;
            Duration = duration;
        }
        
        public override void Cast(Wizard source, Entity _)
        {
            source.LoseMana(ManaCost);
            source.GainEffect(new Recharge());
        }

        public override int DoUpkeep(Entity source)
        {
            if(source is Wizard wizard) wizard.GainMana(_recharge);
            Duration--;
            return Duration;
        }

        public override Effect Clone() => new Recharge(Duration);
    }
}
