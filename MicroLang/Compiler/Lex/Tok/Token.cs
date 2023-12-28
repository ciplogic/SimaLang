namespace MicroLang.Compiler.Lex.Tok;

public record struct Token(TokenKind Kind, string Text)
{
    public override string ToString() 
        => $"{Kind.ToString()[0]}.{Text} ";
}