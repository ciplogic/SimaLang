using MicroLang;
using MicroLang.Compiler;

SlCompiler compiler = new SlCompiler("Examples/Libs");
//compiler.ParseFile(@"D:\Oss\MicroLang\MicroLang\Examples\Libs\System\Res.sl");
var sysLib = compiler.CompileLib("System");

//compiler.ParseFile(@"Examples/Libs/System/Res.sl");
