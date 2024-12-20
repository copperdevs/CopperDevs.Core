using CopperDevs.Core.Serialization.Nodes;

namespace CopperDevs.Core.Serialization;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class TomlParseException(TomlTable parsed, IEnumerable<TomlSyntaxException> exceptions) : Exception("TOML file contains format errors")
{
    public TomlTable ParsedTable { get; } = parsed;

    public IEnumerable<TomlSyntaxException> SyntaxErrors { get; } = exceptions;
}
