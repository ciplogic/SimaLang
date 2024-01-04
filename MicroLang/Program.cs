using MicroLang.Compiler;

void Main()
{
 
    var tokenFileExample01 = SlCompiler.CompileFile("Examples/file01.sl");

    SlCompiler compiler = new SlCompiler("Examples/Libs");
//compiler.ParseFile(@"D:\Oss\MicroLang\MicroLang\Examples\Libs\System\Res.sl");
    var sysLib = compiler.CompileLib("System");
    var tokenFile = SlCompiler.CompileFile("Examples/Src/Lexer/Token.sl");
    var tokenFileMain = SlCompiler.CompileFile("Examples/Src/main.sl");
    Console.WriteLine("Finished.");
    //compiler.ParseFile(@"Examples/Libs/System/Res.sl");
}

Main();