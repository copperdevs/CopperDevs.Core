namespace CopperDevs.Core.Serialization;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class TomlSyntaxException(string message, TOMLParser.ParseState state, int line, int col) : Exception(message)
{
    public TOMLParser.ParseState ParseState { get; } = state;

    public int Line { get; } = line;

    public int Column { get; } = col;
}
