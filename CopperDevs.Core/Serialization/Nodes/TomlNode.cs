using System.Collections;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace CopperDevs.Core.Serialization.Nodes;

public abstract class TomlNode : IEnumerable
{
    public virtual bool HasValue { get; } = false;
    public virtual bool IsArray { get; } = false;
    public virtual bool IsTable { get; } = false;
    public virtual bool IsString { get; } = false;
    public virtual bool IsInteger { get; } = false;
    public virtual bool IsFloat { get; } = false;
    public bool IsDateTime => IsDateTimeLocal || IsDateTimeOffset;
    public virtual bool IsDateTimeLocal { get; } = false;
    public virtual bool IsDateTimeOffset { get; } = false;
    public virtual bool IsBoolean { get; } = false;
    public virtual string Comment { get; set; }
    public virtual int CollapseLevel { get; set; }

    public virtual TomlTable AsTable => (this as TomlTable)!;
    public virtual TomlString AsString => (this as TomlString)!;
    public virtual TomlInteger AsInteger => (this as TomlInteger)!;
    public virtual TomlFloat AsFloat => (this as TomlFloat)!;
    public virtual TomlBoolean AsBoolean => (this as TomlBoolean)!;
    public virtual TomlDateTimeLocal AsDateTimeLocal => (this as TomlDateTimeLocal)!;
    public virtual TomlDateTimeOffset AsDateTimeOffset => (this as TomlDateTimeOffset)!;
    public virtual TomlDateTime AsDateTime => (this as TomlDateTime)!;
    public virtual TomlArray AsArray => (this as TomlArray)!;

    public virtual int ChildrenCount => 0;

    public virtual TomlNode this[string key]
    {
        get => null!;
        set { }
    }

    public virtual TomlNode this[int index]
    {
        get => null!;
        set { }
    }

    public virtual IEnumerable<TomlNode> Children
    {
        get { yield break; }
    }

    public virtual IEnumerable<string> Keys
    {
        get { yield break; }
    }

    public IEnumerator GetEnumerator() => Children.GetEnumerator();

    public virtual bool TryGetNode(string key, out TomlNode node)
    {
        node = null!;
        return false;
    }

    public virtual bool HasKey(string key) => false;

    public virtual bool HasItemAt(int index) => false;

    public virtual void Add(string key, TomlNode node) { }

    public virtual void Add(TomlNode node) { }

    public virtual void Delete(TomlNode node) { }

    public virtual void Delete(string key) { }

    public virtual void Delete(int index) { }

    public virtual void AddRange(IEnumerable<TomlNode> nodes)
    {
        foreach (var tomlNode in nodes) Add(tomlNode);
    }

    public virtual void WriteTo(TextWriter tw, string name = null!) => tw.WriteLine(ToInlineToml());

    public virtual string ToInlineToml() => ToString();

    #region Native type to TOML cast

    public static implicit operator TomlNode(string value) => new TomlString { Value = value };

    public static implicit operator TomlNode(bool value) => new TomlBoolean { Value = value };

    public static implicit operator TomlNode(long value) => new TomlInteger { Value = value };

    public static implicit operator TomlNode(float value) => new TomlFloat { Value = value };

    public static implicit operator TomlNode(double value) => new TomlFloat { Value = value };

    public static implicit operator TomlNode(DateTime value) => new TomlDateTimeLocal { Value = value };

    public static implicit operator TomlNode(DateTimeOffset value) => new TomlDateTimeOffset { Value = value };

    public static implicit operator TomlNode(TomlNode[] nodes)
    {
        var result = new TomlArray();
        result.AddRange(nodes);
        return result;
    }

    #endregion

    #region TOML to native type cast

    public static implicit operator string(TomlNode value) => value.ToString();

    public static implicit operator int(TomlNode value) => (int)value.AsInteger.Value;

    public static implicit operator long(TomlNode value) => value.AsInteger.Value;

    public static implicit operator float(TomlNode value) => (float)value.AsFloat.Value;

    public static implicit operator double(TomlNode value) => value.AsFloat.Value;

    public static implicit operator bool(TomlNode value) => value.AsBoolean.Value;

    public static implicit operator DateTime(TomlNode value) => value.AsDateTimeLocal.Value;

    public static implicit operator DateTimeOffset(TomlNode value) => value.AsDateTimeOffset.Value;

    #endregion
}
