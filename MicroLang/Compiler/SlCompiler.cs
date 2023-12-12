using MicroLang.Compiler.HighLevelParser;
using MicroLang.Compiler.Lexer.Tok;
using MicroLang.Compiler.Semantic;
using MicroLang.Utils;

namespace MicroLang.Compiler;

internal class SlCompiler
{
    private string LibsPath = "";

    internal SemanticTree Program { get; } = new();
    internal TreeNode ParseFile(string fileName)
    {
        Lexer.Lexer lexer = new Lexer.Lexer();
        string fileNameContent = File.ReadAllText(fileName);
        Res<List<Token>> tokensRes = lexer.Scan(fileNameContent);
        Token[] tokenArray = tokensRes.Value.ToArray();
        HighLevelParse highLevelParse = new HighLevelParse();
        TreeNode rootNode = highLevelParse.ParseFileHighLevel(tokenArray);
        

        return default;
    }
}
