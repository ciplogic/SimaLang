using MicroLang.Compiler.Lex;
using MicroLang.Compiler.Lex.Tok;
using MicroLang.Compiler.Parser.DeclarationsParser;
using MicroLang.Compiler.Parser.DeclarationsParser.Declarations;
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

    public Res<(TreeNodeParse, ModuleDeclarations)> CompileLib(string lib)
    {
        var libFullPath = Path.Join(LibsPath, lib);
        if (!Directory.Exists(libFullPath))
        {
            return Fail<(TreeNodeParse, ModuleDeclarations)>("Compiler: Path not found: " + libFullPath);
        }

        ModuleDeclarations libModule = new()
        {
            Namespace = lib
        };
        var dirFiles = Directory.GetFiles(libFullPath, "*.sl", SearchOption.TopDirectoryOnly);

        var result = new TreeNode("Library");
        result["name"] = lib;

        var root = new TreeNodeParse(AstNodeKind.World)
        {
            Tok = new Token(TokenKind.Comment, lib)
        };
        foreach (string dirFile in dirFiles)
        {
            var hlParsed = CompileFile(dirFile);
            root.Children.Add(hlParsed);
            DeclarationParsing.Execute(libModule, lib, hlParsed);
        }

        return ResUtils.Ok<(TreeNodeParse, ModuleDeclarations)>((root, libModule));
    }

    public static TreeNodeParse CompileFile(string dirFile)
    {
        var lexer = new Lexer();
        var content = File.ReadAllText(dirFile);
        Res<List<Token>> scanResult = lexer.Scan(content);
        var words = scanResult.Value;
        var slice = Slice<Token>.Build(words.ToArray());
        TreeNodeParse hlParsed = ParserPassOne.Parse(slice);
        hlParsed.Tok = new Token(TokenKind.Comment, dirFile);
        return hlParsed;
    }
}
