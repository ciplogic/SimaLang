using MicroLang.Utils;
using static MicroLang.Compiler.Lex.Rules.CharUtilities;

namespace MicroLang.Compiler.Lex.Rules;

internal static class LexerRules
{
    internal static int MatchSpacesLen(Slice<char> text) 
        => MatchFunc(text, IsSpace);
    internal static int MatchIdentifierLen(Slice<char> text) 
        => MatchFunc(text, IsAlpha, IsAlphaDigit);
    internal static int MatchNumberLen(Slice<char> text)
        => MatchFunc(text, IsDigit);

    internal static int MatchStringLen(Slice<char> text)
    {
        char firstChar = text[0];
        if (!IsQuotingChar(firstChar))
        {
            return 0;
        }
        int matchLen = MatchFunc(text, IsQuotingChar, ch => ch != firstChar);
        return matchLen + 1;
    }
    
    internal static int MatchCommentLen(Slice<char> text)
    {
        if (!text.StartsWith("//"))
        {
            return 0;
        }

        for (int i = 2; i < text.Len; i++)
        {
            if (text[i] == '\n')
            {
                return i - 1;
            }
        }

        return text.Len;
    }

    internal static int MatchEolnLen(Slice<char> text)
    {
        if (text.StartsWith("\r\n"))
        {
            return 2;
        }
        
        return MatchFunc(text, IsEoln);
    }

    private static readonly string[] Operators = { 
        "...",".", 
        "=>",
        "<", ">", 
        "=",
        "(", ")", "[", "]", "{", "}", 
        ",",
        "**","*","&",
        "/","+","-",
        ";", ":" 
    };

    internal static int MatchOperatorLen(Slice<char> text) 
        => MatchStartAny(text, Operators);
    
    private static readonly string[] ReservedWords = { 
        "enum",
        "elif",
        "else",
        "enum",
        "fn",
        "for",
        "if",
        "import",
        "interface",
        "return",
        "value",
        "yield"
    };

    internal static int MatchReservedWordLen(Slice<char> text)
    {
        int identifierLen = MatchIdentifierLen(text);
        if (identifierLen == 0)
        {
            return 0;
        }

        foreach (string reservedWord in ReservedWords)
        {
            if (identifierLen != reservedWord.Length)
            {
                continue;
            }

            if (text.StartsWith(reservedWord))
            {
                return identifierLen;
            }
        }

        return 0;
    }
}