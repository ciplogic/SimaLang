namespace MicroLang.Compiler.Lexer.Tok;

public enum TokenKind
{
    None,
    Spaces,
    Eoln,
    Comment,
    ReservedWord,
    Identifier,
    Operator,
    Number,
    String,
}