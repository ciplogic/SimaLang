using MicroLang.Utils;

namespace MicroLang.Compiler.Lex.Tok;

internal record struct TokenRule(Func<Slice<char>, int> Matcher, TokenKind Kind);