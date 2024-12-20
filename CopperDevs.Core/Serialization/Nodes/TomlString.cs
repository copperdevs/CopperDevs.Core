namespace CopperDevs.Core.Serialization.Nodes;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class TomlString : TomlNode
{
    public override bool HasValue { get; } = true;
    public override bool IsString { get; } = true;
    public bool IsMultiline { get; set; }
    public bool MultilineTrimFirstLine { get; set; }
    public bool PreferLiteral { get; set; }

    public string Value { get; set; } = null!;

    public override string ToString() => Value;

    public override string ToInlineToml()
    {
        // Automatically convert literal to non-literal if there are too many literal string symbols
        if (Value.IndexOf(new string(TomlSyntax.LITERAL_STRING_SYMBOL, IsMultiline ? 3 : 1), StringComparison.Ordinal) != -1 && PreferLiteral) PreferLiteral = false;
        var quotes = new string(PreferLiteral ? TomlSyntax.LITERAL_STRING_SYMBOL : TomlSyntax.BASIC_STRING_SYMBOL,
                                IsMultiline ? 3 : 1);
        var result = PreferLiteral ? Value : Value.Escape(!IsMultiline);
        if (IsMultiline)
            result = result.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
        if (IsMultiline && (MultilineTrimFirstLine || !MultilineTrimFirstLine && result.StartsWith(Environment.NewLine)))
            result = $"{Environment.NewLine}{result}";
        return $"{quotes}{result}{quotes}";
    }
}
