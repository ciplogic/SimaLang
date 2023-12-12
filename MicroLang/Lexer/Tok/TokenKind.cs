namespace MicroLang.Lexer.Tok;

internal enum TokenKind
{
    None,
    Spaces,
    Eoln,
    Comment,
    Identifier,
    Operator,
    Number,
    String
}