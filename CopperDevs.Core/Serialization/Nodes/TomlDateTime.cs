namespace CopperDevs.Core.Serialization.Nodes;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class TomlDateTime : TomlNode, IFormattable
{
    public int SecondsPrecision { get; set; }
    public override bool HasValue { get; } = true;
    public virtual string ToString(string format, IFormatProvider formatProvider) => string.Empty;
    public virtual string ToString(IFormatProvider formatProvider) => string.Empty;
    protected virtual string ToInlineTomlInternal() => string.Empty;

    public override string ToInlineToml() => ToInlineTomlInternal()
                                            .Replace(TomlSyntax.RFC3339EmptySeparator, TomlSyntax.ISO861Separator)
                                            .Replace(TomlSyntax.ISO861ZeroZone, TomlSyntax.RFC3339ZeroZone);
}
