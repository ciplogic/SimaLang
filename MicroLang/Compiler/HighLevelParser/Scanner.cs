using System.Text;
using MicroLang.Compiler.Lexer.Tok;
using MicroLang.Utils;

namespace MicroLang.Compiler.HighLevelParser;

public class Scanner(Slice<Token> tokens)
{
    public Slice<Token> Tokens { get; set; } = tokens;
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

    public override string ToString()
    {
        var sb = new StringBuilder();
        for(var i =0; i<Tokens.Len; i++)
        {
            sb.Append(Tokens[i].Text);
            if (Tokens[i].Kind == TokenKind.ReservedWord)
            {
                sb.Append(" ");
            }
        }

        return sb.ToString();
    }

    public bool Peek(string text) 
        => Tokens[0].Text == text;

    public bool Peek(TokenKind kind)
        => Tokens[0].Kind == kind;

    public string PeekText()
    {
        return Tokens[0].Text;
    }

    public bool MoveIf(TokenKind tokenKind)
    {
        if (Tokens[0].Kind != tokenKind)
        {
            return false;
        }
        Move();
        return true;

    }
}