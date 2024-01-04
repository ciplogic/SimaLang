using MicroLang.Compiler.Parser.DeclarationsParser.Declarations.Common;
using MicroLang.Compiler.Parser.FirstPassParser;
using MicroLang.Utils;

namespace MicroLang.Compiler.Parser.DeclarationsParser.Declarations;

internal class InterfaceDeclaration : NamedDeclaration
{
    public InterfaceDeclaration(Slice<TreeNodeParse> declarationNode)
    {
        Setup(declarationNode[0].Tok.Text, NamedDeclarationKind.Interface);
    }
}