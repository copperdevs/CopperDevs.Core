using System.Text;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace CopperDevs.Core.Serialization.Nodes;

public class TomlTable : TomlNode
{
    private Dictionary<string, TomlNode> children = [];
    internal bool IsImplicit;

    public override bool HasValue { get; } = false;
    public override bool IsTable { get; } = true;
    public bool IsInline { get; set; }
    public Dictionary<string, TomlNode> RawTable => children ??= [];

    public override TomlNode this[string key]
    {
        get
        {
            if (RawTable.TryGetValue(key, out var result)) return result;
            var lazy = new TomlLazy(this);
            RawTable[key] = lazy;
            return lazy;
        }
        set => RawTable[key] = value;
    }

    public override int ChildrenCount => RawTable.Count;
    public override IEnumerable<TomlNode> Children => RawTable.Select(kv => kv.Value);
    public override IEnumerable<string> Keys => RawTable.Select(kv => kv.Key);
    public override bool HasKey(string key) => RawTable.ContainsKey(key);
    public override void Add(string key, TomlNode node) => RawTable.Add(key, node);
    public override bool TryGetNode(string key, out TomlNode node) => RawTable.TryGetValue(key, out node!);
    public override void Delete(TomlNode node) => RawTable.Remove(RawTable.First(kv => kv.Value == node).Key);
    public override void Delete(string key) => RawTable.Remove(key);

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(TomlSyntax.INLINE_TABLE_START_SYMBOL);

        if (ChildrenCount != 0)
        {
            var collapsed = CollectCollapsedItems(normalizeOrder: false);

            if (collapsed.Count != 0)
                sb.Append(' ')
                  .Append($"{TomlSyntax.ITEM_SEPARATOR} ".Join(collapsed.Select(n =>
                                                                   $"{n.Key} {TomlSyntax.KEY_VALUE_SEPARATOR} {n.Value.ToInlineToml()}")));
            sb.Append(' ');
        }

        sb.Append(TomlSyntax.INLINE_TABLE_END_SYMBOL);
        return sb.ToString();
    }

    private LinkedList<KeyValuePair<string, TomlNode>> CollectCollapsedItems(string prefix = "", int level = 0, bool normalizeOrder = true)
    {
        var nodes = new LinkedList<KeyValuePair<string, TomlNode>>();
        var postNodes = normalizeOrder ? new LinkedList<KeyValuePair<string, TomlNode>>() : nodes;

        foreach (var keyValuePair in RawTable)
        {
            var node = keyValuePair.Value;
            var key = keyValuePair.Key.AsKey();

            if (node is TomlTable tbl)
            {
                var subnodes = tbl.CollectCollapsedItems($"{prefix}{key}.", level + 1, normalizeOrder);
                // Write main table first before writing collapsed items
                if (subnodes.Count == 0 && node.CollapseLevel == level)
                {
                    postNodes.AddLast(new KeyValuePair<string, TomlNode>($"{prefix}{key}", node));
                }
                foreach (var kv in subnodes)
                    postNodes.AddLast(kv);
            }
            else if (node.CollapseLevel == level)
                nodes.AddLast(new KeyValuePair<string, TomlNode>($"{prefix}{key}", node));
        }

        if (normalizeOrder)
            foreach (var kv in postNodes)
                nodes.AddLast(kv);

        return nodes;
    }

    public override void WriteTo(TextWriter tw, string name = null) => WriteTo(tw, name, true);

    internal void WriteTo(TextWriter tw, string name, bool writeSectionName)
    {
        // The table is inline table
        if (IsInline && name != null)
        {
            tw.WriteLine(ToInlineToml());
            return;
        }

        var collapsedItems = CollectCollapsedItems();

        if (collapsedItems.Count == 0)
            return;

        var hasRealValues = !collapsedItems.All(n => n.Value is TomlTable { IsInline: false } or TomlArray { IsTableArray: true });

        Comment?.AsComment(tw);

        if (name != null && (hasRealValues || Comment != null) && writeSectionName)
        {
            tw.Write(TomlSyntax.ARRAY_START_SYMBOL);
            tw.Write(name);
            tw.Write(TomlSyntax.ARRAY_END_SYMBOL);
            tw.WriteLine();
        }
        else if (Comment != null) // Add some spacing between the first node and the comment
        {
            tw.WriteLine();
        }

        var namePrefix = name == null ? "" : $"{name}.";
        var first = true;

        foreach (var collapsedItem in collapsedItems)
        {
            var key = collapsedItem.Key;
            if (collapsedItem.Value is TomlArray { IsTableArray: true } or TomlTable { IsInline: false })
            {
                if (!first) tw.WriteLine();
                first = false;
                collapsedItem.Value.WriteTo(tw, $"{namePrefix}{key}");
                continue;
            }
            first = false;

            collapsedItem.Value.Comment?.AsComment(tw);
            tw.Write(key);
            tw.Write(' ');
            tw.Write(TomlSyntax.KEY_VALUE_SEPARATOR);
            tw.Write(' ');

            collapsedItem.Value.WriteTo(tw, $"{namePrefix}{key}");
        }
    }
}
