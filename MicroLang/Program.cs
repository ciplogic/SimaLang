// See https://aka.ms/new-console-template for more information

using MicroLang;

Console.WriteLine("Hello, World!");
var compiler = new SlCompiler();
compiler.ParseFile("Examples/file01.sl");