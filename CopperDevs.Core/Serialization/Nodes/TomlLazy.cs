namespace CopperDevs.Core.Serialization.Nodes;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

internal class TomlLazy(TomlNode parent) : TomlNode
{
    private readonly TomlNode parent = parent;
    private TomlNode replacement = null!;

    public override TomlNode this[int index]
    {
        get => Set<TomlArray>()[index];
        set => Set<TomlArray>()[index] = value;
    }

    public override TomlNode this[string key]
    {
        get => Set<TomlTable>()[key];
        set => Set<TomlTable>()[key] = value;
    }

    public override void Add(TomlNode node) => Set<TomlArray>().Add(node);

    public override void Add(string key, TomlNode node) => Set<TomlTable>().Add(key, node);

    public override void AddRange(IEnumerable<TomlNode> nodes) => Set<TomlArray>().AddRange(nodes);

    private TomlNode Set<T>() where T : TomlNode, new()
    {
        if (replacement != null) return replacement;

        var newNode = new T
        {
            Comment = Comment
        };

        if (parent.IsTable)
        {
            var key = parent.Keys.FirstOrDefault(s => parent.TryGetNode(s, out var node) && node.Equals(this));
            if (key == null) return default(T)!;

            parent[key] = newNode;
        }
        else if (parent.IsArray)
        {
            var index = parent.Children.TakeWhile(child => child != this).Count();
            if (index == parent.ChildrenCount) return default(T)!;
            parent[index] = newNode;
        }
        else
        {
            return default(T)!;
        }

        replacement = newNode;
        return newNode;
    }
}
