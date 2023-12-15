using System.Text;

namespace MicroLang.Compiler.Semantic;

record TreeNode(List<TreeNode> Children, Dictionary<string, string> Props)
{
    internal TreeNode()
        : this(new List<TreeNode>(), new Dictionary<string, string>())
    {
    }
    internal TreeNode(string name)
        : this()
    {
        this["type"] = name;
    }

    public string Type
        => this["type"];

    private string GetField(string fieldKey)
    {
        return Props.TryGetValue(fieldKey, out string result) 
            ? result 
            : string.Empty;
    }

    internal TreeNode Child(string childType)
    {
        TreeNode? childMatch = Children.FirstOrDefault(c => c.Type == childType);
        if (childMatch is not null)
        {
            return childMatch;
        }
        TreeNode result = new TreeNode(childType);
        Children.Add(result);
        return result;
    }

    public string this[string key]
    {
        get => GetField(key);
        set => Props[key] = value;
    }

    public override string ToString()
    {
        return ToStringIndented(0);
    }

    string ToStringIndented(int indent)
    {
        string spaces = new string(' ', indent);
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("{");
        foreach (KeyValuePair<string, string> kv in Props)
        {
            sb
                .Append(spaces)
                .Append("  ")
                .Append($"{kv.Key}: {kv.Value},")
                .AppendLine();
        }

        foreach (TreeNode child in Children)
        {
            string childText = child.ToStringIndented(indent + 2);
            sb
                .Append(spaces)
                .Append("  ")
                .Append(childText)
                .AppendLine();
        }
        sb.Append(spaces).AppendLine("}");

        return sb.ToString();

    }
}
