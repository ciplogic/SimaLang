using MicroLang.Lexer.Tok;
using MicroLang.Utils;

namespace MicroLang;

public class SlCompiler
{
    internal AstNode ParseFile(string fileName)
    {
        Lexer.Lexer lexer = new Lexer.Lexer();
        string fileNameContent = File.ReadAllText(fileName);
        Res<List<Token>> tokensRes = lexer.Scan(fileNameContent);
        var tokens = tokensRes.Value;
        

        return default;
    }
}

class AstNode(string Name)
{
    public readonly List<AstNode> Children = new();
    public readonly Dictionary<string, string> Props = new();
}