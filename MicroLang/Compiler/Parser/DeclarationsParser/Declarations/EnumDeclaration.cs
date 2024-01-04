using MicroLang.Compiler.Parser.DeclarationsParser.Declarations.Common;
using MicroLang.Compiler.Parser.FirstPassParser;
using MicroLang.Utils;

namespace MicroLang.Compiler.Parser.DeclarationsParser.Declarations;

internal class EnumDeclaration : NamedDeclaration
{
    public EnumDeclaration(Slice<TreeNodeParse> declarationNode)
    {
        Setup(declarationNode[0].Tok.Text, NamedDeclarationKind.Enum);
    }
}