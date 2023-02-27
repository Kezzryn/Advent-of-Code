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
    internal class Spell
    {
        protected const int _manaCost = 0;
        public virtual void Cast(Wizard source, Entity target) => throw new NotImplementedException();
        public virtual int ManaCost() => throw new NotImplementedException();
        public virtual SpellNames SpellName => throw new NotImplementedException();
    }
    internal class Effect : Spell
    {
        protected int _upkeepDuration;
        public int Duration => _upkeepDuration;
  
        // returns true when expired.
        public virtual bool Upkeep(Wizard source) => throw new NotImplementedException();
        public virtual bool Upkeep(Entity source) => throw new NotImplementedException();
        public virtual Effect Clone() => throw new NotImplementedException();
    }
    internal class MagicMissile : Spell
    {
        public override SpellNames SpellName => SpellNames.MagicMissile;
        private new const int _manaCost = 53;
        private const int _damage = 4;
        public override int ManaCost() => _manaCost;
        public override void Cast(Wizard source, Entity target)
        {
            source.LoseMana(_manaCost);
            target.LoseHP(_damage);
        }
    }
    internal class Drain : Spell
    {
        public override SpellNames SpellName => SpellNames.Drain;
        private new const int _manaCost = 73;
        private const int _leech = 2;
        public override int ManaCost() => _manaCost;
        public override void Cast(Wizard source, Entity target)
        {
                source.LoseMana(_manaCost);
                source.GainHP(_leech);
                target.LoseHP(_leech);
        }
    }
    internal class Shield : Effect
    {
        public override SpellNames SpellName => SpellNames.Shield;
        private new const int _manaCost = 113;
        private const int _armorAmount = 7;

        public Shield(int duration = 6)
        {
            _upkeepDuration = duration;
        }
        public override int ManaCost() => _manaCost;
        public override void Cast(Wizard source, Entity _)
        {
            source.LoseMana(_manaCost);
            source.GainArmor(_armorAmount);
            source.GainEffect(new Shield());
        }
        public override bool Upkeep(Entity source)
        {
            _upkeepDuration--;
            if (_upkeepDuration <= 0)
            {
                source.LoseArmor(_armorAmount);
                return true;
            }
            return false;
        }
        public override Effect Clone() =>  new Shield(_upkeepDuration);
    }
    internal class Poison : Effect
    {
        public override SpellNames SpellName => SpellNames.Poison;
        private new const int _manaCost = 173;
        private const int _damage = 3;

        public Poison(int duration = 6)
        {
            _upkeepDuration = duration;
        }
        public override int ManaCost() => _manaCost;
        public override void Cast(Wizard source, Entity target)
        {
            source.LoseMana(_manaCost);
            target.GainEffect(new Poison());
        }
        public override bool Upkeep(Entity source)
        {
            source.LoseHP(_damage);
            _upkeepDuration--;
            return _upkeepDuration <= 0;
        }
        public override Effect Clone() => new Poison(_upkeepDuration);
    }
    internal class Recharge : Effect
    {
        public override SpellNames SpellName => SpellNames.Recharge;
        private new const int _manaCost = 229;
        private const int _recharge = 101;
        public Recharge(int duration = 5)
        {
            _upkeepDuration = duration;
        }
        public override int ManaCost() => _manaCost;
        public override void Cast(Wizard source, Entity _)
        {
            source.LoseMana(_manaCost);
            source.GainEffect(new Recharge());
        }
        public override bool Upkeep(Entity source) => Upkeep((Wizard)source);
        public override bool Upkeep(Wizard source)
        {
            source.GainMana(_recharge);
            _upkeepDuration--;
            return _upkeepDuration <= 0;
        }
        public override Effect Clone() => new Recharge(_upkeepDuration);
    }
}