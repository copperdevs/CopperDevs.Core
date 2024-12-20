using System.Globalization;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace CopperDevs.Core.Serialization.Nodes;


public class TomlInteger : TomlNode
{
    public enum Base
    {
        Binary = 2,
        Octal = 8,
        Decimal = 10,
        Hexadecimal = 16
    }

    public override bool IsInteger { get; } = true;
    public override bool HasValue { get; } = true;
    public Base IntegerBase { get; set; } = Base.Decimal;

    public long Value { get; set; }

    public override string ToString() => Value.ToString();

    public override string ToInlineToml() =>
        IntegerBase != Base.Decimal
            ? $"0{TomlSyntax.BaseIdentifiers[(int)IntegerBase]}{Convert.ToString(Value, (int)IntegerBase)}"
            : Value.ToString(CultureInfo.InvariantCulture);
}
