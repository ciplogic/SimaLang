namespace MicroLang.Utils;

record struct Slice<T>(T[] Data, int Start, int Len)
{
    internal T this[int index] => Data[Start + index];

    internal static Slice<T> Build(T[] data) 
        => new(data, 0, data.Length);

    internal Slice<T> SubSlice(int subSpanStart) 
        => new(Data, Start + subSpanStart, Len - subSpanStart);

    internal int IndexOf(Predicate<T> match)
    {
        int pos = 0;
        while (pos<Len)
        {
            if (match(this[pos]))
            {
                return pos;
            }
            pos++;
        }

        return -1;

    }

    public override string ToString() 
        => string.Join("", Arr);

    public T[] Arr
    {
        get
        {
            List<T> r = new List<T>(Len);
            for (int i = 0; i < Len; i++)
            {
                r.Add(Data[Start + i]);
            }

            return r.ToArray();
        }
    }

    public Slice<T> SubSlice(int start, int len)
    {
        return new Slice<T>(Data, this.Start + start, len);
    }
}