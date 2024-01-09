using MicroLang.Compiler;
using MicroLang.Compiler.Parser.DeclarationsParser.Declarations.Common;

void Main()
{

    var appModule = new ModuleDeclarations();
    var tokenFileExample01 = SlCompiler.CompileFile(appModule, "Examples/file01.sl");

    SlCompiler compiler = new SlCompiler("Examples/Libs");
    //compiler.ParseFile(@"D:\Oss\MicroLang\MicroLang\Examples\Libs\System\Res.sl");
    var sysLib = compiler.CompileLib("System");
    var tokenFile = SlCompiler.CompileFile(appModule, "Examples/Src/Lexer/Token.sl");
    var tokenFileMain = SlCompiler.CompileFile(appModule, "Examples/Src/main.sl");
    Console.WriteLine("Finished.");
    //compiler.ParseFile(@"Examples/Libs/System/Res.sl");
}

Main();