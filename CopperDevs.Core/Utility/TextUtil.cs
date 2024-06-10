using System.Text.RegularExpressions;

namespace CopperDevs.Core.Utility;

/// <summary>
/// Utility methods for dealing with text
/// </summary>
public static partial class TextUtil
{
    /// <summary>
    /// Convert a string to title case
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

    [GeneratedRegex("(\\B[A-Z])")]
    private static partial Regex TitleCaseRegex();
}