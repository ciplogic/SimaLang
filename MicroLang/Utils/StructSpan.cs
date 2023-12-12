namespace MicroLang.Utils;

record struct StructSpan<T>(T[] Data, int Start, int Len)
{
    internal T this[int index] => Data[Start + index];

    internal static StructSpan<T> Build(T[] data) 
        => new(data, 0, data.Length);

    internal StructSpan<T> SubSpan(int subSpanStart)
    {
        return new StructSpan<T>(Data, Start + subSpanStart, Len - subSpanStart);
    }

    public override string ToString() 
        => string.Join("", Data.Skip(Start));
}