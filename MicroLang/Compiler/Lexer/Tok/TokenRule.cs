﻿using MicroLang.Utils;

namespace MicroLang.Compiler.Lexer.Tok;

internal record struct TokenRule(Func<Slice<char>, int> matcher, TokenKind Kind);