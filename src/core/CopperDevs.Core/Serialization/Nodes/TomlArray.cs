using System;
using System.Text;

namespace CopperDevs.Core.Serialization.Nodes;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class TomlArray : TomlNode
{
    private List<TomlNode> values = [];

    public override bool HasValue { get; } = true;
    public override bool IsArray { get; } = true;
    public bool IsMultiline { get; set; }
    public bool IsTableArray { get; set; }
    public List<TomlNode> RawArray => values ??= [];

    public override TomlNode this[int index]
    {
        get
        {
            if (index < RawArray.Count) return RawArray[index];
            var lazy = new TomlLazy(this);
            this[index] = lazy;
            return lazy;
        }
        set
        {
            if (index == RawArray.Count)
                RawArray.Add(value);
            else
                RawArray[index] = value;
        }
    }

    public override int ChildrenCount => RawArray.Count;

    public override IEnumerable<TomlNode> Children => RawArray.AsEnumerable();

    public override void Add(TomlNode node) => RawArray.Add(node);

    public override void AddRange(IEnumerable<TomlNode> nodes) => RawArray.AddRange(nodes);

    public override void Delete(TomlNode node) => RawArray.Remove(node);

    public override void Delete(int index) => RawArray.RemoveAt(index);

    public override string ToString() => ToString(false);

    public string ToString(bool multiline)
    {
        var sb = new StringBuilder();
        sb.Append(TomlSyntax.ARRAY_START_SYMBOL);
        if (ChildrenCount != 0)
        {
            var arrayStart = multiline ? $"{Environment.NewLine}  " : " ";
            var arraySeparator = multiline ? $"{TomlSyntax.ITEM_SEPARATOR}{Environment.NewLine}  " : $"{TomlSyntax.ITEM_SEPARATOR} ";
            var arrayEnd = multiline ? Environment.NewLine : " ";
            sb.Append(arrayStart)
              .Append(arraySeparator.Join(RawArray.Select(n => n.ToInlineToml())))
              .Append(arrayEnd);
        }
        sb.Append(TomlSyntax.ARRAY_END_SYMBOL);
        return sb.ToString();
    }

    public override void WriteTo(TextWriter tw, string name = null!)
    {
        // If it's a normal array, write it as usual
        if (!IsTableArray)
        {
            tw.WriteLine(ToString(IsMultiline));
            return;
        }

        if (Comment is not null)
        {
            tw.WriteLine();
            Comment.AsComment(tw);
        }
        tw.Write(TomlSyntax.ARRAY_START_SYMBOL);
        tw.Write(TomlSyntax.ARRAY_START_SYMBOL);
        tw.Write(name);
        tw.Write(TomlSyntax.ARRAY_END_SYMBOL);
        tw.Write(TomlSyntax.ARRAY_END_SYMBOL);
        tw.WriteLine();

        var first = true;

        foreach (var tomlNode in RawArray)
        {
            if (tomlNode is not TomlTable tbl)
                throw new TomlFormatException("The array is marked as array table but contains non-table nodes!");

            // Ensure it's parsed as a section
            tbl.IsInline = false;

            if (!first)
            {
                tw.WriteLine();

                Comment?.AsComment(tw);
                tw.Write(TomlSyntax.ARRAY_START_SYMBOL);
                tw.Write(TomlSyntax.ARRAY_START_SYMBOL);
                tw.Write(name);
                tw.Write(TomlSyntax.ARRAY_END_SYMBOL);
                tw.Write(TomlSyntax.ARRAY_END_SYMBOL);
                tw.WriteLine();
            }

            first = false;

            // Don't write section since it's already written here
            tbl.WriteTo(tw, name, false);
        }
    }
}

