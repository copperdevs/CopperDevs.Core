using System.Globalization;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace CopperDevs.Core.Serialization.Nodes;

public class TomlFloat : TomlNode, IFormattable
{
    public override bool IsFloat { get; } = true;
    public override bool HasValue { get; } = true;

    public double Value { get; set; }

    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

    public string ToString(string format, IFormatProvider formatProvider) => Value.ToString(format, formatProvider);

    public string ToString(IFormatProvider formatProvider) => Value.ToString(formatProvider);

    public override string ToInlineToml() =>
        Value switch
        {
            var v when double.IsNaN(v) => TomlSyntax.NAN_VALUE,
            var v when double.IsPositiveInfinity(v) => TomlSyntax.INF_VALUE,
            var v when double.IsNegativeInfinity(v) => TomlSyntax.NEG_INF_VALUE,
            var v => v.ToString("G", CultureInfo.InvariantCulture).ToLowerInvariant()
        };
}
