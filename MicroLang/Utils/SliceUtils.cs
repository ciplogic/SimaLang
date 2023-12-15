namespace MicroLang.Utils;

static class SliceUtils
{
    internal static Slice<T> SkipWhile<T>(this Slice<T> _this, Predicate<T> predicate)
    {
        for (int i = 0; i < _this.Len; i++)
        {
            if (predicate(_this[i]))
            {
                return _this.SubSlice(i); 
            }
        }

        return _this.SubSlice(0, 0);
    }
    internal static Slice<T> TakeWhile<T>(this Slice<T> _this, Predicate<T> predicate)
    {
        for (int i = 0; i < _this.Len; i++)
        {
            if (!predicate(_this[i]))
            {
                return _this.SubSlice(0, i-1); 
            }
        }

        return _this;
    }
    internal static Slice<T> Skip<T>(this Slice<T> _this, int index)
    {
        return _this.SubSlice(index);
        
    }
    internal static Slice<T> Take<T>(this Slice<T> _this, int index)
    {
        return _this.SubSlice(0, index);
    }

    internal static Slice<T>[] SplitWhen<T>(this Slice<T> _this, Predicate<T> isSplittingItem, bool skipEmpty = true)
    {
        List<Slice<T>> result = new();
        int startPos = 0;
        for (int i = 0; i < _this.Len; i++)
        {
            bool isSeparatorItem = isSplittingItem(_this[i]);
            if (isSeparatorItem)
            {
                AddSlice(startPos, i - startPos);
                startPos = i + 1;
            }                
        }
        AddSlice(startPos, _this.Len- startPos);

        void AddSlice(int startPos, int len)
        {
            if (!skipEmpty || len > 0)
            {
                Slice<T> item = _this.SubSlice(startPos, len);
                result.Add(item);
            }
        }

        return result.ToArray();
    }
}