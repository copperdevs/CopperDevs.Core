using System.Globalization;
using System.Text;

namespace CopperDevs.Core.Serialization;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

internal static class StringUtils
{
    public static string AsKey(this string key)
    {
        var quote = key == string.Empty || key.Any(c => !TomlSyntax.IsBareKey(c));
        return !quote ? key : $"{TomlSyntax.BASIC_STRING_SYMBOL}{key.Escape()}{TomlSyntax.BASIC_STRING_SYMBOL}";
    }

    public static string Join(this string self, IEnumerable<string> subItems)
    {
        var sb = new StringBuilder();
        var first = true;

        foreach (var subItem in subItems)
        {
            if (!first) sb.Append(self);
            first = false;
            sb.Append(subItem);
        }

        return sb.ToString();
    }

    public delegate bool TryDateParseDelegate<T>(string s, string format, IFormatProvider ci, DateTimeStyles dts, out T dt);

    public static bool TryParseDateTime<T>(string s,
                                           string[] formats,
                                           DateTimeStyles styles,
                                           TryDateParseDelegate<T> parser,
                                           out T dateTime,
                                           out int parsedFormat)
    {
        parsedFormat = 0;
        dateTime = default!;
        for (var i = 0; i < formats.Length; i++)
        {
            var format = formats[i];
            if (!parser(s, format, CultureInfo.InvariantCulture, styles, out dateTime)) continue;
            parsedFormat = i;
            return true;
        }

        return false;
    }

    public static void AsComment(this string self, TextWriter tw)
    {
        foreach (var line in self.Split(TomlSyntax.NEWLINE_CHARACTER))
            tw.WriteLine($"{TomlSyntax.COMMENT_SYMBOL} {line.Trim()}");
    }

    public static string RemoveAll(this string txt, char toRemove)
    {
        var sb = new StringBuilder(txt.Length);
        foreach (var c in txt.Where(c => c != toRemove))
            sb.Append(c);
        return sb.ToString();
    }

    public static string Escape(this string txt, bool escapeNewlines = true)
    {
        var stringBuilder = new StringBuilder(txt.Length + 2);
        for (var i = 0; i < txt.Length; i++)
        {
            var c = txt[i];

            static string CodePoint(string txt, ref int i, char c) => char.IsSurrogatePair(txt, i)
                ? $"\\U{char.ConvertToUtf32(txt, i++):X8}"
                : $"\\u{(ushort)c:X4}";

            stringBuilder.Append(c switch
            {
                '\b' => @"\b",
                '\t' => @"\t",
                '\n' when escapeNewlines => @"\n",
                '\f' => @"\f",
                '\r' when escapeNewlines => @"\r",
                '\\' => @"\\",
                '\"' => @"\""",
                var _ when TomlSyntax.MustBeEscaped(c, !escapeNewlines) || TOML.ForceASCII && c > sbyte.MaxValue =>
                    CodePoint(txt, ref i, c),
                var _ => c
            });
        }

        return stringBuilder.ToString();
    }

    public static bool TryUnescape(this string txt, out string unescaped, out Exception exception)
    {
        try
        {
            exception = null!;
            unescaped = txt.Unescape();
            return true;
        }
        catch (Exception e)
        {
            exception = e;
            unescaped = null!;
            return false;
        }
    }

    public static string Unescape(this string txt)
    {
        if (string.IsNullOrEmpty(txt)) return txt;
        var stringBuilder = new StringBuilder(txt.Length);
        for (var i = 0; i < txt.Length;)
        {
            var num = txt.IndexOf('\\', i);
            var next = num + 1;
            if (num < 0 || num == txt.Length - 1) num = txt.Length;
            stringBuilder.Append(txt, i, num - i);
            if (num >= txt.Length) break;
            var c = txt[next];

            static string CodePoint(int next, string txt, ref int num, int size)
            {
                if (next + size >= txt.Length) throw new Exception("Undefined escape sequence!");
                num += size;
                return char.ConvertFromUtf32(Convert.ToInt32(txt.Substring(next + 1, size), 16));
            }

            stringBuilder.Append(c switch
            {
                'b' => "\b",
                't' => "\t",
                'n' => "\n",
                'f' => "\f",
                'r' => "\r",
                '\'' => "\'",
                '\"' => "\"",
                '\\' => "\\",
                'u' => CodePoint(next, txt, ref num, 4),
                'U' => CodePoint(next, txt, ref num, 8),
                var _ => throw new Exception("Undefined escape sequence!")
            });
            i = num + 2;
        }

        return stringBuilder.ToString();
    }
}
