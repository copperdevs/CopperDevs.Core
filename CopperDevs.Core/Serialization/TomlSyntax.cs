using System.Text.RegularExpressions;

namespace CopperDevs.Core.Serialization;


internal static class TomlSyntax
{
    #region Type Patterns

    public const string TRUE_VALUE = "true";
    public const string FALSE_VALUE = "false";
    public const string NAN_VALUE = "nan";
    public const string POS_NAN_VALUE = "+nan";
    public const string NEG_NAN_VALUE = "-nan";
    public const string INF_VALUE = "inf";
    public const string POS_INF_VALUE = "+inf";
    public const string NEG_INF_VALUE = "-inf";

    public static bool IsBoolean(string s) => s is TRUE_VALUE or FALSE_VALUE;

    public static bool IsPosInf(string s) => s is INF_VALUE or POS_INF_VALUE;

    public static bool IsNegInf(string s) => s == NEG_INF_VALUE;

    public static bool IsNaN(string s) => s is NAN_VALUE or POS_NAN_VALUE or NEG_NAN_VALUE;

    public static bool IsInteger(string s) => IntegerPattern.IsMatch(s);

    public static bool IsFloat(string s) => FloatPattern.IsMatch(s);

    public static bool IsIntegerWithBase(string s, out int numberBase)
    {
        numberBase = 10;
        var match = BasedIntegerPattern.Match(s);
        if (!match.Success) return false;
        IntegerBases.TryGetValue(match.Groups["base"].Value, out numberBase);
        return true;
    }

    /**
     * A pattern to verify the integer value according to the TOML specification.
     */
    public static readonly Regex IntegerPattern =
        new(@"^(\+|-)?(?!_)(0|(?!0)(_?\d)*)$", RegexOptions.Compiled);

    /**
     * A pattern to verify a special 0x, 0o and 0b forms of an integer according to the TOML specification.
     */
    public static readonly Regex BasedIntegerPattern =
        new(@"^0(?<base>x|b|o)(?!_)(_?[0-9A-F])*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    /**
     * A pattern to verify the float value according to the TOML specification.
     */
    public static readonly Regex FloatPattern =
        new(@"^(\+|-)?(?!_)(0|(?!0)(_?\d)+)(((e(\+|-)?(?!_)(_?\d)+)?)|(\.(?!_)(_?\d)+(e(\+|-)?(?!_)(_?\d)+)?))$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

    /**
     * A helper dictionary to map TOML base codes into the radii.
     */
    public static readonly Dictionary<string, int> IntegerBases = new()
    {
        ["x"] = 16,
        ["o"] = 8,
        ["b"] = 2
    };

    /**
     * A helper dictionary to map non-decimal bases to their TOML identifiers
     */
    public static readonly Dictionary<int, string> BaseIdentifiers = new()
    {
        [2] = "b",
        [8] = "o",
        [16] = "x"
    };

    public const string RFC3339EmptySeparator = " ";
    public const string ISO861Separator = "T";
    public const string ISO861ZeroZone = "+00:00";
    public const string RFC3339ZeroZone = "Z";

    /**
     * Valid date formats with timezone as per RFC3339.
     */
    public static readonly string[] RFC3339Formats =
    {
        "yyyy'-'MM-ddTHH':'mm':'ssK", "yyyy'-'MM-ddTHH':'mm':'ss'.'fK", "yyyy'-'MM-ddTHH':'mm':'ss'.'ffK",
        "yyyy'-'MM-ddTHH':'mm':'ss'.'fffK", "yyyy'-'MM-ddTHH':'mm':'ss'.'ffffK",
        "yyyy'-'MM-ddTHH':'mm':'ss'.'fffffK", "yyyy'-'MM-ddTHH':'mm':'ss'.'ffffffK",
        "yyyy'-'MM-ddTHH':'mm':'ss'.'fffffffK"
    };

    /**
     * Valid date formats without timezone (assumes local) as per RFC3339.
     */
    public static readonly string[] RFC3339LocalDateTimeFormats =
    {
        "yyyy'-'MM-ddTHH':'mm':'ss", "yyyy'-'MM-ddTHH':'mm':'ss'.'f", "yyyy'-'MM-ddTHH':'mm':'ss'.'ff",
        "yyyy'-'MM-ddTHH':'mm':'ss'.'fff", "yyyy'-'MM-ddTHH':'mm':'ss'.'ffff",
        "yyyy'-'MM-ddTHH':'mm':'ss'.'fffff", "yyyy'-'MM-ddTHH':'mm':'ss'.'ffffff",
        "yyyy'-'MM-ddTHH':'mm':'ss'.'fffffff"
    };

    /**
     * Valid full date format as per TOML spec.
     */
    public static readonly string LocalDateFormat = "yyyy'-'MM'-'dd";

    /**
     * Valid time formats as per TOML spec.
     */
    public static readonly string[] RFC3339LocalTimeFormats =
    {
        "HH':'mm':'ss", "HH':'mm':'ss'.'f", "HH':'mm':'ss'.'ff", "HH':'mm':'ss'.'fff", "HH':'mm':'ss'.'ffff",
        "HH':'mm':'ss'.'fffff", "HH':'mm':'ss'.'ffffff", "HH':'mm':'ss'.'fffffff"
    };

    #endregion

    #region Character definitions

    public const char ARRAY_END_SYMBOL = ']';
    public const char ITEM_SEPARATOR = ',';
    public const char ARRAY_START_SYMBOL = '[';
    public const char BASIC_STRING_SYMBOL = '\"';
    public const char COMMENT_SYMBOL = '#';
    public const char ESCAPE_SYMBOL = '\\';
    public const char KEY_VALUE_SEPARATOR = '=';
    public const char NEWLINE_CARRIAGE_RETURN_CHARACTER = '\r';
    public const char NEWLINE_CHARACTER = '\n';
    public const char SUBKEY_SEPARATOR = '.';
    public const char TABLE_END_SYMBOL = ']';
    public const char TABLE_START_SYMBOL = '[';
    public const char INLINE_TABLE_START_SYMBOL = '{';
    public const char INLINE_TABLE_END_SYMBOL = '}';
    public const char LITERAL_STRING_SYMBOL = '\'';
    public const char INT_NUMBER_SEPARATOR = '_';

    public static readonly char[] NewLineCharacters = { NEWLINE_CHARACTER, NEWLINE_CARRIAGE_RETURN_CHARACTER };

    public static bool IsQuoted(char c) => c is BASIC_STRING_SYMBOL or LITERAL_STRING_SYMBOL;

    public static bool IsWhiteSpace(char c) => c is ' ' or '\t';

    public static bool IsNewLine(char c) => c is NEWLINE_CHARACTER or NEWLINE_CARRIAGE_RETURN_CHARACTER;

    public static bool IsLineBreak(char c) => c == NEWLINE_CHARACTER;

    public static bool IsEmptySpace(char c) => IsWhiteSpace(c) || IsNewLine(c);

    public static bool IsBareKey(char c) =>
        c is >= 'A' and <= 'Z' or >= 'a' and <= 'z' or >= '0' and <= '9' or '_' or '-';

    public static bool MustBeEscaped(char c, bool allowNewLines = false)
    {
        var result = c is >= '\u0000' and <= '\u0008' or '\u000b' or '\u000c' or >= '\u000e' and <= '\u001f' or '\u007f';
        if (!allowNewLines)
            result |= c is >= '\u000a' and <= '\u000e';
        return result;
    }

    public static bool IsValueSeparator(char c) =>
        c is ITEM_SEPARATOR or ARRAY_END_SYMBOL or INLINE_TABLE_END_SYMBOL;

    #endregion
}

