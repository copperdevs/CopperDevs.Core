using System.Globalization;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace CopperDevs.Core.Serialization.Nodes;

public class TomlDateTimeOffset : TomlDateTime
{
    public override bool IsDateTimeOffset { get; } = true;
    public DateTimeOffset Value { get; set; }

    public override string ToString() => Value.ToString(CultureInfo.CurrentCulture);
    public override string ToString(IFormatProvider formatProvider) => Value.ToString(formatProvider);

    public override string ToString(string format, IFormatProvider formatProvider) =>
        Value.ToString(format, formatProvider);

    protected override string ToInlineTomlInternal() => Value.ToString(TomlSyntax.RFC3339Formats[SecondsPrecision]);
}
