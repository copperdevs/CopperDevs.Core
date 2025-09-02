using System.Globalization;

namespace CopperDevs.Core.Serialization.Nodes;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class TomlDateTimeLocal : TomlDateTime
{
    public enum DateTimeStyle
    {
        Date,
        Time,
        DateTime
    }

    public override bool IsDateTimeLocal { get; } = true;
    public DateTimeStyle Style { get; set; } = DateTimeStyle.DateTime;
    public DateTime Value { get; set; }

    public override string ToString() => Value.ToString(CultureInfo.CurrentCulture);

    public override string ToString(IFormatProvider formatProvider) => Value.ToString(formatProvider);

    public override string ToString(string format, IFormatProvider formatProvider) =>
        Value.ToString(format, formatProvider);

    public override string ToInlineToml() =>
        Style switch
        {
            DateTimeStyle.Date => Value.ToString(TomlSyntax.LocalDateFormat),
            DateTimeStyle.Time => Value.ToString(TomlSyntax.RFC3339LocalTimeFormats[SecondsPrecision]),
            var _ => Value.ToString(TomlSyntax.RFC3339LocalDateTimeFormats[SecondsPrecision])
        };
}
