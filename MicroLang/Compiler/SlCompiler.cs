using MicroLang.Compiler.HighLevelParser;
using MicroLang.Compiler.Lexer.Tok;
using MicroLang.Compiler.Semantic;
using MicroLang.Utils;
using static MicroLang.Utils.ResUtils;

namespace MicroLang.Compiler;

internal class SlCompiler
{
    public string LibsPath { get; }

    public SlCompiler(string libsPath)
    {
        LibsPath = libsPath;
    }

    internal SemanticTree Program { get; } = new();
    internal TreeNode ParseFile(string fileName)
    {
        string fileNameContent = File.ReadAllText(fileName);
        Lexer.Lexer lexer = new Lexer.Lexer();
        Res<List<Token>> tokensRes = lexer.Scan(fileNameContent);
        Token[] tokenArray = tokensRes.Value.ToArray();
        HighLevelParse highLevelParse = new HighLevelParse();
        TreeNode rootNode = highLevelParse.ParseFileHighLevel(tokenArray);
        return rootNode;
    }

    public Res<TreeNode> CompileLib(string lib)
    {
        var libFullPath = Path.Join(LibsPath, lib);
        if (!Directory.Exists(libFullPath))
        {
            return Fail<TreeNode>("Compiler: Path not found: " + libFullPath);
        }

        var dirFiles = Directory.GetFiles(libFullPath, "*.sl", SearchOption.TopDirectoryOnly);

        var result = new TreeNode("Library");
        result["name"] = lib;

        foreach (string dirFile in dirFiles)
        {
            TreeNode compiledFile = this.ParseFile(dirFile);
            result.Children.Add(compiledFile);
        }

        return ResUtils.Ok<TreeNode>(result);
    }
}
