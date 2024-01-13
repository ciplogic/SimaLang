using MicroLang.Compiler.Lex;
using MicroLang.Compiler.Lex.Tok;
using MicroLang.Compiler.Parser.DeclarationsParser;
using MicroLang.Compiler.Parser.DeclarationsParser.Declarations.Common;
using MicroLang.Compiler.Parser.FirstPassParser;
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

    public Res<ModuleDeclarations> CompileLib(ModuleDeclarations libModule, string lib)
    {
        string libFullPath = Path.Join(LibsPath, lib);
        if (!Directory.Exists(libFullPath))
        {
            return Fail<ModuleDeclarations>($"Compiler: Path not found: {libFullPath}.");
        }
        
        string[] dirFiles = Directory.GetFiles(libFullPath, "*.sl", SearchOption.TopDirectoryOnly);
        foreach (string dirFile in dirFiles)
        {
            TreeNodeParse hlParsed = CompileFile(libModule, dirFile);
        }

        return ResUtils.Ok<ModuleDeclarations>(libModule);
    }

    public static TreeNodeParse CompileFile(ModuleDeclarations libModule, string dirFile)
    {
        Lexer lexer = new Lexer();
        string content = File.ReadAllText(dirFile);
        Res<List<Token>> scanResult = lexer.Scan(content);
        List<Token> words = scanResult.Value;
        Slice<Token> slice = Slice<Token>.Build(words.ToArray());
        TreeNodeParse hlParsed = ParserPassOne.Parse(slice);
        DeclarationParsing.Execute(libModule, dirFile, hlParsed);
        hlParsed.Tok = new Token(TokenKind.Comment, dirFile);
        return hlParsed;
    }
}
