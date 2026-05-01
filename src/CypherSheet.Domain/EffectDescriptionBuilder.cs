namespace CypherSheet.Domain;

/// <summary>
/// Classe estática responsável por gerar descrições em linguagem natural para efeitos ativos.
/// Separada da UI para permitir testes unitários e de propriedade sem dependência de Blazor.
/// </summary>
public static class EffectDescriptionBuilder
{
    /// <summary>
    /// Gera a descrição mecânica completa de um efeito ativo, incluindo o escopo AppliesTo.
    /// </summary>
    /// <param name="effect">O efeito ativo a ser descrito.</param>
    /// <returns>
    /// String com a descrição em linguagem natural, ex: "2 passo(s) mais fácil em Defesa Speed".
    /// Retorna string vazia se Value for zero.
    /// </returns>
    /// <remarks>
    /// Convenção de sinal: <c>Value &lt; 0</c> representa um buff (benefício ao personagem),
    /// enquanto <c>Value &gt; 0</c> representa um debuff (penalidade ao personagem).
    /// Essa inversão semântica é intencional e encapsulada nas propriedades
    /// <see cref="ActiveEffect.IsBuff"/> e <see cref="ActiveEffect.IsDebuff"/>.
    /// </remarks>
    public static string Build(ActiveEffect effect)
    {
        if (effect.Value == 0)
            return string.Empty;

        var modifierDescription = effect.ModifierType switch
        {
            ModifierType.Step => BuildStepDescription(effect.Value),
            ModifierType.FlatDamage => BuildFlatDamageDescription(effect.Value),
            ModifierType.Armor => BuildArmorDescription(effect.Value),
            ModifierType.TemporaryEdge => BuildTemporaryEdgeDescription(effect.Value),
            _ => string.Empty
        };

        if (string.IsNullOrEmpty(modifierDescription))
            return string.Empty;

        var appliesToLabel = GetAppliesToLabel(effect.AppliesTo);
        return $"{modifierDescription} {appliesToLabel}";
    }

    /// <summary>
    /// Retorna o texto localizado para cada valor do enum AppliesTo.
    /// </summary>
    /// <param name="appliesTo">O escopo de aplicação do efeito.</param>
    /// <returns>String localizada em português, ex: "em Defesa Speed".</returns>
    private static string GetAppliesToLabel(AppliesTo appliesTo) => appliesTo switch
    {
        AppliesTo.AnyRoll => "em Qualquer Rolagem",
        AppliesTo.AllDefenses => "em Defesas — Todas",
        AppliesTo.MightDefense => "em Defesa Might",
        AppliesTo.SpeedDefense => "em Defesa Speed",
        AppliesTo.IntellectDefense => "em Defesa Intellect",
        AppliesTo.AllAttacks => "em Ataques — Todos",
        AppliesTo.MeleeAttack => "em Ataque Melee",
        AppliesTo.RangedAttack => "em Ataque à Distância",
        AppliesTo.SkillTests => "em Testes de Perícia",
        _ => string.Empty
    };

    // Value < 0 → mais fácil (buff); Value > 0 → mais difícil (debuff)
    private static string BuildStepDescription(int value)
    {
        var abs = Math.Abs(value);
        return value < 0
            ? $"{abs} passo(s) mais fácil"
            : $"{abs} passo(s) mais difícil";
    }

    // Value > 0 → +X dano fixo; Value < 0 → -X dano fixo
    private static string BuildFlatDamageDescription(int value)
    {
        var abs = Math.Abs(value);
        return value > 0
            ? $"+{abs} dano fixo"
            : $"-{abs} dano fixo";
    }

    // Value > 0 → +X Armor; Value < 0 → -X Armor
    private static string BuildArmorDescription(int value)
    {
        var abs = Math.Abs(value);
        return value > 0
            ? $"+{abs} Armor"
            : $"-{abs} Armor";
    }

    // Value > 0 → +X Edge temporário; Value < 0 → -X Edge temporário
    private static string BuildTemporaryEdgeDescription(int value)
    {
        var abs = Math.Abs(value);
        return value > 0
            ? $"+{abs} Edge temporário"
            : $"-{abs} Edge temporário";
    }
}
