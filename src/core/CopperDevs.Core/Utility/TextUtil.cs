using System.Text.RegularExpressions;

namespace CopperDevs.Core.Utility;

/// <summary>
/// Utility methods for dealing with text
/// </summary>
public static partial class TextUtil
{
    /// <summary>
    /// Converts a string to use title case
    /// </summary>
    /// <param name="input">Input text</param>
    /// <returns>Input text converted to title case</returns>
    public static string ConvertToTitleCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // Using regular expressions to split the string by camelCase
        var result = TitleCaseRegex().Replace(input, " $1");

        // Capitalizing the first character and lowercasing the rest
        result = char.ToUpper(result[0]) + result[1..].ToLower();
        return result;
    }

    /// <summary>
    /// Converts a string to use kebab case
    /// </summary>
    /// <param name="input">Input text</param>
    /// <returns>Input text in kebab case</returns>
    public static string ToKebabCase(string input)
    {
        return string.IsNullOrEmpty(input) ? input : KebabCaseRegex().Replace(input, "-$1").ToLower();
    }

    [GeneratedRegex("(\\B[A-Z])")]
    private static partial Regex TitleCaseRegex();

    [GeneratedRegex(@"(?<!^)(?<!-)((?<=\p{Ll})\p{Lu}|\p{Lu}(?=\p{Ll}))")]
    private static partial Regex KebabCaseRegex();
}