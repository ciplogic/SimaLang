using MicroLang.Compiler.Lexer.Tok;
using MicroLang.Utils;

namespace MicroLang.Compiler.HighLevelParser;

public class Scanner(Slice<Token> tokens)
{
    private Slice<Token> Tokens { get; set; } = tokens;
    public bool CanMove => Tokens.Len > 0;

    public Token Move(int count = 1)
    {
        Token lastToken = Tokens[count - 1];
        Tokens = Tokens.SubSlice(count);
        return lastToken;
    }

    public Scanner Advance(string expectedText)
    {
        var tokenExpected = Move(1);
        if (tokenExpected.Text != expectedText)
        {
            throw new InvalidDataException(expectedText);
        }

        return this;
    }

    public Slice<Token> TakeWhile(Predicate<Token> predicate)
    {
        return Tokens.TakeWhile(predicate);
    }

    public bool MoveIf(string text)
    {
        if (Tokens[0].Text == text)
        {
            Move();
            return true;
        }

        ;
        return false;
    }

    public bool MoveIf(params string[] texts)
    {
        foreach (var text in texts)
        {
            if (MoveIf(text))
            {
                return true;
            }
        }

        return false;
    }

    public bool Peek(string text) 
        => Tokens[0].Text == text;
}