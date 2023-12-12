using MicroLang.Compiler.Lexer.Rules;
using MicroLang.Compiler.Lexer.Tok;
using MicroLang.Utils;
using static MicroLang.Utils.ResUtils;
namespace MicroLang.Compiler.Lexer;

class Lexer
{
    private List<TokenRule> _tokenRules = BuildDefaults();
    internal bool SkipSpaces = true;

    private static List<TokenRule> BuildDefaults()
    {
        List<TokenRule> results =
        [
            new(LexerRules.MatchSpacesLen, TokenKind.Spaces),
            new(LexerRules.MatchEolnLen, TokenKind.Eoln),
            new(LexerRules.MatchCommentLen, TokenKind.Comment),
            new(LexerRules.MatchNumberLen, TokenKind.Number),
            new(LexerRules.MatchStringLen, TokenKind.String),
            new(LexerRules.MatchOperatorLen, TokenKind.Operator),
            new(LexerRules.MatchReservedWordLen, TokenKind.ReservedWord),
            new(LexerRules.MatchIdentifierLen, TokenKind.Identifier),
        ];
        return results;
    }

    Token? MatchToken(StructSpan<char> span)
    {
        foreach (TokenRule tokenRule in _tokenRules)
        {
            var matchLen = tokenRule.matcher(span);
            if (matchLen == 0)
            {
                continue;
            }

            Token token = new Token(span.AsText(matchLen), tokenRule.Kind);
            return token;
        }

        return default;
    }

    private static bool IsSpaceToken(TokenKind tokenKind)
    {
        return tokenKind == TokenKind.Spaces || tokenKind == TokenKind.Comment;
    }

    internal Res<List<Token>> Scan(string fileNameContent)
    {
        List<Token> result = new(fileNameContent.Length / 8);
        StructSpan<char> span = StructSpan<char>.Build(fileNameContent.ToCharArray());
        while (span.Len != 0)
        {
            Token? token = MatchToken(span);
            if (!token.HasValue)
            {
                string notFoundText = span.AsText(span.Len);
                return Fail<List<Token>>($"Cannot find token for: {notFoundText}");
            }

            if (!SkipSpaces  || !IsSpaceToken(token.Value.Kind))
            {
                result.Add(token.Value);
            }
            span = span.SubSpan(token.Value.Text.Length);
        }

        return result.Ok();
    }
}