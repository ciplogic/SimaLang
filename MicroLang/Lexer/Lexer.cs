using MicroLang.Lexer.Rules;
using MicroLang.Lexer.Tok;
using MicroLang.Utils;
using static MicroLang.Utils.ResUtils;
namespace MicroLang.Lexer;

class Lexer
{
    private List<TokenRule> _tokenRules = BuildDefaults();

    private static List<TokenRule> BuildDefaults()
    {
        List<TokenRule> results =
        [
            new (LexerRules.MatchSpacesLen, TokenKind.Spaces),
            new (LexerRules.MatchEolnLen, TokenKind.Eoln),
            new (LexerRules.MatchCommentLen, TokenKind.Comment),
            new (LexerRules.MatchNumberLen, TokenKind.Number),
            new (LexerRules.MatchStringLen, TokenKind.String),
            new (LexerRules.MatchOperatorLen, TokenKind.Operator),
            new (LexerRules.MatchIdentifierLen, TokenKind.Identifier),
        ];
        return results;
    }

    Token? MatchToken(StructSpan<char> span)
    {
        bool found = false;
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

    public Res<List<Token>> Scan(string fileNameContent)
    {
        List<Token> result = new List<Token>();
        StructSpan<char> span = StructSpan<char>.Build(fileNameContent.ToCharArray());
        while (span.Len!=0)
        {
            Token? token = MatchToken(span);
            if (!token.HasValue)
            {
                string notFoundText = span.AsText(span.Len);
                return Fail<List<Token>>($"Cannot find token for: {notFoundText}");
            }
            result.Add(token.Value);
            span = span.SubSpan(token.Value.Text.Length);
        }

        return result.Ok();
    }
}