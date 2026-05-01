using System.Text.Json.Serialization;

namespace CypherSheet.Domain;

public class ActiveEffect
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public string? Origin { get; set; }
    public ModifierType ModifierType { get; set; }
    public int Value { get; set; }
    public AppliesTo AppliesTo { get; set; }
    public DurationType DurationType { get; set; }
    public int? RemainingTurns { get; set; }

    [JsonIgnore]
    public bool IsExpired =>
        DurationType == DurationType.Turns && RemainingTurns == 0;
}
