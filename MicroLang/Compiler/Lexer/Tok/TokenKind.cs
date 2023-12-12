namespace MicroLang.Compiler.Lexer.Tok;

internal enum TokenKind
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