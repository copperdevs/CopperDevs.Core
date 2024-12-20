namespace CopperDevs.Core.Serialization.Nodes;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class TomlBoolean : TomlNode
{
    public override bool IsBoolean { get; } = true;
    public override bool HasValue { get; } = true;

    public bool Value { get; set; }

    public override string ToString() => Value.ToString();

    public override string ToInlineToml() => Value ? TomlSyntax.TRUE_VALUE : TomlSyntax.FALSE_VALUE;
}
