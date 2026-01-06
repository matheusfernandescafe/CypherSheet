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

    public enum SkillLevel
    {
        Inability,
        Trained,
        Specialized
    }

    public class Character
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "Novo Personagem";
        public string Descriptor { get; set; } = "";
        public string Type { get; set; } = "";
        public string Focus { get; set; } = "";
        public int Tier { get; set; } = 1;
        public int XP { get; set; } = 0;

        public Pool Might { get; set; }
        public Pool Speed { get; set; }
        public Pool Intellect { get; set; }

        public int Effort { get; set; } = 1;
        public int Armor { get; set; } = 0;

        public bool Impaired { get; set; } = false;
        public bool Debilitated { get; set; } = false;

        private List<Ability> _abilities = new();
        public List<Ability> Abilities { get => _abilities; set => _abilities = value ?? new(); }

        private List<Skill> _skills = new();
        public List<Skill> Skills { get => _skills; set => _skills = value ?? new(); }

        private List<Cypher> _cyphers = new();
        public List<Cypher> Cyphers { get => _cyphers; set => _cyphers = value ?? new(); }

        private List<Item> _equipment = new();
        public List<Item> Equipment { get => _equipment; set => _equipment = value ?? new(); }

        private List<Note> _notes = new();
        public List<Note> Notes { get => _notes; set => _notes = value ?? new(); }
        
        public Recovery Recovery { get; set; } = new Recovery();
        
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
        public string Source { get; set; } = ""; 
    }

    public class Skill
    {
        public string Name { get; set; } = "";
        public SkillLevel Level { get; set; } = SkillLevel.Trained;
        public string Source { get; set; } = "";
    }

    public class Cypher
    {
        public string Name { get; set; } = "";
        public string Level { get; set; } = "";
        public string Effect { get; set; } = "";
        public string Source { get; set; } = "";
    }

    public class Item
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int Quantity { get; set; } = 1;
    }

    public class Note
    {
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
    }

    public class Recovery
    {
        public bool OneActionUsed { get; set; }
        public bool TenMinutesUsed { get; set; }
        public bool OneHourUsed { get; set; }
        public bool TenHoursUsed { get; set; }
        public int CustomRollBonus { get; set; } = 0;

        public void Reset()
        {
            OneActionUsed = false;
            TenMinutesUsed = false;
            OneHourUsed = false;
            TenHoursUsed = false;
        }
        
        public int GetRollBonus(int tier)
        {
            return tier + CustomRollBonus;
        }
    }
}
