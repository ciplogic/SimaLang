using MicroLang.Utils;

namespace MicroLang.Lexer.Tok;

internal record struct TokenRule(Func<StructSpan<char>, int> matcher, TokenKind Kind);