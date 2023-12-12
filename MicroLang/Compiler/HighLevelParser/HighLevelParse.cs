using MicroLang.Compiler.Lexer.Tok;
using MicroLang.Compiler.Semantic;
using MicroLang.Utils;

namespace MicroLang.Compiler.HighLevelParser;

internal class HighLevelParse
{
    internal TreeNode ParseFileHighLevel(Token[] tokens)
    {
        StructSpan<Token> tokensSpan = StructSpan<Token>.Build(tokens);
        
        return default;
    }
}