namespace CypherSheet.Domain;

public enum AppliesTo
{
    AnyRoll,           // Qualquer Rolagem
    AllDefenses,       // Defesas — Todas
    MightDefense,      // Defesa Might
    SpeedDefense,      // Defesa Speed
    IntellectDefense,  // Defesa Intellect
    AllAttacks,        // Ataques — Todos
    MeleeAttack,       // Ataque Melee
    RangedAttack,      // Ataque à Distância
    SkillTests,        // Testes de Perícia
    Other              // Outro (descrição livre)
}
