using System.Text.Json;

namespace MicroLang.Compiler.Semantic;

public record TreeNode(Dictionary<string, string> Fields, List<TreeNode> Children)
{
    internal TreeNode()
        : this(new Dictionary<string, string>(), new List<TreeNode>())
    {
    }

    internal TreeNode(string name)
        : this()
    {
        this["type"] = name;
    }

    private string Type
        => this["type"];

    private string GetField(string fieldKey)
    {
        return Fields.TryGetValue(fieldKey, out string result)
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
        set => Fields[key] = value;
    }

    public override string ToString()
    {
        Dictionary<string, object> objToSerialize = ToPrintableDict();
        return JsonSerializer.Serialize(objToSerialize, new JsonSerializerOptions()
        {
            WriteIndented = true
        });
    }

    Dictionary<string, object> ToPrintableDict()
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        foreach (KeyValuePair<string, string> field in Fields)
        {
            result[field.Key] = field.Value;
        }

        if (Children.Count == 0)
        {
            return result;
        }

        Dictionary<string, object>[] items = Children.Select(
            it => it.ToPrintableDict())
            .ToArray();
        result["children"] = items;

        return result;
    }
}