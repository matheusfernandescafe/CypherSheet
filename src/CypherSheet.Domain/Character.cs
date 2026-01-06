using System;
using System.Collections.Generic;
using System.Linq;

namespace CypherSheet.Domain
{
    public enum DamageTrack
    {
        Hale,
        Impaired,
        Debilitated,
        Dead
    }

    public class Character
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; set; } = "Novo Personagem";
        public string Descriptor { get; set; } = "";
        public string Type { get; set; } = "";
        public string Focus { get; set; } = "";
        public int Tier { get; set; } = 1;
        public int XP { get; set; } = 0;

        public Pool Might { get; private set; }
        public Pool Speed { get; private set; }
        public Pool Intellect { get; private set; }

        public List<Ability> Abilities { get; private set; } = new List<Ability>();
        public Recovery Recovery { get; private set; } = new Recovery();
        
        // Needed for serialization
        public Character() 
        {
            Might = new Pool("Might", 10);
            Speed = new Pool("Speed", 10);
            Intellect = new Pool("Intellect", 10);
        }

        public Character(string name, Pool might, Pool speed, Pool intellect)
        {
            Name = name;
            Might = might;
            Speed = speed;
            Intellect = intellect;
        }

        public DamageTrack GetDamageTrack()
        {
            int poolsAtZero = 0;
            if (Might.Current == 0) poolsAtZero++;
            if (Speed.Current == 0) poolsAtZero++;
            if (Intellect.Current == 0) poolsAtZero++;

            return poolsAtZero switch
            {
                0 => DamageTrack.Hale,
                1 => DamageTrack.Impaired,
                2 => DamageTrack.Debilitated,
                _ => DamageTrack.Dead
            };
        }
    }

    public class Ability
    {
        public string Name { get; set; } = "";
        public string Cost { get; set; } = "";
        public string Description { get; set; } = "";
        public string Source { get; set; } = ""; // Type/Focus/Descriptor
    }

    public class Recovery
    {
        public bool OneActionUsed { get; set; }
        public bool TenMinutesUsed { get; set; }
        public bool OneHourUsed { get; set; }
        public bool TenHoursUsed { get; set; }

        public void Reset()
        {
            OneActionUsed = false;
            TenMinutesUsed = false;
            OneHourUsed = false;
            TenHoursUsed = false;
        }
        
        public int GetRollBonus(int tier)
        {
            return tier; // Simplification, usually d6 + tier
        }
    }
}
