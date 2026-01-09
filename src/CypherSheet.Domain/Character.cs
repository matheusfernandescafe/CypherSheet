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

    public enum SkillType
    {
        Might,
        Speed,
        Intellect
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
        public int TotalXPEarned { get; set; } = 0;
        public int AdvancementsUsed { get; set; } = 0;
        
        // Character Advancement Options - Count how many times each was used
        public int IncreaseCapabilitiesCount { get; set; } = 0;
        public int MoveTowardPerfectionCount { get; set; } = 0;
        public int ExtraEffortCount { get; set; } = 0;
        public int SkillTrainingCount { get; set; } = 0;
        public int OtherAdvancementCount { get; set; } = 0;

        // Track advancement types used in current tier
        public List<string> CurrentTierAdvancementTypes { get; set; } = new List<string>();

        public int GetTotalAdvancementsUsed()
        {
            return IncreaseCapabilitiesCount + MoveTowardPerfectionCount + 
                   ExtraEffortCount + SkillTrainingCount + OtherAdvancementCount;
        }

        public int GetCurrentTierAdvancementsUsed()
        {
            return CurrentTierAdvancementTypes.Count;
        }

        public bool CanUseAdvancementType(string advancementType)
        {
            // Can't use the same type twice in the same tier
            return !CurrentTierAdvancementTypes.Contains(advancementType);
        }

        public void ResetAllProgressions()
        {
            XP = 0;
            TotalXPEarned = 0;
            Tier = 1;
            IncreaseCapabilitiesCount = 0;
            MoveTowardPerfectionCount = 0;
            ExtraEffortCount = 0;
            SkillTrainingCount = 0;
            OtherAdvancementCount = 0;
            CurrentTierAdvancementTypes.Clear();
            Effort = 1; // Reset effort to base value
        }

        public void CheckTierAdvancement()
        {
            var currentTierAdvancements = GetCurrentTierAdvancementsUsed();
            if (currentTierAdvancements >= 4)
            {
                Tier++;
                // Reset advancement types for new tier
                CurrentTierAdvancementTypes.Clear();
            }
        }

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
        
        public decimal Money { get; set; } = 0;
        
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
        public SkillType Type { get; set; } = SkillType.Might;
        public string Page { get; set; } = "";
        public string Source { get; set; } = ""; 
    }

    public class Skill
    {
        public string Name { get; set; } = "";
        public SkillLevel Level { get; set; } = SkillLevel.Trained;
        public SkillType Type { get; set; } = SkillType.Might;
        public string Description { get; set; } = "";
        public string Source { get; set; } = "";
    }

    public enum CypherType
    {
        Cypher,
        Artifact
    }

    public class Cypher
    {
        public string Name { get; set; } = "";
        public string Level { get; set; } = "";
        public string Depletion { get; set; } = "";
        public string Description { get; set; } = "";
        public bool IsActive { get; set; } = true;
        public CypherType Type { get; set; } = CypherType.Cypher;
        public string Source { get; set; } = "";
    }

    public enum ItemType
    {
        Armadura,
        Arma,
        Roupa,
        Ferramenta,
        Estranheza,
        Material,
        Municao,
        Plano,
        Outro
    }

    public enum ItemLocation
    {
        Consigo,
        Mochila,
        Casa
    }

    public class Item
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public ItemType Type { get; set; } = ItemType.Outro;
        public int Quantity { get; set; } = 1;
        public decimal Value { get; set; } = 0;
        public ItemLocation Location { get; set; } = ItemLocation.Mochila;
    }

    public enum NoteType
    {
        Local,
        Personagem,
        Item,
        Missao,
        Outro
    }

    public class Note
    {
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public NoteType Type { get; set; } = NoteType.Outro;
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
