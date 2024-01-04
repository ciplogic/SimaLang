namespace MicroLang.Compiler.Parser.DeclarationsParser.Declarations.Common;

internal abstract class NamedDeclaration 
{
    public string Name { get; set; }
    public string NameSpace { get; set; }
    public NamedDeclarationKind Kind { get; set; }


    protected void Setup(string name, NamedDeclarationKind kind)
    {
        Name = name;
        Kind = kind;
    }
}