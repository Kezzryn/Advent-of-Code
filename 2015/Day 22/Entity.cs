﻿namespace AoC_2015_Day_22
{
    internal abstract class Entity
    {
        protected int _hp;
        protected int _armor;
        public readonly Dictionary<SpellNames, Effect> Effects = [ ];
        public Entity(int hp, int armor)
        {
            _hp = hp;
            _armor = armor;
        }
        public bool IsDead => _hp <= 0;
        public void GainHP(int health) => _hp += health;
        public void LoseHP(int damage) => _hp -= int.Max(damage - _armor, 1);
        public void Bleed(int damage) => _hp -= damage;
        public void GainArmor(int armor) => _armor += armor;
        public void LoseArmor(int armor) => _armor -= int.Min(armor, _armor);
        public void GainEffect(Effect effect) => Effects.Add(effect.SpellName, effect);
        public void DoEffects() 
        {
            List<SpellNames> expiredEffects = [];
            foreach ((SpellNames spellName, Effect effect) in Effects)
            {
                int numberTurnsRemaing = effect.DoUpkeep(this);
                if (numberTurnsRemaing <= 0) expiredEffects.Add(spellName);
            }
            foreach (SpellNames spellName in expiredEffects) Effects.Remove(spellName);
        }
        public bool HasEffect(SpellNames spellName) => Effects.ContainsKey(spellName);
    }
    internal class Boss : Entity
    {
        private readonly int _damage;

        public Boss(string[] stats)
            : base(0,0)
        {
            foreach (string stat in stats)
            {
                string[] split = stat.Split(':');
                switch (split[0])
                {
                    case "Hit Points":
                        _hp = int.Parse(split[1]);
                        break;
                    case "Damage":
                        _damage = int.Parse(split[1]);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public Boss(Boss clone)
            : base(clone._hp, clone._armor)
        {
            _damage = clone._damage;
            foreach (Effect effect in clone.Effects.Values)
            {
                GainEffect(effect.Clone()); 
            }
        }

        public void Attack(Entity target) => target.LoseHP(_damage);
    }
    internal class Wizard : Entity
    {
        private int _mana = 0;
        public int ManaSpent = 0;

        public readonly List<string> actionList = [];

        private static readonly Dictionary<SpellNames, Spell> _spellBook = new()
        {
            { SpellNames.MagicMissile,  new MagicMissile() },
            { SpellNames.Drain,         new Drain() },
            { SpellNames.Shield,        new Shield() },
            { SpellNames.Poison,        new Poison() },
            { SpellNames.Recharge,      new Recharge() }
        };

        public Wizard(int hp, int mana)
            : base(hp, 0)
        {
            _mana = mana;
        }

        public Wizard(Wizard clone)
            : base(clone._hp, clone._armor)
        {
            _mana = clone._mana;
            ManaSpent = clone.ManaSpent;
            foreach (Effect effect in clone.Effects.Values)
                GainEffect(effect.Clone());
            
            foreach (string action in clone.actionList)
                actionList.Add(action);
        }

        public void GainMana(int mana) => _mana += mana;

        public void LoseMana(int mana)
        {
            ManaSpent += mana;
            _mana -= mana;
        }

        public bool CastSpell(SpellNames spell, Entity target)
        {
            if (_mana <= _spellBook[spell].ManaCost) return false;
            if (HasEffect(spell) || target.HasEffect(spell)) return false;
            
            _spellBook[spell].Cast(this, target);

            actionList.Add(spell.ToString());
            return true;
        }
    }
}