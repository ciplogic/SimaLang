using MicroLang.Compiler.Lex;
using MicroLang.Compiler.Lex.Tok;
using MicroLang.Compiler.Parser.FirstPassParser;
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

    public Res<PassOneAstNode> CompileLib(string lib)
    {
        var libFullPath = Path.Join(LibsPath, lib);
        if (!Directory.Exists(libFullPath))
        {
            return Fail<PassOneAstNode>("Compiler: Path not found: " + libFullPath);
        }

        var dirFiles = Directory.GetFiles(libFullPath, "*.sl", SearchOption.TopDirectoryOnly);

        var result = new TreeNode("Library");
        result["name"] = lib;

        var root = new PassOneAstNode(AstNodeKind.World);
        root.Tok = new Token(TokenKind.Comment, lib);
        foreach (string dirFile in dirFiles)
        {
            var lexer = new Lexer();
            var content = File.ReadAllText(dirFile);
            var words = lexer.Scan(content).Value;
            var slice = Slice<Token>.Build(words.ToArray());
            PassOneAstNode hlParsed = ParserPassOne.Parse(slice);
            hlParsed.Tok = new Token(TokenKind.Comment, dirFile);
            root.Children.Add(hlParsed);
        }

        return ResUtils.Ok<PassOneAstNode>(root);
    }
}
