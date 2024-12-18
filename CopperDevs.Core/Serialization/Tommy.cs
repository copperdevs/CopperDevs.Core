#region LICENSE

/*
 * MIT License
 * 
 * Copyright (c) 2020 Denis Zhidkikh
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

#endregion

using System.Globalization;
using System.Text;
using CopperDevs.Core.Serialization.Nodes;
using CopperDevs.Core.Utility;

namespace CopperDevs.Core.Serialization;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public static class TOML
{
    public static bool ForceASCII { get; set; } = false;

    public static TomlTable Parse(TextReader reader)
    {
        using var parser = new TOMLParser(reader) { ForceASCII = ForceASCII };
        return parser.Parse();
    }
}

public class TOMLParser : SafeDisposable
{
    public enum ParseState
    {
        None,
        KeyValuePair,
        SkipToNextLine,
        Table
    }

    private readonly TextReader reader;
    private ParseState currentState;
    private int line, col;
    private List<TomlSyntaxException> syntaxErrors = [];

    public TOMLParser(TextReader reader)
    {
        this.reader = reader;
        line = col = 0;
    }

    public bool ForceASCII { get; set; }

    public override void DisposeResources() => reader?.Dispose();

    public TomlTable Parse()
    {
        syntaxErrors = [];
        line = col = 1;
        var rootNode = new TomlTable();
        var currentNode = rootNode;
        currentState = ParseState.None;
        var keyParts = new List<string>();
        var arrayTable = false;
        StringBuilder latestComment = null!;
        var firstComment = true;

        int currentChar;
        while ((currentChar = reader.Peek()) >= 0)
        {
            var c = (char)currentChar;

            if (currentState == ParseState.None)
            {
                // Skip white space
                if (TomlSyntax.IsWhiteSpace(c)) goto consume_character;

                if (TomlSyntax.IsNewLine(c))
                {
                    // Check if there are any comments and so far no items being declared
                    if (latestComment != null && firstComment)
                    {
                        rootNode.Comment = latestComment.ToString().TrimEnd();
                        latestComment = null!;
                        firstComment = false;
                    }

                    if (TomlSyntax.IsLineBreak(c))
                        AdvanceLine();

                    goto consume_character;
                }

                // Start of a comment; ignore until newline
                if (c == TomlSyntax.COMMENT_SYMBOL)
                {
                    latestComment ??= new StringBuilder();
                    latestComment.AppendLine(ParseComment());
                    AdvanceLine(1);
                    continue;
                }

                // Encountered a non-comment value. The comment must belong to it (ignore possible newlines)!
                firstComment = false;

                if (c == TomlSyntax.TABLE_START_SYMBOL)
                {
                    currentState = ParseState.Table;
                    goto consume_character;
                }

                if (TomlSyntax.IsBareKey(c) || TomlSyntax.IsQuoted(c))
                {
                    currentState = ParseState.KeyValuePair;
                }
                else
                {
                    AddError($"Unexpected character \"{c}\"");
                    continue;
                }
            }

            if (currentState == ParseState.KeyValuePair)
            {
                var keyValuePair = ReadKeyValuePair(keyParts);

                if (keyValuePair == null)
                {
                    latestComment = null!;
                    keyParts.Clear();

                    if (currentState != ParseState.None)
                        AddError("Failed to parse key-value pair!");
                    continue;
                }

                keyValuePair.Comment = (latestComment?.ToString()?.TrimEnd())!;
                var inserted = InsertNode(keyValuePair, currentNode, keyParts);
                latestComment = null!;
                keyParts.Clear();
                if (inserted)
                    currentState = ParseState.SkipToNextLine;
                continue;
            }

            if (currentState == ParseState.Table)
            {
                if (keyParts.Count == 0)
                {
                    // We have array table
                    if (c == TomlSyntax.TABLE_START_SYMBOL)
                    {
                        // Consume the character
                        ConsumeChar();
                        arrayTable = true;
                    }

                    if (!ReadKeyName(ref keyParts, TomlSyntax.TABLE_END_SYMBOL))
                    {
                        keyParts.Clear();
                        continue;
                    }

                    if (keyParts.Count == 0)
                    {
                        AddError("Table name is emtpy.");
                        arrayTable = false;
                        latestComment = null!;
                        keyParts.Clear();
                    }

                    continue;
                }

                if (c == TomlSyntax.TABLE_END_SYMBOL)
                {
                    if (arrayTable)
                    {
                        // Consume the ending bracket so we can peek the next character
                        ConsumeChar();
                        var nextChar = reader.Peek();
                        if (nextChar < 0 || (char)nextChar != TomlSyntax.TABLE_END_SYMBOL)
                        {
                            AddError($"Array table {".".Join(keyParts)} has only one closing bracket.");
                            keyParts.Clear();
                            arrayTable = false;
                            latestComment = null!;
                            continue;
                        }
                    }

                    currentNode = CreateTable(rootNode, keyParts, arrayTable);
                    if (currentNode != null)
                    {
                        currentNode.IsInline = false;
                        currentNode.Comment = (latestComment?.ToString()?.TrimEnd())!;
                    }

                    keyParts.Clear();
                    arrayTable = false;
                    latestComment = null!;

                    if (currentNode == null)
                    {
                        if (currentState != ParseState.None)
                            AddError("Error creating table array!");
                        // Reset a node to root in order to try and continue parsing
                        currentNode = rootNode;
                        continue;
                    }

                    currentState = ParseState.SkipToNextLine;
                    goto consume_character;
                }

                if (keyParts.Count != 0)
                {
                    AddError($"Unexpected character \"{c}\"");
                    keyParts.Clear();
                    arrayTable = false;
                    latestComment = null!;
                }
            }

            if (currentState == ParseState.SkipToNextLine)
            {
                if (TomlSyntax.IsWhiteSpace(c) || c == TomlSyntax.NEWLINE_CARRIAGE_RETURN_CHARACTER)
                    goto consume_character;

                if (c is TomlSyntax.COMMENT_SYMBOL or TomlSyntax.NEWLINE_CHARACTER)
                {
                    currentState = ParseState.None;
                    AdvanceLine();

                    if (c == TomlSyntax.COMMENT_SYMBOL)
                    {
                        col++;
                        ParseComment();
                        continue;
                    }

                    goto consume_character;
                }

                AddError($"Unexpected character \"{c}\" at the end of the line.");
            }

        consume_character:
            reader.Read();
            col++;
        }

        if (currentState != ParseState.None && currentState != ParseState.SkipToNextLine)
            AddError("Unexpected end of file!");

        if (syntaxErrors.Count > 0)
            throw new TomlParseException(rootNode, syntaxErrors);

        return rootNode;
    }

    private bool AddError(string message, bool skipLine = true)
    {
        syntaxErrors.Add(new TomlSyntaxException(message, currentState, line, col));
        // Skip the whole line in hope that it was only a single faulty value (and non-multiline one at that)
        if (skipLine)
        {
            reader.ReadLine();
            AdvanceLine(1);
        }
        currentState = ParseState.None;
        return false;
    }

    private void AdvanceLine(int startCol = 0)
    {
        line++;
        col = startCol;
    }

    private int ConsumeChar()
    {
        col++;
        return reader.Read();
    }

    #region Key-Value pair parsing

    /**
     * Reads a single key-value pair.
     * Assumes the cursor is at the first character that belong to the pair (including possible whitespace).
     * Consumes all characters that belong to the key and the value (ignoring possible trailing whitespace at the end).
     * 
     * Example:
     * foo = "bar"  ==> foo = "bar"
     * ^                           ^
     */
    private TomlNode ReadKeyValuePair(List<string> keyParts)
    {
        int cur;
        while ((cur = reader.Peek()) >= 0)
        {
            var c = (char)cur;

            if (TomlSyntax.IsQuoted(c) || TomlSyntax.IsBareKey(c))
            {
                if (keyParts.Count != 0)
                {
                    AddError("Encountered extra characters in key definition!");
                    return null!;
                }

                if (!ReadKeyName(ref keyParts, TomlSyntax.KEY_VALUE_SEPARATOR))
                    return null!;

                continue;
            }

            if (TomlSyntax.IsWhiteSpace(c))
            {
                ConsumeChar();
                continue;
            }

            if (c == TomlSyntax.KEY_VALUE_SEPARATOR)
            {
                ConsumeChar();
                return ReadValue();
            }

            AddError($"Unexpected character \"{c}\" in key name.");
            return null!;
        }

        return null!;
    }

    /**
     * Reads a single value.
     * Assumes the cursor is at the first character that belongs to the value (including possible starting whitespace).
     * Consumes all characters belonging to the value (ignoring possible trailing whitespace at the end).
     * 
     * Example:
     * "test"  ==> "test"
     * ^                 ^
     */
    private TomlNode ReadValue(bool skipNewlines = false)
    {
        int cur;
        while ((cur = reader.Peek()) >= 0)
        {
            var c = (char)cur;

            if (TomlSyntax.IsWhiteSpace(c))
            {
                ConsumeChar();
                continue;
            }

            if (c == TomlSyntax.COMMENT_SYMBOL)
            {
                AddError("No value found!");
                return null!;
            }

            if (TomlSyntax.IsNewLine(c))
            {
                if (skipNewlines)
                {
                    reader.Read();
                    AdvanceLine(1);
                    continue;
                }

                AddError("Encountered a newline when expecting a value!");
                return null!;
            }

            if (TomlSyntax.IsQuoted(c))
            {
                var isMultiline = IsTripleQuote(c, out var excess);

                // Error occurred in triple quote parsing
                if (currentState == ParseState.None)
                    return null!;

                var value = isMultiline
                    ? ReadQuotedValueMultiLine(c)
                    : ReadQuotedValueSingleLine(c, excess);

                if (value is null)
                    return null!;

                return new TomlString
                {
                    Value = value,
                    IsMultiline = isMultiline,
                    PreferLiteral = c == TomlSyntax.LITERAL_STRING_SYMBOL
                };
            }

            return c switch
            {
                TomlSyntax.INLINE_TABLE_START_SYMBOL => ReadInlineTable(),
                TomlSyntax.ARRAY_START_SYMBOL => ReadArray(),
                var _ => ReadTomlValue()
            };
        }

        return null!;
    }

    /**
     * Reads a single key name.
     * Assumes the cursor is at the first character belonging to the key (with possible trailing whitespace if `skipWhitespace = true`).
     * Consumes all the characters until the `until` character is met (but does not consume the character itself).
     * 
     * Example 1:
     * foo.bar  ==>  foo.bar           (`skipWhitespace = false`, `until = ' '`)
     * ^                    ^
     * 
     * Example 2:
     * [ foo . bar ] ==>  [ foo . bar ]     (`skipWhitespace = true`, `until = ']'`)
     * ^                             ^
     */
    private bool ReadKeyName(ref List<string> parts, char until)
    {
        var buffer = new StringBuilder();
        var quoted = false;
        var prevWasSpace = false;
        int cur;
        while ((cur = reader.Peek()) >= 0)
        {
            var c = (char)cur;

            // Reached the final character
            if (c == until) break;

            if (TomlSyntax.IsWhiteSpace(c))
            {
                prevWasSpace = true;
                goto consume_character;
            }

            if (buffer.Length == 0) prevWasSpace = false;

            if (c == TomlSyntax.SUBKEY_SEPARATOR)
            {
                if (buffer.Length == 0 && !quoted)
                    return AddError($"Found an extra subkey separator in {".".Join(parts)}...");

                parts.Add(buffer.ToString());
                buffer.Length = 0;
                quoted = false;
                prevWasSpace = false;
                goto consume_character;
            }

            if (prevWasSpace)
                return AddError("Invalid spacing in key name");

            if (TomlSyntax.IsQuoted(c))
            {
                if (quoted)

                    return AddError("Expected a subkey separator but got extra data instead!");

                if (buffer.Length != 0)
                    return AddError("Encountered a quote in the middle of subkey name!");

                // Consume the quote character and read the key name
                col++;
                buffer.Append(ReadQuotedValueSingleLine((char)reader.Read()));
                quoted = true;
                continue;
            }

            if (TomlSyntax.IsBareKey(c))
            {
                buffer.Append(c);
                goto consume_character;
            }

            // If we see an invalid symbol, let the next parser handle it
            break;

        consume_character:
            reader.Read();
            col++;
        }

        if (buffer.Length == 0 && !quoted)
            return AddError($"Found an extra subkey separator in {".".Join(parts)}...");

        parts.Add(buffer.ToString());

        return true;
    }

    #endregion

    #region Non-string value parsing

    /**
     * Reads the whole raw value until the first non-value character is encountered.
     * Assumes the cursor start position at the first value character and consumes all characters that may be related to the value.
     * Example:
     * 
     * 1_0_0_0  ==>  1_0_0_0
     * ^                    ^
     */
    private string ReadRawValue()
    {
        var result = new StringBuilder();
        int cur;
        while ((cur = reader.Peek()) >= 0)
        {
            var c = (char)cur;
            if (c == TomlSyntax.COMMENT_SYMBOL || TomlSyntax.IsNewLine(c) || TomlSyntax.IsValueSeparator(c)) break;
            result.Append(c);
            ConsumeChar();
        }

        // Replace trim with manual space counting?
        return result.ToString().Trim();
    }

    /**
     * Reads and parses a non-string, non-composite TOML value.
     * Assumes the cursor at the first character that is related to the value (with possible spaces).
     * Consumes all the characters that are related to the value.
     * 
     * Example
     * 1_0_0_0 # This is a comment
     * <newline>
     *     ==>  1_0_0_0 # This is a comment
     *     ^    
     * </newline>                                              ^
     */
    private TomlNode ReadTomlValue()
    {
        var value = ReadRawValue();
        TomlNode node = value switch
        {
            var v when TomlSyntax.IsBoolean(v) => bool.Parse(v),
            var v when TomlSyntax.IsNaN(v) => double.NaN,
            var v when TomlSyntax.IsPosInf(v) => double.PositiveInfinity,
            var v when TomlSyntax.IsNegInf(v) => double.NegativeInfinity,
            var v when TomlSyntax.IsInteger(v) => long.Parse(value.RemoveAll(TomlSyntax.INT_NUMBER_SEPARATOR),
                                                             CultureInfo.InvariantCulture),
            var v when TomlSyntax.IsFloat(v) => double.Parse(value.RemoveAll(TomlSyntax.INT_NUMBER_SEPARATOR),
                                                             CultureInfo.InvariantCulture),
            var v when TomlSyntax.IsIntegerWithBase(v, out var numberBase) => new TomlInteger
            {
                Value = Convert.ToInt64(value.Substring(2).RemoveAll(TomlSyntax.INT_NUMBER_SEPARATOR), numberBase),
                IntegerBase = (TomlInteger.Base)numberBase
            },
            var _ => null!
        };
        if (node != null) return node;

        // Normalize by removing space separator
        value = value.Replace(TomlSyntax.RFC3339EmptySeparator, TomlSyntax.ISO861Separator);
        if (StringUtils.TryParseDateTime<DateTime>(value,
                                         TomlSyntax.RFC3339LocalDateTimeFormats,
                                         DateTimeStyles.AssumeLocal,
                                         DateTime.TryParseExact,
                                         out var dateTimeResult,
                                         out var precision))
            return new TomlDateTimeLocal
            {
                Value = dateTimeResult,
                SecondsPrecision = precision
            };

        if (DateTime.TryParseExact(value,
                                   TomlSyntax.LocalDateFormat,
                                   CultureInfo.InvariantCulture,
                                   DateTimeStyles.AssumeLocal,
                                   out dateTimeResult))
            return new TomlDateTimeLocal
            {
                Value = dateTimeResult,
                Style = TomlDateTimeLocal.DateTimeStyle.Date
            };

        if (StringUtils.TryParseDateTime(value,
                                         TomlSyntax.RFC3339LocalTimeFormats,
                                         DateTimeStyles.AssumeLocal,
                                         DateTime.TryParseExact,
                                         out dateTimeResult,
                                         out precision))
            return new TomlDateTimeLocal
            {
                Value = dateTimeResult,
                Style = TomlDateTimeLocal.DateTimeStyle.Time,
                SecondsPrecision = precision
            };

        if (StringUtils.TryParseDateTime<DateTimeOffset>(value,
                                                         TomlSyntax.RFC3339Formats,
                                                         DateTimeStyles.None,
                                                         DateTimeOffset.TryParseExact,
                                                         out var dateTimeOffsetResult,
                                                         out precision))
            return new TomlDateTimeOffset
            {
                Value = dateTimeOffsetResult,
                SecondsPrecision = precision
            };

        AddError($"Value \"{value}\" is not a valid TOML value!");
        return null!;
    }

    /**
     * Reads an array value.
     * Assumes the cursor is at the start of the array definition. Reads all character until the array closing bracket.
     * 
     * Example:
     * [1, 2, 3]  ==>  [1, 2, 3]
     * ^                        ^
     */
    private TomlArray ReadArray()
    {
        // Consume the start of array character
        ConsumeChar();
        var result = new TomlArray();
        TomlNode currentValue = null!;
        var expectValue = true;

        int cur;
        while ((cur = reader.Peek()) >= 0)
        {
            var c = (char)cur;

            if (c == TomlSyntax.ARRAY_END_SYMBOL)
            {
                ConsumeChar();
                break;
            }

            if (c == TomlSyntax.COMMENT_SYMBOL)
            {
                reader.ReadLine();
                AdvanceLine(1);
                continue;
            }

            if (TomlSyntax.IsWhiteSpace(c) || TomlSyntax.IsNewLine(c))
            {
                if (TomlSyntax.IsLineBreak(c))
                    AdvanceLine();
                goto consume_character;
            }

            if (c == TomlSyntax.ITEM_SEPARATOR)
            {
                if (currentValue == null)
                {
                    AddError("Encountered multiple value separators");
                    return null!;
                }

                result.Add(currentValue);
                currentValue = null!;
                expectValue = true;
                goto consume_character;
            }

            if (!expectValue)
            {
                AddError("Missing separator between values");
                return null!;
            }
            currentValue = ReadValue(true);
            if (currentValue == null)
            {
                if (currentState != ParseState.None)
                    AddError("Failed to determine and parse a value!");
                return null!;
            }
            expectValue = false;

            continue;
        consume_character:
            ConsumeChar();
        }

        if (currentValue != null) result.Add(currentValue);
        return result;
    }

    /**
     * Reads an inline table.
     * Assumes the cursor is at the start of the table definition. Reads all character until the table closing bracket.
     * 
     * Example:
     * { test = "foo", value = 1 }  ==>  { test = "foo", value = 1 }
     * ^                                                            ^
     */
    private TomlNode ReadInlineTable()
    {
        ConsumeChar();
        var result = new TomlTable { IsInline = true };
        TomlNode currentValue = null!;
        var separator = false;
        var keyParts = new List<string>();
        int cur;
        while ((cur = reader.Peek()) >= 0)
        {
            var c = (char)cur;

            if (c == TomlSyntax.INLINE_TABLE_END_SYMBOL)
            {
                ConsumeChar();
                break;
            }

            if (c == TomlSyntax.COMMENT_SYMBOL)
            {
                AddError("Incomplete inline table definition!");
                return null!;
            }

            if (TomlSyntax.IsNewLine(c))
            {
                AddError("Inline tables are only allowed to be on single line");
                return null!;
            }

            if (TomlSyntax.IsWhiteSpace(c))
                goto consume_character;

            if (c == TomlSyntax.ITEM_SEPARATOR)
            {
                if (currentValue == null)
                {
                    AddError("Encountered multiple value separators in inline table!");
                    return null!;
                }

                if (!InsertNode(currentValue, result, keyParts))
                    return null!;
                keyParts.Clear();
                currentValue = null!;
                separator = true;
                goto consume_character;
            }

            separator = false;
            currentValue = ReadKeyValuePair(keyParts);
            continue;

        consume_character:
            ConsumeChar();
        }

        if (separator)
        {
            AddError("Trailing commas are not allowed in inline tables.");
            return null!;
        }

        if (currentValue != null && !InsertNode(currentValue, result, keyParts))
            return null!;

        return result;
    }

    #endregion

    #region String parsing

    /**
     * Checks if the string value a multiline string (i.e. a triple quoted string).
     * Assumes the cursor is at the first quote character. Consumes the least amount of characters needed to determine if the string is multiline.
     * 
     * If the result is false, returns the consumed character through the `excess` variable.
     * 
     * Example 1:
     * """test"""  ==>  """test"""
     * ^                   ^
     * 
     * Example 2:
     * "test"  ==>  "test"         (doesn't return the first quote)
     * ^             ^
     * 
     * Example 3:
     * ""  ==>  ""        (returns the extra `"` through the `excess` variable)
     * ^          ^
     */
    private bool IsTripleQuote(char quote, out char excess)
    {
        // Copypasta, but it's faster...

        int cur;
        // Consume the first quote
        ConsumeChar();
        if ((cur = reader.Peek()) < 0)
        {
            excess = '\0';
            return AddError("Unexpected end of file!");
        }

        if ((char)cur != quote)
        {
            excess = '\0';
            return false;
        }

        // Consume the second quote
        excess = (char)ConsumeChar();
        if ((cur = reader.Peek()) < 0 || (char)cur != quote) return false;

        // Consume the final quote
        ConsumeChar();
        excess = '\0';
        return true;
    }

    /**
     * A convenience method to process a single character within a quote.
     */
    private bool ProcessQuotedValueCharacter(char quote,
                                             bool isNonLiteral,
                                             char c,
                                             StringBuilder sb,
                                             ref bool escaped)
    {
        if (TomlSyntax.MustBeEscaped(c))
            return AddError($"The character U+{(int)c:X8} must be escaped in a string!");

        if (escaped)
        {
            sb.Append(c);
            escaped = false;
            return false;
        }

        if (c == quote) return true;
        if (isNonLiteral && c == TomlSyntax.ESCAPE_SYMBOL)
            escaped = true;
        if (c == TomlSyntax.NEWLINE_CHARACTER)
            return AddError("Encountered newline in single line string!");

        sb.Append(c);
        return false;
    }

    /**
     * Reads a single-line string.
     * Assumes the cursor is at the first character that belongs to the string.
     * Consumes all characters that belong to the string (including the closing quote).
     * 
     * Example:
     * "test"  ==>  "test"
     * ^                 ^
     */
    private string ReadQuotedValueSingleLine(char quote, char initialData = '\0')
    {
        var isNonLiteral = quote == TomlSyntax.BASIC_STRING_SYMBOL;
        var sb = new StringBuilder();
        var escaped = false;

        if (initialData != '\0')
        {
            var shouldReturn =
                ProcessQuotedValueCharacter(quote, isNonLiteral, initialData, sb, ref escaped);
            if (currentState == ParseState.None) return null!;
            if (shouldReturn)
                if (isNonLiteral)
                {
                    if (sb.ToString().TryUnescape(out var res, out var ex)) return res;
                    AddError(ex.Message);
                    return null!;
                }
                else
                    return sb.ToString();
        }

        int cur;
        var readDone = false;
        while ((cur = reader.Read()) >= 0)
        {
            // Consume the character
            col++;
            var c = (char)cur;
            readDone = ProcessQuotedValueCharacter(quote, isNonLiteral, c, sb, ref escaped);
            if (readDone)
            {
                if (currentState == ParseState.None) return null!;
                break;
            }
        }

        if (!readDone)
        {
            AddError("Unclosed string.");
            return null!;
        }

        if (!isNonLiteral) return sb.ToString();
        if (sb.ToString().TryUnescape(out var unescaped, out var unescapedEx)) return unescaped;
        AddError(unescapedEx.Message);
        return null!;
    }

    /**
     * Reads a multiline string.
     * Assumes the cursor is at the first character that belongs to the string.
     * Consumes all characters that belong to the string and the three closing quotes.
     * 
     * Example:
     * """test"""  ==>  """test"""
     * ^                       ^
     */
    private string ReadQuotedValueMultiLine(char quote)
    {
        var isBasic = quote == TomlSyntax.BASIC_STRING_SYMBOL;
        var sb = new StringBuilder();
        var escaped = false;
        var skipWhitespace = false;
        var skipWhitespaceLineSkipped = false;
        var quotesEncountered = 0;
        var first = true;
        int cur;
        while ((cur = ConsumeChar()) >= 0)
        {
            var c = (char)cur;
            if (TomlSyntax.MustBeEscaped(c, true))
            {
                AddError($"The character U+{(int)c:X8} must be escaped!");
                return null!;
            }
            // Trim the first newline
            if (first && TomlSyntax.IsNewLine(c))
            {
                if (TomlSyntax.IsLineBreak(c))
                    first = false;
                else
                    AdvanceLine();
                continue;
            }

            first = false;
            //TODO: Reuse ProcessQuotedValueCharacter
            // Skip the current character if it is going to be escaped later
            if (escaped)
            {
                sb.Append(c);
                escaped = false;
                continue;
            }

            // If we are currently skipping empty spaces, skip
            if (skipWhitespace)
            {
                if (TomlSyntax.IsEmptySpace(c))
                {
                    if (TomlSyntax.IsLineBreak(c))
                    {
                        skipWhitespaceLineSkipped = true;
                        AdvanceLine();
                    }
                    continue;
                }

                if (!skipWhitespaceLineSkipped)
                {
                    AddError("Non-whitespace character after trim marker.");
                    return null!;
                }

                skipWhitespaceLineSkipped = false;
                skipWhitespace = false;
            }

            // If we encounter an escape sequence...
            if (isBasic && c == TomlSyntax.ESCAPE_SYMBOL)
            {
                var next = reader.Peek();
                var nc = (char)next;
                if (next >= 0)
                {
                    // ...and the next char is empty space, we must skip all whitespaces
                    if (TomlSyntax.IsEmptySpace(nc))
                    {
                        skipWhitespace = true;
                        continue;
                    }

                    // ...and we have \" or \, skip the character
                    if (nc == quote || nc == TomlSyntax.ESCAPE_SYMBOL) escaped = true;
                }
            }

            // Count the consecutive quotes
            if (c == quote)
                quotesEncountered++;
            else
                quotesEncountered = 0;

            // If the are three quotes, count them as closing quotes
            if (quotesEncountered == 3) break;

            sb.Append(c);
        }

        // TOML actually allows to have five ending quotes like
        // """"" => "" belong to the string + """ is the actual ending
        quotesEncountered = 0;
        while ((cur = reader.Peek()) >= 0)
        {
            var c = (char)cur;
            if (c == quote && ++quotesEncountered < 3)
            {
                sb.Append(c);
                ConsumeChar();
            }
            else break;
        }

        // Remove last two quotes (third one wasn't included by default)
        sb.Length -= 2;
        if (!isBasic) return sb.ToString();
        if (sb.ToString().TryUnescape(out var res, out var ex)) return res;
        AddError(ex.Message);
        return null!;
    }

    #endregion

    #region Node creation

    private bool InsertNode(TomlNode node, TomlNode root, IList<string> path)
    {
        var latestNode = root;
        if (path.Count > 1)
            for (var index = 0; index < path.Count - 1; index++)
            {
                var subkey = path[index];
                if (latestNode.TryGetNode(subkey, out var currentNode))
                {
                    if (currentNode.HasValue)
                        return AddError($"The key {".".Join(path)} already has a value assigned to it!");
                }
                else
                {
                    currentNode = new TomlTable();
                    latestNode[subkey] = currentNode;
                }

                latestNode = currentNode;
                if (latestNode is TomlTable { IsInline: true })
                    return AddError($"Cannot assign {".".Join(path)} because it will edit an immutable table.");
            }

        if (latestNode.HasKey(path[path.Count - 1]))
            return AddError($"The key {".".Join(path)} is already defined!");
        latestNode[path[path.Count - 1]] = node;
        node.CollapseLevel = path.Count - 1;
        return true;
    }

    private TomlTable CreateTable(TomlNode root, IList<string> path, bool arrayTable)
    {
        if (path.Count == 0) return null!;
        var latestNode = root;
        for (var index = 0; index < path.Count; index++)
        {
            var subkey = path[index];

            if (latestNode.TryGetNode(subkey, out var node))
            {
                if (node.IsArray && arrayTable)
                {
                    var arr = (TomlArray)node;

                    if (!arr.IsTableArray)
                    {
                        AddError($"The array {".".Join(path)} cannot be redefined as an array table!");
                        return null!;
                    }

                    if (index == path.Count - 1)
                    {
                        latestNode = new TomlTable();
                        arr.Add(latestNode);
                        break;
                    }

                    latestNode = arr[arr.ChildrenCount - 1];
                    continue;
                }

                if (node is TomlTable { IsInline: true })
                {
                    AddError($"Cannot create table {".".Join(path)} because it will edit an immutable table.");
                    return null!;
                }

                if (node.HasValue)
                {
                    if (node is not TomlArray { IsTableArray: true } array)
                    {
                        AddError($"The key {".".Join(path)} has a value assigned to it!");
                        return null!;
                    }

                    latestNode = array[array.ChildrenCount - 1];
                    continue;
                }

                if (index == path.Count - 1)
                {
                    if (arrayTable && !node.IsArray)
                    {
                        AddError($"The table {".".Join(path)} cannot be redefined as an array table!");
                        return null!;
                    }

                    if (node is TomlTable { isImplicit: false })
                    {
                        AddError($"The table {".".Join(path)} is defined multiple times!");
                        return null!;
                    }
                }
            }
            else
            {
                if (index == path.Count - 1 && arrayTable)
                {
                    var table = new TomlTable();
                    var arr = new TomlArray
                    {
                        IsTableArray = true
                    };
                    arr.Add(table);
                    latestNode[subkey] = arr;
                    latestNode = table;
                    break;
                }

                node = new TomlTable { isImplicit = true };
                latestNode[subkey] = node;
            }

            latestNode = node;
        }

        var result = (TomlTable)latestNode;
        result.isImplicit = false;
        return result;
    }

    #endregion

    #region Misc parsing

    private string ParseComment()
    {
        ConsumeChar();
        var commentLine = reader.ReadLine()?.Trim() ?? "";
        if (commentLine.Any(ch => TomlSyntax.MustBeEscaped(ch)))
            AddError("Comment must not contain control characters other than tab.", false);
        return commentLine;
    }

    #endregion
}
