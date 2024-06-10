using System.Text.RegularExpressions;

namespace CopperDevs.Core.Utility;

public static partial class TextUtil
{
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