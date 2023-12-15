namespace MicroLang.Compiler.Constants;

public class StringTable
{
    private Dictionary<string, int> Keys { get; } = new();
    public List<string> Texts { get; } = new();

    public int Update(string value)
    {
        if (Keys.TryGetValue(value, out int index))
        {
            return index;
        }

        index = Texts.Count;
        Keys[value] = index;
        Texts.Add(value);
        return index;
    }
}
