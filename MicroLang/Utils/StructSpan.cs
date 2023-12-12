namespace MicroLang.Utils;

record struct StructSpan<T>(T[] Data, int Start, int Len)
{
    public T this[int index]
    {
        get => Data[Start + index];
    }

    public static StructSpan<T> Build(T[] data) 
        => new(data, 0, data.Length);

    public StructSpan<T> SubSpan(int subSpanStart)
    {
        return new StructSpan<T>(Data, Start + subSpanStart, Len - subSpanStart);
    }

    public override string ToString()
    {
        return string.Join("", Data.Skip(Start));
    }
}