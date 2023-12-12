using MicroLang.Utils;

namespace MicroLang.Compiler.Lexer.Rules;

internal static class CharUtilities
{
    internal static int MatchFunc(StructSpan<char> text, Predicate<char> matchFirst, Predicate<char> matchOthers)
    {
        if (!matchFirst(text[0]))
        {
            return 0;
        }

        for (var i = 1; i < text.Len; i++)
        {
            if (!matchOthers(text[i]))
            {
                return i;
            }
        }

        return text.Len;
    }
    internal static int MatchFunc(StructSpan<char> text, Predicate<char> matchAll) 
        => MatchFunc(text, matchAll, matchAll);

    internal static int MatchStartAny(StructSpan<char> text, string[] textsStartWith)
    {
        foreach (var startsWith in textsStartWith)
        {
            if (text[0] != startsWith[0])
            {
                continue;
            }

            if (text.StartsWith(startsWith))
            {
                return startsWith.Length;
            }
        }

        return 0;
    } 

    internal static string AsText(this StructSpan<char> text, int len)
    {
        var subText = new string(text.Data, text.Start, len);
        return subText;
    }
    internal static bool StartsWith(this StructSpan<char> text, string hay)
    {
        if (hay.Length > text.Len)
        {
            return false;
        }

        for (var i = 0; i < hay.Length; i++)
        {
            if (text[i] != hay[i])
            {
                return false;
            }
        }

        return true;
    }
    internal static bool IsSpace(char ch)
        => ch == ' ' || ch == '\t';
    internal static bool IsDigit(char ch)
        => ch >= '0' && ch <= '9';

    internal static bool IsAlpha(char ch)
        => (ch >= 'a' && ch <= 'z') ||
           (ch >= 'A' && ch <= 'Z') ||
           (ch == '_');

    internal static bool IsAlphaDigit(char ch)
        => IsAlpha(ch) || IsDigit(ch);
    internal static bool IsEoln(char ch)
        => ch == '\n';

    internal static bool IsQuotingChar(char firstChar)
        => (firstChar == '"') || (firstChar == '\'') || (firstChar == '`');
}