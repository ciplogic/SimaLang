using MicroLang.Utils;
using static MicroLang.Compiler.Lexer.Rules.CharUtilities;

namespace MicroLang.Compiler.Lexer.Rules;

internal static class LexerRules
{
    internal static int MatchSpacesLen(StructSpan<char> text) 
        => MatchFunc(text, IsSpace);
    internal static int MatchIdentifierLen(StructSpan<char> text) 
        => MatchFunc(text, IsAlpha, IsAlphaDigit);
    internal static int MatchNumberLen(StructSpan<char> text)
        => MatchFunc(text, IsDigit);

    internal static int MatchStringLen(StructSpan<char> text)
    {
        var firstChar = text[0];
        if (!IsQuotingChar(firstChar))
        {
            return 0;
        }
        var matchLen = MatchFunc(text, IsQuotingChar, ch => ch == firstChar);
        return matchLen + 1;
    }
    
    internal static int MatchCommentLen(StructSpan<char> text)
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

    internal static int MatchEolnLen(StructSpan<char> text)
    {
        if (text.StartsWith("\r\n"))
        {
            return 2;
        }
        
        return MatchFunc(text, IsEoln);
    }

    private static string[] Operators = { 
        "...",".", 
        "=>",
        "<", ">", 
        "=",
        "(", ")", "[", "]", "{", "}", 
        ",",
        "*","/","&",
        ";", ":" 
    };

    internal static int MatchOperatorLen(StructSpan<char> text) 
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

    internal static int MatchReservedWordLen(StructSpan<char> text)
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