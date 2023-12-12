using MicroLang.Utils;

namespace MicroLang.Compiler.Lexer.Tok;

internal record struct TokenRule(Func<StructSpan<char>, int> matcher, TokenKind Kind);