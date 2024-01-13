using MicroLang.Compiler;
using MicroLang.Compiler.Parser.DeclarationsParser.Declarations.Common;
using MicroLang.Compiler.Parser.FirstPassParser;
using MicroLang.Utils;

void Main()
{

    ModuleDeclarations appModule = new ModuleDeclarations();
    TreeNodeParse tokenFileExample01 = SlCompiler.CompileFile(appModule, "Examples/file01.sl");

    SlCompiler compiler = new SlCompiler("Examples/Libs");
    //compiler.ParseFile(@"D:\Oss\MicroLang\MicroLang\Examples\Libs\System\Res.sl");
    Res<ModuleDeclarations> sysLib = compiler.CompileLib(appModule, "System");
    TreeNodeParse tokenFile = SlCompiler.CompileFile(appModule, "Examples/Src/Lexer/Token.sl");
    TreeNodeParse tokenFileMain = SlCompiler.CompileFile(appModule, "Examples/Src/main.sl");
    Console.WriteLine("Finished.");
    //compiler.ParseFile(@"Examples/Libs/System/Res.sl");
}

Main();